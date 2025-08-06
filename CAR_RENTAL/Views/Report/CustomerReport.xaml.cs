using CAR_RENTAL.Model.ModalViews.Customer;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CAR_RENTAL.Views.Report
{
    /// <summary>
    /// Interaction logic for CustomerReport.xaml
    /// </summary>
    public partial class CustomerReport : UserControl
    {
        public CustomerReport()
        {
            InitializeComponent();
            DateTime toDay = DateTime.Now;
            choseDate.SelectedDate = toDay;
            for (int i = 1; i <= 12; i++)
            {
                ComboBoxItem month = new ComboBoxItem();
                month.Content = "Tháng " + i.ToString();
                month.Tag = i;
                choseMonth.Items.Add(month);
                choseMonth.SelectedIndex = toDay.Month - 1;
            }
        }

        private void Reload(object sender, RoutedEventArgs e)
        {
            if(choseMonth.SelectedItem != null || choseDate.SelectedDate.Value != null)
            {
                LoadCustomerReport();
            }
            else
            {
                return;
            }
        }

        public void LoadCustomerReport()
        {
            try
            {
                DateTime toDay = new DateTime();            
                HashSet<CustomerView> cus = new HashSet<CustomerView>();
                cus = CustomerRepository.Instance.GetAll();
                showTotalCus.Text = cus.Count.ToString();
                if(choseDate != null)
                {
                    var items = CustomerRepository.Instance.FindCreateByDate(choseDate.SelectedDate.Value);
                    showTotalNewDate.Text = items.Count.ToString();
                }
                ComboBoxItem selectedMonth = choseMonth.SelectedItem as ComboBoxItem;
                int selectMonth = Convert.ToInt32(selectedMonth.Tag);
                var itemsMonth = CustomerRepository.Instance.FindCreateByMonth(selectMonth);
                showTotalNewMonth.Text = itemsMonth.Count.ToString();

                var active = CustomerRepository.Instance.FindTotalActive();
                showTotalActive.Text = active.Count.ToString();
                var deActive = CustomerRepository.Instance.FindTotalDeactive();
                showTotalDeactive.Text = deActive.Count.ToString();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        
    }
}
