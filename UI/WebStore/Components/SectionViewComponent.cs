using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.Components
{
    public class SectionViewComponent : ViewComponent
    {
        private readonly IProductData _ProductData;

        public SectionViewComponent(IProductData ProductData)
        {
            _ProductData = ProductData;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var sections = GetSections();
            return View(sections);
        }

        private List<SectionViewModel> GetSections()
        {
            var sections = _ProductData.GetSections();
            var parent_sections = sections.Where(section => section.ParentId is null).ToArray();

            var parent_sections_views = parent_sections
                .Select(parent_section => new SectionViewModel
                {
                    Id = parent_section.Id,
                    Name = parent_section.Name,
                    Order = parent_section.Order,
                }).ToList();

            foreach (var parent_sections_view in parent_sections_views)
            {
                var childs = sections.Where(section => section.ParentId == parent_sections_view.Id);
                foreach (var child_section in childs)
                    parent_sections_view.ChildSections.Add(new SectionViewModel
                    {
                        Id = child_section.Id,
                        Name = child_section.Name,
                        Order = child_section.Order,
                        ParentSection = parent_sections_view
                    });
                parent_sections_view.ChildSections.Sort((a, b) => Comparer<int>.Default.Compare(a.Order, b.Order));
            }
            parent_sections_views.Sort((a, b) => Comparer<int>.Default.Compare(a.Order, b.Order));
            return parent_sections_views;
        }
    }
}
