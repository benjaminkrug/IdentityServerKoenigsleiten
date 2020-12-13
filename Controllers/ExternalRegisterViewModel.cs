namespace IdentityServer.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ExternalRegisterViewModel
    {
        public string Username { get; set; }
        public string ReturnUrl { get; set; }
    }
}
