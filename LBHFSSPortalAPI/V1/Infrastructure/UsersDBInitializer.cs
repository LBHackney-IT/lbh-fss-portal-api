//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Globalization;
//using System.Linq;
//using System.Threading.Tasks;

//namespace LBHFSSPortalAPI.V1.Infrastructure
//{
//    public class UsersDBInitializer : DropCreateDatabaseAlways<LBHFSSPortalAPI.V1.Infrastructure.DatabaseContext>
//    {
//        protected override void Seed(LBHFSSPortalAPI.V1.Infrastructure.DatabaseContext context)
//        {
//            IList<Users> users = new List<Users>();
//            var cultureInfo = new CultureInfo("de-DE");

//            users.Add(new Users()
//            {
//                Name = "Jane Doe",
//                SubId = Guid.NewGuid().ToString(),
//                CreatedAt = DateTime.Parse("08/18/2018 07:22:16", cultureInfo),
//                Email = "",
//                Status = "verified"
//            });

//            context.Users.AddRange(users);

//            base.Seed(context);
//        }
//    }
//}
