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
    /// Lógica de interacción para MenuInicioView.xaml
    /// </summary>
    public partial class MenuInicioView : Window
    {
        public MenuInicioView()
        {
            InitializeComponent();
        }

        private void btnInventario_Click(object sender, RoutedEventArgs e)
        {
            var inventario = new MenuInventarioView();
            inventario.Show();
            Close();
        }

        private void btnMenuVentas_Click(object sender, RoutedEventArgs e)
        {
            var ventas = new MenuVentasView();
            ventas.Show();
            Close();
        }

        private void btnMinimizar_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnMenuInicio_Click(object sender, RoutedEventArgs e)
        {
            var Inicio = new InicioView();
            Inicio.Show();
            Close();
        }
    }
}
