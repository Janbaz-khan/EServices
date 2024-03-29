﻿using System.Web.Mvc;

namespace EServices.Areas.Parent
{
    public class ParentAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Parent";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Parent_default",
                "Parent/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "EServices.Areas.Parent.Controllers" }
            );
        }
    }
}