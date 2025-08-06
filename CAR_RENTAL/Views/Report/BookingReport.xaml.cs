using CAR_RENTAL.Model.ModalViews.Booking;
using CAR_RENTAL.Model.ModalViews.BookingDetails;
using CAR_RENTAL.Model.Repositories;
using CAR_RENTAL.Views.Booking;
using CAR_RENTAL.Views.Car;
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
            DateTime toDay = DateTime.Now;
            for (int i = 1; i <= 12; i++)
            {
                ComboBoxItem month = new ComboBoxItem();
                month.Content = "Tháng " + i.ToString();
                month.Tag = i;
                inputChoseMonthRevenue.Items.Add(month);
                inputChoseMonthRevenue.SelectedIndex = toDay.Month -1;
            }

            for (int i = 1; i <= 12; i++)
            {
                ComboBoxItem month = new ComboBoxItem();
                month.Content = "Tháng " + i.ToString();
                month.Tag = i;
                inputChoseMonthBooking.Items.Add(month);
                inputChoseMonthBooking.SelectedIndex = toDay.Month-1;
            }
            LoadbookingReport();
            LoadBookings();
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
        public void ChangeTypeBooking(object sender, RoutedEventArgs e)
        {
            try
            {
                ComboBoxItem selectedItem = choseTypeBooking.SelectedItem as ComboBoxItem;
                string type = selectedItem.Content.ToString();                
                if(completeOrCancelDate == null)
                {
                    return;
                }
                else
                {
                    completeOrCancelDate.Children.Clear();
                    completeOrCancelDate.RowDefinitions.Clear();                    
                }
                if(reasonCancel == null)
                {
                    return;
                }
                else
                {
                    reasonCancel.Children.Clear();
                    reasonCancel.RowDefinitions.Clear();
                }
                LoadBookings(type);
                
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private void LoadBookings(string filter = null)
        {
            if(Bookings == null)
            {
                return;
            }
            else
            {
                Bookings.RowDefinitions.Clear();
                Bookings.Children.Clear();
            }
                HashSet<BookingView> items = new HashSet<BookingView>();
            if(filter == null)
            {
                items = BookingRepository.Instance.FindBookingByStatus("Waiting");
            }
            else
            {
                items = BookingRepository.Instance.FindBookingByStatus(filter);
            }
            
            if (items.Count > 0)
            {
                
                var index = 0;

                foreach (var item in items)
                {
                    RowDefinition rowDefinition = new RowDefinition();
                    rowDefinition.Height = new GridLength(25);
                    BrushConverter bc = new BrushConverter();
                    Brush rowBackground = Brushes.White;
                    if (item.BookingStatus == "Waiting")
                        rowBackground = (Brush)bc.ConvertFromString("#DFFFD6");
                    else if (item.BookingStatus == "Completed")
                        rowBackground = (Brush)bc.ConvertFromString("#D3D3D3");
                    else if (item.BookingStatus == "Canceled")
                        rowBackground = (Brush)bc.ConvertFromString("#FFC0CB");
                    Bookings.RowDefinitions.Add(rowDefinition);

                    //Column 1 (No.)
                    Border brItemNo = new Border();
                    TextBlock tbItemNo = new TextBlock();
                    tbItemNo.Text = (index + 1).ToString();
                    tbItemNo.TextAlignment = TextAlignment.Center;
                    tbItemNo.VerticalAlignment = VerticalAlignment.Center;
                    brItemNo.Child = tbItemNo;
                    brItemNo.Height = 25;
                    brItemNo.Background = rowBackground;
                    Grid.SetColumn(brItemNo, 0);
                    Grid.SetRow(brItemNo, index);
                    Bookings.Children.Add(brItemNo);

                    //Column 2 (Booking ID)
                    Border brItemBookingId = new Border();
                    TextBlock tbItemBookingId = new TextBlock();
                    tbItemBookingId.Text = item.ID.ToString();
                    tbItemBookingId.VerticalAlignment = VerticalAlignment.Center;
                    tbItemBookingId.TextAlignment = TextAlignment.Center;
                    brItemBookingId.Child = tbItemBookingId;
                    brItemBookingId.Height = 25;
                    brItemBookingId.Background = rowBackground;
                    Grid.SetColumn(brItemBookingId, 1);
                    Grid.SetRow(brItemBookingId, index);
                    Bookings.Children.Add(brItemBookingId);

                    //Column 3 (Name)
                    Border brItemName = new Border();
                    TextBlock tbItemName = new TextBlock();
                    tbItemName.Text = item.CustomerView.Name;
                    tbItemName.TextAlignment = TextAlignment.Center;
                    tbItemName.VerticalAlignment = VerticalAlignment.Center;
                    brItemName.Child = tbItemName;
                    brItemName.Height = 25;
                    brItemName.Background = rowBackground;
                    Grid.SetColumn(brItemName, 2);
                    Grid.SetRow(brItemName, index);
                    Bookings.Children.Add(brItemName);

                    //Column 4 (Id Card Number)
                    Border brItemIdCard = new Border();
                    TextBlock tbItemIdCard = new TextBlock();
                    tbItemIdCard.Text = item.CustomerView.CusIdCard;
                    tbItemIdCard.VerticalAlignment = VerticalAlignment.Center;
                    tbItemIdCard.TextAlignment = TextAlignment.Center;
                    brItemIdCard.Child = tbItemIdCard;
                    brItemIdCard.Height = 25;
                    brItemIdCard.Background = rowBackground;
                    Grid.SetColumn(brItemIdCard, 3);
                    Grid.SetRow(brItemIdCard, index);
                    Bookings.Children.Add(brItemIdCard);

                    //Column 5(Phone)
                    Border brItemPhone = new Border();
                    TextBlock tbItemPhone = new TextBlock();
                    tbItemPhone.Text = item.CustomerView.Phone;
                    tbItemPhone.VerticalAlignment = VerticalAlignment.Center;
                    tbItemPhone.TextAlignment = TextAlignment.Center;
                    brItemPhone.Child = tbItemPhone;
                    brItemPhone.Height = 25;
                    brItemPhone.Background = rowBackground;
                    Grid.SetColumn(brItemPhone, 4);
                    Grid.SetRow(brItemPhone, index);
                    Bookings.Children.Add(brItemPhone);

                    //Column 6 (Car amount)
                    var carNumber = BookingRepository.Instance.CheckTotalCar(item.ID);
                    Border brItemCarAmount = new Border();
                    TextBlock tbItemCarAmount = new TextBlock();
                    tbItemCarAmount.Text = carNumber.ToString();
                    tbItemCarAmount.TextAlignment = TextAlignment.Center;
                    tbItemCarAmount.VerticalAlignment = VerticalAlignment.Center;
                    brItemCarAmount.Child = tbItemCarAmount;
                    brItemCarAmount.Height = 25;
                    brItemCarAmount.Background = rowBackground;
                    Grid.SetColumn(brItemCarAmount, 5);
                    Grid.SetRow(brItemCarAmount, index);
                    Bookings.Children.Add(brItemCarAmount);

                    //Column 7 (Booking date)
                    Border brItemBookingDate = new Border();
                    TextBlock tbItemBookingDate = new TextBlock();
                    tbItemBookingDate.Text = item.OrderDate.ToString();
                    tbItemBookingDate.TextAlignment = TextAlignment.Center;
                    tbItemBookingDate.VerticalAlignment = VerticalAlignment.Center;
                    brItemBookingDate.Child = tbItemBookingDate;
                    brItemBookingDate.Height = 25;
                    brItemBookingDate.Background = rowBackground;
                    Grid.SetColumn(brItemBookingDate, 6);
                    Grid.SetRow(brItemBookingDate, index);
                    Bookings.Children.Add(brItemBookingDate);

                    //Column 8(Booking status)
                    Border brItemBookingStatus = new Border();
                    TextBlock tbItemBookingStatus = new TextBlock();
                    tbItemBookingStatus.Text = item.BookingStatus;
                    tbItemBookingStatus.VerticalAlignment = VerticalAlignment.Center;
                    tbItemBookingStatus.TextAlignment = TextAlignment.Center;
                    brItemBookingStatus.Child = tbItemBookingStatus;
                    brItemBookingStatus.Height = 25;
                    brItemBookingStatus.Background = rowBackground;
                    Grid.SetColumn(brItemBookingStatus, 7);
                    Grid.SetRow(brItemBookingStatus, index);
                    Bookings.Children.Add(brItemBookingStatus);

                    //Column 9 (Date complete / Date canceled)
                    if (item.BookingStatus == "Completed")
                    {
                        Label lbItemCompleted = new Label();
                        lbItemCompleted.Content = "Complete Date";
                        lbItemCompleted.Width = 142;
                        lbItemCompleted.VerticalAlignment = VerticalAlignment.Center;
                        lbItemCompleted.HorizontalContentAlignment = HorizontalAlignment.Center;
                        Grid.SetColumn(lbItemCompleted, 0);
                        Grid.SetRow(lbItemCompleted, 0);
                        completeOrCancelDate.Children.Add(lbItemCompleted);
                        Border brItemCompleted = new Border();
                        TextBlock tbItemCompleted = new TextBlock();
                        tbItemCompleted.Text = item.CompletedDate.ToString();
                        tbItemCompleted.VerticalAlignment = VerticalAlignment.Center;
                        tbItemCompleted.TextAlignment = TextAlignment.Center;
                        brItemCompleted.Child = tbItemCompleted;
                        brItemCompleted.Height = 25;
                        brItemCompleted.Background = rowBackground;
                        Grid.SetColumn(brItemCompleted, 8);
                        Grid.SetRow(brItemCompleted, index);
                        Bookings.Children.Add(brItemCompleted);
                    }
                    else if (item.BookingStatus == "Canceled")
                    {
                        Label lbItemCanceled = new Label();
                        lbItemCanceled.Content = "Cancel Date";
                        lbItemCanceled.Width = 142;
                        lbItemCanceled.VerticalAlignment = VerticalAlignment.Center;  
                        lbItemCanceled.HorizontalContentAlignment = HorizontalAlignment.Center;
                        Grid.SetColumn(lbItemCanceled, 0);
                        Grid.SetRow(lbItemCanceled, 0);
                        completeOrCancelDate.Children.Add(lbItemCanceled);
                        Border brItemCanceled = new Border();
                        TextBlock tbItemCanceled = new TextBlock();
                        tbItemCanceled.Text = item.CancelDate.ToString();
                        tbItemCanceled.VerticalAlignment = VerticalAlignment.Center;
                        tbItemCanceled.TextAlignment = TextAlignment.Center;
                        brItemCanceled.Child = tbItemCanceled;
                        brItemCanceled.Height = 25;
                        brItemCanceled.Background = rowBackground;
                        Grid.SetColumn(brItemCanceled, 8);
                        Grid.SetRow(brItemCanceled, index);
                        Bookings.Children.Add(brItemCanceled);
                    }

                    //Column 10 (Reason Cancel)
                    if (item.ReasonCancel != null)
                    {
                        Label lbItemReason = new Label();
                        lbItemReason.Content = "Reason Cancel";
                        lbItemReason.Width = 414;
                        lbItemReason.VerticalAlignment = VerticalAlignment.Center;
                        lbItemReason.HorizontalContentAlignment = HorizontalAlignment.Center;  
                        Grid.SetColumn(lbItemReason, 0);
                        Grid.SetRow(lbItemReason, 0);
                        reasonCancel.Children.Add(lbItemReason);
                        Border brItemReason = new Border();
                        TextBlock tbItemReason = new TextBlock();
                        tbItemReason.Text = item.ReasonCancel;
                        tbItemReason.VerticalAlignment = VerticalAlignment.Center;
                        tbItemReason.TextAlignment = TextAlignment.Center;
                        brItemReason.Child = tbItemReason;
                        brItemReason.Height = 25;
                        brItemReason.Background = rowBackground;
                        Grid.SetColumn(brItemReason, 9);
                        Grid.SetRow(brItemReason, index);
                        Bookings.Children.Add(brItemReason);
                    }

                    index++;
                }
            }
        }

    }
}
