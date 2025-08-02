using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAR_RENTAL.Model.ModalViews.Customer;
using CAR_RENTAL.Model.ModalViews.BookingDetails;

namespace CAR_RENTAL.Model.ModalViews.Booking
{
    internal class BookingView
    {
        public int ID { get; set; }
        public int? CusId { get; set; }
        public string BookingStatus { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Deposit { get; set; }
        public int? PaymentTypeId { get; set; }
        public decimal? TotalPrice { get; set; }
        public string PaymentTypeName { get; set; }
        public decimal? DepositCash { get; set; }
        public int DepositHasPaid { get; set; }
        public int PaymentConfirm {  get; set; }
        public int? Paid {  get; set; }
        public decimal Find { get; set; }
        public decimal Refund { get; set; }
        public int IsCancel { set; get; }
        public string ReasonCancel { set; get; }
        public DateTime? OrderDate { set; get; }
        public DateTime? CompletedDate { set; get; }
        public CustomerView  CustomerView { set; get; }
        public HashSet<BookingDetailsView> BookingDetailsView { set; get; }
        
    }
}
