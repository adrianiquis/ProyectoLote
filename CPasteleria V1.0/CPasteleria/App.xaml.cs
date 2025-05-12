using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CPasteleria.View;

namespace CPasteleria
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var inicioView = new InicioView();  
            //var inicioView = new InventarioMostrarView();  
            inicioView.Show();
            inicioView.IsVisibleChanged += (s, ev) =>
            {
                if ((inicioView.IsVisible == false) && (inicioView.IsLoaded))
                {
                    var mainView = new MainWindow();
                    mainView.Show();
                    inicioView.Close();
                };
            };
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb != null && tb.Text == "Ingresar valor")
            {
                tb.Text = "";
                tb.Foreground = Brushes.Black;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb != null && string.IsNullOrWhiteSpace(tb.Text))
            {
                tb.Text = "Ingresar valor";
                tb.Foreground = Brushes.Gray;
            }
        }
    }
}
