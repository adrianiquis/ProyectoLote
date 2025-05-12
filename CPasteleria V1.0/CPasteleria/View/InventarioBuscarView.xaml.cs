// CPasteleria/View/InventarioBuscarView.xaml.cs
using System;
using System.Windows;
using CPasteleria.CustomControls; // <-- Añadir si usas mensajes personalizados
using CPasteleria.Model;
using CPasteleria.Repositories;
using System.Collections.Generic; // <-- Añadir

namespace CPasteleria.View
{
    public partial class InventarioBuscarView : Window
    {
        // Asume controles:
        // Button: btnNombre, btnTodos, btnCancelar, btnMinimizar, btnSalir

        public InventarioBuscarView()
        {
            InitializeComponent();
        }

        private void btnNombre_Click(object sender, RoutedEventArgs e)
        {
            var porNombre = new InventarioBuscarNombreView();
            porNombre.Show();
            Close();
        }

        private void btnTodos_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IPastelRepository repo = new PastelRepository();
                IEnumerable<PastelModel> todosLosPasteles = repo.GetAll();

                // Pasar la lista completa a la vista de mostrar
                var mostrarView = new InventarioMostrarView(todosLosPasteles); // <--- Pasar lista
                mostrarView.Show();
                Close(); // Cerrar esta ventana
            }
            catch (Exception ex)
            {
                CustomOkMessageBox.Show($"Error al obtener todos los pasteles: {ex.Message}");
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            var menuInventario = new MenuInventarioView();
            menuInventario.Show();
            Close();
        }

        private void btnMinimizar_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}