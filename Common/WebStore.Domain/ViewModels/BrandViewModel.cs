using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.ViewModels
{
    public class BrandViewModel : INamedEntity, IOrderedEntity
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }
    }
}
