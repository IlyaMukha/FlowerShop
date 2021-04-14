using FlowerShop.MessegeFolder;
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
    /// Логика взаимодействия для MessageS.xaml
    /// </summary>
    public partial class MessageS : Window
    {
        DBContext db;
        string y;
        public MessageS(string x)
        {
            InitializeComponent();

            db = new DBContext();

            db.Message1.Load();

            db.Users1.Load();

            y = x;

            CommandBinding binding = new CommandBinding(ApplicationCommands.New);
            binding.Executed += new ExecutedRoutedEventHandler(SM);
            Send.CommandBindings.Add(binding);
        }
        private void SM(object sender, ExecutedRoutedEventArgs e)
        {
            Message message = new Message();
            message.message1 = MessageOne.Text;
            message.tell = y;
            db.Message1.Add(message);
            db.SaveChanges();
            MessageBox.Show("Спасибо за отзыв");
            this.Close();
        }
    }
}
