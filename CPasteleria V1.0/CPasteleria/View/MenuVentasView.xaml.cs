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

namespace CPasteleria.View
{
    /// <summary>
    /// Lógica de interacción para MenuVentasView.xaml
    /// </summary>
    public partial class MenuVentasView : Window
    {
        public MenuVentasView()
        {
            InitializeComponent();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnMinimizar_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnRealizarVenta_Click(object sender, RoutedEventArgs e)
        {
            var realizarVenta = new RealizarVentaView();
            realizarVenta.Show();
            Close();
        }

        private void btnCrearTicket_Click(object sender, RoutedEventArgs e)
        {
            var crearTicket = new CrearTicketView();
            crearTicket.Show();
            Close();
        }

        private void btnVerVentas_Click(object sender, RoutedEventArgs e)
        {
            var ventas = new VerVentasView();
            ventas.Show();
            Close();
        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            var menuInicio = new MenuInicioView();
            menuInicio.Show();
            Close();
        }
    }
}
