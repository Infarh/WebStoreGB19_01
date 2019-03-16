using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModels.BreadCrumbs;
using WebStore.Interfaces.Services;

namespace WebStore.Components
{
    public class BreadCrumbsViewComponent : ViewComponent
    {
        private readonly IProductData _ProductData;

        public BreadCrumbsViewComponent(IProductData ProductData) => _ProductData = ProductData;

        public IViewComponentResult Invoke(BreadCrumbType type, int id, BreadCrumbType FromType)
        {
            if(!Enum.IsDefined(typeof(BreadCrumbType), type)) throw new InvalidEnumArgumentException(nameof(type), (int)type, typeof(BreadCrumbType));
            if(!Enum.IsDefined(typeof(BreadCrumbType), FromType)) throw new InvalidEnumArgumentException(nameof(FromType), (int)FromType, typeof(BreadCrumbType));

            switch (type)
            {
                case BreadCrumbType.Section:
                    var section = _ProductData.GetSectionById(id);
                    return View(new []
                    {
                        new BreadCrumbsViewModel
                        {
                            BreadCrumbType = type,
                            Id = section.Id.ToString(),
                            Name = section.Name
                        }
                    });
                case BreadCrumbType.Brand:
                    var brand = _ProductData.GetBrandById(id);
                    return View(new[]
                    {
                        new BreadCrumbsViewModel
                        {
                            BreadCrumbType = type,
                            Id =brand.Id.ToString(),
                            Name = brand.Name
                        }
                    });
                case BreadCrumbType.Item:
                    return View(GetItemBreadCrumbs(id, FromType, type));
            }

            return View(new BreadCrumbsViewModel[0]);
        }

        private IEnumerable<BreadCrumbsViewModel> GetItemBreadCrumbs(int id, BreadCrumbType FromType, BreadCrumbType type)
        {
            var item = _ProductData.GetProductById(id);

            var crumbs = new List<BreadCrumbsViewModel>();

            if(FromType == BreadCrumbType.Section)
                crumbs.Add(new BreadCrumbsViewModel
                {
                    BreadCrumbType = FromType,
                    Id = item.Section.Id.ToString(),
                    Name = item.Section.Name
                });
            else
                crumbs.Add(new BreadCrumbsViewModel
                {
                    BreadCrumbType = FromType,
                    Id = item.Brand.Id.ToString(),
                    Name = item.Brand.Name
                });

            crumbs.Add(new BreadCrumbsViewModel
            {
                BreadCrumbType = type,
                Id = item.Id.ToString(),
                Name = item.Name
            });

            return crumbs;
        }
    }
}
