using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebStore.Components
{
    //[ViewComponent("")]
    public class LoginLogoutViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => View();
    }
}
