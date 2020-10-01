using System.Collections.Generic;
using System.Globalization;

namespace LBHFSSPortalAPI.V1.Validations
{
    public static class UserRoleValidator
    {
        private static readonly List<string> _validUserRoles = new List<string> { "VSCO", "Viewer", "Admin" };

        public static List<string> ToValidList(List<string> roles)
        {
            var validatedRoles = new List<string>();

            if (roles != null)
            {
                foreach (var role in roles)
                {
                    foreach (var validRole in _validUserRoles)
                    {
                        if (role.Trim().ToLower(CultureInfo.CurrentCulture) == validRole.ToLower(CultureInfo.CurrentCulture))
                        {
                            validatedRoles.Add(validRole);
                        }
                    }
                }
            }

            return validatedRoles;
        }
    }
}
