using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities
{
    /// <summary>Товар</summary>
    [Table("Products")]
    public class Product : NamedEntity, IOrderedEntity
    {
        public int Order { get; set; }

        public int SectionId { get; set; }

        [ForeignKey(nameof(SectionId))]
        public virtual Section Section { get; set; }

        public int BrendId { get; set; }

        [ForeignKey(nameof(BrendId))]
        public virtual Brand Brand { get; set; }

        //[Column("ProductPrice")]
        public decimal Price { get; set; }

        [NotMapped]
        public string Test { get; set; }

        public string ImageUrl { get; set; }
    }
}
