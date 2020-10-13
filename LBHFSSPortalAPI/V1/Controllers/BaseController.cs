using LBHFSSPortalAPI.V1.Boundary;
using LBHFSSPortalAPI.V1.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LBHFSSPortalAPI.V1.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            ConfigureJsonSerializer();
        }

        protected string AccessToken => Request.Cookies[Cookies.AccessTokenName];

        public static void ConfigureJsonSerializer()
        {
            JsonConvert.DefaultSettings = () =>
            {
                var settings = new JsonSerializerSettings();
                settings.Formatting = Formatting.Indented;
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

                settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                settings.DateFormatHandling = DateFormatHandling.IsoDateFormat;

                return settings;
            };
        }

        public override BadRequestObjectResult BadRequest([ActionResultObjectValue] object error)
        {
            if (error is UseCaseException uce)
                return base.BadRequest(new { uce.UserErrorMessage, uce.DevErrorMessage });

            return base.BadRequest(error);
        }
    }
}
