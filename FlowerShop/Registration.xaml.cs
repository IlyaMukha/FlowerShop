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
using FlowerShop.UsersFolder;

namespace FlowerShop
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        DBContext db;
        public Registration()
        {
            InitializeComponent();

            db = new DBContext();

            db.Users1.Load();

            CommandBinding binding = new CommandBinding(ApplicationCommands.New);
            binding.Executed += new ExecutedRoutedEventHandler(BLoginM);
            BLogin.CommandBindings.Add(binding);

            CommandBinding binding1 = new CommandBinding(ApplicationCommands.New);
            binding1.Executed += new ExecutedRoutedEventHandler(BRegistrationM);
            BRegistration.CommandBindings.Add(binding1);

        }
        private void BLoginM(object sender, ExecutedRoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void BRegistrationM(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                if(CheckPhone(UPhone.Text) == false && CheckPhone1(UPhone.Text) == true && CheckPassword(UPassword.Password, UPassword1.Password))
                {
                    try
                    {
                        if (UName.Text == "")
                        {
                            MessageBox.Show("Не все поля заполнены!");
                        }
                        else
                        {
                            UsersFolder.Users AddUser = new UsersFolder.Users();
                            AddUser.UPhone = UPhone.Text;
                            AddUser.Uname = UName.Text;
                            AddUser.UPassword = Convert.ToString(UPassword.Password.GetHashCode());
                            db.Users1.Add(AddUser);
                            db.SaveChanges();
                            MessageBox.Show(UName.Text + " спасибо за регистрацию!");
                            Main main = new Main(UPhone.Text);
                            main.Show();
                            this.Close();
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public bool CheckPhone(string str1)
        {
            try
            {
               string str = str1.Remove(0, 1);
                if (str.Length != 12)
                {
                    MessageBox.Show("Телефон не может быть: +" + str);
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        public bool CheckPhone1 (string str)
        {
            var collection = db.Users1.ToList();
            foreach (var item in collection)
            {
                if (item.UPhone == str)
                {
                    MessageBox.Show("Данный номер телефона уже зарегестрирован!");
                    return false;
                }
            }
            return true;
        }
        public bool CheckPassword(string str, string str1)
        {
            if(str.Length < 6)
            {
                MessageBox.Show("Пароль должен иметь не менее 6 символов!");
                return false;
            }
            else
            {
                if (str != str1)
                {
                    MessageBox.Show("Пароли не совпадают!");
                    return false;
                }
                else
                    return true;
            }
        }

        private void UPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }
    }
}
