using FlowerShop.UsersFolder;
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
using System.Windows.Shapes;

namespace FlowerShop
{
    /// <summary>
    /// Логика взаимодействия для ChangePrise.xaml
    /// </summary>
    public partial class ChangePrise : Window
    {
        DBContext db;
        Flowers Flowers2 = new Flowers();
        public ChangePrise( Flowers x)
        {
            InitializeComponent();

            db = new DBContext();

            var collection = db.Flowers1.ToList();
            foreach (var item in collection)
            {
                if (item.Id == x.Id)
                {
                    Flowers2 = item;
                }
            }

            CommandBinding binding = new CommandBinding(ApplicationCommands.New);
            binding.Executed += new ExecutedRoutedEventHandler(SubM);
            Sub.CommandBindings.Add(binding);
        }

        private void SubM(object sender, ExecutedRoutedEventArgs e)
        {
            Flowers2.Price = MyButton.Text;
            db.SaveChanges();
            this.Close();
        }
        private void MyButton_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }
    }
}
