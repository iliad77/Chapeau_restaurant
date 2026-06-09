using Chapeau.Models.Enums;
using Chapeau.Repositories.Interfaces;
using Chapeau.Services.Interfaces;
using Chapeau.ViewModels;

namespace Chapeau.Services
{
    public class RestaurantSevice:IRestaurantService
    {
        private readonly IRestaurantRepo _resRepo;
        public RestaurantSevice(IRestaurantRepo resRepo) 
        {
            _resRepo = resRepo;
        }

        public List<RestaurantViewModel> GetRestaurantOverview(OrderStatus? status)
        {
            return _resRepo.GetRestaurantOverview(status);
        }
    }
}
