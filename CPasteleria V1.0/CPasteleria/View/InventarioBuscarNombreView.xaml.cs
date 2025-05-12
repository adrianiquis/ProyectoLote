// CPasteleria/View/InventarioBuscarNombreView.xaml.cs
using System;
using System.Linq;
using System.Windows;
using CPasteleria.CustomControls;
using CPasteleria.Model;
using CPasteleria.Repositories;
using System.Collections.Generic; // <--- Añadir

namespace CPasteleria.View
{
    public partial class InventarioBuscarNombreView : Window
    {
        // Asume controles:
        // ComboBox: seleccionNombre
        // Button: btnBuscar, btnCancelar, btnRegresar, btnMinimizar, btnCerrar

        public InventarioBuscarNombreView()
        {
            InitializeComponent();
            LoadPastelNames();
        }

        private void LoadPastelNames()
        {
            // Carga los nombres como en InventarioQuitarView
            try
            {
                IPastelRepository repo = new PastelRepository();
                var nombres = repo.GetAllNames().ToList();
                seleccionNombre.ItemsSource = nombres;

                if (nombres.Any())
                {
                    seleccionNombre.SelectedIndex = 0;
                    btnBuscar.IsEnabled = true;
                }
                else
                {
                    btnBuscar.IsEnabled = false;
                    CustomOkMessageBox.Show("No hay pasteles en el inventario.");
                }
            }
            catch (Exception ex)
            {
                CustomOkMessageBox.Show($"Error al cargar nombres: {ex.Message}");
                seleccionNombre.ItemsSource = null;
                btnBuscar.IsEnabled = false;
            }
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string nombreSeleccionado = seleccionNombre.SelectedItem as string;
                if (string.IsNullOrEmpty(nombreSeleccionado))
                {
                    CustomOkMessageBox.Show("Seleccione un pastel para buscar.");
                    return;
                }

                IPastelRepository repo = new PastelRepository();
                PastelModel pastelEncontrado = repo.GetByName(nombreSeleccionado);

                if (pastelEncontrado != null)
                {
                    // Crear una lista que solo contenga el pastel encontrado
                    var listaPasteles = new List<PastelModel> { pastelEncontrado };

                    // Pasar la lista a la vista de mostrar
                    var mostrarView = new InventarioMostrarView(listaPasteles); // <--- Pasar lista
                    mostrarView.Show();
                    Close(); // Cerrar esta ventana
                }
                else
                {
                    CustomOkMessageBox.Show($"No se encontró el pastel '{nombreSeleccionado}'.");
                    // Podrías recargar la lista por si hubo cambios mientras tanto
                    // LoadPastelNames();
                }
            }
            catch (Exception ex)
            {
                CustomOkMessageBox.Show($"Error al buscar pastel: {ex.Message}");
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            var menuInventario = new MenuInventarioView();
            menuInventario.Show();
            Close();
        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            var buscarPastel = new InventarioBuscarView();
            buscarPastel.Show();
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