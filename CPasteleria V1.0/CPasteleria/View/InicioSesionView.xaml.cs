using System;
using System.Windows;
using System.Windows.Input;    // Necesario para Window_MouseDown
using CPasteleria.ViewModel; // Necesario para LoginViewModel

namespace CPasteleria.View
{
    public partial class InicioSesionView : Window
    {
        public InicioSesionView()
        {
            InitializeComponent();

            // El DataContext se asigna en el XAML:
            // <Window.DataContext>
            //     <viewModel:LoginViewModel/>
            // </Window.DataContext>

            // Intentar obtener el ViewModel del DataContext y suscribirse al evento
            if (this.DataContext is LoginViewModel viewModel)
            {
                viewModel.LoginSuccess += ViewModel_LoginSuccess;
            }
            // Opcional: Podrías añadir un 'else' aquí para loggear o manejar
            // el caso en que el DataContext no sea del tipo esperado, aunque
            // si el XAML está bien, esto no debería ocurrir.
        }

        private void ViewModel_LoginSuccess(object sender, EventArgs e)
        {
            // Este método se llama cuando el evento LoginSuccess se dispara desde el LoginViewModel.
            // Aquí es donde manejas la navegación a la siguiente ventana.
            var menuInicioView = new MenuInicioView(); // La ventana a la que quieres navegar
            menuInicioView.Show();
            this.Close(); // Cierra la ventana actual de InicioSesionView
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            // Decide la lógica apropiada para cerrar:
            // Opción 1: Simplemente cerrar la ventana de login.
            // Close();

            // Opción 2: Regresar a la pantalla de Inicio (si es el flujo deseado).
            var inicioView = new InicioView();
            inicioView.Show();
            this.Close();
        }

        private void btnMinimizar_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        // El método btnIniciarSesion_Click ya no es necesario aquí
        // porque el botón en XAML usa Command="{Binding LoginCommand}".
        // La lógica está en LoginViewModel.ExecuteLoginCommand.
        /*
        private void btnIniciarSesion_Click(object sender, RoutedEventArgs e)
        {
            // ... (este bloque permanece comentado o se elimina)
        }
        */

        private void btnRegistro_Click(object sender, RoutedEventArgs e)
        {
            var registroView = new RegistroView(); // Asumiendo que RegistroView es la ventana correcta
            registroView.Show();
            this.Close(); // Cierra la ventana actual de inicio de sesión
        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            var inicioView = new InicioView(); // Asumiendo que InicioView es la ventana a la que regresar
            inicioView.Show();
            this.Close(); // Cierra la ventana actual de inicio de sesión
        }
    }
}