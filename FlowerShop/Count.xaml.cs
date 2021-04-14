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
using FlowerShop.BagFolder;

namespace FlowerShop
{
    /// <summary>
    /// Логика взаимодействия для Count.xaml
    /// </summary>
    public partial class Count : Window
    {
        DBContext db;

        Bag bag1 = new Bag();
        public Count(Bag bag )
        {
            InitializeComponent();

            bag1 = bag;

            db = new DBContext();

            db.Bags1.Load();

            CommandBinding binding = new CommandBinding(ApplicationCommands.New);
            binding.Executed += new ExecutedRoutedEventHandler(SubM);
            Sub.CommandBindings.Add(binding);
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
        }
        private void SubM(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                bag1.coutnFlow = MyButton.Text;
                db.Bags1.Add(bag1);
                db.SaveChanges();

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Slider1_Loaded(object sender, RoutedEventArgs e)
        {
            Slider1.Minimum = 0;
            Slider1.Maximum = bag1.count;
        }
    }
}
