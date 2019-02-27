namespace WebStore.Domain.Entities.Base.Interfaces
{
    /// <summary>Сущность с идентфикатором</summary>
    public interface IBaseEntity
    {
        /// <summary>Идентификатор</summary>
        int Id { get; set; }
    }
}
