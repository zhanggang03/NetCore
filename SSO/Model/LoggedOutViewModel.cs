using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Model
{
    public class LoggedOutViewModel
    {
        public string AutomaticRedirectAfterSignOut { get; set; }
        public string PostLogoutRedirectUri { get; set; }
        public string ClientName { get; set; }
        public string SignOutIframeUrl { get; set; }
        public string LogoutId { get; set; }
        public string ExternalAuthenticationScheme { get; set; }

        public bool TriggerExternalSignout = false;
    }
}
