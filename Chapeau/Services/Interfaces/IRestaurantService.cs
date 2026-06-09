using Chapeau.Models.Enums;
using Chapeau.ViewModels;

namespace Chapeau.Services.Interfaces
{
    public interface IRestaurantService
    {
        List<RestaurantViewModel> GetRestaurantOverview(OrderStatus? status);
    }
}
