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
using CPasteleria.CustomControls;

namespace CPasteleria.View
{
    /// <summary>
    /// Lógica de interacción para InicioView.xaml
    /// </summary>
    public partial class InicioView : Window
    {
        public InicioView()
        {
            InitializeComponent();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            var result = CustomYNMessageBox.Show("¿Desea salir?");
            if (result == true)
            {
                Close();
            }
        }

        private void btnMinimizar_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnRegistro_Click(object sender, RoutedEventArgs e)
        {
            var registroView = new RegistroView();
            registroView.Show();
            Close();
        }

        private void btnInicioSesion_Click(object sender, RoutedEventArgs e)
        {
            var inicioSesionView = new InicioSesionView();
            inicioSesionView.Show();
            Close();
        }
    }
}
