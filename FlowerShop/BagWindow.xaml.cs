using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
    /// Логика взаимодействия для BagWindow.xaml
    /// </summary>
    public partial class BagWindow : Window
    {
        DBContext db;
        private static readonly ImageConverter _imageConverter = new ImageConverter();
        public string _phone;
        public long _count;
        public BagWindow( string Phone)
        {
            InitializeComponent();
            _phone = Phone;

            db = new DBContext();
            db.Bags1.Load();

            CommandBinding binding = new CommandBinding(ApplicationCommands.New);
            binding.Executed += new ExecutedRoutedEventHandler(SendM);
            Send.CommandBindings.Add(binding);

            LoadFlowers();
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

        void myButton_Click(object sender, RoutedEventArgs e)
        {
            var colection = db.Bags1.ToList();
            Button button = (Button)sender;
            foreach (var item in colection)
            {
                if (item.Name == button.Name)
                {
                    db.Bags1.Remove(item);
                    _count = 0;
                    db.SaveChanges();
                    MyPanel.Children.Clear();
                    Count.Content = "";

                    LoadFlowers();

                    MessageBox.Show("Удален: " + item.Name);
                    break;
                }
            }

        }
        private void SendM(object sender, ExecutedRoutedEventArgs e)
        {
            HistoryFolder.Histiory histiory = new HistoryFolder.Histiory();

            histiory.UserPhone = _phone;
            histiory.Date = DateTime.Now.ToShortDateString();

             // отправитель - устанавливаем адрес и отображаемое в письме имя
            MailAddress from = new MailAddress("ilya060100@mail.ru", "Клиент");
            // кому отправляем
            MailAddress to = new MailAddress("ilya06010011599@gmail.com");
            // создаем объект сообщения
            MailMessage m = new MailMessage(from, to);
            // тема письма
            m.Subject = "Клиент:" + _phone;

            var x = db.Bags1.ToList();
            var y = db.Flowers1.ToList();
            foreach (var item in x)
            {
                if (item.UserPhone == _phone)
                {
                    try
                        
                    {
                        // текст письма
                        m.Body += "Товар:" + item.Name + " " + item.coutnFlow + " шт." + "<br>";
                        histiory.Items = "Товар:" + item.Name + " " + item.coutnFlow + " шт. ||";
                        foreach (var item1 in y)
                        {
                            if (item1.Name == item.Name)
                            {
                                item1.count = item1.count - Convert.ToInt32(item.coutnFlow);
                                
                            }

                        }
                        db.Bags1.Remove(item);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            db.Histiorie1.Add(histiory);
            db.SaveChanges();
            m.Body += "Общая сумма:" + _count + "р." + "";
            // письмо представляет код html
            m.IsBodyHtml = true;
            // адрес smtp-сервера и порт, с которого будем отправлять письмо
            SmtpClient smtp = new SmtpClient("smtp.mail.ru");
            // логин и пароль
            smtp.Credentials = new NetworkCredential("ilya060100@mail.ru", "vekmnbrb11599");
            smtp.EnableSsl = true;
            try
            {
                smtp.Send(m);
                MessageBox.Show("Ваш заказ принят, в скором времени оператор свяжется с вами!");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
            this.Close();

        }
        public void LoadFlowers()
        {
            var x = db.Bags1.ToList();
            foreach (var item in x)
            {
                if (item.UserPhone == _phone)
                {
                    StackPanel txtBox = new StackPanel();
                    txtBox.Name = "SP";
                    txtBox.Margin = new Thickness(5);
                    MyPanel.Children.Add(txtBox);//все остальные элементы добавляются по аналогии 
                    System.Windows.Controls.Image image1 = new System.Windows.Controls.Image();
                    image1.Width = 200;
                    image1.Height = 200;
                    Bitmap image = GetImageFromByteArray(item.Image);
                    image1.Source = BitmapToImageSource(image);

                    Label label = new Label();
                    label.Content = item.Name;
                    label.HorizontalContentAlignment = HorizontalAlignment.Center;

                    Label label1 = new Label();
                    label1.Content = item.Price + "р.";
                    label1.HorizontalContentAlignment = HorizontalAlignment.Center;

                    Label label2 = new Label();
                    label2.Content = item.coutnFlow + "шт.";
                    label2.HorizontalContentAlignment = HorizontalAlignment.Center;

                    Button button = new Button();
                    button.Name = item.Name;
                    button.Content = "удалить!";
                    button.Click += myButton_Click;

                    txtBox.Children.Add(image1);
                    txtBox.Children.Add(label);
                    txtBox.Children.Add(label1);
                    txtBox.Children.Add(label2);
                    txtBox.Children.Add(button);
                    try
                    {
                        _count += Convert.ToInt64(item.Price) * Convert.ToInt32( item.coutnFlow);
                        Count.Content = _count + "р.";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
    }
}
