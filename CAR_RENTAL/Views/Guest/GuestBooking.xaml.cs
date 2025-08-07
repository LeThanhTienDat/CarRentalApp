using CAR_RENTAL.Model.ModalViews.Booking;
using CAR_RENTAL.Model.ModalViews.BookingDetails;
using CAR_RENTAL.Model.ModalViews.Car;
using CAR_RENTAL.Model.ModalViews.Customer;
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

namespace CAR_RENTAL.Views.Guest
{
    /// <summary>
    /// Interaction logic for GuestBooking.xaml
    /// </summary>
    public partial class GuestBooking : UserControl
    {
        private CustomerView GuestInfo;
        public GuestBooking(CustomerView guest)
        {
            InitializeComponent();
            GuestInfo = guest;
            LoadBookingList("Waiting");
        }

        private void ReloadBooking(object sender, RoutedEventArgs e)
        {
            try
            {
                if (GuestInfo != null)
                {
                    ComboBoxItem selected = ChoseTypeBooking.SelectedItem as ComboBoxItem;
                    string type = selected.Tag.ToString();
                    LoadBookingList(type);                  
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }


        private void LoadBookingList(string bookingStatus)
        {
            try
            {
                if(Bookings != null)
                {
                    Bookings.RowDefinitions.Clear();
                    Bookings.Children.Clear();
                }
                if(GuestInfo.BookingViews == null)
                {
                    MessageBox.Show("Null ne");
                }
                var BList = GuestInfo.BookingViews;
                var index = 0;
                foreach (var item in BList)
                {
                    if(item.BookingStatus == bookingStatus)
                    {
                        

                        RowDefinition rowDefinition = new RowDefinition();
                        rowDefinition.Height = new GridLength(25);
                        Bookings.RowDefinitions.Add(rowDefinition);
                        BrushConverter bc = new BrushConverter();
                        Brush rowBackground = Brushes.White;
                        if(item.BookingStatus == "Waiting")
                        {
                            rowBackground = (Brush)bc.ConvertFromString("#DFFFD6");
                        }else if(item.BookingStatus == "Completed")
                        {
                            rowBackground = (Brush)bc.ConvertFromString("#D3D3D3");
                        }
                        else
                        {
                            rowBackground = (Brush)bc.ConvertFromString("#FFC0CB");
                        }

                        //Column 1 (No.)
                        Border brItemNo = new Border();
                        TextBlock tbItemNo = new TextBlock();
                        tbItemNo.Text = (index +1).ToString();
                        tbItemNo.VerticalAlignment = VerticalAlignment.Center;
                        tbItemNo.TextAlignment = TextAlignment.Center;
                        brItemNo.Child = tbItemNo;
                        brItemNo.Background = rowBackground;
                        brItemNo.Height = 25;
                        Grid.SetColumn(brItemNo, 0);
                        Grid.SetRow(brItemNo, index);
                        Bookings.Children.Add(brItemNo);

                        //Column 2(Booking ID)
                        Border brItemId = new Border();
                        TextBlock tbItemId = new TextBlock();
                        tbItemId.Text = item.ID.ToString();
                        tbItemId.VerticalAlignment = VerticalAlignment.Center;
                        tbItemId.TextAlignment = TextAlignment.Center;
                        brItemId.Child = tbItemId;
                        brItemId.Background = rowBackground;
                        brItemId.Height = 25;
                        Grid.SetColumn(brItemId, 1);
                        Grid.SetRow(brItemId, index);
                        Bookings.Children.Add(brItemId);

                        //Column 3 (Booking Date)
                        Border brItemBookingDate = new Border();
                        TextBlock tbItemBookingDate = new TextBlock();
                        tbItemBookingDate.Text = item.OrderDate.ToString();
                        tbItemBookingDate.VerticalAlignment = VerticalAlignment.Center;
                        tbItemBookingDate.TextAlignment = TextAlignment.Center; 
                        brItemBookingDate.Child = tbItemBookingDate;
                        brItemBookingDate.Height = 25;
                        brItemBookingDate.Background = rowBackground;
                        Grid.SetColumn(brItemBookingDate, 2);
                        Grid.SetRow(brItemBookingDate, index);
                        Bookings.Children.Add(brItemBookingDate);

                        //Column 4 (CarAmout)
                        Border brItemCarAmount = new Border();
                        TextBlock tbItemCarAmount = new TextBlock();
                        if(item.BookingDetailsView != null)
                        {
                            tbItemCarAmount.Text = item.BookingDetailsView.Count().ToString();
                        }
                        else
                        {
                            tbItemCarAmount.Text = "0";
                        }
                        tbItemCarAmount.VerticalAlignment = VerticalAlignment.Center;
                        tbItemCarAmount.TextAlignment = TextAlignment.Center;
                        brItemCarAmount.Child = tbItemCarAmount;
                        brItemCarAmount.Height= 25;
                        brItemCarAmount.Background = rowBackground;
                        Grid.SetColumn(brItemCarAmount, 3);
                        Grid.SetRow(brItemCarAmount, index);
                        Bookings.Children.Add(brItemCarAmount);

                        //Column 5 (Status)
                        Border brItemStatus = new Border();
                        TextBlock tbItemStatus = new TextBlock();
                        tbItemStatus.Text = item.BookingStatus;
                        tbItemStatus.VerticalAlignment = VerticalAlignment.Center;
                        tbItemStatus.TextAlignment = TextAlignment.Center;
                        brItemStatus.Child = tbItemStatus;
                        brItemStatus.Height = 25;
                        brItemStatus.Background = rowBackground;
                        Grid.SetColumn(brItemStatus, 4);
                        Grid.SetRow(brItemStatus, index);
                        Bookings.Children.Add(brItemStatus);

                        //Column 6 (Action)
                        Border brItemAction = new Border();  
                        Button btnItemDetail = new Button();
                        btnItemDetail.Content = "View Detail";
                        btnItemDetail.VerticalAlignment = VerticalAlignment.Center;
                        btnItemDetail.HorizontalAlignment = HorizontalAlignment.Center;
                        btnItemDetail.Width = 120;
                        btnItemDetail.FontSize = 16;
                        btnItemDetail.Click += (object sender, RoutedEventArgs e) =>
                        {
                            var cars = item.BookingDetailsView;
                            LoadCars(cars);
                            decimal? countFine = 0;
                            decimal? countRefund = 0;
                            foreach (var i in item.BookingDetailsView)
                            {                                                             
                                if (i.Fine != null)
                                {
                                    countFine += i.Fine;
                                }
                                if (i.Refund != null)
                                {
                                    countRefund += i.Refund;
                                }
                            }
                            showBookingId.Text = item.ID.ToString();
                            showBookingStatus.Text = item.BookingStatus;
                            showDiscount.Text = item.Discount != 0 && item.Discount != null ? item.Discount.ToString() + " %" : "0 %";
                            showDeposit.Text = item.Deposit != 0 && item.Deposit != null ? item.Deposit.ToString() + " %" : "0 %";
                            showDepositCash.Text = item.DepositCash != 0 && item.DepositCash != null ? item.DepositCash.ToString()+ " VND" : "0 VND";
                            showTotalPrice.Text = item.TotalPrice != 0 ? item.TotalPrice.ToString() + " VND" : "0 VND";
                            decimal? remainCash = item.TotalPrice - (item.DepositCash == null ? 0 : item.DepositCash);
                            showRemainCash.Text = remainCash != 0 ? remainCash.ToString() + " VND" : "0 VND";
                            decimal? mustPay = remainCash + countFine - countRefund;
                            showMustPay.Text = mustPay != 0 ? mustPay.ToString() + " VND" : "0 VND";
                            showFine.Text = countFine != 0 ? countFine.ToString() + " VND" : "0 VND";
                            showRefund.Text = countRefund != 0 ? countRefund.ToString() + " VND" : "0 VND";
                            showPaymentConfirm.Text = item.PaymentConfirm == 1 ? "Yes" : "No";

                        };
                        brItemAction.Child = btnItemDetail;
                        brItemAction.Background = rowBackground;
                        Grid.SetColumn(brItemAction, 5);
                        Grid.SetRow(brItemAction, index);
                        Bookings.Children.Add(brItemAction);

                        
                        index++;
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private void LoadCars(HashSet<BookingDetailsView> bookingDetails)
        {
            try
            {
                if(Cars != null)
                {
                    Cars.Children.Clear();
                    Cars.RowDefinitions.Clear();
                }
                if(bookingDetails != null)
                {
                    var index = 0;
                    var items = bookingDetails;
                    foreach(var item in items)
                    {
                        RowDefinition rowDefinition = new RowDefinition();
                        rowDefinition.Height = new GridLength(100);
                        Cars.RowDefinitions.Add(rowDefinition);

                        BrushConverter bc = new BrushConverter();
                        Brush rowBackground = Brushes.White;
                        if (item.BookingDetailsStatus == "Hiring")
                        {
                            rowBackground = (Brush)bc.ConvertFromString("#DFFFD6");
                        }
                        else if (item.BookingDetailsStatus == "Returned")
                        {
                            rowBackground = (Brush)bc.ConvertFromString("#D3D3D3");
                        }
                        
                        //Column 1 (No.)
                        Border brItemNo = new Border();
                        TextBlock tbItemNo = new TextBlock();
                        tbItemNo.Text = (index + 1).ToString();
                        tbItemNo.VerticalAlignment = VerticalAlignment.Center;
                        tbItemNo.TextAlignment = TextAlignment.Center;
                        brItemNo.Child = tbItemNo;
                        brItemNo.Height = 100;
                        brItemNo.Width = 66;
                        brItemNo.VerticalAlignment= VerticalAlignment.Center;
                        brItemNo.HorizontalAlignment = HorizontalAlignment.Center;
                        brItemNo.Background = rowBackground;
                        Grid.SetColumn(brItemNo, 0);
                        Grid.SetRow(brItemNo, index);
                        Cars.Children.Add(brItemNo);

                        //Column 2 (Image)
                        Border brItemImage = new Border();
                        Image img = new Image();
                        string imagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", item.CarView.Image);
                        img.Source = new BitmapImage(new Uri(imagePath));
                        img.Height = 100;
                        img.Width = 150;
                        brItemImage.Child = img;
                        brItemImage.VerticalAlignment = VerticalAlignment.Center;
                        brItemImage.HorizontalAlignment = HorizontalAlignment.Center;
                        brItemImage.Background = rowBackground;
                        Grid.SetColumn(brItemImage, 1);
                        Grid.SetRow(brItemImage, index);
                        Cars.Children.Add(brItemImage);

                        //Column 3 (Brand)
                        Border brItemBrand = new Border();
                        TextBlock tbItemBrand = new TextBlock();
                        tbItemBrand.Text = item.CarView.Brand;
                        tbItemBrand.VerticalAlignment = VerticalAlignment.Center;
                        tbItemBrand.TextAlignment = TextAlignment.Center;
                        brItemBrand.Child = tbItemBrand;
                        brItemBrand.Height = 100;
                        brItemBrand.Width = 110;
                        brItemBrand.Background = rowBackground;
                        Grid.SetColumn(brItemBrand, 2);
                        Grid.SetRow(brItemBrand, index);
                        Cars.Children.Add(brItemBrand);


                        //Column 4 (Model);
                        Border brItemModel = new Border();
                        TextBlock tbItemModel = new TextBlock();
                        tbItemModel.Text = item.CarView.Model;
                        tbItemModel.VerticalAlignment = VerticalAlignment.Center;
                        tbItemModel.TextAlignment = TextAlignment.Center;
                        tbItemModel.TextWrapping = TextWrapping.Wrap;
                        brItemModel.Child = tbItemModel;
                        brItemModel.Height = 100;
                        brItemModel.Width = 104;
                        brItemModel.Background = rowBackground;
                        Grid.SetColumn(brItemModel, 3);
                        Grid.SetRow(brItemModel, index);
                        Cars.Children.Add(brItemModel);

                        //Column 5 (Licence Plate)
                        Border brItemLicense = new Border();
                        TextBlock tbItemLicense = new TextBlock();
                        tbItemLicense.Text = item.CarView.LicensePlate;
                        tbItemLicense.VerticalAlignment = VerticalAlignment.Center;
                        tbItemLicense.TextAlignment = TextAlignment.Center;
                        brItemLicense.Child = tbItemLicense;
                        brItemLicense.Height = 100;
                        brItemLicense.Width = 119;
                        brItemLicense.Background = rowBackground;
                        Grid.SetColumn(brItemLicense, 4);
                        Grid.SetRow(brItemLicense, index);
                        Cars.Children.Add(brItemLicense);

                        //Column 6 (Seat Count)
                        Border brItemSeat = new Border();
                        TextBlock tbItemSeat = new TextBlock();
                        tbItemSeat.Text = item.CarView.SeatCount.ToString();
                        tbItemSeat.VerticalAlignment = VerticalAlignment.Center;
                        tbItemSeat.TextAlignment = TextAlignment.Center;
                        brItemSeat.Child = tbItemSeat;
                        brItemSeat.Height = 100;
                        brItemSeat.Width = 94;
                        brItemSeat.Background = rowBackground;
                        Grid.SetColumn(brItemSeat, 5);
                        Grid.SetRow(brItemSeat, index);
                        Cars.Children.Add(brItemSeat);

                        //Column 7 (Color)
                        Border brItemColor = new Border();
                        TextBlock tbItemColor = new TextBlock();
                        tbItemColor.Text = item.CarView.Color;
                        tbItemColor.VerticalAlignment = VerticalAlignment.Center;
                        tbItemColor.TextAlignment = TextAlignment.Center;
                        brItemColor.Child = tbItemColor;
                        brItemColor.Height = 100;
                        brItemColor.Width = 74;
                        brItemColor.Background = rowBackground;
                        Grid.SetColumn(brItemColor, 6);
                        Grid.SetRow(brItemColor, index);
                        Cars.Children.Add(brItemColor);

                        //Column 7 (Price per Day)
                        Border brItemPriceDay = new Border();
                        TextBlock tbItemPriceDay = new TextBlock();
                        tbItemPriceDay.Text = item.CarView.PricePerDay.ToString();
                        tbItemPriceDay.VerticalAlignment = VerticalAlignment.Center;
                        tbItemPriceDay.TextAlignment = TextAlignment.Center;
                        brItemPriceDay.Child = tbItemPriceDay;
                        brItemPriceDay.Height = 100;
                        brItemPriceDay.Width = 130;
                        brItemPriceDay.Background = rowBackground;
                        Grid.SetColumn(brItemPriceDay, 7);
                        Grid.SetRow(brItemPriceDay, index);
                        Cars.Children.Add(brItemPriceDay);

                        //Column 8 (Start Date)
                        Border brItemStartDate = new Border();
                        TextBlock tbItemStartDate = new TextBlock();
                        tbItemStartDate.Text = item.StartDate.ToString();
                        tbItemStartDate.VerticalAlignment = VerticalAlignment.Center;
                        tbItemStartDate.TextAlignment = TextAlignment.Center;
                        tbItemStartDate.TextWrapping = TextWrapping.Wrap;
                        tbItemStartDate.Width = 107;
                        brItemStartDate.Child = tbItemStartDate;
                        brItemStartDate.Height = 100;
                        brItemStartDate.Width = 114;
                        brItemStartDate.Background = rowBackground;
                        Grid.SetColumn(brItemStartDate, 8);
                        Grid.SetRow(brItemStartDate, index);
                        Cars.Children.Add(brItemStartDate);

                        //Column 9 (End Date)
                        Border brItemEndDate = new Border();
                        TextBlock tbItemEndDate = new TextBlock();
                        tbItemEndDate.Text = item.EndDate.ToString();
                        tbItemEndDate.VerticalAlignment = VerticalAlignment.Center;
                        tbItemEndDate.TextAlignment = TextAlignment.Center;
                        tbItemEndDate.TextWrapping = TextWrapping.Wrap;
                        tbItemEndDate.Width = 107;
                        brItemEndDate.Child = tbItemEndDate;
                        brItemEndDate.Height = 100;
                        brItemEndDate.Width = 113;
                        brItemEndDate.Background = rowBackground;
                        Grid.SetColumn(brItemEndDate, 9);
                        Grid.SetRow(brItemEndDate, index);
                        Cars.Children.Add(brItemEndDate);

                        //Column 10 (Total Day)
                        DateTime start = item.StartDate.Value;
                        DateTime end = item.EndDate.Value;
                        int total = (end - start).Days +1;
                        Border brItemTotalDay = new Border();
                        TextBlock tbItemTotalDay = new TextBlock();
                        tbItemTotalDay.Text = total.ToString();
                        tbItemTotalDay.VerticalAlignment = VerticalAlignment.Center;
                        tbItemTotalDay.TextAlignment = TextAlignment.Center;
                        brItemTotalDay.Child = tbItemTotalDay;
                        brItemTotalDay.Height = 100;
                        brItemTotalDay.Width = 110;
                        brItemTotalDay.Background = rowBackground;
                        Grid.SetColumn(brItemTotalDay, 10);
                        Grid.SetRow(brItemTotalDay, index);
                        Cars.Children.Add(brItemTotalDay);

                        //Column 11 (Total Price)
                        Border brItemPriceCar = new Border();
                        TextBlock tbItemPriceCar = new TextBlock();
                        tbItemPriceCar.Text = item.PricePerCar.ToString();
                        tbItemPriceCar.VerticalAlignment = VerticalAlignment.Center;
                        tbItemPriceCar.TextAlignment = TextAlignment.Center;
                        brItemPriceCar.Child = tbItemPriceCar;
                        brItemPriceCar.Height = 100;
                        brItemPriceCar.Width = 175;
                        brItemPriceCar.Background= rowBackground;
                        Grid.SetColumn(brItemPriceCar, 11);
                        Grid.SetRow(brItemPriceCar, index);
                        Cars.Children.Add(brItemPriceCar);
                      
                        index++;
                    }
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}
