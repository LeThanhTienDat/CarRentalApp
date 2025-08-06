using CAR_RENTAL.Model.ModalViews.Booking;
using CAR_RENTAL.Model.ModalViews.Car;
using CAR_RENTAL.Model.ModalViews.Customer;
using CAR_RENTAL.Model.Repositories;
using CAR_RENTAL.Model.ModalViews.BookingDetails;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Globalization;
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
using System.Xml.Linq;

namespace CAR_RENTAL.Views.Booking
{
    /// <summary>
    /// Interaction logic for Booking.xaml
    /// </summary>
    
    public partial class Booking : UserControl
    {
        public int? IdCusChosen = null;
        public int? IdCarChosen = null;
        public int IdBookingReturn = 0;
        public decimal? PricePerDay = null;
        public decimal? PricePerCar = null;
        public int IdBookingDetails = 0;
        

        public Booking()
        {
            InitializeComponent();
            var categories = CategoryRepository.Instance.GetAll();
            ComboBoxItem allItem = new ComboBoxItem();
            allItem.Content = "All";
            allItem.Tag = 0;
            categoryChose.Items.Add(allItem);
            categoryChose.SelectedIndex = 0;
            foreach (var category in categories)
            {
                ComboBoxItem cateItem = new ComboBoxItem();
                cateItem.Content = category.Title;
                cateItem.Tag = category.ID;
                categoryChose.Items.Add(cateItem);              
            }
            
            //LoadCars();
            LoadCustomers();
            LoadPaymentType();
            LoadBookings();
        }
        public void SelectCategoryChange(object sender, RoutedEventArgs e)
        {
            ComboBox categories = sender as ComboBox;
            if (categories != null)
            {
                ComboBoxItem selectedItem = categories.SelectedItem as ComboBoxItem;
                if (selectedItem != null)
                {
                    var tag = selectedItem.Tag;
                    Cars.RowDefinitions.Clear();
                    Cars.Children.Clear();
                    LoadCars("category");
                }
            }
        }
        public void SearchChange(object sender, RoutedEventArgs e)
        {
            Cars.Children.Clear();
            Cars.RowDefinitions.Clear();
            LoadCars("searchname", inputSearchName.Text);
        }
        private void CusIdChange(object sender, TextChangedEventArgs e)
        {
            TextBox cusId = sender as TextBox;
            if (string.IsNullOrWhiteSpace(cusId.Text))
            {
                LoadCustomers();
            }
            else
            {
                string idNumber = cusId.Text.Trim();
                LoadCustomers(idNumber);
            }
        }
        public void LoadPaymentType()
        {
            var items = PaymentTypeRepository.Instance.GetAll();
            foreach (var item in items)
            {
                ComboBoxItem paymentList = new ComboBoxItem();
                paymentList.Tag = item.ID;
                paymentList.Content = item.Name;
                inputPayment.Items.Add(paymentList);
                inputPayment.SelectedIndex = 0;
            }
        }
        public void LoadCustomers(string idNumber = null)
        {
            Customers.RowDefinitions.Clear();
            Customers.Children.Clear();
            var cus = new HashSet<CustomerView>();
            if(idNumber == null)
            {
                cus = CustomerRepository.Instance.GetAll();
            }
            else
            {
                cus = CustomerRepository.Instance.FindByIdNumber(idNumber);
            }

            //Load customer
            if(cus.Count > 0)
            {
                var index = 0;
                foreach (var item in cus)
                {
                    RowDefinition rowDefinition = new RowDefinition();
                    rowDefinition.Height = new GridLength(25);
                    Customers.RowDefinitions.Add(rowDefinition);
                    BrushConverter bc = new BrushConverter();
                    Brush rowBackground = Brushes.White;
                    rowBackground = (Brush)bc.ConvertFromString("#DFFFD6");

                    //Column 1 (ID number)
                    Border brItemId = new Border();
                    TextBlock tbItemId = new TextBlock();
                    tbItemId.Text = item.CusIdCard;
                    tbItemId.VerticalAlignment = VerticalAlignment.Center;
                    tbItemId.TextAlignment = TextAlignment.Center;
                    brItemId.Child = tbItemId;
                    brItemId.Height = 25;
                    Grid.SetColumn(brItemId, 0);
                    Grid.SetRow(brItemId, index);
                    Customers.Children.Add(brItemId);

                    //Column 2(Name)
                    Border brItemName = new Border();
                    TextBlock tbItemName = new TextBlock();
                    tbItemName.Text = item.Name;
                    tbItemName.TextAlignment = TextAlignment.Center;
                    tbItemName.VerticalAlignment = VerticalAlignment.Center;
                    brItemName.Child= tbItemName;
                    brItemName.Height = 25;
                    Grid.SetColumn(brItemName, 1);
                    Grid.SetRow(brItemName, index);
                    Customers.Children.Add(brItemName);

                    //Column 3(Action)
                    Border brItemAction = new Border();
                    Button btnItemAction = new Button();
                    btnItemAction.Content = "Chose";
                    btnItemAction.Click += (object sender, RoutedEventArgs e) =>
                    {                       
                        btnSaveBookingUpdate.IsEnabled = false;
                        btnPlanCar.IsEnabled = false;
                        RemoveCarUpdateInfo();
                        showIdCus.Text = item.ID.ToString();
                        showName.Text = item.Name;
                        IdCusChosen = item.ID;
                        inputDeposit.Text = "";
                        inputDepositCash.Text = "";
                        inputDiscount.Text = "";
                        LoadBookingDetails(IdCusChosen);
                        LoadBookings(item.Name);
                    };
                    btnItemAction.Height = 25;
                    btnItemAction.Width = 50;
                    brItemAction.Child = btnItemAction;
                    brItemAction.Height = 25;
                    Grid.SetColumn(brItemAction, 2);
                    Grid.SetRow(brItemAction, index);
                    Customers.Children.Add(brItemAction);



                    index++;
                }
            }



        }
        public void LoadCars(string typeFilter = null, string filter = null)
        {
            var cars = new HashSet<CarView>();       
            switch (typeFilter)
            {
                case "category":
                    ComboBoxItem selectedItem = categoryChose.SelectedItem as ComboBoxItem;
                    if (selectedItem != null)
                    {
                        string content = selectedItem.Content.ToString();
                        cars = CarRepository.Instance.FindByCategory(content);
                    }
                    break;
                case "searchname":
                    cars = CarRepository.Instance.FindAll(filter);
                    break;
                default:
                    cars = CarRepository.Instance.FindAllWaiting("Waiting");
                    break;
            }
            
                    
            
            if(cars.Count > 0)
            {
                Cars.RowDefinitions.Clear();
                Cars.Children.Clear();
                var index = 0;
                foreach (var car in cars)
                {
                    RowDefinition rowDefinition = new RowDefinition();
                    rowDefinition.Height = new GridLength(120);
                    Cars.RowDefinitions.Add(rowDefinition);
                    Border brItem = new Border();
                    brItem.BorderThickness = new Thickness(1);
                    brItem.BorderBrush = Brushes.Gray;
                    brItem.Margin = new Thickness(5);
                    
                    Grid grItemCar = new Grid();
                    ColumnDefinition imgItem = new ColumnDefinition()
                    {
                        Width = new GridLength(150)
                    };
                    ColumnDefinition brandItem = new ColumnDefinition()
                    {
                        Width = new GridLength(120)
                    };
                    ColumnDefinition choseItem = new ColumnDefinition()
                    {
                        Width = new GridLength(100)
                    };

                    grItemCar.ColumnDefinitions.Add(imgItem);
                    grItemCar.ColumnDefinitions.Add(brandItem);
                    grItemCar.ColumnDefinitions.Add(choseItem);

                    //Add image
                    Image ItemImg = new Image();
                    ItemImg.Width = 140;
                    ItemImg.Height = 80;
                    string imagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", car.Image);
                    ItemImg.Source = new BitmapImage(new Uri(imagePath));
                    Grid.SetColumn(ItemImg, 0);
                    grItemCar.Children.Add(ItemImg);

                    //Add Model
                    Grid grInfo = new Grid();
                    grInfo.Margin = new Thickness(0, 15, 0, 0);
                    RowDefinition rowModel = new RowDefinition();
                    RowDefinition rowPrice = new RowDefinition();
                    grInfo.RowDefinitions.Add(rowModel);                   
                    grInfo.RowDefinitions.Add(rowPrice);
                    //Lable 1
                    Label lbModel = new Label();
                    TextBlock modelText = new TextBlock();
                    modelText.Text = car.Model;
                    modelText.TextWrapping = TextWrapping.Wrap;
                    modelText.TextAlignment = TextAlignment.Center;
                    lbModel.Content = modelText;                   
                    lbModel.FontSize = 12;
                    lbModel.Height = 60;
                    lbModel.Width = 110;                                                     
                    lbModel.VerticalAlignment = VerticalAlignment.Center;
                    lbModel.HorizontalAlignment = HorizontalAlignment.Center;
                    Grid.SetRow(lbModel, 0);
                    grInfo.Children.Add(lbModel);
                    //Lable 2
                    Label lbPrice = new Label();
                    TextBlock priceText = new TextBlock();
                    var price = car.PricePerDay;
                    string priceFormatted = price.ToString("C0", CultureInfo.GetCultureInfo("vi-VN"));
                    priceText.Text = priceFormatted + "/Ngày";
                    priceText.TextWrapping = TextWrapping.Wrap;
                    priceText.TextAlignment = TextAlignment.Center;
                    lbPrice.Content = priceText;
                    lbPrice.FontSize = 12;
                    lbPrice.Height = 25;
                    lbPrice.Width = 110;
                    lbPrice.VerticalAlignment = VerticalAlignment.Center;
                    lbPrice.HorizontalAlignment = HorizontalAlignment.Center;
                    Grid.SetRow(lbPrice, 2);
                    grInfo.Children.Add(lbPrice);


                    Grid.SetColumn(grInfo, 1);
                    grItemCar.Children.Add(grInfo);

                    //Add chose button and status of car
                    Grid grAction = new Grid();
                    RowDefinition rowStatus = new RowDefinition();
                    RowDefinition rowChose = new RowDefinition();
                    grAction.RowDefinitions.Add(rowStatus);
                    grAction.RowDefinitions.Add(rowChose);
                    Label lbStatus = new Label();
                    lbStatus.Content = car.CarStatus.ToString();
                    lbStatus.FontSize = 16;
                    lbStatus.Height = 35;
                    lbStatus.Width = 70;
                    lbStatus.BorderBrush = Brushes.Green;          
                    lbStatus.BorderThickness = new Thickness(2); 
                    lbStatus.Padding = new Thickness(5);
                    lbStatus.VerticalAlignment = VerticalAlignment.Center;
                    lbStatus.HorizontalAlignment = HorizontalAlignment.Center;
                    Grid.SetRow(lbStatus,0);
                    grAction.Children.Add(lbStatus);

                    Button btnChose = new Button();
                    btnChose.Content = "Chose";
                    btnChose.FontSize = 16;
                    btnChose.Height = 35;
                    btnChose.Width = 70;
                    btnChose.Click += (object sender, RoutedEventArgs e) =>
                    {
                        RemoveCarUpdateInfo();
                        showTotalPaid.Text = "";
                        btnPlanCar.IsEnabled = true;
                        IdCarChosen = car.ID;                      
                        showModel.Text = car.Model;
                        showSeatCount.Text = car.SeatCount.ToString();
                        var showPrice = car.PricePerDay;
                        string  showPriceFormatted = showPrice.ToString("C0", CultureInfo.GetCultureInfo("vi-VN"));
                        showPricePerDay.Text = showPriceFormatted + "/Ngày";
                        PricePerDay = null;
                        PricePerDay = car.PricePerDay;
                        if (!string.IsNullOrEmpty(showTotalDay.Text))
                        {                            
                            var totalPrice = PricePerDay * (Convert.ToInt32(showTotalDay.Text));
                            showTotalPerCar.Text = totalPrice.ToString() + "VNĐ";
                        }
                    };
                    Grid.SetRow(btnChose,1);
                    grAction.Children.Add(btnChose);

                    Grid.SetColumn(grAction, 2);
                    grItemCar.Children.Add(grAction);


                    brItem.Child = grItemCar;
                    Grid.SetRow(brItem, index);
                    
                    Cars.Children.Add(brItem);
                    //Column 2(



                    index++;
                }
            }
        }

        


        //Check If user Pay in full or Deposit
        private void PaymentChange(object sender, SelectionChangedEventArgs e)
        {

            ComboBoxItem paymentID = inputPayment.SelectedItem as ComboBoxItem;
            if (paymentID.Content.ToString().Trim() == "Deposit")
            {
                inputDeposit.IsEnabled = true;
            }
            else
            {
                inputDeposit.Text = "";
                inputDeposit.IsEnabled = false;

            }           
        }

        private void changeDate(object sender, SelectionChangedEventArgs e)
        {
            if (inputStartDate.SelectedDate.HasValue)
            {
                var check = CheckCurrentDate(inputStartDate.SelectedDate.Value);
                if (check == false)
                {
                    inputStartDate.SelectedDate = null;
                    showTotalDay.Text = "";
                }
            }
            if (inputEndDate.SelectedDate.HasValue)
            {
                var check = CheckCurrentDate(inputEndDate.SelectedDate.Value);
                if(check == false)
                {
                    inputEndDate.SelectedDate = null;
                    showTotalDay.Text = null;
                }
            }
            if (inputStartDate.SelectedDate.HasValue && inputEndDate.SelectedDate.HasValue)
            {
                DateTime start = inputStartDate.SelectedDate.Value; 
                DateTime end = inputEndDate.SelectedDate.Value;

                //Check valid date
                checkDate(start, end);

                
                int Total = (end - start).Days +1;
                if(Total > 0)
                {
                    showTotalDay.Text = Total.ToString();
                }
                else
                {
                    showTotalDay.Text = "";
                }
                    PricePerCar = Total;
                //MessageBox.Show(PricePerDay.ToString());

                //if (!string.IsNullOrEmpty(PricePerDay.ToString()))
                //{
                //    var totalPrice = PricePerDay * (Convert.ToInt32(showTotalDay.Text));
                //    if(totalPrice > 0)
                //    {
                //        showTotalPerCar.Text = totalPrice.ToString() + " VNĐ";
                //    }                   
                //}
                //if(IsBookingOrBookingDetails == 2)
                //{
                    var totalPrice = PricePerDay * Convert.ToInt32(showTotalDay.Text);
                    if (totalPrice > 0)
                    {
                        showTotalPerCar.Text = totalPrice.ToString() + " VNĐ";
                    }
                //}
            }
        }
        private bool CheckCurrentDate(DateTime date)
        {
            var now = DateTime.Today;
            if (date< now)
            {               
                MessageBox.Show("Please choose a date that is today or later!", "Status");
                return false;
            }
            return true;
        }
        private void checkDate(DateTime start, DateTime end)
        {
            
            if((end - start).Days < 0)
            {
                MessageBox.Show("Date chosen is inValid!", "Status");
                inputStartDate.SelectedDate = null;
                inputEndDate.SelectedDate = null;
            }
        }

        //Store Car into Booking
        private void PlanCar(object sender, RoutedEventArgs e)
        {
            BookingView setBooking = new BookingView();
            setBooking.CusId = IdCusChosen;
            setBooking.BookingStatus = "Waiting";
            setBooking.OrderDate = DateTime.Now;
            setBooking.Paid = 0;
            if (!string.IsNullOrEmpty(inputDiscount.Text))
            {
                setBooking.Discount = Convert.ToInt32(inputDiscount.Text);
            }
            if(!string.IsNullOrEmpty(inputDeposit.Text))
            {
                setBooking.Deposit = Convert.ToInt32(inputDeposit.Text);
            }
            ComboBoxItem getPaymentTypeId = inputPayment.SelectedItem as ComboBoxItem;
            if (getPaymentTypeId != null)
            {
                var tagValue = getPaymentTypeId.Tag;
                int paymentTypeId = Convert.ToInt32(tagValue);
                setBooking.PaymentTypeId = paymentTypeId;
            }

            var checkBooking = BookingRepository.Instance.IsExistBooking(IdCusChosen, "Waiting");
            
            if(checkBooking == null)
            {
                //create new booking
                BookingRepository.Instance.Create(setBooking);
                IdBookingReturn = setBooking.ID;
            }
            else
            {
                IdBookingReturn = checkBooking.ID;
            }
                //plan car
            BookingDetailsView setBookingDetails = new BookingDetailsView();
            
            setBookingDetails.BookingId = IdBookingReturn;
            setBookingDetails.CarId = IdCarChosen;
            setBookingDetails.BookingDetailsStatus = "Hiring";
            setBookingDetails.BookingDate = DateTime.Now;
            setBookingDetails.StartDate = inputStartDate.SelectedDate.Value;
            setBookingDetails.EndDate = inputEndDate.SelectedDate.Value;
            setBookingDetails.PricePerCar = decimal.Parse(showTotalPerCar.Text.Substring(0, showTotalPerCar.Text.Length - 4));
            BookingDetailsRepository.Instance.Create(setBookingDetails);


            //Change status of car chosen
            CarRepository.Instance.UpdateStatusCar(IdCarChosen, "Booked");

            //Reload Cars info
            Cars.RowDefinitions.Clear();
            Cars.Children.Clear();
            LoadCars("category");

            //Reload Bookings info
            Bookings.RowDefinitions.Clear();
            Bookings.Children.Clear();
            LoadBookings();

            //Load totalPrice 
            var item = BookingRepository.Instance.FindByBookingId(IdBookingReturn);         
            showTotalPaid.Text = item.TotalPrice.ToString() + " VNĐ";
            if(item.PaymentTypeName == "Deposit" && item.DepositHasPaid != 1)
            {
                decimal? depositCash = (item.Deposit * item.TotalPrice) / 100;
                inputDepositCash.Text = depositCash.ToString();
                BookingView updateDepositCash = new BookingView();
                updateDepositCash.ID = IdBookingReturn;
                updateDepositCash.DepositCash = depositCash;
                BookingRepository.Instance.UpdateDepositCash(updateDepositCash);

            }
            else if(item.PaymentTypeName == "Deposit" && (item.DepositHasPaid == 1))
            {
                inputDepositCash.Text = item.DepositCash.ToString();
            }
                //Load Deposit Cash must paid


                //Load Cars planed
                LoadBookingDetails(IdCusChosen);

            //Clear Car data
            showModel.Text = "";
            showSeatCount.Text = "";
            showPricePerDay.Text = "";
            showTotalPerCar.Text = "";
            inputStartDate.SelectedDate= null;
            inputEndDate.SelectedDate= null;
            showTotalDay.Text = "";
            IdCarChosen = null;
            PricePerDay = null;
            PricePerCar = null;
        }

        private void LoadBookingDetails(int? cus_id)
        {
            var items = BookingDetailsRepository.Instance.FindByCusId(cus_id);            
            CarsChosen.Children.Clear();
            CarsChosen.RowDefinitions.Clear();
            if (items.Count > 0)
            {
                var index = 0;
                foreach (var item in items)
                {
                    RowDefinition rowDefinition = new RowDefinition();
                    rowDefinition.Height = new GridLength(50);
                    CarsChosen.RowDefinitions.Add(rowDefinition);
                    BrushConverter bc = new BrushConverter();
                    Brush rowBackground = Brushes.White;
                    if(item.ActualReturnDate != null)
                    {
                        rowBackground = (Brush)bc.ConvertFromString("#D3D3D3"); //returned
                    }
                    else
                    {
                        rowBackground = (Brush)bc.ConvertFromString("#DFFFD6"); //On Using
                    }
                    //Column 1 (License plate)
                    Border brItemLicense = new Border();
                    TextBlock tbItemLicense = new TextBlock();
                    tbItemLicense.Text = item.LicensePlate;
                    tbItemLicense.TextAlignment = TextAlignment.Center;
                    tbItemLicense.VerticalAlignment = VerticalAlignment.Center;
                    brItemLicense.Child = tbItemLicense;
                    brItemLicense.Height = 50;
                    brItemLicense.Background = rowBackground;
                    Grid.SetColumn(brItemLicense, 0);
                    Grid.SetRow(brItemLicense, index);
                    CarsChosen.Children.Add(brItemLicense);

                    //Column 2 (Model)
                    Border brItemModel = new Border();
                    TextBlock tbItemModel = new TextBlock();
                    tbItemModel.Text = item.Model;
                    tbItemModel.TextAlignment = TextAlignment.Center;
                    tbItemModel.VerticalAlignment = VerticalAlignment.Center;
                    tbItemModel.TextWrapping = TextWrapping.Wrap;
                    brItemModel.Child = tbItemModel;
                    brItemModel.Height = 50;
                    brItemModel.Width = 83;
                    brItemModel.Background = rowBackground;
                    Grid.SetColumn(brItemModel, 1);
                    Grid.SetRow(brItemModel, index);
                    CarsChosen.Children.Add(brItemModel);

                    //Column 3 (Seat count)
                    Border brItemSeat = new Border();
                    TextBlock tbItemSeat = new TextBlock();
                    tbItemSeat.Text = item.SeatCount.ToString();                   
                    tbItemSeat.TextAlignment = TextAlignment.Center;
                    tbItemSeat.VerticalAlignment = VerticalAlignment.Center;
                    brItemSeat.Child = tbItemSeat;
                    brItemSeat.Height = 50;
                    brItemSeat.Background = rowBackground;
                    Grid.SetColumn(brItemSeat, 2);
                    Grid.SetRow(brItemSeat, index);
                    CarsChosen.Children.Add(brItemSeat);

                    //Column 4 (Start Date)
                    Border brItemStartDate = new Border();
                    TextBlock tbItemStartDate = new TextBlock();
                    tbItemStartDate.Text = item.StartDate.ToString();
                    tbItemStartDate.TextAlignment = TextAlignment.Center;
                    tbItemStartDate.VerticalAlignment = VerticalAlignment.Center;
                    tbItemStartDate.TextWrapping = TextWrapping.Wrap;
                    brItemStartDate.Child = tbItemStartDate;
                    brItemStartDate.Height = 50;
                    brItemStartDate.Width = 81;
                    brItemStartDate.Background = rowBackground;
                    Grid.SetColumn(brItemStartDate, 3);
                    Grid.SetRow(brItemStartDate, index);
                    CarsChosen.Children.Add(brItemStartDate);

                    //Column 5 (End Date)
                    Border brItemEndDate = new Border();
                    TextBlock tbItemEndDate = new TextBlock();
                    tbItemEndDate.Text = item.EndDate.ToString();
                    tbItemEndDate.VerticalAlignment = VerticalAlignment.Center;
                    tbItemEndDate.TextAlignment = TextAlignment.Center;
                    tbItemEndDate.TextWrapping = TextWrapping.Wrap;
                    brItemEndDate.Child = tbItemEndDate;
                    brItemEndDate.Height = 50;
                    brItemEndDate.Width = 80;
                    brItemEndDate.Background = rowBackground;
                    Grid.SetColumn(brItemEndDate, 4);
                    Grid.SetRow(brItemEndDate, index);
                    CarsChosen.Children.Add(brItemEndDate);

                    //Column 6 (Return date)
                    Border brItemReturnDate = new Border();
                    TextBlock tbItemReturnDate = new TextBlock();
                    tbItemReturnDate.Text = item.ActualReturnDate == null ? "On using" : item.ActualReturnDate.ToString();
                    tbItemReturnDate.TextAlignment = TextAlignment.Center;
                    tbItemReturnDate.VerticalAlignment = VerticalAlignment.Center;
                    tbItemReturnDate.TextWrapping = TextWrapping.Wrap;
                    brItemReturnDate.Child = tbItemReturnDate;
                    brItemReturnDate.Height = 50;
                    brItemReturnDate.Width = 74;
                    brItemReturnDate.Background = rowBackground;
                    Grid.SetColumn(brItemReturnDate, 5);
                    Grid.SetRow(brItemReturnDate, index);
                    CarsChosen.Children.Add(brItemReturnDate);

                    //Column 7 (Status Return)
                    Border brItemStatusReturn = new Border();
                    TextBlock tbItemStatusReturn = new TextBlock();
                    tbItemStatusReturn.Text = item.StatusReturn == 1 ? "Yes" : "No";
                    tbItemStatusReturn.TextAlignment = TextAlignment.Center;
                    tbItemStatusReturn.VerticalAlignment = VerticalAlignment.Center;
                    brItemStatusReturn.Child = tbItemStatusReturn;
                    brItemStatusReturn.Height = 50;
                    brItemStatusReturn.Background = rowBackground;
                    Grid.SetColumn(brItemStatusReturn, 6);
                    Grid.SetRow(brItemStatusReturn, index);
                    CarsChosen.Children.Add(brItemStatusReturn);

                    //Column 8 (Total Price)
                    Border brItemTotalPrice = new Border();
                    TextBlock tbItemTotalPrice = new TextBlock();
                    tbItemTotalPrice.Text = item.Total.ToString()+ " VNĐ";
                    tbItemTotalPrice.TextAlignment = TextAlignment.Center;
                    tbItemTotalPrice.VerticalAlignment = VerticalAlignment.Center;
                    brItemTotalPrice.Child = tbItemTotalPrice;
                    brItemTotalPrice.Height = 50;
                    brItemTotalPrice.Background = rowBackground;
                    Grid.SetColumn(brItemTotalPrice, 7);
                    Grid.SetRow(brItemTotalPrice, index);
                    CarsChosen.Children.Add(brItemTotalPrice);

                    //Column 9 (booking Details ID)
                    Border brItemBookingDetailsId = new Border();
                    TextBlock tbItemBookingDetailsId = new TextBlock();
                    tbItemBookingDetailsId.Text = item.ID.ToString();
                    tbItemBookingDetailsId.TextAlignment = TextAlignment.Center;
                    tbItemBookingDetailsId.VerticalAlignment = VerticalAlignment.Center;
                    brItemBookingDetailsId.Child = tbItemBookingDetailsId;
                    brItemBookingDetailsId.Height = 50;
                    brItemBookingDetailsId.Background = rowBackground;
                    Grid.SetColumn(brItemBookingDetailsId, 8);
                    Grid.SetRow(brItemBookingDetailsId, index);
                    CarsChosen.Children.Add(brItemBookingDetailsId);

                    //Column 10 (Action : edit + Remove)
                    Grid grItemAction = new Grid();
                    grItemAction.Height = 50;
                    ColumnDefinition colEdit = new ColumnDefinition();
                    ColumnDefinition colRemove = new ColumnDefinition();
                    grItemAction.ColumnDefinitions.Add(colEdit);
                    grItemAction.ColumnDefinitions.Add(colRemove);
                    grItemAction.Background = rowBackground;
                        //btnEdit
                    Button btnEdit = new Button();
                    btnEdit.Content = "Edit";
                    btnEdit.Height = 25;
                    btnEdit.Width = 50;
                    btnEdit.Click += (object sender, RoutedEventArgs e) =>
                    {                       
                        btnSaveCarUpdate.IsEnabled = true;
                        inputReturnDate.IsEnabled = true;
                        btnPlanCar.IsEnabled = false;
                        btnCancelSaveCarUpdate.IsEnabled = true;
                        showModel.Text = item.Model;
                        showSeatCount.Text = item.SeatCount.ToString();
                        showPricePerDay.Text = item.CarView.PricePerDay.ToString()+" VNĐ";
                        inputStartDate.SelectedDate = item.StartDate;
                        inputEndDate.SelectedDate = item.EndDate;
                        PricePerDay = item.CarView.PricePerDay;
                        showTotalPerCar.Text = item.Total.ToString() + " VNĐ";
                        IdBookingDetails = item.ID;
                        IdCarChosen = item.CarId;
                                                                                             
                    };
                    Grid.SetColumn(btnEdit, 0);
                    grItemAction.Children.Add(btnEdit);
                        //btnRemove
                    Button btnRemove = new Button();
                    btnRemove.Content = "Remove";
                    btnRemove.Height = 25;
                    btnRemove.Width = 50;
                    btnRemove.Click += (object sender, RoutedEventArgs e) =>
                    {
                        IdCusChosen = Convert.ToInt32(showIdCus.Text);
                        //Update status car
                        var get_car_id = item.CarId;                     
                        var checkUpdateStatus = CarRepository.Instance.UpdateStatusCar(get_car_id, "Waiting");
                        if (checkUpdateStatus)
                        {
                            //delete booking details
                            var del = new BookingDetailsView();
                            del.ID = item.ID;
                            var checkDel = BookingDetailsRepository.Instance.Delete(del);
                            if (checkDel)
                            {
                                MessageBox.Show("Remove successful!");
                                var reloadTotalPrice = BookingRepository.Instance.FindById(IdCusChosen);
                                showTotalPaid.Text = reloadTotalPrice.TotalPrice.ToString() + " VNĐ";
                                LoadBookings();
                                LoadBookingDetails(IdCusChosen);
                                LoadCars();
                                showModel.Text = "";
                                showSeatCount.Text = "";
                                showPricePerDay.Text = "";
                                showTotalPerCar.Text = "";
                                inputStartDate.SelectedDate = null;
                                inputEndDate.SelectedDate = null;
                                showTotalDay.Text = "";
                                IdCarChosen = null;
                                PricePerDay = null;
                                PricePerCar = null;
                            }
                            else
                            {
                                MessageBox.Show("There is error when trying to remove!");
                            }                          
                        }
                        else
                        {
                            MessageBox.Show("There is error when update car's status");
                        }                       
                    };
                    Grid.SetColumn(btnRemove, 1);
                    grItemAction.Children.Add(btnRemove);

                    Grid.SetColumn(grItemAction, 9);
                    Grid.SetRow(grItemAction, index);
                    CarsChosen.Children.Add(grItemAction);
                    index++;
                }
            }
        }
        

        private void SearchBookings(object sender, TextChangedEventArgs e)
        {
            TextBox searchBox = sender as TextBox;
            var filter = searchBox.Text.Trim();           
            LoadBookings(filter);
        }
        
        private void LoadBookings(string filter = null)
        {
            var items = BookingRepository.Instance.FindAllWaiting(filter);
            Bookings.RowDefinitions.Clear();
            Bookings.Children.Clear();
            if(items.Count > 0)
            {
                var index = 0;
                foreach(var item in items)
                {
                    RowDefinition rowDefinition = new RowDefinition();
                    rowDefinition.Height = new GridLength(25);
                    BrushConverter bc = new BrushConverter();
                    Brush rowBackground = (Brush)bc.ConvertFromString("#DFFFD6");                  
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

                    //Column 2 (Name)
                    if(item.CustomerView != null)
                    {
                        Border brItemName = new Border();
                        TextBlock tbItemName = new TextBlock();
                        tbItemName.Text = item.CustomerView.Name;
                        tbItemName.TextAlignment = TextAlignment.Center;
                        tbItemName.VerticalAlignment = VerticalAlignment.Center;
                        brItemName.Child = tbItemName;
                        brItemName.Height = 25;
                        brItemName.Background = rowBackground;
                        Grid.SetColumn(brItemName, 1);
                        Grid.SetRow(brItemName, index);
                        Bookings.Children.Add(brItemName);
                    }

                    //Column 3 (Id Card Number)
                    if(item.CustomerView != null)
                    {
                        Border brItemIdCard = new Border();
                        TextBlock tbItemIdCard = new TextBlock();
                        tbItemIdCard.Text = item.CustomerView.CusIdCard;
                        tbItemIdCard.VerticalAlignment = VerticalAlignment.Center;
                        tbItemIdCard.TextAlignment = TextAlignment.Center;
                        brItemIdCard.Child = tbItemIdCard;
                        brItemIdCard.Height = 25;
                        brItemIdCard.Background = rowBackground;
                        Grid.SetColumn(brItemIdCard, 2);
                        Grid.SetRow(brItemIdCard, index);
                        Bookings.Children.Add(brItemIdCard);
                    }

                    //Column 4(Phone)
                    if(item.CustomerView != null)
                    {
                        Border brItemPhone = new Border();
                        TextBlock tbItemPhone = new TextBlock();
                        tbItemPhone.Text = item.CustomerView.Phone;
                        tbItemPhone.VerticalAlignment = VerticalAlignment.Center;
                        tbItemPhone.TextAlignment = TextAlignment.Center;
                        brItemPhone.Child = tbItemPhone;
                        brItemPhone.Height = 25;
                        brItemPhone.Background = rowBackground;
                        Grid.SetColumn(brItemPhone, 3);
                        Grid.SetRow(brItemPhone, index);
                        Bookings.Children.Add(brItemPhone);
                    }

                    //Column 5 (Car amount)
                    var carNumber = BookingRepository.Instance.CheckTotalCar(item.ID);
                    Border brItemCarAmount = new Border();
                    TextBlock tbItemCarAmount = new TextBlock();
                    tbItemCarAmount.Text = carNumber.ToString();
                    tbItemCarAmount.TextAlignment = TextAlignment.Center;
                    tbItemCarAmount.VerticalAlignment = VerticalAlignment.Center;
                    brItemCarAmount.Child = tbItemCarAmount;
                    brItemCarAmount.Height = 25;
                    brItemCarAmount.Background = rowBackground;
                    Grid.SetColumn(brItemCarAmount, 4);
                    Grid.SetRow(brItemCarAmount, index);
                    Bookings.Children.Add(brItemCarAmount);

                    //Column 6 (Booking date)
                    Border brItemBookingDate = new Border();
                    TextBlock tbItemBookingDate = new TextBlock();
                    tbItemBookingDate.Text = item.OrderDate.ToString();
                    tbItemBookingDate.TextAlignment = TextAlignment.Center;
                    tbItemBookingDate.VerticalAlignment = VerticalAlignment.Center;
                    brItemBookingDate.Child = tbItemBookingDate;
                    brItemBookingDate.Height = 25;
                    brItemBookingDate.Background = rowBackground;
                    Grid.SetColumn(brItemBookingDate, 5);
                    Grid.SetRow(brItemBookingDate, index);
                    Bookings.Children.Add(brItemBookingDate);

                    //Column 7 (Booking status)
                    Border brItemBookingStatus = new Border();
                    TextBlock tbItemBookingStatus = new TextBlock();
                    tbItemBookingStatus.Text = item.BookingStatus;
                    tbItemBookingStatus.VerticalAlignment = VerticalAlignment.Center;
                    tbItemBookingStatus.TextAlignment = TextAlignment.Center;
                    brItemBookingStatus.Child = tbItemBookingStatus;
                    brItemBookingStatus.Height = 25;
                    brItemBookingStatus.Background = rowBackground;
                    Grid.SetColumn(brItemBookingStatus, 6);
                    Grid.SetRow(brItemBookingStatus, index);
                    Bookings.Children.Add(brItemBookingStatus);
                    
                    //Column 8 (Paid)
                    Border brItemPaid = new Border();
                    TextBlock tbItemPaid = new TextBlock();
                    tbItemPaid.Text = item.Paid != null ? (item.Paid == 1 ? "Yes" : "No") : "";
                    tbItemPaid.VerticalAlignment = VerticalAlignment.Center;
                    tbItemPaid.TextAlignment = TextAlignment.Center;
                    brItemPaid.Child = tbItemPaid;
                    brItemPaid.Height = 25;
                    brItemPaid.Background = rowBackground;
                    Grid.SetColumn(brItemPaid, 7);
                    Grid.SetRow(brItemPaid, index);
                    Bookings.Children.Add(brItemPaid);

                    //Column 9 (Action)
                    Grid grItemAction = new Grid();
                    ColumnDefinition colEdit = new ColumnDefinition();
                    ColumnDefinition colComplete = new ColumnDefinition();
                    ColumnDefinition colCancel = new ColumnDefinition();
                    grItemAction.ColumnDefinitions.Add(colEdit);
                    grItemAction.ColumnDefinitions.Add(colComplete);
                    grItemAction.ColumnDefinitions.Add(colCancel);

                    Button btnEdit = new Button();
                    btnEdit.Content = "Edit";
                    btnEdit.Height = 25;
                    btnEdit.Width = 50;
                    btnEdit.Click += (object sender, RoutedEventArgs e) =>
                    {
                        EditBooking(item.ID, item.CusId);
                        IdBookingReturn = item.ID;
                        var rs = BookingRepository.Instance.FindByBookingId(item.ID);
                        inputDepositHasPaid.IsEnabled = true;
                        inputDepositCash.IsEnabled = true;
                        showTotalPaid.Text = rs.TotalPrice.ToString() + " VNĐ";
                    };
                    Grid.SetColumn(btnEdit, 0);
                    grItemAction.Children.Add(btnEdit);                    
                    Button btnComplete = new Button();
                    btnComplete.Content = "Complete";
                    btnComplete.Height = 25;
                    btnComplete.Width = 60;
                    btnComplete.Click += (object sender, RoutedEventArgs e) =>
                    {
                        var getCarNotReturn = BookingDetailsRepository.Instance.FindAllNotReturn(item.ID);
                        if (getCarNotReturn != null && getCarNotReturn.Count == 0)
                        {
                            var showComplete = new BookingComplete(item.ID)
                            {
                                Owner = Window.GetWindow(this)
                            };
                            bool? result = showComplete.ShowDialog();
                            if(result == true)
                            {
                                MessageBox.Show("This Order has been completed!");
                                LoadBookings();
                                CarsChosen.RowDefinitions.Clear();
                                CarsChosen.Children.Clear();

                            }

                        }
                        else if( getCarNotReturn != null && getCarNotReturn.Count > 0 )
                        {
                            MessageBox.Show("Some cars were not returned. Please check and try again!");
                        }
                    };
                    Grid.SetColumn(btnComplete, 1);
                    grItemAction.Children.Add(btnComplete);
                    var backgroundCancelBtn = (Brush)bc.ConvertFromString("#FFC0CB");
                    Button btnCancel = new Button();
                    btnCancel.Content = "Cancel";
                    btnCancel.Height = 25;
                    btnCancel.Width = 60;
                    btnCancel.Background = backgroundCancelBtn;
                    btnCancel.Click += (object sender, RoutedEventArgs e) =>
                    {
                        var getCarNotReturn = BookingDetailsRepository.Instance.FindAllNotReturn(item.ID);
                        if( getCarNotReturn == null || getCarNotReturn.Count == 0)
                        {
                            var confirm = MessageBox.Show("Cancel this order ?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                            if (confirm == MessageBoxResult.Yes)
                            {

                                Window getReason = new Views.Booking.ReasonCancel(item.ID);
                                bool? result = getReason.ShowDialog();
                                if (result == true)
                                {
                                    MessageBox.Show("This booking has been canceled!");
                                    LoadBookings();
                                }
                                else
                                {
                                    MessageBox.Show("There is error when cancel this booking, please try again!");
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Some cars were not returned. Please make sure all car has been returned before try again!");
                        }                     
                    };
                    Grid.SetColumn(btnCancel, 2);
                    grItemAction.Children.Add(btnCancel);

                    grItemAction.Background = rowBackground;
                    Grid.SetColumn(grItemAction, 8);
                    Grid.SetRow(grItemAction, index);
                    Bookings.Children.Add(grItemAction);

                    index++;
                }
            }
        }

        private void btnLoadBookings(object sender, RoutedEventArgs e)
        {
            LoadBookings();
        }
        private void EditBooking(int? booking_id, int? cus_id)
        {
            IdCusChosen = cus_id;           
            var info = BookingRepository.Instance.FindByBookingId(booking_id);
            if (info != null)
            {
                btnSaveBookingUpdate.IsEnabled = true;
                showIdCus.Text = info.CusId.ToString();
                showName.Text = info.CustomerView.Name;
                var paymentType = PaymentTypeRepository.Instance.GetAll();
                
                foreach (ComboBoxItem item in inputPayment.Items)
                {
                    if (item.Content.ToString() == info.PaymentTypeName)
                    {
                        inputPayment.SelectedItem = item;
                    }
                }
                if(info.DepositHasPaid ==1 || info.DepositHasPaid == 0) { 
                    inputDepositHasPaid.IsChecked = info.DepositHasPaid == 1 ? true : false;
                }
                if(info.DepositCash != null || info.DepositCash > 0)
                {
                    inputDepositCash.Text = info.DepositCash.ToString();
                }   
                if(info.DepositCash > 0 || info.DepositCash != null)
                {
                    inputDepositCash.Text = info.DepositCash.ToString();
                }
                if(info.PaymentTypeName == "Deposit")
                {
                    inputDeposit.IsEnabled = true;
                    inputDeposit.Text = info.Deposit.ToString();
                }
                if(info.Discount != null)
                {
                    inputDiscount.Text = info.Discount.ToString();
                }
                
                CarsChosen.RowDefinitions.Clear();
                CarsChosen.Children.Clear();
                LoadBookingDetails(info.CusId);
            }
        }

        private void SaveBookingUpdate(object sender, RoutedEventArgs e)
        {
            if(IdBookingReturn != 0)
            {
                inputDepositHasPaid.IsEnabled = false;
                inputDepositCash.IsEnabled = false;
                var item = new BookingView();
                item.ID = IdBookingReturn;
                if (!string.IsNullOrEmpty(inputDiscount.Text))
                {
                    item.Discount = Convert.ToInt32(inputDiscount.Text);
                }
                if (!string.IsNullOrEmpty(inputDeposit.Text))
                {
                    item.Deposit = Convert.ToInt32(inputDeposit.Text);
                }
                item.DepositHasPaid = inputDepositHasPaid.IsChecked == true ? 1 : 0;
                if (!string.IsNullOrEmpty(inputDepositCash.Text))
                {
                    item.DepositCash = Convert.ToInt32(inputDepositCash.Text);
                }
                ComboBoxItem getPaymentTypeId = inputPayment.SelectedItem as ComboBoxItem;
                if (getPaymentTypeId != null)
                {
                    var tagValue = getPaymentTypeId.Tag;
                    int paymentTypeId = Convert.ToInt32(tagValue);
                    item.PaymentTypeId = paymentTypeId;
                }
                var checkUpdate = BookingRepository.Instance.Update(item);
                if (checkUpdate)
                {
                    MessageBox.Show("Update successful!");
                    showName.Text = "";
                    showIdCus.Text = "";
                    inputDeposit.Text = "";
                    inputDepositCash.Text = "";
                    inputDiscount.Text = "";
                    showModel.Text = "";
                    showSeatCount.Text = "";
                    showPricePerDay.Text = "";
                    showTotalPerCar.Text = "";
                    showTotalPaid.Text = "";
                    inputStartDate.SelectedDate = null;
                    inputEndDate.SelectedDate = null;
                    PricePerDay = null;
                    PricePerCar = null;
                    btnSaveBookingUpdate.IsEnabled = false;
                    inputDepositHasPaid.IsChecked = false;
                }
            }
        }

        private void SaveCarUpdate(object sender, RoutedEventArgs e)
        {
            if (IdBookingDetails != 0)
            {
                var item = new BookingDetailsView();
                item.ID = IdBookingDetails;
                item.StartDate = inputStartDate.SelectedDate;
                item.EndDate = inputEndDate.SelectedDate;
                item.PricePerCar = decimal.Parse(showTotalPerCar.Text.Substring(0, showTotalPerCar.Text.Length - 4));
                if(inputReturnDate.SelectedDate != null)
                {
                    item.ActualReturnDate = inputReturnDate.SelectedDate;
                    item.StatusReturn = 1;
                    item.BookingDetailsStatus = "Returned";
                    CarRepository.Instance.UpdateStatusCar(IdCarChosen, "Waiting");
                    BookingDetailsView newStatus = new BookingDetailsView();
                    
                }

                var checkUpdate = BookingDetailsRepository.Instance.Update(item);
                if (checkUpdate)
                {
                    MessageBox.Show("Update successful!");
                    LoadBookingDetails(IdCusChosen);
                    btnSaveCarUpdate.IsEnabled = false;
                    btnCancelSaveCarUpdate.IsEnabled = false;
                    showModel.Text = "";
                    showSeatCount.Text = "";
                    showPricePerDay.Text = "";
                    showTotalPerCar.Text = "";
                    inputStartDate.SelectedDate = null;
                    inputEndDate.SelectedDate = null;
                    showTotalDay.Text = "";
                    IdCarChosen = null;
                    PricePerDay = null;
                    PricePerCar = null;

                }
            }
        }

        private void CancelSaveCarUpdate(object sender, RoutedEventArgs e)
        {
            btnPlanCar.IsEnabled = false;
            RemoveCarUpdateInfo();
        }
        private void RemoveCarUpdateInfo()
        {
            btnSaveCarUpdate.IsEnabled = false;
            btnCancelSaveCarUpdate.IsEnabled = false;           
            inputReturnDate.IsEnabled = false;
            showModel.Text = "";
            showSeatCount.Text = "";
            showPricePerDay.Text = "";
            showTotalPerCar.Text = "";
            inputStartDate.SelectedDate = null;
            inputEndDate.SelectedDate = null;
            showTotalDay.Text = "";
            IdCarChosen = null;
            PricePerDay = null;
            PricePerCar = null;
        }

        private void IntegerAndLimitInput(object sender, TextCompositionEventArgs e)
        {
            var input = (TextBox)sender;
            string newInput = input.Text.Insert(input.SelectionStart, e.Text);
            if(int.TryParse(newInput, out int value) && value >=0 && value <= 100)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        private void OnlyIntegerInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }
    }
}
