using Chapeau.Models;

namespace Chapeau.Repositories.Interfaces
{
    public interface IBillRepository
    {
        List<Bill> GetBills();
        Bill? AddBill(Bill bill);
        void MarkOrderAsFinished(Order orderId);
        void FreeUpBooth(Booth boothId);
        void UpdateBill(Bill user);
        void DeleteBill(int id);
        //public List<OrderItem> GetItemsForOrder(int orderId);
    }
}
