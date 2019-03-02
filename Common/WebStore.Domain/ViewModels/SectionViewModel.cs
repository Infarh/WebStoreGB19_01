using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.ViewModels
{
    public class SectionViewModel : INamedEntity, IOrderedEntity
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public List<SectionViewModel> ChildSections { get; set; } = new List<SectionViewModel>();
        public SectionViewModel ParentSection { get; set; }
    }
}
