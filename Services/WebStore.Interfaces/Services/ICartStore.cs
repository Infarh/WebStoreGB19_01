using WebStore.Domain.ViewModels;

namespace WebStore.Interfaces.Services
{
    public interface ICartStore
    {
        Cart Cart { get; set; }
    }
}
