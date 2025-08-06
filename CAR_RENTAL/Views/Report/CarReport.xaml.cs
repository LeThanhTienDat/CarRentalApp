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
    /// Interaction logic for CarReport.xaml
    /// </summary>
    public partial class CarReport : UserControl
    {
        public CarReport()
        {
            InitializeComponent();
            LoadCarReport();
        }
        
        private void LoadCarReport()
        {
            var totalCar = CarRepository.Instance.CountAllCar();
            showTotalCar.Text = totalCar.ToString() + " Xe";

            var onHiring = CarRepository.Instance.CountOnHiring();
            showOnHiring.Text = onHiring.ToString() + " Xe";

            var onWaiting = CarRepository.Instance.CountWaiting();
            showWaiting.Text = onWaiting.ToString() + " Xe";

            var onMaintaining = CarRepository.Instance.CountMaintaining();
            showMaintaining.Text = onMaintaining.ToString() + " Xe";

            var deactive = CarRepository.Instance.CountDeactive();
            showDeactive.Text = deactive.ToString() + " Xe";

            var mostRented = CarRepository.Instance.FindMostRented();
            if (mostRented != null)
            {
                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(25);
                BrushConverter bc = new BrushConverter();
                Brush rowBackground = (Brush)bc.ConvertFromString("#DFFFD6");
                mostRentedCar.RowDefinitions.Add(rowDefinition);

                //Column 1
                Border brItemCarId = new Border();
                TextBlock tbItemCarId = new TextBlock();
                tbItemCarId.Text = mostRented.ID.ToString();
                tbItemCarId.VerticalAlignment = VerticalAlignment.Center;
                tbItemCarId.TextAlignment= TextAlignment.Center;    
                brItemCarId.Child = tbItemCarId;
                brItemCarId.Height = 25;
                brItemCarId.Background = rowBackground;
                Grid.SetColumn(brItemCarId, 0);
                Grid.SetRow(brItemCarId, 1);
                mostRentedCar.Children.Add(brItemCarId);

                //Column 2
                Border brItemLicensePlate = new Border();
                TextBlock tbItemLicensePlate = new TextBlock();
                tbItemLicensePlate.Text = mostRented.LicensePlate.ToString();
                tbItemLicensePlate.VerticalAlignment = VerticalAlignment.Center;
                tbItemLicensePlate.TextAlignment= TextAlignment.Center;
                brItemLicensePlate.Child= tbItemLicensePlate;
                brItemLicensePlate.Height = 25;
                brItemLicensePlate.Background = rowBackground;
                Grid.SetColumn(brItemLicensePlate, 1);
                Grid.SetRow(brItemLicensePlate, 1);
                mostRentedCar.Children.Add(brItemLicensePlate);

                //Column 3
                Border brItemBrand = new Border();
                TextBlock tbItemBrand = new TextBlock();
                tbItemBrand.Text = mostRented.Brand.ToString();
                tbItemBrand.VerticalAlignment = VerticalAlignment.Center;
                tbItemBrand.TextAlignment = TextAlignment.Center;
                brItemBrand.Child = tbItemBrand;
                brItemBrand.Height = 25;
                brItemBrand.Background = rowBackground;
                Grid.SetColumn(brItemBrand, 2);
                Grid.SetRow(brItemBrand, 1);
                mostRentedCar.Children.Add(brItemBrand);

                //Column 4
                Border brItemModel = new Border();
                TextBlock tbItemModel = new TextBlock();
                tbItemModel.Text = mostRented.Model.ToString();
                tbItemModel.VerticalAlignment = VerticalAlignment.Center;
                tbItemModel.TextAlignment = TextAlignment.Center;
                brItemModel.Child = tbItemModel;
                brItemModel.Height = 25;
                brItemModel.Background = rowBackground;
                Grid.SetColumn(brItemModel, 3);
                Grid.SetRow(brItemModel, 1);
                mostRentedCar.Children.Add(brItemModel);

                //Column 5
                Border brItemCount = new Border();
                TextBlock tbItemCount = new TextBlock();
                tbItemCount.Text = mostRented.RentCount.ToString();
                tbItemCount.VerticalAlignment = VerticalAlignment.Center;
                tbItemCount.TextAlignment = TextAlignment.Center;
                brItemCount.Child = tbItemCount;
                brItemCount.Height = 25;
                brItemCount.Background = rowBackground;
                Grid.SetColumn(brItemCount, 4);
                Grid.SetRow(brItemCount, 1);
                mostRentedCar.Children.Add(brItemCount);

                //Column 6
                Border brItemCategory = new Border();
                TextBlock tbItemCategory = new TextBlock();
                tbItemCategory.Text = mostRented.CategoryName;
                tbItemCategory.VerticalAlignment = VerticalAlignment.Center;
                tbItemCategory.TextAlignment = TextAlignment.Center;
                brItemCategory.Child = tbItemCategory;
                brItemCategory.Height = 25;
                brItemCategory.Background = rowBackground;
                Grid.SetColumn(brItemCategory, 5);
                Grid.SetRow(brItemCategory, 1);
                mostRentedCar.Children.Add(brItemCategory);

                //Column 7
                Border brItemCarType = new Border();
                TextBlock tbItemCarType = new TextBlock();
                tbItemCarType.Text = mostRented.CarTypeName;
                tbItemCarType .VerticalAlignment = VerticalAlignment.Center;
                tbItemCarType.TextAlignment = TextAlignment.Center;
                brItemCarType.Child = tbItemCarType;
                brItemCarType .Height = 25;
                brItemCarType .Background = rowBackground;
                Grid.SetColumn(brItemCarType, 6);
                Grid.SetRow(brItemCarType, 1);
                mostRentedCar.Children.Add(brItemCarType);

            }

            var leastRented = CarRepository.Instance.FindLeastRented();
            if (leastRented != null)
            {
                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(25);
                BrushConverter bc = new BrushConverter();
                Brush rowBackground = (Brush)bc.ConvertFromString("#DFFFD6");
                leastRentedCar.RowDefinitions.Add(rowDefinition);

                //Column 1
                Border brItemCarId = new Border();
                TextBlock tbItemCarId = new TextBlock();
                tbItemCarId.Text = leastRented.ID.ToString();
                tbItemCarId.VerticalAlignment = VerticalAlignment.Center;
                tbItemCarId.TextAlignment = TextAlignment.Center;
                brItemCarId.Child = tbItemCarId;
                brItemCarId.Height = 25;
                brItemCarId.Background = rowBackground;
                Grid.SetColumn(brItemCarId, 0);
                Grid.SetRow(brItemCarId, 1);
                leastRentedCar.Children.Add(brItemCarId);

                //Column 2
                Border brItemLicensePlate = new Border();
                TextBlock tbItemLicensePlate = new TextBlock();
                tbItemLicensePlate.Text = leastRented.LicensePlate.ToString();
                tbItemLicensePlate.VerticalAlignment = VerticalAlignment.Center;
                tbItemLicensePlate.TextAlignment = TextAlignment.Center;
                brItemLicensePlate.Child = tbItemLicensePlate;
                brItemLicensePlate.Height = 25;
                brItemLicensePlate.Background = rowBackground;
                Grid.SetColumn(brItemLicensePlate, 1);
                Grid.SetRow(brItemLicensePlate, 1);
                leastRentedCar.Children.Add(brItemLicensePlate);

                //Column 3
                Border brItemBrand = new Border();
                TextBlock tbItemBrand = new TextBlock();
                tbItemBrand.Text = leastRented.Brand.ToString();
                tbItemBrand.VerticalAlignment = VerticalAlignment.Center;
                tbItemBrand.TextAlignment = TextAlignment.Center;
                brItemBrand.Child = tbItemBrand;
                brItemBrand.Height = 25;
                brItemBrand.Background = rowBackground;
                Grid.SetColumn(brItemBrand, 2);
                Grid.SetRow(brItemBrand, 1);
                leastRentedCar.Children.Add(brItemBrand);

                //Column 4
                Border brItemModel = new Border();
                TextBlock tbItemModel = new TextBlock();
                tbItemModel.Text = leastRented.Model.ToString();
                tbItemModel.VerticalAlignment = VerticalAlignment.Center;
                tbItemModel.TextAlignment = TextAlignment.Center;
                brItemModel.Child = tbItemModel;
                brItemModel.Height = 25;
                brItemModel.Background = rowBackground;
                Grid.SetColumn(brItemModel, 3);
                Grid.SetRow(brItemModel, 1);
                leastRentedCar.Children.Add(brItemModel);

                //Column 5
                Border brItemCount = new Border();
                TextBlock tbItemCount = new TextBlock();
                tbItemCount.Text = leastRented.RentCount.ToString();
                tbItemCount.VerticalAlignment = VerticalAlignment.Center;
                tbItemCount.TextAlignment = TextAlignment.Center;
                brItemCount.Child = tbItemCount;
                brItemCount.Height = 25;
                brItemCount.Background = rowBackground;
                Grid.SetColumn(brItemCount, 4);
                Grid.SetRow(brItemCount, 1);
                leastRentedCar.Children.Add(brItemCount);

                //Column 6
                Border brItemCategory = new Border();
                TextBlock tbItemCategory = new TextBlock();
                tbItemCategory.Text = leastRented.CategoryName;
                tbItemCategory.VerticalAlignment = VerticalAlignment.Center;
                tbItemCategory.TextAlignment = TextAlignment.Center;
                brItemCategory.Child = tbItemCategory;
                brItemCategory.Height = 25;
                brItemCategory.Background = rowBackground;
                Grid.SetColumn(brItemCategory, 5);
                Grid.SetRow(brItemCategory, 1);
                leastRentedCar.Children.Add(brItemCategory);

                //Column 7
                Border brItemCarType = new Border();
                TextBlock tbItemCarType = new TextBlock();
                tbItemCarType.Text = leastRented.CarTypeName;
                tbItemCarType.VerticalAlignment = VerticalAlignment.Center;
                tbItemCarType.TextAlignment = TextAlignment.Center;
                brItemCarType.Child = tbItemCarType;
                brItemCarType.Height = 25;
                brItemCarType.Background = rowBackground;
                Grid.SetColumn(brItemCarType, 6);
                Grid.SetRow(brItemCarType, 1);
                leastRentedCar.Children.Add(brItemCarType);

            }
        }
    }
}
