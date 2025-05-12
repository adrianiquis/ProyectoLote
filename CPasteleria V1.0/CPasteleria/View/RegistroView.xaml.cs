using System;
// ... otros usings ...
using System.Windows;
using CPasteleria.CustomControls;

namespace CPasteleria.View
{
    public partial class RegistroView : Window
    {
        // Controles XAML asumidos:
        // TextBox: txtBoxUsuario, txtBoxNombre
        // Button: btnContinuarRegistro, btnIniciarSesion, btnRegresar, etc.

        public RegistroView()
        {
            InitializeComponent();
        }

        private void btnContinuarRegistro_Click(object sender, RoutedEventArgs e)
        {
            string usuario = txtBoxUsuario.Text;
            string nombre = txtBoxNombre.Text;

            // Validación simple antes de pasar a la siguiente ventana
            if (string.IsNullOrWhiteSpace(usuario))
            {
                CustomOkMessageBox.Show("El nombre de usuario es obligatorio.");
                return;
            }
            if (usuario.Length > 20) // Coincidir con VARCHAR(20) de la BD
            {
                CustomOkMessageBox.Show("El nombre de usuario no puede exceder los 20 caracteres.");
                return;
            }
            if (string.IsNullOrWhiteSpace(nombre))
            {
                CustomOkMessageBox.Show("El nombre es obligatorio.");
                return;
            }
            if (nombre.Length > 40) // Coincidir con VARCHAR(40) de la BD
            {
                CustomOkMessageBox.Show("El nombre no puede exceder los 40 caracteres.");
                return;
            }


            // Pasar los datos a la siguiente ventana
            var continuar = new RegistroView1(usuario, nombre);
            continuar.Show();
            Close();
        }

        // Otros botones sin cambios...
        private void btnIniciarSesion_Click(object sender, RoutedEventArgs e)
        {
            var iniciarSesion = new InicioSesionView();
            iniciarSesion.Show();
            Close();
        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            var InicioView = new InicioView();
            InicioView.Show();
            Close();
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        { Close(); }

        private void btnMinimizar_Click(object sender, RoutedEventArgs e)
        { this.WindowState = WindowState.Minimized; }
    }
}