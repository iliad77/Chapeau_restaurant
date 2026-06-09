namespace Chapeau.Models
{
    public class Bill
    {
        public int billId { get; set; }
        public Order Order { get; set; }

        public DateTime Date_Time { get; set; }
        public decimal Alcholic_vat { get; set; }
        public decimal nonAlcohol_vat { get; set; }
        public float Tip_Amount { get; set; }
        public decimal Totall_price { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        public Bill() { }
        public Bill(int billId, Order order, DateTime dateTime, decimal alcholic_vat, decimal _nonAlcohol_vat, float _Tip_Amount, decimal _totallA_price, string _Status, string Comment)
        {

            this.billId = billId;
            this.Order = order;
            this.Date_Time = dateTime;
            this.Alcholic_vat = alcholic_vat;
            this.nonAlcohol_vat = _nonAlcohol_vat;
            this.Tip_Amount = _Tip_Amount;
            this.Status = _Status;
            this.Comment = Comment;
            this.Totall_price = _totallA_price;

        }
        public void ProcessPayment(decimal? tipAmount, decimal? amountPaid, string feedback)
        {
            decimal calculatedTip = 0;

            if (tipAmount.HasValue && tipAmount.Value > 0)
            {
                calculatedTip = tipAmount.Value;
            }
            else if (amountPaid.HasValue && amountPaid.Value > this.Totall_price)
            {
                calculatedTip = amountPaid.Value - this.Totall_price;
            }

            this.Tip_Amount = (float)calculatedTip;
            this.Comment = feedback;
            this.Status = "Paid";
        }
    }
}
