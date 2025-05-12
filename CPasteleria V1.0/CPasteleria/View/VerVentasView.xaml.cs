using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CPasteleria.CustomControls;
using CPasteleria.Repositories;
using CPasteleria.Model;
using System.Collections.Generic; // Necesario

namespace CPasteleria.View
{
    public partial class VerVentasView : Window
    {
        private readonly IVentaRepository ventaRepository;

        public VerVentasView()
        {
            InitializeComponent();
            ventaRepository = new VentaRepository();
        }

        private void btnIdVenta_Click(object sender, RoutedEventArgs e)
        {
            var verId = new VerVentasIdView();
            verId.Show();
            Close();
        }

        private void btnTodas_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IEnumerable<VentaModel> todasLasVentas = ventaRepository.GetAll();

                if (todasLasVentas != null && todasLasVentas.Any())
                {
                    // Pasar la lista completa a la vista de mostrar
                    var mostrarView = new VerVentasMostrarView(todasLasVentas); // Constructor que acepta lista
                    mostrarView.Show();
                    Close();
                }
                else
                {
                    CustomOkMessageBox.Show("No hay ventas registradas para mostrar.");
                }
            }
            catch (Exception ex)
            {
                CustomOkMessageBox.Show($"Error al obtener todas las ventas: {ex.Message}");
            }
        }


        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            var menuVentas = new MenuVentasView();
            menuVentas.Show();
            Close();
        }

        // Mantener si quieres mover la ventana
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnMinimizar_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            // Salir aquí usualmente cierra esta ventana y regresa al menú anterior
            var menuVentas = new MenuVentasView();
            menuVentas.Show();
            Close();
            // O simplemente Close(); si es el comportamiento deseado
        }
    }
}