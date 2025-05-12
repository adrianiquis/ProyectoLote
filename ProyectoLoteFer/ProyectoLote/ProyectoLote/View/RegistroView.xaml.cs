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

            string usuario = txtBoxUsuario.Text.Trim();
            string nombre = txtBoxNombre.Text.Trim();

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(nombre))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Campos vacíos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBox.Show("Registro completado con éxito.", "Registro", MessageBoxButton.OK, MessageBoxImage.Information);


            this.Close();
        }

        private void btnIniciarSesion_Click(object sender, RoutedEventArgs e)
        {
            var login = new LoginView();
            login.Show();
            this.Close();
        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            var inicio = new PantallaInicio();
            inicio.Show();
            this.Close();
        }
    }
}