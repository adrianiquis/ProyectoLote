// CPasteleria/View/InventarioQuitarView.xaml.cs
using System;
using System.Linq; // <-- Añadir para .Any()
using System.Windows;
using CPasteleria.CustomControls;
using CPasteleria.Model;
using CPasteleria.Repositories;

namespace CPasteleria.View
{
    public partial class InventarioQuitarView : Window
    {
        // Asume que tienes controles llamados:
        // ComboBox: seleccionNombre
        // Button: btnQuitar, btnCancelar, btnMinimizar, btnCerrar

        public InventarioQuitarView()
        {
            InitializeComponent();
            LoadPastelNames(); // Carga los nombres al iniciar la ventana
        }

        private void LoadPastelNames()
        {
            try
            {
                IPastelRepository repo = new PastelRepository();
                var nombres = repo.GetAllNames().ToList(); // Obtener lista de nombres
                seleccionNombre.ItemsSource = nombres; // Asignar al ComboBox

                // Opcional: seleccionar el primero si la lista no está vacía
                if (nombres.Any())
                {
                    seleccionNombre.SelectedIndex = 0;
                }
                else
                {
                    // Deshabilitar el botón de quitar si no hay pasteles
                    btnQuitar.IsEnabled = false;
                    CustomOkMessageBox.Show("No hay pasteles en el inventario para quitar.");
                }
            }
            catch (Exception ex)
            {
                CustomOkMessageBox.Show($"Error al cargar nombres de pastel: {ex.Message}");
                seleccionNombre.ItemsSource = null;
                btnQuitar.IsEnabled = false; // Deshabilitar si hay error
            }
        }

        private void btnQuitar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Obtener nombre seleccionado
                string nombreSeleccionado = seleccionNombre.SelectedItem as string;

                if (string.IsNullOrEmpty(nombreSeleccionado))
                {
                    CustomOkMessageBox.Show("Por favor, seleccione un pastel para quitar.");
                    return;
                }

                // Confirmación
                var result = CustomYNMessageBox.Show($"¿Está seguro de que desea eliminar el pastel '{nombreSeleccionado}'?");
                if (result != true) // Si el usuario dice No o cierra el diálogo
                {
                    return;
                }

                // Obtener el ID del pastel para eliminarlo
                IPastelRepository repo = new PastelRepository();
                PastelModel pastel = repo.GetByName(nombreSeleccionado);

                if (pastel == null)
                {
                    // Esto no debería pasar si el ComboBox se cargó bien, pero es una buena verificación
                    CustomOkMessageBox.Show("El pastel seleccionado no fue encontrado en la base de datos.");
                    LoadPastelNames(); // Recargar por si acaso
                    return;
                }

                // Eliminar de la BD
                repo.Remove(pastel.IDPastel);

                CustomOkMessageBox.Show($"Pastel '{nombreSeleccionado}' eliminado exitosamente.");

                // Actualizar la lista en el ComboBox y mantener la ventana abierta
                LoadPastelNames();

                // O si prefieres regresar al menú después de quitar:
                // var menuInventario = new MenuInventarioView();
                // menuInventario.Show();
                // Close();
            }
            catch (Exception ex)
            {
                CustomOkMessageBox.Show($"Error al quitar pastel: {ex.Message}\n{ex.StackTrace}");
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

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}