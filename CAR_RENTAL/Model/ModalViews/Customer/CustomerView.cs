using CAR_RENTAL.Model.ModalViews.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAR_RENTAL.Model.ModalViews.Customer
{
    public class CustomerView
    {
        public int ID   { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone {  get; set; }
        public string Address { get; set; }
        public int? CityId { get; set; }
        public int? DistrictId   { get; set; }
        public int? Active   { get; set; }
        public string CusIdCard { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CityName { get; set; }
        public string DistrictName { get; set; }
        public HashSet<BookingView> BookingViews { get; set; }


    }
}
