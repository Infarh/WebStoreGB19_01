using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.DomainEntities.Entities.Base.Interfaces;

namespace WebStore.Models
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
