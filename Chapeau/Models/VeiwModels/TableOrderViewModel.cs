using Chapeau.Models.Enums;

namespace Chapeau.Models.VeiwModels
{
    public class TableOrderViewModel
    {
        public int BoothId { get; set; }

        public List<GroupedItemViewModel> GroupedItems { get; set; } = new List<GroupedItemViewModel>();

        public decimal TotalVatLow { get; set; }
        public decimal TotalVatHigh { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class GroupedItemViewModel
    {
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }


        public decimal SubTotal => Quantity * UnitPrice;
    }
    public class ProcessPaymentViewModel
    {
        public int BoothId { get; set; }
        public decimal CurrentTotal { get; set; } 

        public decimal? AmountPaid { get; set; }
        public decimal? TipAmount { get; set; }

        public PaymentMethod Method { get; set; }
        public string Feedback { get; set; }
    }
}

