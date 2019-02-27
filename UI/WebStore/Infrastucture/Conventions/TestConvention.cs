using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace WebStore.Infrastucture.Conventions
{
    public class TestConvention : IActionModelConvention
    {
        public void Apply(ActionModel action)
        {
            action.Filters.Add(new ValidateAntiForgeryTokenAttribute());
        }
    }
}
