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
using System.Windows.Navigation;
using System.Windows.Shapes;
using FlowerShop.UsersFolder;
using FlowerShop.AdminFloder;

namespace FlowerShop
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DBContext db;
        public MainWindow()
        {
            InitializeComponent();

            db = new DBContext();
            db.Admin1.Load();

            db.Users1.Load();

            CommandBinding binding = new CommandBinding(ApplicationCommands.New);
            binding.Executed += new ExecutedRoutedEventHandler(BRegistrationM);
            BRegistration.CommandBindings.Add(binding);

            CommandBinding binding1 = new CommandBinding(ApplicationCommands.New);
            binding1.Executed += new ExecutedRoutedEventHandler(BEnterM);
            BEnter.CommandBindings.Add(binding1);
        }

        private void BRegistrationM(object sender, ExecutedRoutedEventArgs e)
        {
            Registration registration = new Registration();
            registration.Show();
            this.Close();
        }

        private void BEnterM(object sender, ExecutedRoutedEventArgs e)
        {
            var collection = db.Admin1.ToList();
            foreach (var item in collection)
            {
                if (UPhone.Text == item.AName && UPassword.Password == item.APassword)
                {
                    Main main = new Main(item.APhone);
                    main.Show();
                    this.Close();
                }
            }
            if(CheckLoogin(UPhone.Text, Convert.ToString(UPassword.Password.GetHashCode())) == true)
            {
                Main main = new Main(UPhone.Text);
                main.Show();
                this.Close();
            }
        }

        public bool CheckLoogin(string phone, string pass)
        {
            var collection = db.Users1.ToList();
            foreach (var item in collection)
            {
                if(item.UPhone == phone && item.UPassword == pass)
                {
                    return true;
                }
            }
            if (CheckPhoneis(phone) == -1)
            {
                MessageBox.Show("Пользователь с телефоном " + phone + " не зарегестрирован!");
                return false;
            }
            else
            {
                MessageBox.Show("Не верный пароль!");
                return false;
            }
        }

        public int CheckPhoneis(string phone)
        {
            var collection = db.Users1.ToList();
            foreach (var item in collection)
            {
                if (item.UPhone == phone)
                {
                    return 1;
                }
            }
            return -1;
        }
    }
}
