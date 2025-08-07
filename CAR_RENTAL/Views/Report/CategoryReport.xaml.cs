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
    /// Interaction logic for CategoryReport.xaml
    /// </summary>
    public partial class CategoryReport : UserControl
    {
        public CategoryReport()
        {
            InitializeComponent();
            LoadCategoryReport();
        }
        private void LoadCategoryReport()
        {
            try
            {
                var items = CategoryRepository.Instance.GetAll();
                showTotalCate.Text = items.Count.ToString();

                var itemsActive = CategoryRepository.Instance.CountByStatus("Active");
                showTotalActive.Text = itemsActive.Count.ToString();

                var itemsDeactive = CategoryRepository.Instance.CountByStatus("Deactive");
                showTotalDeactive.Text = itemsDeactive.Count.ToString();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
