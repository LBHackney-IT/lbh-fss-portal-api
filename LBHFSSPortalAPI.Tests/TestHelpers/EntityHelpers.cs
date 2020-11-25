using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Infrastructure;

namespace LBHFSSPortalAPI.Tests.TestHelpers
{
    public static class EntityHelpers
    {
        const int _count = 3;
        public static Service CreateService()
        {
            var service = Randomm.Build<Service>()
                .Without(s => s.Id)
                .With(s => s.Organisation, CreateOrganisation())
                .With(s => s.Image, CreateFile)
                .With(s => s.ServiceTaxonomies, CreateServiceTaxonomies(1))
                .With(s => s.ServiceLocations, CreateServiceLocations())
                .Create();
            return service;
        }

        public static User CreateUser()
        {
            return Randomm.Build<User>().Without(u => u.Id).Create();
        }

        public static File CreateFile()
        {
            return Randomm.Build<File>().Without(f => f.Id).Create();
        }

        public static Role CreateRole(string roleName)
        {
            var role = Randomm.Build<Role>()
                .Without(r => r.Id)
                .Create();
            if (!string.IsNullOrWhiteSpace(roleName))
            {
                role.Name = roleName;
            }

            return role;
        }

        public static SynonymGroup CreateSynonymGroup()
        {
            var synonymGroup = Randomm.Build<SynonymGroup>()
                .Without(s => s.Id)
                .Create();
            synonymGroup.SynonymWords = new List<SynonymWord>();
            return synonymGroup;
        }

        public static SynonymWord CreateSynonymWord()
        {
            return Randomm.Build<SynonymWord>()
                .Without(s => s.Id)
                .With(s => s.Group, CreateSynonymGroup())
                .Create();
        }
        public static Organisation CreateOrganisation()
        {
            var organisation = Randomm.Build<Organisation>()
                .Without(o => o.Id)
                .With(o => o.ReviewerU, CreateUser())
                .Create();
            return organisation;
        }

        public static OrganisationRequest CreatePostOrganisation()
        {
            var organisation = Randomm.Build<OrganisationRequest>()
                .Without(o => o.ReviewerId)
                .Create();
            return organisation;
        }

        public static ServiceLocation CreateServiceLocation()
        {
            var serviceLocation = Randomm.Build<ServiceLocation>()
                .Without(sl => sl.Id)
                .With(sl => sl.Latitude, (decimal) Randomm.Latitude())
                .With(sl => sl.Longitude, (decimal) Randomm.Longitude())
                .With(sl => sl.Service, Randomm.Build<Service>()
                    .Without(s => s.Id)
                    .With(s => s.Organisation, CreateOrganisation())
                    .With(s => s.Image, CreateFile)
                    .Create()
                )
                .Create();
            return serviceLocation;
        }

        public static Session CreateSession(string role)
        {
            var user = Randomm.Build<User>()
                .Without(u => u.Id)
                .Without(u => u.UserRoles)
                .Create();
            user.UserRoles = new List<UserRole>();
            user.UserRoles.Add(CreateUserRole(user, role));
            var session = Randomm.Build<Session>()
                .Without(o => o.Id)
                .With(o => o.User, user)
                .Create();
            session.LastAccessAt = DateTime.Now;
            return session;
        }

        public static Taxonomy CreateTaxonomy()
        {
            var taxonomy = Randomm.Build<Taxonomy>()
                .Without(t => t.Id)
                .Create();
            return taxonomy;
        }

        public static UserOrganisation CreateUserOrganisation()
        {
            var userOrganisation = Randomm.Build<UserOrganisation>()
                .Without(uo => uo.Id)
                .With(uo => uo.User, CreateUser())
                .With(uo => uo.Organisation, CreateOrganisation())
                .Create();
            return userOrganisation;
        }

        public static ServiceTaxonomy CreateServiceTaxonomy()
        {
            var serviceTaxonomy = Randomm.Build<ServiceTaxonomy>()
                .Without(st => st.Id)
                .With(st => st.Service, Randomm.Build<Service>().Without(s => s.Id)
                    .With(s => s.Organisation, CreateOrganisation())
                    .With(s => s.Image, CreateFile)
                    .Create())
                .With(st => st.Taxonomy, Randomm.Build<Taxonomy>().Without(t => t.Id).Create())
                .Create();
            return serviceTaxonomy;
        }

        public static ICollection<Taxonomy> CreateTaxonomies(int count = _count)
        {
            var taxonomies = new List<Taxonomy>();
            for (var a = 0; a < count; a++)
            {
                taxonomies.Add(CreateTaxonomy());
            }

            return taxonomies;
        }

        public static ICollection<Service> CreateServices(int count = _count)
        {
            var services = new List<Service>();
            for (var a = 0; a < count; a++)
            {
                services.Add(CreateService());
            }
            return services;
        }

        public static ICollection<ServiceTaxonomy> CreateServiceTaxonomies(int count = _count)
        {
            var serviceTaxonomies = new List<ServiceTaxonomy>();
            for (var a = 0; a < count; a++)
            {
                serviceTaxonomies.Add(CreateServiceTaxonomy());
            }
            return serviceTaxonomies;
        }

        public static ICollection<ServiceLocation> CreateServiceLocations(int count = _count)
        {
            var serviceLocations = new List<ServiceLocation>();
            for (var a = 0; a < count; a++)
            {
                serviceLocations.Add(CreateServiceLocation());
            }
            return serviceLocations;
        }

        public static ICollection<Organisation> CreateOrganisations(int count = _count)
        {
            var organisations = new List<Organisation>();
            for (var a = 0; a < count; a++)
            {
                organisations.Add(CreateOrganisation());
            }
            return organisations;
        }

        public static SynonymGroup CreateSynonymGroupWithWords(int count = 3)
        {
            var synonymGroup = CreateSynonymGroup();
            for (var a = 0; a < count; a++)
            {
                var synomymWord = new SynonymWord { GroupId = synonymGroup.Id, Word = Randomm.Create<string>() };
                synonymGroup.SynonymWords.Add(synomymWord);
            }
            return synonymGroup;
        }


        public static UserRole CreateUserRole(User user, string role)
        {
            var userRole = Randomm.Build<UserRole>()
                .Without(ur => ur.Id)
                .With(ur => ur.User, user)
                .With(ur => ur.Role, CreateRole(role))
                .Create();
            return userRole;
        }
    }
}
