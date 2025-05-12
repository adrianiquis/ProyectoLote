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
    /// Lógica de interacción para MenuInventarioView.xaml
    /// </summary>
    public partial class MenuInventarioView : Window
    {
        public MenuInventarioView()
        {
            InitializeComponent();
        }

        private void btnMinimizar_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            var agregar = new InventarioAgregarView();
            agregar.Show();
            Close();
        }

        private void btnQuitar_Click(object sender, RoutedEventArgs e)
        {
            var quitar = new InventarioQuitarView();
            quitar.Show();
            Close();
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            var buscar = new InventarioBuscarView();
            buscar.Show();
            Close();
        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            var editar = new InventarioEditarView();
            editar.Show();
            Close();
        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            var menuInicioView = new MenuInicioView();
            menuInicioView.Show();
            Close();
        }
    }
}
