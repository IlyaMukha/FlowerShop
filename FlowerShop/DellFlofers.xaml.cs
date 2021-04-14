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
    /// Логика взаимодействия для DellFlofers.xaml
    /// </summary>
    public partial class DellFlofers : Window
    {
        DBContext db;
        private static readonly ImageConverter _imageConverter = new ImageConverter();
        public DellFlofers()
        {
            InitializeComponent();
            db = new DBContext();

            db.Flowers1.Load();
            LoadFlowers();
            
        }
        public void LoadFlowers()
        {
            var x = db.Flowers1.ToList();
            foreach (var item in x)
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
                label.Content = item.Type;
                label.FontSize = 15;
                label.HorizontalContentAlignment = HorizontalAlignment.Center;

                Label label1 = new Label();
                label1.Content = item.Price + ".p";
                label1.FontSize = 15;
                label1.HorizontalContentAlignment = HorizontalAlignment.Center;

                Button button = new Button();
                button.Name = item.Name;
                button.Content = "Удалить";
                button.Click += myButton_Click;

                Label label2 = new Label();
                label2.Content = "Осталось  на складе";
                label2.FontSize = 15;
                label2.HorizontalContentAlignment = HorizontalAlignment.Center;

                Label label3 = new Label();
                label3.Content = item.count + "ШТ.";
                label3.FontSize = 15;
                label3.HorizontalContentAlignment = HorizontalAlignment.Center;



                txtBox.Children.Add(image1);
                txtBox.Children.Add(label);
                txtBox.Children.Add(label1);
                txtBox.Children.Add(label2);
                txtBox.Children.Add(label3);

                txtBox.Children.Add(button);
            }
        }
        void myButton_Click(object sender, RoutedEventArgs e)
        {
            var colection = db.Flowers1.ToList();
            Button button = (Button)sender;
            foreach (var item in colection)
            {
                if (item.Name == button.Name)
                {
                    db.Flowers1.Remove(item);
                    db.SaveChanges();
                    MyPanel.Children.Clear();

                    LoadFlowers();

                    MessageBox.Show("Удален: " + item.Name);
                    break;
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
