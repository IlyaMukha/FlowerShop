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
    /// Логика взаимодействия для InfoWindow.xaml
    /// </summary>
    public partial class InfoWindow : Window
    {
        public InfoWindow()
        {
            InitializeComponent();
        }
        private void mapView_Loaded(object sender, RoutedEventArgs e)
        {
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            // choose your provider here
            mapView.MapProvider = GMap.NET.MapProviders.OpenStreetMapProvider.Instance;
            mapView.MinZoom = 2;
            mapView.MaxZoom = 17;
            // whole world zoom
            mapView.Zoom = 2;
            // lets the map use the mousewheel to zoom
            mapView.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            // lets the user drag the map
            mapView.CanDragMap = true;
            // lets the user drag the map with the left mouse button
            mapView.DragButton = MouseButton.Left;
            mapView.Position = new GMap.NET.PointLatLng(53.889764, 27.558004);
            mapView.Zoom = 30;

            GMap.NET.WindowsPresentation.GMapMarker marker2 = new GMap.NET.WindowsPresentation.GMapMarker(new GMap.NET.PointLatLng(53.889764, 27.558004));
            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            System.Windows.Media.Imaging.BitmapImage bitmapImage = new System.Windows.Media.Imaging.BitmapImage();

            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(@"C:\Users\Mukha\Desktop\Ico\lotus.png");
            bitmapImage.EndInit();
            image.Width = 50;
            image.Height = 50;
            image.Source = bitmapImage;
            image.ToolTip = "Привет! =)";
            marker2.Shape = image;
            mapView.Markers.Add(marker2);
        }
        
    }
}
