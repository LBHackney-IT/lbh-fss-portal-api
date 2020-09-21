using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;

namespace LBHFSSPortalAPI.V1.Infrastructure
{

    public class DatabaseContext : DbContext
    {
        //TODO: rename DatabaseContext to reflect the data source it is representing. eg. MosaicContext.
        //Guidance on the context class can be found here https://github.com/LBHackney-IT/lbh-base-api/wiki/DatabaseContext
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<File> Files { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<ServiceLocation> ServiceLocations { get; set; }
        public virtual DbSet<ServiceTaxonomy> ServiceTaxonomies { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<SynonymGroup> SynonymGroups { get; set; }
        public virtual DbSet<SynonymWord> SynonymWords { get; set; }
        public virtual DbSet<Taxonomy> Taxonomies { get; set; }
        public virtual DbSet<UserOrganization> UserOrganizations { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http: //go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql(
                    "Host=localhost;Database=fss-public_dev;Username=postgres;Password=mypassword");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<File>(entity =>
            {
                entity.ToTable("files");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.Url)
                    .HasColumnName("url")
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<Organization>(entity =>
            {
                entity.ToTable("organizations");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AdultSafeguardingLeadFirstName)
                    .HasColumnName("adult_safeguarding_lead_first_name")
                    .HasColumnType("character varying");

                entity.Property(e => e.AdultSafeguardingLeadLastName)
                    .HasColumnName("adult_safeguarding_lead_last_name")
                    .HasColumnType("character varying");

                entity.Property(e => e.AdultSafeguardingLeadTrainingMonth)
                    .HasColumnName("adult_safeguarding_lead_training_month")
                    .HasColumnType("character varying");

                entity.Property(e => e.AdultSafeguardingLeadTrainingYear)
                    .HasColumnName("adult_safeguarding_lead_training_year")
                    .HasColumnType("character varying");

                entity.Property(e => e.CharityNumber)
                    .HasColumnName("charity_number")
                    .HasColumnType("character varying");

                entity.Property(e => e.ChildSafeguardingLeadFirstName)
                    .HasColumnName("child_safeguarding_lead_first_name")
                    .HasColumnType("character varying");

                entity.Property(e => e.ChildSafeguardingLeadLastName)
                    .HasColumnName("child_safeguarding_lead_last_name")
                    .HasColumnType("character varying");

                entity.Property(e => e.ChildSafeguardingLeadTrainingMonth)
                    .HasColumnName("child_safeguarding_lead_training_month")
                    .HasColumnType("character varying");

                entity.Property(e => e.ChildSafeguardingLeadTrainingYear)
                    .HasColumnName("child_safeguarding_lead_training_year")
                    .HasColumnType("character varying");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.FundingOther).HasColumnName("funding_other");

                entity.Property(e => e.HasAdultSafeguardingLead).HasColumnName("has_adult_safeguarding_lead");

                entity.Property(e => e.HasAdultSupport).HasColumnName("has_adult_support");

                entity.Property(e => e.HasChildSafeguardingLead).HasColumnName("has_child_safeguarding_lead");

                entity.Property(e => e.HasChildSupport).HasColumnName("has_child_support");

                entity.Property(e => e.HasEnhancedSupport).HasColumnName("has_enhanced_support");

                entity.Property(e => e.HasHcOrColGrant).HasColumnName("has_hc_or_col_grant");

                entity.Property(e => e.HasHcvsOrHgOrAelGrant).HasColumnName("has_hcvs_or_hg_or_ael_grant");

                entity.Property(e => e.IsHackneyBased).HasColumnName("is_hackney_based");

                entity.Property(e => e.IsLocalOfferListed).HasColumnName("is_local_offer_listed");

                entity.Property(e => e.IsLotteryFunded).HasColumnName("is_lottery_funded");

                entity.Property(e => e.IsRegisteredCharity).HasColumnName("is_registered_charity");

                entity.Property(e => e.IsTraRegistered).HasColumnName("is_tra_registered");

                entity.Property(e => e.LotteryFundedProject)
                    .HasColumnName("lottery_funded_project")
                    .HasColumnType("character varying");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("character varying");

                entity.Property(e => e.ReviewedAt).HasColumnName("reviewed_at");

                entity.Property(e => e.ReviewerMessage).HasColumnName("reviewer_message");

                entity.Property(e => e.ReviewerUid).HasColumnName("reviewer_uid");

                entity.Property(e => e.RslOrHaAssociation)
                    .HasColumnName("rsl_or_ha_association")
                    .HasColumnType("character varying");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("character varying");

                entity.Property(e => e.SubmittedAt).HasColumnName("submitted_at");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.HasOne(d => d.ReviewerU)
                    .WithMany(p => p.Organizations)
                    .HasForeignKey(d => d.ReviewerUid)
                    .HasConstraintName("organizations_reviewer_uid_fkey");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<ServiceLocation>(entity =>
            {
                entity.ToTable("service_locations");

                entity.HasIndex(e => e.ServiceId)
                    .HasName("IX_service_locations_revision_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address1)
                    .HasColumnName("address_1")
                    .HasColumnType("character varying");

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasColumnType("character varying");

                entity.Property(e => e.Country)
                    .HasColumnName("country")
                    .HasColumnType("character varying");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.Latitude)
                    .HasColumnName("latitude")
                    .HasColumnType("numeric");

                entity.Property(e => e.Longitude)
                    .HasColumnName("longitude")
                    .HasColumnType("numeric");

                entity.Property(e => e.PostalCode)
                    .HasColumnName("postal_code")
                    .HasColumnType("character varying");

                entity.Property(e => e.ServiceId).HasColumnName("service_id");

                entity.Property(e => e.StateProvince)
                    .HasColumnName("state_province")
                    .HasColumnType("character varying");

                entity.Property(e => e.Uprn).HasColumnName("uprn");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.ServiceLocations)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("service_locations_service_id_fkey");
            });

            modelBuilder.Entity<ServiceTaxonomy>(entity =>
            {
                entity.ToTable("service_taxonomies");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.ServiceId).HasColumnName("service_id");

                entity.Property(e => e.TaxonomyId).HasColumnName("taxonomy_id");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.ServiceTaxonomies)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("service_taxonomies_service_id_fkey");

                entity.HasOne(d => d.Taxonomy)
                    .WithMany(p => p.ServiceTaxonomies)
                    .HasForeignKey(d => d.TaxonomyId)
                    .HasConstraintName("service_taxonomies_taxonomy_id_fkey");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("services");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("character varying");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasColumnType("character varying");

                entity.Property(e => e.Facebook)
                    .HasColumnName("facebook")
                    .HasColumnType("character varying");

                entity.Property(e => e.ImageId).HasColumnName("image_id");

                entity.Property(e => e.Instagram)
                    .HasColumnName("instagram")
                    .HasColumnType("character varying");

                entity.Property(e => e.Keywords).HasColumnName("keywords");

                entity.Property(e => e.Linkedin)
                    .HasColumnName("linkedin")
                    .HasColumnType("character varying");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("character varying");

                entity.Property(e => e.OrganizationId).HasColumnName("organization_id");

                entity.Property(e => e.ReferralEmail)
                    .HasColumnName("referral_email")
                    .HasColumnType("character varying");

                entity.Property(e => e.ReferralLink)
                    .HasColumnName("referral_link")
                    .HasColumnType("character varying");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("character varying");

                entity.Property(e => e.Telephone)
                    .HasColumnName("telephone")
                    .HasColumnType("character varying");

                entity.Property(e => e.Twitter)
                    .HasColumnName("twitter")
                    .HasColumnType("character varying");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.Website)
                    .HasColumnName("website")
                    .HasColumnType("character varying");

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.Services)
                    .HasForeignKey(d => d.ImageId)
                    .HasConstraintName("services_image_id_fkey");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.Services)
                    .HasForeignKey(d => d.OrganizationId)
                    .HasConstraintName("services_organization_id_fkey");
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.ToTable("sessions");

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp with time zone");

                entity.Property(e => e.IpAddress)
                    .HasColumnName("ip_address")
                    .HasColumnType("character varying");

                entity.Property(e => e.LastAccessAt)
                    .HasColumnName("last_access_at")
                    .HasColumnType("timestamp with time zone");

                entity.Property(e => e.Payload).HasColumnName("payload");

                entity.Property(e => e.UserAgent).HasColumnName("user_agent");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Sessions)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("sessions_user_id_fkey");
            });

            modelBuilder.Entity<SynonymGroup>(entity =>
            {
                entity.ToTable("synonym_groups");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<SynonymWord>(entity =>
            {
                entity.ToTable("synonym_words");

                entity.HasIndex(e => e.GroupId);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.GroupId).HasColumnName("group_id");

                entity.Property(e => e.Word)
                    .HasColumnName("word")
                    .HasColumnType("character varying");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.SynonymWords)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("synonym_words_group_id_fkey");
            });

            modelBuilder.Entity<Taxonomy>(entity =>
            {
                entity.ToTable("taxonomies");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("character varying");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("character varying");

                entity.Property(e => e.ParentId).HasColumnName("parent_id");

                entity.Property(e => e.Vocabulary)
                    .HasColumnName("vocabulary")
                    .HasColumnType("character varying");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("character varying");

                entity.Property(e => e.Weight).HasColumnName("weight");
            });

            modelBuilder.Entity<UserOrganization>(entity =>
            {
                entity.ToTable("user_organizations");

                entity.HasIndex(e => e.Id);

                entity.HasIndex(e => e.OrganizationId);

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.OrganizationId).HasColumnName("organization_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.UserOrganizations)
                    .HasForeignKey(d => d.OrganizationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_organizations_organization_id_fkey");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserOrganizations)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_organizations_user_id_fkey");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("user_roles");

                entity.HasIndex(e => e.RoleId);

                entity.HasIndex(e => new { e.Id, e.RoleId })
                    .HasName("user_roles_id_role_id_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.UserRoles)
                    .HasForeignKey<UserRole>(d => d.Id)
                    .HasConstraintName("user_roles_id_fkey");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("user_roles_role_id_fkey");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasColumnType("character varying");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("character varying");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("character varying");

                entity.Property(e => e.SubId)
                    .HasColumnName("sub_id")
                    .HasColumnType("character varying");
            });

            SetupSeedData(modelBuilder);
        }

        private static void SetupSeedData(ModelBuilder modelBuilder)
        {
            var cultureInfo = new CultureInfo("en-GB");

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Name = "Jane Doe",
                    SubId = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.Parse("18/08/2018 11:22:16", cultureInfo),
                    Email = "jane.doe@blueyonder.co.uk",
                    Status = "active"
                });

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 2,
                    Name = "Mark Williams",
                    SubId = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.Parse("11/03/2019 15:42:55", cultureInfo),
                    Email = "mark.williams@bighouse.org",
                    Status = "active"
                });

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 3,
                    Name = "Janet Graham",
                    SubId = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.Parse("30/04/2020 14:36:32", cultureInfo),
                    Email = "janet.graham@grcdentists.co.uk",
                    Status = "unverified"
                });

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 4,
                    Name = "Ronnie O'Sullivan",
                    SubId = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.Parse("11/08/2020 08:28:46", cultureInfo),
                    Email = "ronnie.osullivan@onefourseven.com",
                    Status = "verified"
                });

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 5,
                    Name = "Betty Davis",
                    SubId = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.Parse("13/08/2020 11:46:19", cultureInfo),
                    Email = "betty.davis@baesystems.co.uk",
                    Status = "verified"
                });
        }
    }
}
