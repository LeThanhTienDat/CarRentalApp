using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAR_RENTAL.Model.ModalViews.Car
{
    public class CarView
    {
        public int ID { get; set; }
        public int? CateId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public decimal PricePerDay { get; set; }
        public string CarStatus { get; set; }
        public string Image { get; set; }
        public string LicensePlate { get; set; }
        public int? SeatCount { get; set; }
        public string Color { get; set; }
        public int? Active {  get; set; }
        public int? CarTypeId { get; set; }
        public string CategoryName { get; set; }
        public string CarTypeName { get; set; }
        public int RentCount { get; set; }
    }
}
