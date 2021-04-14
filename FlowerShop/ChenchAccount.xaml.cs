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
using DevExpress.Mvvm.Native;
using FlowerShop.UsersFolder;
using Microsoft.Win32;
using Img = System.Drawing.Image;

namespace FlowerShop
{
    /// <summary>
    /// Логика взаимодействия для ChenchAccount.xaml
    /// </summary>
    public partial class ChenchAccount : Window
    {
        DBContext db;
        Users Userscol = new Users();
        private static readonly ImageConverter _imageConverter = new ImageConverter();
        public string _un;
        public byte[] a;
        public ChenchAccount(string _UserName)
        {
            InitializeComponent();
            db = new DBContext();

            db.Users1.Load();
            _un = _UserName;

            CommandBinding binding = new CommandBinding(ApplicationCommands.New);
            binding.Executed += new ExecutedRoutedEventHandler(SaveM);
            Save.CommandBindings.Add(binding);

            CommandBinding binding1 = new CommandBinding(ApplicationCommands.New);
            binding1.Executed += new ExecutedRoutedEventHandler(BCloseM);
            BClose.CommandBindings.Add(binding1);

        }

        private void IMGNew_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
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
                    if (b.Length > 10000000)
                    {
                        MessageBox.Show("Image is too big");
                        this.Close();
                    }
                    else
                    {

                        Bitmap image = GetImageFromByteArray(b);
                        IMGNew.Source = BitmapToImageSource(image);
                        a = b;
                    }
                }

            }
            catch (Exception ex)
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
        private void SaveM(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                var collection = db.Users1.ToList();
                foreach (var item in collection)
                {
                    if (item.Uname == _un)
                    {
                        if(NewName.Text == "")
                        {
                            item.Uname = item.Uname;
                            break;
                        }
                        else
                        {
                            item.Uname = NewName.Text;
                            item.Image = a;
                            db.SaveChanges();
                            this.Close();
                            break;
                        }
                        
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BCloseM(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
