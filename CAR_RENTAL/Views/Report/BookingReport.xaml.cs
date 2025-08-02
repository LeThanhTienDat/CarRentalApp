using CAR_RENTAL.Model.Repositories;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CAR_RENTAL.Views.Report
{
    /// <summary>
    /// Interaction logic for BookingReport.xaml
    /// </summary>
    public partial class BookingReport : UserControl
    {
        public BookingReport()
        {
            InitializeComponent();
            for (int i = 1; i <= 12; i++)
            {
                ComboBoxItem month = new ComboBoxItem();
                month.Content = "Tháng " + i.ToString();
                month.Tag = i;
                inputChoseMonthRevenue.Items.Add(month);
                inputChoseMonthRevenue.SelectedIndex = 0;
            }

            for (int i = 1; i <= 12; i++)
            {
                ComboBoxItem month = new ComboBoxItem();
                month.Content = "Tháng " + i.ToString();
                month.Tag = i;
                inputChoseMonthBooking.Items.Add(month);
                inputChoseMonthBooking.SelectedIndex = 0;
            }
            LoadbookingReport();
        }
        public void Reload(object sender, RoutedEventArgs e)
        {
            LoadbookingReport();
        }

        private void LoadbookingReport()
        {
            
            
            DateTime toDay = DateTime.Now;

            //Daily Revenue
            if (!chosenDailyRevenue.SelectedDate.HasValue)
            {
                chosenDailyRevenue.SelectedDate = toDay;
            }
            var totalDailyRevenue = BookingRepository.Instance.GetDailyRevenue(chosenDailyRevenue.SelectedDate.Value);
            if(totalDailyRevenue == 0)
            {
                showDailyRevenue.Text = "0 VND";
            }
            else
            {
                showDailyRevenue.Text = totalDailyRevenue.ToString() + " VND";
            }

            //Monthly Revenue
            ComboBoxItem revenueMonthChose = inputChoseMonthRevenue.SelectedItem as ComboBoxItem;
            if(revenueMonthChose != null)
            {
                var tag = Convert.ToInt32(revenueMonthChose.Tag);

                var totalMonthlyRevenue = BookingRepository.Instance.GetMonthlyRevenue(tag);
                if(totalMonthlyRevenue == 0)
                {
                    showMonthlyRevenue.Text = " 0 VND";
                }
                else
                {
                    showMonthlyRevenue.Text = totalMonthlyRevenue.ToString() + " VND";
                }
            }

            //Daily Booking
            if (!chosenDailyBooking.SelectedDate.HasValue)
            {
                chosenDailyBooking.SelectedDate = toDay;
            }
            var totalDailyBooking = BookingRepository.Instance.CountDailyBooking(chosenDailyBooking.SelectedDate.Value);
            showDailyBooking.Text = totalDailyBooking.ToString();

            //Monthly Booking
            ComboBoxItem bookingMonthlyChose = inputChoseMonthBooking.SelectedItem as ComboBoxItem;
            if(bookingMonthlyChose != null)
            {
                var tag = Convert.ToInt32(bookingMonthlyChose.Tag);

                var totalMonthlyBooking = BookingRepository.Instance.CountMonthlyBookig(tag);
                showMonthlyBooking.Text = totalMonthlyBooking.ToString();
            }

            //Booking Canceled
            var bookingCanceled = BookingRepository.Instance.CountBookingCanceled();
            showBookingCanceled.Text = bookingCanceled.ToString();
            
            //Booking processing
            var bookingProcessing = BookingRepository.Instance.CountBookingProcessing();
            showBookingProcessing.Text = bookingProcessing.ToString();

            //Booking Completed
            var bookingCompleted = BookingRepository.Instance.CountBookingCompleted();
            showBookingCompleted.Text = bookingCompleted.ToString();
        }
    }
}
