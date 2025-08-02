using CAR_RENTAL.Model.ModalViews.Booking;
using CAR_RENTAL.Model.ModalViews.BookingDetails;
using CAR_RENTAL.Model.ModalViews.Category;
using CAR_RENTAL.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for BookingComplete.xaml
    /// </summary>
    public partial class BookingComplete : Window
    {
        public BookingComplete(int booking_id)
        {
            InitializeComponent();
            LoadBookingComplete(booking_id);
        }
        public void LoadBookingComplete(int booking_id)
        {
            var item = BookingRepository.Instance.FindByBookingId(booking_id);
            
            if(item != null)
            {
                showName.Text = item.CustomerView.Name;
                showIdCardNumber.Text = item.CustomerView.CusIdCard;
                showCarAmount.Text = item.BookingDetailsView.Count.ToString();
                int countLate = 0;
                int countSoon = 0;
                int countOnTime = 0;
                decimal? countFind = 0;
                decimal? countRefund = 0;
                foreach (var i in item.BookingDetailsView)
                {
                    DateTime endDate = i.EndDate.Value;
                    DateTime actualReturn = i.ActualReturnDate.Value;
                    int checkDays = (actualReturn - endDate).Days;
                    if(checkDays > 0)
                    {
                        countLate++;
                    }else if(checkDays < 0)
                    {
                        countSoon++;
                    }
                    else
                    {
                        countOnTime++;
                    }
                    if(i.Refund != null)
                    {
                        countFind += i.Fine;
                    }
                    if(i.Refund!= null)
                    {
                        countRefund += i.Refund;
                    }
                }
                showCarReturnLate.Text = countLate.ToString();
                showCarReturnSoon.Text = countSoon.ToString();
                showCarReturnOnTime.Text = countOnTime.ToString();
                showBookingId.Text = item.ID.ToString();
                showBookingDate.Text = item.OrderDate.ToString();
                showDiscount.Text = item.Discount.ToString()+ " %";
                showDeposit.Text = item.Deposit.ToString()+ " %";
                showDepositCash.Text = item.DepositCash.ToString()+ " VND";
                showDepositCashHasPaid.IsChecked = item.DepositHasPaid == 1 ? true : false;
                showTotalPrice.Text = item.TotalPrice.ToString() + " VND";
                showFine.Text = countFind.ToString() +" VND";
                showRefund.Text = countRefund.ToString() + " VND";
                decimal? remainCash = item.TotalPrice - item.DepositCash;
                showRemainCash.Text = remainCash.ToString() + " VND";              

                var listCars = BookingDetailsRepository.Instance.FindByBookingId(item.ID);               
                LoadCars(listCars);

            }
            
        }
        private void LoadCars(HashSet<BookingDetailsView> items)
        {
            
            
            
            if (items.Count > 0)
            {
                var index = 0;
                foreach (var item in items)
                {
                    //MessageBox.Show(item.LicensePlate.ToString());
                    RowDefinition rowDefinition = new RowDefinition();
                    rowDefinition.Height = new GridLength(50);
                    Cars.RowDefinitions.Add(rowDefinition);
                    BrushConverter bc = new BrushConverter();
                    Brush rowBackground = Brushes.White;
                    DateTime endDate = item.EndDate.Value;
                    DateTime actualReturn = item.ActualReturnDate.Value;
                    int checkDays = (actualReturn - endDate).Days;
                    if (checkDays > 0)
                    {
                        rowBackground = (Brush)bc.ConvertFromString("#FFC0CB");
                    }
                    else if (checkDays < 0)
                    {
                        rowBackground = (Brush)bc.ConvertFromString("#FFFFE0");
                    }
                    else
                    {
                        rowBackground = (Brush)bc.ConvertFromString("#DFFFD6");
                    }
                    
                    //Column 1 (License plate)
                    Border brItemLicense = new Border();
                    TextBlock tbItemLicense = new TextBlock();
                    tbItemLicense.Text = item.LicensePlate;
                    tbItemLicense.TextAlignment = TextAlignment.Center;
                    tbItemLicense.VerticalAlignment = VerticalAlignment.Center;
                    brItemLicense.Child = tbItemLicense;
                    brItemLicense.Height = 50;
                    brItemLicense.Width = 90;
                    brItemLicense.Background = rowBackground;
                    Grid.SetColumn(brItemLicense, 0);
                    Grid.SetRow(brItemLicense, index);
                    Cars.Children.Add(brItemLicense);

                    //Column 2 (Model)
                    Border brItemModel = new Border();
                    TextBlock tbItemModel = new TextBlock();
                    tbItemModel.Text = item.Model;
                    tbItemModel.TextAlignment = TextAlignment.Center;
                    tbItemModel.VerticalAlignment = VerticalAlignment.Center;
                    tbItemModel.TextWrapping = TextWrapping.Wrap;
                    brItemModel.Child = tbItemModel;
                    brItemModel.Height = 50;
                    brItemModel.Width = 130;
                    brItemModel.Background = rowBackground;
                    Grid.SetColumn(brItemModel, 1);
                    Grid.SetRow(brItemModel, index);
                    Cars.Children.Add(brItemModel);

                    //Column 3 (Seat count)
                    Border brItemSeat = new Border();
                    TextBlock tbItemSeat = new TextBlock();
                    tbItemSeat.Text = item.SeatCount.ToString();
                    tbItemSeat.TextAlignment = TextAlignment.Center;
                    tbItemSeat.VerticalAlignment = VerticalAlignment.Center;
                    brItemSeat.Child = tbItemSeat;
                    brItemSeat.Height = 50;
                    brItemSeat.Width = 70;
                    brItemSeat.Background = rowBackground;
                    Grid.SetColumn(brItemSeat, 2);
                    Grid.SetRow(brItemSeat, index);
                    Cars.Children.Add(brItemSeat);

                    //Column 4 (Start Date)
                    Border brItemStartDate = new Border();
                    TextBlock tbItemStartDate = new TextBlock();
                    tbItemStartDate.Text = item.StartDate.ToString();
                    tbItemStartDate.TextAlignment = TextAlignment.Center;
                    tbItemStartDate.VerticalAlignment = VerticalAlignment.Center;
                    tbItemStartDate.TextWrapping = TextWrapping.Wrap;
                    brItemStartDate.Child = tbItemStartDate;
                    brItemStartDate.Height = 50;
                    brItemStartDate.Width = 100;
                    brItemStartDate.Background = rowBackground;
                    Grid.SetColumn(brItemStartDate, 3);
                    Grid.SetRow(brItemStartDate, index);
                    Cars.Children.Add(brItemStartDate);

                    //Column 5 (End Date)
                    Border brItemEndDate = new Border();
                    TextBlock tbItemEndDate = new TextBlock();
                    tbItemEndDate.Text = item.EndDate.ToString();
                    tbItemEndDate.VerticalAlignment = VerticalAlignment.Center;
                    tbItemEndDate.TextAlignment = TextAlignment.Center;
                    tbItemEndDate.TextWrapping = TextWrapping.Wrap;
                    brItemEndDate.Child = tbItemEndDate;
                    brItemEndDate.Height = 50;
                    brItemEndDate.Width = 100;
                    brItemEndDate.Background = rowBackground;
                    Grid.SetColumn(brItemEndDate, 4);
                    Grid.SetRow(brItemEndDate, index);
                    Cars.Children.Add(brItemEndDate);

                    //Column 6 (Return date)
                    Border brItemReturnDate = new Border();
                    TextBlock tbItemReturnDate = new TextBlock();
                    tbItemReturnDate.Text = item.ActualReturnDate == null ? "On using" : item.ActualReturnDate.ToString();
                    tbItemReturnDate.TextAlignment = TextAlignment.Center;
                    tbItemReturnDate.VerticalAlignment = VerticalAlignment.Center;
                    tbItemReturnDate.TextWrapping = TextWrapping.Wrap;
                    brItemReturnDate.Child = tbItemReturnDate;
                    brItemReturnDate.Height = 50;
                    brItemReturnDate.Width = 100;
                    brItemReturnDate.Background = rowBackground;
                    Grid.SetColumn(brItemReturnDate, 5);
                    Grid.SetRow(brItemReturnDate, index);
                    Cars.Children.Add(brItemReturnDate);

                    //Column 7 (Status Return)
                    Border brItemStatusReturn = new Border();
                    TextBlock tbItemStatusReturn = new TextBlock();
                    tbItemStatusReturn.Text = item.StatusReturn == 1 ? "Yes" : "No";
                    tbItemStatusReturn.TextAlignment = TextAlignment.Center;
                    tbItemStatusReturn.VerticalAlignment = VerticalAlignment.Center;
                    brItemStatusReturn.Child = tbItemStatusReturn;
                    brItemStatusReturn.Height = 50;
                    brItemStatusReturn.Width = 80;
                    brItemStatusReturn.Background = rowBackground;
                    Grid.SetColumn(brItemStatusReturn, 6);
                    Grid.SetRow(brItemStatusReturn, index);
                    Cars.Children.Add(brItemStatusReturn);

                    //Column 8 (Total Price)
                    Border brItemTotalPrice = new Border();
                    TextBlock tbItemTotalPrice = new TextBlock();
                    tbItemTotalPrice.Text = item.Total.ToString() + " VNĐ";
                    tbItemTotalPrice.TextAlignment = TextAlignment.Center;
                    tbItemTotalPrice.VerticalAlignment = VerticalAlignment.Center;
                    brItemTotalPrice.Child = tbItemTotalPrice;
                    brItemTotalPrice.Height = 50;
                    brItemTotalPrice.Width = 127;
                    brItemTotalPrice.Background = rowBackground;
                    Grid.SetColumn(brItemTotalPrice, 7);
                    Grid.SetRow(brItemTotalPrice, index);
                    Cars.Children.Add(brItemTotalPrice);

                    //Column 9 (Fine)
                    Border brItemFine = new Border();
                    TextBlock tbItemFine = new TextBlock();
                    tbItemFine.Text = item.Fine.ToString() + " VND";
                    tbItemFine.VerticalAlignment = VerticalAlignment.Center;
                    tbItemFine.TextAlignment = TextAlignment.Center;
                    brItemFine.Child = tbItemFine;
                    brItemFine.Height = 50;
                    brItemFine.Width = 115;
                    brItemFine.Background = rowBackground;
                    Grid.SetColumn(brItemFine, 8);
                    Grid.SetRow(brItemFine, index);
                    Cars.Children.Add(brItemFine);

                    //Column 10 (Refund)
                    Border brItemRefund = new Border();
                    TextBlock tbItemRefund = new TextBlock();
                    tbItemRefund.Text = item.Refund.ToString() + " VND";
                    tbItemRefund.VerticalAlignment = VerticalAlignment.Center;
                    tbItemRefund.TextAlignment = TextAlignment.Center;
                    brItemRefund.Child = tbItemRefund;
                    brItemRefund.Height = 50;
                    brItemRefund.Width = 126;
                    brItemRefund.Background = rowBackground;
                    Grid.SetColumn(brItemRefund, 9);
                    Grid.SetRow(brItemRefund, index);
                    Cars.Children.Add(brItemRefund);


                    index++;
                
            }
        }
    }

        private void SaveComplete(object sender, RoutedEventArgs e)
        {
            if(inputPaymentConfirm.IsChecked == false)
            {
                MessageBox.Show("Payment confirm doesn't checked, please check all Information and Check Confirm box!");
            }
            else
            {
                var confirm = MessageBox.Show($"Are you sure the Customer paid all Remain Cash ?", "Confirm Delete",
                                                    MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (confirm == MessageBoxResult.Yes)
                {
                    var completedDate = DateTime.Now;
                    BookingView update = new BookingView();
                    update.ID = Convert.ToInt32(showBookingId.Text);
                    update.PaymentConfirm = 1;
                    update.BookingStatus = "Completed";
                    update.CompletedDate = completedDate;
                    var check = BookingRepository.Instance.UpdateConfirmComplete(update);
                    if (check == true)
                    {
                        DialogResult = true;
                        this.Close();
                    }
                    else
                    {
                        DialogResult = false;
                        this.Close();
                    }
                }
            }
        }

        private void CancelComplete(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
