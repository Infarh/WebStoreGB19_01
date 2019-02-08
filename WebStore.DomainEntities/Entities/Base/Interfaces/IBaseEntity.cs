using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.DomainEntities.Entities.Base.Interfaces
{
    /// <summary>Сущность с идентфикатором</summary>
    public interface IBaseEntity
    {
        /// <summary>Идентификатор</summary>
        int Id { get; set; }
    }
}
