using FlowerShop.UsersFolder;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using FlowerShop.AdminFloder;
using FlowerShop.BagFolder;
using System.Data.Entity;
using System.Linq;
using System.Windows.Input;
using Img = System.Drawing.Image;
using Microsoft.Win32;
using System.Windows.Controls;
using System;

namespace FlowerShop
{
    public partial class Main : Window
    {
        DBContext db;
        private static readonly ImageConverter _imageConverter = new ImageConverter();
        UsersFolder.Users user = new UsersFolder.Users();
        public string b;
        public Main(string a)
        {
            InitializeComponent();

            db = new DBContext();
            db.Admin1.Load();

            db.Bags1.Load();

            db.Flowers1.Load();

            db.Users1.Load();

            b = a;


            if (CheckAdmin(a) == true)
            {
                AB1.Visibility = Visibility.Visible;
                AB2.Visibility = Visibility.Visible;
                AB3.Visibility = Visibility.Visible;

            }
            else if (CheckUser(a) == true)
            {
                UName.Content = user.Uname;
                
            }

            CommandBinding binding = new CommandBinding(ApplicationCommands.New);
            binding.Executed += new ExecutedRoutedEventHandler(ExitM);
            Exit.CommandBindings.Add(binding);

            CommandBinding binding1 = new CommandBinding(ApplicationCommands.New);
            binding1.Executed += new ExecutedRoutedEventHandler(AB1M);
            AB1.CommandBindings.Add(binding1);

            CommandBinding binding2 = new CommandBinding(ApplicationCommands.New);
            binding2.Executed += new ExecutedRoutedEventHandler(BBagM);
            BBag.CommandBindings.Add(binding2);

            CommandBinding binding3 = new CommandBinding(ApplicationCommands.New);
            binding3.Executed += new ExecutedRoutedEventHandler(RFiltrM);
            RFiltr.CommandBindings.Add(binding3);

            CommandBinding binding4 = new CommandBinding(ApplicationCommands.New);
            binding4.Executed += new ExecutedRoutedEventHandler(TFiltrM);
            TFiltr.CommandBindings.Add(binding4);

            CommandBinding binding5 = new CommandBinding(ApplicationCommands.New);
            binding5.Executed += new ExecutedRoutedEventHandler(BFiltrM);
            BFiltr.CommandBindings.Add(binding5);

            CommandBinding binding6 = new CommandBinding(ApplicationCommands.New);
            binding6.Executed += new ExecutedRoutedEventHandler(AFiltrM);
            AFiltr.CommandBindings.Add(binding6);

            CommandBinding binding7 = new CommandBinding(ApplicationCommands.New);
            binding7.Executed += new ExecutedRoutedEventHandler(CAccM);
            CAcc.CommandBindings.Add(binding7);

            CommandBinding binding8 = new CommandBinding(ApplicationCommands.New);
            binding8.Executed += new ExecutedRoutedEventHandler(AB2M);
            AB2.CommandBindings.Add(binding8);


            CommandBinding binding9 = new CommandBinding(ApplicationCommands.New);
            binding9.Executed += new ExecutedRoutedEventHandler(AB3M);
            AB3.CommandBindings.Add(binding9);

            CommandBinding binding10 = new CommandBinding(ApplicationCommands.New);
            binding10.Executed += new ExecutedRoutedEventHandler(MS);
            MessageSend.CommandBindings.Add(binding10);

            CommandBinding binding11 = new CommandBinding(ApplicationCommands.New);
            binding11.Executed += new ExecutedRoutedEventHandler(MVM);
            MV.CommandBindings.Add(binding11);

            LoadFlowers();
        }
        void myButton_Click(object sender, RoutedEventArgs e)
        {
            var colection = db.Flowers1.ToList();
            Bag korzina = new Bag();
            Button button = (Button)sender;
            foreach (var item in colection)
            {
                if (item.Name == button.Name)
                {
                    //UsersFolder.Users users = db.Users1.Where(item1 => item1.UPhone.Equals(user.UPhone)).FirstOrDefault();
                    //korzina.Users2 = users;
                    korzina.Name = item.Name;
                    korzina.Price = item.Price;
                    korzina.UserPhone = user.UPhone;
                    korzina.Type = item.Type;
                    korzina.Image = item.Image;
                    korzina.count = item.count;
                    if (korzina.count <= 0)
                    {
                        MessageBox.Show("Нет в наличии!");
                        break;
                    }
                    else
                    {
                        Count count = new Count(korzina);
                        count.ShowDialog();
                       
                        break;
                    }
                }
            }
            colection.Clear();

        }
        private void ExitM(object sender, ExecutedRoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
        private void MVM(object sender, ExecutedRoutedEventArgs e)
        {
            Otzivi otzivi = new Otzivi();
            otzivi.ShowDialog();
        }
        private void BBagM(object sender, ExecutedRoutedEventArgs e)
        {
            BagWindow bagWindow = new BagWindow(user.UPhone);
            bagWindow.ShowDialog();
            MyPanel.Children.Clear();
            LoadFlowers();
        }
        private void AB1M(object sender, ExecutedRoutedEventArgs e)
        {
            AddFlower addFlower = new AddFlower(MyPanel);
            addFlower.Show();
        }
        private void AB2M(object sender, ExecutedRoutedEventArgs e)
        {
            DellFlofers dellFlofers = new DellFlofers();
            dellFlofers.ShowDialog();
            MyPanel.Children.Clear();
            LoadFlowers();
        }
        private void AB3M(object sender, ExecutedRoutedEventArgs e)
        {
            AllUsers allUsers = new AllUsers();
            allUsers.ShowDialog();
        }
        public static Bitmap GetImageFromByteArray(byte[] byteArray)
        {
            Bitmap bm = (Bitmap)_imageConverter.ConvertFrom(byteArray);

            if (bm != null && (bm.HorizontalResolution != (int)bm.HorizontalResolution ||
                               bm.VerticalResolution != (int)bm.VerticalResolution))
            {
                // Correct a strange glitch that has been observed in the test program when converting 
                //  from a PNG file image created by CopyImageToByteArray() - the dpi value "drifts" 
                //  slightly away from the nominal integer value
                bm.SetResolution((int)(bm.HorizontalResolution + 0.5f),
                                 (int)(bm.VerticalResolution + 0.5f));
            }

            return bm;
        }
        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        public bool CheckAdmin(string str)
        {
            var collection = db.Admin1.ToList();
            foreach (var item in collection)
            {
                if (item.APhone == str)
                {
                    UName.Content = item.AName;
                    return true;
                }
            }
            return false;
        }

        public bool CheckUser(string str)
        {
            var collection = db.Users1.ToList();
            foreach (var item in collection)
            {
                if (item.UPhone == str)
                {
                    user.Uname = item.Uname;
                    user.UPhone = item.UPhone;
                    if(item.Image != null)
                    {
                        Bitmap image = GetImageFromByteArray(item.Image);
                        UImage.Source = BitmapToImageSource(image);
                        UImage.Width = 150;
                        UImage.Height = 150;
                    }
                    return true;
                }
            }
            return false;
        }
        private void AFiltrM(object sender, ExecutedRoutedEventArgs e)
        {
            MyPanel.Children.Clear();
            LoadFlowers();
        }
        private void RFiltrM(object sender, ExecutedRoutedEventArgs e)
        {
            db = new DBContext();
            db.Flowers1.Load();
            MyPanel.Children.Clear();
            var x = db.Flowers1.ToList();
            foreach (var item in x)
            {
                if (item.Type == "Роза" && item.count > 0)
                {
                    StackPanel txtBox = new StackPanel();
                    txtBox.Name = "SP";
                    txtBox.Margin = new Thickness(10);
                    MyPanel.Children.Add(txtBox);//все остальные элементы добавляются по аналогии 
                    System.Windows.Controls.Image image1 = new System.Windows.Controls.Image();
                    image1.Width = 200;
                    image1.Height = 200;
                    Bitmap image = GetImageFromByteArray(item.Image);
                    image1.Source = BitmapToImageSource(image);

                    Label label = new Label();
                    label.Content = item.Type;
                    label.FontSize = 15;
                    label.HorizontalContentAlignment = HorizontalAlignment.Center;

                    Label label1 = new Label();
                    label1.Content = "Цена: " + item.Price + "p";
                    label1.FontSize = 15;
                    label1.HorizontalContentAlignment = HorizontalAlignment.Center;

                    Button button = new Button();
                    button.Name = item.Name;
                    button.FontSize = 15;
                    button.Content = "Добавить в корзину";
                    button.Click += myButton_Click;



                    txtBox.Children.Add(image1);
                    txtBox.Children.Add(label);
                    txtBox.Children.Add(label1);
                    txtBox.Children.Add(button);
                }
            }
        }

        private void BFiltrM(object sender, ExecutedRoutedEventArgs e)
        {
            db = new DBContext();
            db.Flowers1.Load();
            MyPanel.Children.Clear();
            var x = db.Flowers1.ToList();
            foreach (var item in x)
            {
                if (item.Type == "Гвоздика" && item.count > 0)
                {
                    StackPanel txtBox = new StackPanel();
                    txtBox.Name = "SP";
                    txtBox.Margin = new Thickness(10);
                    MyPanel.Children.Add(txtBox);//все остальные элементы добавляются по аналогии 
                    System.Windows.Controls.Image image1 = new System.Windows.Controls.Image();
                    image1.Width = 200;
                    image1.Height = 200;
                    Bitmap image = GetImageFromByteArray(item.Image);
                    image1.Source = BitmapToImageSource(image);

                    Label label = new Label();
                    label.Content = item.Type;
                    label.FontSize = 15;
                    label.HorizontalContentAlignment = HorizontalAlignment.Center;

                    Label label1 = new Label();
                    label1.Content = "Цена: " + item.Price + ".p";
                    label1.FontSize = 15;
                    label1.HorizontalContentAlignment = HorizontalAlignment.Center;

                    Button button = new Button();
                    button.Name = item.Name;
                    button.FontSize = 15;
                    button.Content = "Добавить в корзину";
                    button.Click += myButton_Click;



                    txtBox.Children.Add(image1);
                    txtBox.Children.Add(label);
                    txtBox.Children.Add(label1);
                    txtBox.Children.Add(button);
                }
            }
        }
        private void TFiltrM(object sender, ExecutedRoutedEventArgs e)
        {
            db = new DBContext();
            db.Flowers1.Load();
            MyPanel.Children.Clear();
            var x = db.Flowers1.ToList();
            foreach (var item in x)
            {
                if (item.Type == "Тюльпан" && item.count > 0)
                {
                    StackPanel txtBox = new StackPanel();
                    txtBox.Name = "SP";
                    txtBox.Margin = new Thickness(10);
                    MyPanel.Children.Add(txtBox);//все остальные элементы добавляются по аналогии 
                    System.Windows.Controls.Image image1 = new System.Windows.Controls.Image();
                    image1.Width = 200;
                    image1.Height = 200;
                    Bitmap image = GetImageFromByteArray(item.Image);
                    image1.Source = BitmapToImageSource(image);

                    Label label = new Label();
                    label.Content = item.Type;
                    label.FontSize = 15;
                    label.HorizontalContentAlignment = HorizontalAlignment.Center;

                    Label label1 = new Label();
                    label1.Content = "Цена: " + item.Price + ".p";
                    label1.FontSize = 15;
                    label1.HorizontalContentAlignment = HorizontalAlignment.Center;

                    Button button = new Button();
                    button.Name = item.Name;
                    button.FontSize = 15;
                    button.Content = "Добавить в корзину";
                    button.Click += myButton_Click;



                    txtBox.Children.Add(image1);
                    txtBox.Children.Add(label);
                    txtBox.Children.Add(label1);
                    txtBox.Children.Add(button);
                }
            }
        }
        private void CAccM(object sender, ExecutedRoutedEventArgs e)
        {
            if(MessageBox.Show("После изменения произойдет выход из профиля!", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                ChenchAccount chenchAccount = new ChenchAccount(user.Uname);
                chenchAccount.ShowDialog();
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }
        public void LoadFlowers()
        {
            db = new DBContext();
            db.Flowers1.Load();
            var x = db.Flowers1.ToList();
            foreach (var item in x)
            {
                if (item.count > 0)
                {
                    StackPanel txtBox = new StackPanel();
                    txtBox.Name = "SP";
                    txtBox.Margin = new Thickness(10);
                    MyPanel.Children.Add(txtBox);//все остальные элементы добавляются по аналогии 
                    System.Windows.Controls.Image image1 = new System.Windows.Controls.Image();
                    image1.Width = 200;
                    image1.Height = 200;
                    try
                    {
                        Bitmap image = GetImageFromByteArray(item.Image);
                        image1.Source = BitmapToImageSource(image);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    Label label = new Label();
                    label.Content = item.Type;
                    label.FontSize = 15;
                    label.HorizontalContentAlignment = HorizontalAlignment.Center;

                    Label label1 = new Label();
                    label1.FontSize = 15;
                    label1.Content = "Цена: " + item.Price + "р.";
                    label1.HorizontalContentAlignment = HorizontalAlignment.Center;

                    Button button = new Button();
                    button.Name = item.Name;
                    button.FontSize = 15;
                    button.Content = "Добавить в корзину";
                    button.Click += myButton_Click;



                    txtBox.Children.Add(image1);
                    txtBox.Children.Add(label);
                    txtBox.Children.Add(label1);
                    txtBox.Children.Add(button);
                }
            }
        }
        private void MS(object sender, ExecutedRoutedEventArgs e)
        {
            MessageS messageS = new MessageS(user.Uname);
            messageS.ShowDialog();
        }
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            InfoWindow infoWindow = new InfoWindow();
            infoWindow.ShowDialog();
        }
    }
    }
