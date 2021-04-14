using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Img = System.Drawing.Image;
using System.Drawing;
using FlowerShop.UsersFolder;
using System.Data.Entity;

namespace FlowerShop
{
    /// <summary>
    /// Логика взаимодействия для AddFlower.xaml
    /// </summary>
    public partial class AddFlower : Window
    {
        DBContext db;
        Flowers collection = new Flowers();
        private static readonly ImageConverter _imageConverter = new ImageConverter();
        System.Windows.Controls.WrapPanel y;
        public bool prow = true;
        public AddFlower(System.Windows.Controls.WrapPanel x)
        {
            InitializeComponent();

            db = new DBContext();
            db.Flowers1.Load();

            y = x;

            CommandBinding binding = new CommandBinding(ApplicationCommands.New);
            binding.Executed += new ExecutedRoutedEventHandler(AddM);
            Add.CommandBindings.Add(binding);
        }
        private void AddM(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                collection.Name = FName.Text;
                collection.Price = FPrice.Text;
                collection.Type = CBType.Text;
                collection.count = Convert.ToInt32( FCount.Text);
                db.Flowers1.Add(collection);
                db.SaveChanges();

                StackPanel txtBox = new StackPanel();
                txtBox.Name = "SP";
                txtBox.Margin = new Thickness(10);
                y.Children.Add(txtBox);//все остальные элементы добавляются по аналогии 
                System.Windows.Controls.Image image1 = new System.Windows.Controls.Image();
                image1.Width = 200;
                image1.Height = 200;
                image1.Source = MyImage.Source;

                Label label = new Label();
                label.Content = CBType.Text;
                label.FontSize = 15;
                label.HorizontalContentAlignment = HorizontalAlignment.Center;

                Label label1 = new Label();
                label1.Content = "Цена: " + FPrice.Text + "р.";
                label1.FontSize = 15;
                label1.HorizontalContentAlignment = HorizontalAlignment.Center;

                Button button = new Button();
                button.Name = FName.Text;
                button.FontSize = 15;
                button.Content = "Добавить в корзину";


                txtBox.Children.Add(image1);
                txtBox.Children.Add(label);
                txtBox.Children.Add(label1);
                txtBox.Children.Add(button);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try { 
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = Directory.GetCurrentDirectory();
            dlg.FileName = @"Picture";
            dlg.DefaultExt = "*.png";
            dlg.Filter = "Pictures (.png)|*.png";

            bool? result = dlg.ShowDialog();
                if (result == true)
                {
                    string filename = dlg.FileName;

                    Img pic = Img.FromFile(filename);
                    System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                    pic.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] b = memoryStream.ToArray();
                    if (b.Length > 1000000)
                    {
                        MessageBox.Show("Image is too big");
                        this.Close();
                    }
                    else
                    {

                        Bitmap image = GetImageFromByteArray(b);
                        MyImage.Source = BitmapToImageSource(image);
                        collection.Image = b;
                        prow = false;
                    }
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        private void FPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }
        private void FCount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }
    }
}
