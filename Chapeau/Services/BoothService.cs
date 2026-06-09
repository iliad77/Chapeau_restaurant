using Chapeau.Models;
using Chapeau.Repositories.Interface;
using Chapeau.Services.Interfaces;

namespace Chapeau.Services
{
    public class BoothService:IBoothService
    {
        private readonly IBoothRepo _boothRepo;

        public BoothService(IBoothRepo boothRepo)
        {
            _boothRepo = boothRepo;
        }
        public List<Booth> GetAllBooth() { return _boothRepo.GetAllBooth(); }
        public Booth GetBooth(int id) { return _boothRepo.GetBooth(id); }

        public void createBooth(Booth booth) { _boothRepo.AddBooth(booth); }
        public void updateBooth(Booth booth) { _boothRepo.UpdateBooth(booth); }
        public void deleteBooth(int id) { _boothRepo.DeleteBooth(id); }
    }
}
