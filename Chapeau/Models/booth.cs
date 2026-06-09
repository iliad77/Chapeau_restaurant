using Chapeau.Models.Enums;

namespace Chapeau.Models
{
    public class Booth
    {
        public int booth_Id { get; set; }
        public int booth_num { get; set; }
        public int seats { get; set; }
        public BoothStatus booth_status { get; set; }

        public Booth(int booth_id, int booth_num, int seats, BoothStatus booth_status)
        {
            this.booth_Id = booth_id;
            this.booth_num = booth_num;
            this.seats = seats;
            this.booth_status = booth_status;
        }

        public Booth() { }
    }
    
}
