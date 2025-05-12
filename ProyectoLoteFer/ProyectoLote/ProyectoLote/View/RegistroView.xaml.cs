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

namespace ProyectoLote.View
{
    /// <summary>
    /// Interaction logic for RegistroView.xaml
    /// </summary>
    public partial class RegistroView : Window
    {
        public RegistroView()
        {
            InitializeComponent();
        }
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnMinimizar_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
        private void btnContinuarRegistro_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
        private void btnIniciarSesion_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
