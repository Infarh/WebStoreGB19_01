using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.DomainEntities.Entities.Base;
using WebStore.DomainEntities.Entities.Base.Interfaces;

namespace WebStore.DomainEntities.Entities
{
    /// <summary>Бренд</summary>
    public class Brand : NamedEntity, IOrderedEntity
    {
        public int Order { get; set; }
    }
}
