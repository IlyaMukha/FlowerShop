using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace FlowerShop
{
    /// <summary>
    /// Логика взаимодействия для AHistioryw.xaml
    /// </summary>
    public partial class AHistioryw : Window
    {
        DBContext db;
        public AHistioryw()
        {
            InitializeComponent();

            db = new DBContext();

            db.Histiorie1.Load();

            LoadHistory();
        }

        public void LoadHistory()
        {
            db = new DBContext();
            db.Histiorie1.Load();
            var x = db.Histiorie1.ToList();
            foreach (var item in x)
            {
                StackPanel txtBox = new StackPanel();
                txtBox.Name = "SP";
                txtBox.Margin = new Thickness(10);
                txtBox.Background = Brushes.White;
                MyPanel.Children.Add(txtBox);//все остальные элементы добавляются по аналогии 

                Label label = new Label();
                label.Content = "Клиент - " + item.UserPhone;
                label.FontSize = 15;
                label.HorizontalContentAlignment = HorizontalAlignment.Center;

                Label label1 = new Label();
                label1.FontSize = 15;
                label1.Content = "Дата: " + item.Date;
                label1.HorizontalContentAlignment = HorizontalAlignment.Center;

                Label label2 = new Label();
                label2.FontSize = 15;
                label2.Content = item.Items;
                label2.HorizontalContentAlignment = HorizontalAlignment.Center;

                txtBox.Children.Add(label);
                txtBox.Children.Add(label1);
                txtBox.Children.Add(label2);
            }
        }
    }
}
