using CAR_RENTAL.Model.ModalViews.Booking;
using CAR_RENTAL.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CAR_RENTAL.Views.Booking
{
    /// <summary>
    /// Interaction logic for ReasonCancel.xaml
    /// </summary>
    public partial class ReasonCancel : Window
    {
        private int BookingId;
        public ReasonCancel(int booking_id)
        {
            InitializeComponent();
            this.BookingId = booking_id;            
        }

        private void StopCancel(object sender, RoutedEventArgs e)
        {
            this.Close();
            DialogResult = false;
        }

        private void ConfirmCancel(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime toDay = DateTime.Now;
                var changeStatus = new BookingView();
                changeStatus.ID = BookingId;
                changeStatus.CancelDate = toDay;
                changeStatus.BookingStatus = "Canceled";
                changeStatus.ReasonCancel = inputReasonCancel.Text;
                var check = BookingRepository.Instance.CancelBooking(changeStatus);
                if (check)
                {
                    DialogResult = true;
                    this.Close();
                }
                else
                {
                    DialogResult= false;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
