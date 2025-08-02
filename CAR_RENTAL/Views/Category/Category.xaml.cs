using CAR_RENTAL.Components;
using CAR_RENTAL.Model.ModalViews.Category;
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

namespace CAR_RENTAL.Views.Category
{
    /// <summary>
    /// Interaction logic for Category.xaml
    /// </summary>
    public partial class Category : UserControl
    {
        public Category()
        {
            InitializeComponent();
            LoadCates();
        }

        private void btnAddCategory(object sender, RoutedEventArgs e)
        {
            if(inputTitle.Text.Length < 6)
            {
                MessageBox.Show("Title too short (at least 6 char), please try again!");
            }
            else
            {
                CategoryView item = new CategoryView();
                item.Title = inputTitle.Text;
                item.Description = inputDescription.Text;
                item.Active = inputActive.IsChecked == true ? 1 : 0;
                CategoryRepository.Instance.Create(item);
                if (item.ID > 0)
                {
                    inputTitle.Text = "";
                    inputDescription.Text = "";
                    inputActive.IsChecked = false;
                    MessageBox.Show("Add successful!", "Note");
                    LoadCates();
                }
            }    
        }


        public void LoadCates()
        {
            var items = CategoryRepository.Instance.GetAll();
            int index = 0;
            foreach(var item in items)
            {
                BrushConverter bc = new BrushConverter();
                Brush rowBackground = (Brush)bc.ConvertFromString("#DFFFD6");
                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(25);
                Cates.RowDefinitions.Add(rowDefinition);
                //Set column 1
                Border brItem = new Border();
                TextBlock tbItem = new TextBlock();
                tbItem.Text = (index+1).ToString();
                tbItem.TextAlignment = TextAlignment.Center;
                tbItem.VerticalAlignment = VerticalAlignment.Center;
                brItem.Child = tbItem;
                brItem.Height = 25;
                brItem.Background = rowBackground;
                Grid.SetColumn(brItem, 0);
                Grid.SetRow(brItem, index);
                Cates.Children.Add(brItem);

                //Set column 2
                Border brItemTitle = new Border();
                TextBlock tbItemTitle = new TextBlock();
                tbItemTitle.Text = item.Title;
                tbItemTitle.TextAlignment = TextAlignment.Center;
                tbItemTitle.VerticalAlignment = VerticalAlignment.Center;
                brItemTitle.Child = tbItemTitle;
                brItemTitle.Height = 25;
                brItemTitle.Background = rowBackground;
                Grid.SetColumn(brItemTitle, 1);
                Grid.SetRow(brItemTitle, index);
                Cates.Children.Add(brItemTitle);

                //Set Column 3
                Border brItemDescription = new Border();
                TextBlock tbItemDescription = new TextBlock();
                tbItemDescription.Text = item.Description;
                tbItemDescription.TextAlignment = TextAlignment.Center;
                tbItemDescription.VerticalAlignment = VerticalAlignment.Center;
                brItemDescription.Child = tbItemDescription;
                brItemDescription.Height = 25;
                brItemDescription.Background = rowBackground;
                Grid.SetColumn(brItemDescription, 2);
                Grid.SetRow(brItemDescription, index);
                Cates.Children.Add(brItemDescription);

                //Set Column 4
                Border brItemActive = new Border();
                MyCheckBox cbItemActive = new MyCheckBox();
                cbItemActive.IsChecked = (item.Active == 1 ? true : false);
                cbItemActive.ValuePrimaryKey = item.ID;
                cbItemActive.VerticalAlignment = VerticalAlignment.Center;
                cbItemActive.HorizontalAlignment = HorizontalAlignment.Center;
                cbItemActive.IsEnabled = false;
                brItemActive.Child = cbItemActive;
                brItemActive.Height = 25;
                brItemActive.Background = rowBackground;
                Grid.SetColumn(brItemActive, 3);
                Grid.SetRow(brItemActive, index);
                Cates.Children.Add(brItemActive);

                //Set Column 5 (Action)
                Grid grItemAction = new Grid();
                ColumnDefinition colEdit = new ColumnDefinition();
                ColumnDefinition colDelete = new ColumnDefinition();
                grItemAction.ColumnDefinitions.Add(colEdit);
                grItemAction.ColumnDefinitions.Add(colDelete);
                grItemAction.Background = rowBackground;
                Button btnEdit = new Button();
                btnEdit.Content = "Edit";
                btnEdit.Width = 50;
                btnEdit.Click += (object sender, RoutedEventArgs e) =>
                {
                    var categoryEdit = new CategoryEdit(item.ID)
                    {
                        Owner = Window.GetWindow(this)
                    };
                    bool? result = categoryEdit.ShowDialog();
                    if(result == true)
                    {
                        MessageBox.Show("Edit successful!", "Status");
                        Cates.RowDefinitions.Clear();
                        Cates.Children.Clear();
                        LoadCates();
                    }
                    else
                    {
                        MessageBox.Show("Edit fail, please try again!","Status");
                    }
                };
                Grid.SetColumn(btnEdit, 0);
                grItemAction.Children.Add(btnEdit);
                Button btnDelete = new Button();
                btnDelete.Content = "Delete";
                btnDelete.Width = 50;
                btnDelete.Click += (object sender, RoutedEventArgs e) =>
                {
                    var confirm = MessageBox.Show($"Are you sure to Delete this Category ?", "Confirm Delete",
                                                    MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if(confirm == MessageBoxResult.Yes)
                    {
                        CategoryView itemDelete = new CategoryView();
                        itemDelete.ID = item.ID;
                        var checkDel = CategoryRepository.Instance.Delete(itemDelete);
                        if (checkDel)
                        {
                            MessageBox.Show("Delete successful", "Status");
                            Cates.RowDefinitions.Clear();
                            Cates.Children.Clear();
                            LoadCates();
                        }
                        else
                        {
                            MessageBox.Show("Delete fail, please try again!", "Status");
                        }

                    }
                };
                Grid.SetColumn(btnDelete, 1);
                grItemAction.Children.Add(btnDelete);
                Grid.SetColumn(grItemAction, 4);
                Grid.SetRow(grItemAction, index);
                Cates.Children.Add(grItemAction);


                index++;


            }
        }
    }
}
