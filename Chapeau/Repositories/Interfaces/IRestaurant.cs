using Chapeau.Models.Enums;
using Chapeau.ViewModels;

namespace Chapeau.Repositories.Interfaces
{
    public interface IRestaurantRepo
    {
        List<RestaurantViewModel> GetRestaurantOverview(OrderStatus? status);
    }
}
