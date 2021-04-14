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
    /// Логика взаимодействия для Otzivi.xaml
    /// </summary>
    public partial class Otzivi : Window
    {
        DBContext db;
        public Otzivi()
        {
            InitializeComponent();

            db = new DBContext();

            db.Message1.Load();

            LoadMess();
        }
        public void LoadMess()
        {
            db = new DBContext();
            db.Message1.Load();
            var x = db.Message1.ToList();
            foreach (var item in x)
            {
                    StackPanel txtBox = new StackPanel();
                    txtBox.Name = "SP";
                    txtBox.Margin = new Thickness(10);
                txtBox.Background = Brushes.White;
                    MyPanel.Children.Add(txtBox);//все остальные элементы добавляются по аналогии 

                    Label label = new Label();
                    label.Content = "Клиент - " + item.tell;
                    label.FontSize = 15;
                    label.HorizontalContentAlignment = HorizontalAlignment.Center;

                    Label label1 = new Label();
                    label1.FontSize = 15;
                    label1.Content = "Отзыв: " + item.message1; 
                    label1.HorizontalContentAlignment = HorizontalAlignment.Center;

                    txtBox.Children.Add(label);
                    txtBox.Children.Add(label1);
            }
        }
    }
}
