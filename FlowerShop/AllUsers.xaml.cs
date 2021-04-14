using FlowerShop.UsersFolder;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
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
    /// Логика взаимодействия для AllUsers.xaml
    /// </summary>
    public partial class AllUsers : Window
    {
        DBContext db;
        private static readonly ImageConverter _imageConverter = new ImageConverter();
        public AllUsers()
        {
            InitializeComponent();
            db = new DBContext();

            db.Flowers1.Load();
            LoadUsers();
        }
        public void LoadUsers()
        {
            db = new DBContext();
            db.Flowers1.Load();
            var x = db.Flowers1.ToList();
            foreach (var item in x)
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
                    label1.Content = item.Price + "р.";
                    label1.HorizontalContentAlignment = HorizontalAlignment.Center;

                Button button1 = new Button();
                button1.Name = item.Name + "12";
                button1.FontSize = 15;
                button1.Content = "Изменить цену";
                button1.HorizontalAlignment = HorizontalAlignment.Center;
                button1.Width = 160;
                button1.Click += myButtont_Click1;

                    Button button = new Button();
                    button.Name = item.Name;
                    button.FontSize = 15;
                    button.Content = "Добавить количество";
                    button.HorizontalAlignment = HorizontalAlignment.Center;
                button.Width = 160;
                    button.Click += myButton_Click;

                    Label label2 = new Label();
                    label2.Content = "Количетво на складе";
                label2.FontSize = 15;
                label2.HorizontalContentAlignment = HorizontalAlignment.Center;

                    Label label3 = new Label();
                    label3.Content = item.count + " ШТ.";
                label3.FontSize = 15;
                label3.HorizontalContentAlignment = HorizontalAlignment.Center;



                txtBox.Children.Add(image1);
                    txtBox.Children.Add(label);
                    txtBox.Children.Add(label1);
                    txtBox.Children.Add(label2);
                    txtBox.Children.Add(label3);
                    txtBox.Children.Add(button);
                    txtBox.Children.Add(button1);
                
            }
            }

        private void myButtont_Click1(object sender, RoutedEventArgs e)
        {
            var collection = db.Flowers1.ToList();
            Button button = (Button)sender;
            foreach (var item in collection)
            {
                if (item.Name + "12" == button.Name)
                {
                    ChangePrise changePrise = new ChangePrise(item);
                    changePrise.ShowDialog();
                    MyPanel.Children.Clear();
                    LoadUsers();
                }
            }
        }

        void myButton_Click(object sender, RoutedEventArgs e)
        {
            var collection = db.Flowers1.ToList();
            Button button = (Button)sender;
            foreach (var item in collection)
            {
                if (item.Name == button.Name)
                {
                    AddCount addCount = new AddCount(item);
                    addCount.ShowDialog();
                    MyPanel.Children.Clear();
                    LoadUsers();
                }
            }
           
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
    }
}
