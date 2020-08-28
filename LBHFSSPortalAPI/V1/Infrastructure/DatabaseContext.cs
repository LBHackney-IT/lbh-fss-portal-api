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

        public DbSet<DatabaseEntity> DatabaseEntities { get; set; }

        public virtual DbSet<Organizations> Organizations { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<ServiceLocations> ServiceLocations { get; set; }
        public virtual DbSet<ServiceRevisions> ServiceRevisions { get; set; }
        public virtual DbSet<ServiceTaxonomies> ServiceTaxonomies { get; set; }
        public virtual DbSet<Services> Services { get; set; }
        public virtual DbSet<Sessions> Sessions { get; set; }
        public virtual DbSet<SynonymGroups> SynonymGroups { get; set; }
        public virtual DbSet<SynonymWords> SynonymWords { get; set; }
        public virtual DbSet<Taxonomies> Taxonomies { get; set; }
        public virtual DbSet<UserOrganizations> UserOrganizations { get; set; }
        public virtual DbSet<UserRoles> UserRoles { get; set; }
        public virtual DbSet<Users> Users { get; set; }

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
            modelBuilder.Entity<Organizations>(entity =>
            {
                entity.ToTable("organizations");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.ToTable("roles");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<ServiceLocations>(entity =>
            {
                entity.ToTable("service_locations");

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

                entity.Property(e => e.RevisionId).HasColumnName("revision_id");

                entity.Property(e => e.StateProvince)
                    .HasColumnName("state_province")
                    .HasColumnType("character varying");

                entity.Property(e => e.Uprn).HasColumnName("uprn");

                entity.HasOne(d => d.Revision)
                    .WithMany(p => p.ServiceLocations)
                    .HasForeignKey(d => d.RevisionId)
                    .HasConstraintName("service_locations_revision_id_fkey");
            });

            modelBuilder.Entity<ServiceRevisions>(entity =>
            {
                entity.ToTable("service_revisions");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AuthorId).HasColumnName("author_id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("character varying");

                entity.Property(e => e.Facebook)
                    .HasColumnName("facebook")
                    .HasColumnType("character varying");

                entity.Property(e => e.Instagram)
                    .HasColumnName("instagram")
                    .HasColumnType("character varying");

                entity.Property(e => e.Linkedin)
                    .HasColumnName("linkedin")
                    .HasColumnType("character varying");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("character varying");

                entity.Property(e => e.ReviewedAt).HasColumnName("reviewed_at");

                entity.Property(e => e.ReviewerMessage).HasColumnName("reviewer_message");

                entity.Property(e => e.ReviewerUid).HasColumnName("reviewer_uid");

                entity.Property(e => e.ServiceId).HasColumnName("service_id");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("character varying");

                entity.Property(e => e.SubmittedAt).HasColumnName("submitted_at");

                entity.Property(e => e.Telephone)
                    .HasColumnName("telephone")
                    .HasColumnType("character varying");

                entity.Property(e => e.Twitter)
                    .HasColumnName("twitter")
                    .HasColumnType("character varying");

                entity.Property(e => e.Website)
                    .HasColumnName("website")
                    .HasColumnType("character varying");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.ServiceRevisionsAuthor)
                    .HasForeignKey(d => d.AuthorId)
                    .HasConstraintName("service_revisions_author_id_fkey");

                entity.HasOne(d => d.ReviewerU)
                    .WithMany(p => p.ServiceRevisionsReviewerU)
                    .HasForeignKey(d => d.ReviewerUid)
                    .HasConstraintName("service_revisions_reviewer_uid_fkey");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.ServiceRevisions)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("service_revisions_service_id_fkey");
            });

            modelBuilder.Entity<ServiceTaxonomies>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("service_taxonomies");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.RevisionId).HasColumnName("revision_id");

                entity.Property(e => e.TaxonomyId).HasColumnName("taxonomy_id");

                entity.HasOne(d => d.Revision)
                    .WithMany()
                    .HasForeignKey(d => d.RevisionId)
                    .HasConstraintName("service_taxonomies_revision_id_fkey");

                entity.HasOne(d => d.Taxonomy)
                    .WithMany()
                    .HasForeignKey(d => d.TaxonomyId)
                    .HasConstraintName("service_taxonomies_taxonomy_id_fkey");
            });

            modelBuilder.Entity<Services>(entity =>
            {
                entity.ToTable("services");

                entity.HasIndex(e => e.RevisionId)
                    .HasName("services_revision_id_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.OrganizationId).HasColumnName("organization_id");

                entity.Property(e => e.RevisionId).HasColumnName("revision_id");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.Services)
                    .HasForeignKey(d => d.OrganizationId)
                    .HasConstraintName("services_organization_id_fkey");

                entity.HasOne(d => d.Revision)
                    .WithOne(p => p.Services)
                    .HasForeignKey<Services>(d => d.RevisionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("services_revision_id_fkey");
            });

            modelBuilder.Entity<Sessions>(entity =>
            {
                entity.ToTable("sessions");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.SessionId)
                    .HasColumnName("session_id")
                    .HasColumnType("character varying");

                entity.Property(e => e.IpAddress)
                    .HasColumnName("ip_address")
                    .HasColumnType("character varying");

                entity.Property(e => e.LastAccessAt).HasColumnName("last_access_at");

                entity.Property(e => e.Payload).HasColumnName("payload");

                entity.Property(e => e.UserAgent).HasColumnName("user_agent");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Sessions)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("sessions_user_id_fkey");
            });

            modelBuilder.Entity<SynonymGroups>(entity =>
            {
                entity.ToTable("synonym_groups");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<SynonymWords>(entity =>
            {
                entity.ToTable("synonym_words");

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

            modelBuilder.Entity<Taxonomies>(entity =>
            {
                entity.ToTable("taxonomies");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("character varying");

                entity.Property(e => e.ParentId).HasColumnName("parent_id");

                entity.Property(e => e.Vocabulary)
                    .HasColumnName("vocabulary")
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<UserOrganizations>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("user_organizations");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.OrganizationId).HasColumnName("organization_id");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Id)
                    .HasConstraintName("user_organizations_id_fkey");

                entity.HasOne(d => d.Organization)
                    .WithMany()
                    .HasForeignKey(d => d.OrganizationId)
                    .HasConstraintName("user_organizations_organization_id_fkey");
            });

            modelBuilder.Entity<UserRoles>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("user_roles");

                entity.HasIndex(e => new { e.Id, e.RoleId })
                    .HasName("user_roles_id_role_id_idx");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Id)
                    .HasConstraintName("user_roles_id_fkey");

                entity.HasOne(d => d.Role)
                    .WithMany()
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("user_roles_role_id_fkey");
            });

            modelBuilder.Entity<Users>(entity =>
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

            modelBuilder.Entity<Users>().HasData(
                new Users
                {
                    Id = 1,
                    Name = "Jane Doe",
                    SubId = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.Parse("18/08/2018 11:22:16", cultureInfo),
                    Email = "jane.doe@blueyonder.co.uk",
                    Status = "active"
                });

            modelBuilder.Entity<Users>().HasData(
                new Users
                {
                    Id = 2,
                    Name = "Mark Williams",
                    SubId = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.Parse("11/03/2019 15:42:55", cultureInfo),
                    Email = "mark.williams@bighouse.org",
                    Status = "active"
                });

            modelBuilder.Entity<Users>().HasData(
                new Users
                {
                    Id = 3,
                    Name = "Janet Graham",
                    SubId = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.Parse("30/04/2020 14:36:32", cultureInfo),
                    Email = "janet.graham@grcdentists.co.uk",
                    Status = "unverified"
                });

            modelBuilder.Entity<Users>().HasData(
                new Users
                {
                    Id = 4,
                    Name = "Ronnie O'Sullivan",
                    SubId = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.Parse("11/08/2020 08:28:46", cultureInfo),
                    Email = "ronnie.osullivan@onefourseven.com",
                    Status = "verified"
                });

            modelBuilder.Entity<Users>().HasData(
                new Users
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
