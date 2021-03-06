using LBHFSSPortalAPI.V1.Gateways;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;
using LBHFSSPortalAPI.V1.Infrastructure;
using LBHFSSPortalAPI.V1.UseCase;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using LBHFSSPortalAPI.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using LBHFSSPortalAPI.V1.Handlers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using ConnectionInfo = LBHFSSPortalAPI.V1.Infrastructure.ConnectionInfo;
using LBHFSSPortalAPI.V1.Domain;

namespace LBHFSSPortalAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private static List<ApiVersionDescription> _apiVersions { get; set; }
        //TODO update the below to the name of your API
        private const string ApiName = "LBH FSS Portal API";

        // This method gets called by the runtime. Use this method to add services to the container.
        public static void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddApiVersioning(o =>
            {
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.AssumeDefaultVersionWhenUnspecified = true; // assume that the caller wants the default version if they don't specify
                o.ApiVersionReader = new UrlSegmentApiVersionReader(); // read the version number from the url segment header)
            });

            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthorisationHandler>("BasicAuthentication", null);

            services.AddAuthorization(config =>
            {
                var userAuthPolicyBuilder = new AuthorizationPolicyBuilder();
                config.DefaultPolicy = userAuthPolicyBuilder
                    .RequireAuthenticatedUser()
                    .RequireClaim(ClaimTypes.Role)
                    .Build();
            });

            services.AddCors();
            services.AddSingleton<IApiVersionDescriptionProvider, DefaultApiVersionDescriptionProvider>();

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Token",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Your Hackney API Key",
                        Name = "X-Api-Key",
                        Type = SecuritySchemeType.ApiKey
                    });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Token" }
                        },
                        new List<string>()
                    }
                });

                //Looks at the APIVersionAttribute [ApiVersion("x")] on controllers and decides whether or not
                //to include it in that version of the swagger document
                //Controllers must have this [ApiVersion("x")] to be included in swagger documentation!!
                c.DocInclusionPredicate((string docName, ApiDescription apiDesc) =>
                {
                    apiDesc.TryGetMethodInfo(out var methodInfo);

                    var versions = methodInfo?
                        .DeclaringType?.GetCustomAttributes()
                        .OfType<ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions).ToList();

                    return versions?.Any(v => $"{v.GetFormattedApiVersion()}" == docName) ?? false;
                });

                //Get every ApiVersion attribute specified and create swagger docs for them
                foreach (var apiVersion in _apiVersions)
                {
                    var version = $"v{apiVersion.ApiVersion.ToString()}";
                    c.SwaggerDoc(version, new OpenApiInfo
                    {
                        Title = $"{ApiName}-api {version}",
                        Version = version,
                        Description = $"{ApiName} version {version}. Please check older versions for depreciated endpoints."
                    });
                }

                c.CustomSchemaIds(x => x.FullName);
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (System.IO.File.Exists(xmlPath))
                    c.IncludeXmlComments(xmlPath);
            });
            ConfigureDbContext(services);
            RegisterGateways(services);
            RegisterUseCases(services);
        }

        private static void ConfigureDbContext(IServiceCollection services)
        {
            var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

            services.AddDbContext<DatabaseContext>(
                opt => opt.UseNpgsql(connectionString));
        }

        private static void RegisterGateways(IServiceCollection services)
        {
            var addressApiUrl = Environment.GetEnvironmentVariable("ADDRESS_URL");
            var addressApiToken = Environment.GetEnvironmentVariable("ADDRESS_API_TOKEN");
            var addressApiXRefToken = Environment.GetEnvironmentVariable("ADDRESS_XREF_API_TOKEN");
            var addressKey = Environment.GetEnvironmentVariable("ADDRESS_KEY");
            var connInfo = new ConnectionInfo
            {
                ClientId = Environment.GetEnvironmentVariable("CLIENT_ID"),
                UserPoolId = Environment.GetEnvironmentVariable("POOL_ID"),
                NotifyKey = Environment.GetEnvironmentVariable("NOTIFY_KEY")
            };
            services.AddTransient<IAuthenticateGateway>(x => new AuthenticateGateway(connInfo));
            services.AddTransient<INotifyGateway>(x => connInfo.NotifyKey == null ? null : new NotifyGateway(connInfo));
            services.AddTransient<IRepositoryGateway>(x => new RepositoryGateway(connInfo));
            services.AddScoped<IUsersGateway, UsersGateway>();
            services.AddScoped<ISessionsGateway, SessionsGateway>();
            services.AddScoped<IServicesGateway, ServicesGateway>();
            services.AddScoped<IOrganisationsGateway, OrganisationsGateway>();
            services.AddScoped<IUserOrganisationGateway, UserOrganisationGateway>();
            services.AddScoped<ITaxonomyGateway, TaxonomyGateway>();
            services.AddScoped<IAnalyticsGateway, AnalyticsGateway>();
            services.AddScoped<ISynonymGroupsGateway, SynonymGroupsGateway>();
            services.AddScoped<ISynonymWordsGateway, SynonymWordsGateway>();
            services.AddScoped<IGoogleClient, GoogleClient>();
            services.AddHttpClient<IAddressSearchGateway, AddressSearchGateway>(a =>
            {
                a.BaseAddress = new Uri(addressApiUrl);
                a.DefaultRequestHeaders.TryAddWithoutValidation("x-api-key", addressKey);
                a.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", addressApiToken);
            });
            services.AddHttpClient<IAddressXRefGateway, AddressXRefGateway>(a =>
            {
                a.BaseAddress = new Uri(addressApiUrl);
                a.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", addressApiXRefToken);
            });
        }

        private static void RegisterUseCases(IServiceCollection services)
        {
            services.AddScoped<IGetAllUsersUseCase, GetAllUsersUseCase>();
            services.AddScoped<IGetUserUseCase, GetUserUseCase>();
            services.AddScoped<ICreateUserRequestUseCase, CreateUserRequestUseCase>();
            services.AddScoped<IConfirmUserUseCase, ConfirmUserUseCase>();
            services.AddScoped<IAuthenticateUseCase, AuthenticateUseCase>();
            services.AddScoped<IUpdateUserRequestUseCase, UpdateUserRequestUseCase>();
            services.AddScoped<IDeleteUserRequestUseCase, DeleteUserRequestUseCase>();
            services.AddScoped<ICreateServiceUseCase, CreateServiceUseCase>();
            services.AddScoped<IGetServicesUseCase, GetServicesUseCase>();
            services.AddScoped<IUpdateServiceUseCase, UpdateServiceUseCase>();
            services.AddScoped<IDeleteServiceUseCase, DeleteServiceUseCase>();
            services.AddScoped<IOrganisationsUseCase, OrganisationsUseCase>();
            services.AddScoped<IGetAddressesUseCase, GetAddressesUseCase>();
            services.AddScoped<IServiceImageUseCase, ServiceImageUseCase>();
            services.AddScoped<IUserOrganisationUseCase, UserOrganisationLinksUseCase>();
            services.AddScoped<ITaxonomyUseCase, TaxonomyUseCase>();
            services.AddScoped<IAnalyticsUseCase, AnalyticsUseCase>();
            services.AddScoped<ISynonymsUseCase, SynonymsUseCase>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Set up the cookie requirements
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                // use http only cookies to mitigate XSS attacks
                HttpOnly = HttpOnlyPolicy.Always,

                // always encrypt cookies with TLS/SSL
                Secure = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always,

                MinimumSameSitePolicy = SameSiteMode.None
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            //Get All ApiVersions,
            var api = app.ApplicationServices.GetService<IApiVersionDescriptionProvider>();
            _apiVersions = api.ApiVersionDescriptions.ToList();

            //Swagger ui to view the swagger.json file
            app.UseSwaggerUI(c =>
            {
                foreach (var apiVersionDescription in _apiVersions)
                {
                    //Create a swagger endpoint for each swagger version
                    c.SwaggerEndpoint($"{apiVersionDescription.GetFormattedApiVersion()}/swagger.json",
                        $"{ApiName}-api {apiVersionDescription.GetFormattedApiVersion()}");
                }
            });
            app.UseSwagger();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithOrigins(Environment.GetEnvironmentVariable("ALLOWED_ORIGINS") == null ? "localhost".Split()
                    : Environment.GetEnvironmentVariable("ALLOWED_ORIGINS").Split(',')));
            app.UseEndpoints(endpoints =>
        {
            // SwaggerGen won't find controllers that are routed via this technique.
            endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
        });

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<DatabaseContext>();
                context.Database.EnsureCreated();

            }
        }
    }
}
