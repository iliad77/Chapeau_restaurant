using Chapeau.Models;
using Chapeau.Models.Enums;
using Chapeau.Models.VeiwModels;
using Chapeau.Repositories.Interfaces;
namespace Chapeau.Services
{
    public class BillService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IBillRepository _billRepository;
        public BillService(IOrderRepository orderRepository, IBillRepository billRepository)
        {
            _orderRepository = orderRepository;
            _billRepository = billRepository;
        }



        private Order FetchActiveOrder(int boothId)
        {
            Booth currentTable = new Booth { booth_Id = boothId };
            return _orderRepository.GetActiveOrderWithItems(currentTable);
        }
        public TableOrderViewModel GetTableOrderOverview(int boothId)
        {
            Order fullyLoadedOrder = FetchActiveOrder(boothId);

            if (fullyLoadedOrder == null) return null;

            Bill calculatedBill = fullyLoadedOrder.CalculateBillTotals();

            List<GroupedItemViewModel> formattedItems = new List<GroupedItemViewModel>();

            foreach (OrderItem item in calculatedBill.Items)
            {
                GroupedItemViewModel lineItem = new GroupedItemViewModel
                {
                    ItemName = item.MenuItem.Name,
                    Quantity = item.Quantity,
                    UnitPrice = item.MenuItem.Price
                };
                formattedItems.Add(lineItem);
            }

            return new TableOrderViewModel
            {
                BoothId = boothId,
                TotalVatLow = calculatedBill.nonAlcohol_vat,
                TotalVatHigh = calculatedBill.Alcholic_vat,
                TotalPrice = calculatedBill.Totall_price,
                GroupedItems = formattedItems
            };
        }


        public Bill ProcessCheckout(ProcessPaymentViewModel paymentData)
        {
            Booth currentTable = new Booth { booth_Id = paymentData.BoothId };
            Order activeOrder = FetchActiveOrder(paymentData.BoothId);

            if (activeOrder == null) throw new Exception("No active order found.");

            Bill finalBill = activeOrder.CalculateBillTotals();


            finalBill.ProcessPayment(paymentData.TipAmount, paymentData.AmountPaid, paymentData.Feedback);


            activeOrder.Status = OrderStatus.Finished;
            currentTable.booth_status = 0;


            _billRepository.AddBill(finalBill);
            _billRepository.MarkOrderAsFinished(activeOrder);
            _billRepository.FreeUpBooth(currentTable);

            return finalBill;
        }
    }
}

    
