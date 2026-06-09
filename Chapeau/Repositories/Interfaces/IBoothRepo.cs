using Chapeau.Models;

namespace Chapeau.Repositories.Interface
{
    public interface IBoothRepo
    {
        public List<Booth> GetAllBooth();
        public Booth GetBooth(int id);

        public int AddBooth(Booth booth);

        public int UpdateBooth(Booth booth);

        public int DeleteBooth(int id);
    }
}
