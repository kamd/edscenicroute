using System;
using System.Collections.Generic;
using System.Text;
using EDScenicRouteCoreModels;

namespace EDScenicRouteWeb.Shared.DataValidation
{
    public static class RouteDetailsValidator
    {

        public const int MAX_NAME_LENGTH = 150;
        public static readonly string[] BAD_CHARACTERS = new []{"\"", "$", "%", "=", "&", ";"};

        public static (bool, string) ValidateRouteDetails(RouteDetails details)
        {
            if (details.FromSystemName.Length > MAX_NAME_LENGTH)
            {
                return (false, $"From Name must be less than {MAX_NAME_LENGTH} characters.");
            }

            if (details.ToSystemName.Length > MAX_NAME_LENGTH)
            {
                return (false, $"To Name must be less than {MAX_NAME_LENGTH} characters.");
            }

            foreach (var c in BAD_CHARACTERS)
            {
                if (details.ToSystemName.Contains(c) ||
                    details.FromSystemName.Contains(c))
                {
                    return (false, $"System names may not contain the {c} character.");
                }
            }

            if (details.AcceptableExtraDistance <= 0f || float.IsNaN(details.AcceptableExtraDistance) ||
                float.IsInfinity(details.AcceptableExtraDistance))
            {
                return (false, $"Acceptable Extra Distance must be a positive number.");
            }

            return (true, null);
        }
    }
}
