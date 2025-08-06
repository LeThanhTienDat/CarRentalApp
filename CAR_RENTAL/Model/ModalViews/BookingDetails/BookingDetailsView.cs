using CAR_RENTAL.Model.ModalViews.Car;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAR_RENTAL.Model.ModalViews.BookingDetails
{
    public class BookingDetailsView
    {
        public int ID { get; set; }
        public int? BookingId { get; set; }
        public int? CarId { get; set; }
        public string BookingDetailsStatus { get; set; }
        public DateTime? BookingDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ActualReturnDate { get; set; }
        public decimal? PricePerCar { get; set; }
        public decimal? Fine { get; set; }
        public decimal? Refund { get; set; }
        public int? StatusReturn { get; set; }
        //Car
        public string LicensePlate { get; set; }
        public string Model { get; set; }
        public int? SeatCount { get; set; }
        public decimal? Total { get; set; }
        public CarView CarView { get; set; }
    }
}
