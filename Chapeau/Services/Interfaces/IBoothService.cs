using Chapeau.Models;

namespace Chapeau.Services.Interfaces
{
    public interface IBoothService
    {
        List<Booth> GetAllBooth();
        Booth GetBooth(int id);

        void createBooth(Booth booth);
        void updateBooth(Booth booth);
        void deleteBooth(int id);
    }
}
