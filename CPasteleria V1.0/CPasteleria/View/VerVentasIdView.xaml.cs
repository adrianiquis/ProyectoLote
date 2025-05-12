using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CPasteleria.CustomControls;
using CPasteleria.Model;
using CPasteleria.Repositories;
using System.Collections.Generic; // Necesario

namespace CPasteleria.View
{
    public partial class VerVentasIdView : Window
    {
        private readonly IVentaRepository ventaRepository;

        // Asumiendo ComboBox llamado 'seleccionIdVenta' y TextBox 'txtboxIdVenta' en XAML
        // Se usará el ComboBox para seleccionar, el TextBox no es necesario aquí.

        public VerVentasIdView()
        {
            InitializeComponent();
            ventaRepository = new VentaRepository();
            LoadVentaIds();
        }

        private void LoadVentaIds()
        {
            try
            {
                var ids = ventaRepository.GetAllIds().ToList();
                seleccionIdVenta.ItemsSource = ids; // Cambiado de seleccionNombre a seleccionIdVenta

                if (ids.Any())
                {
                    seleccionIdVenta.SelectedIndex = 0;
                    btnBuscar.IsEnabled = true;
                }
                else
                {
                    CustomOkMessageBox.Show("No hay ventas registradas para buscar.");
                    btnBuscar.IsEnabled = false;
                    seleccionIdVenta.ItemsSource = null;
                }
            }
            catch (Exception ex)
            {
                CustomOkMessageBox.Show($"Error al cargar IDs de venta: {ex.Message}");
                btnBuscar.IsEnabled = false;
            }
        }


        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            if (seleccionIdVenta.SelectedItem == null || !(seleccionIdVenta.SelectedItem is int idSeleccionada))
            {
                CustomOkMessageBox.Show("Seleccione un ID de venta válido.");
                return;
            }

            try
            {
                VentaModel ventaEncontrada = ventaRepository.GetById(idSeleccionada);

                if (ventaEncontrada != null)
                {
                    // Pasar la venta encontrada (que incluye sus detalles) a la vista de mostrar
                    var mostrarView = new VerVentasMostrarView(ventaEncontrada); // Constructor que acepta VentaModel
                    mostrarView.Show();
                    Close();
                }
                else
                {
                    // Esto sería raro si el ID viene del ComboBox cargado
                    CustomOkMessageBox.Show($"No se encontró la venta con ID {idSeleccionada}.");
                    LoadVentaIds(); // Recargar por si acaso
                }
            }
            catch (Exception ex)
            {
                CustomOkMessageBox.Show($"Error al buscar la venta: {ex.Message}\n{ex.StackTrace}");
            }
        }


        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            // Debería regresar a VerVentasView, no MenuVentasView si vino de ahí
            var verVentas = new VerVentasView();
            verVentas.Show();
            Close();
        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            var verVentas = new VerVentasView();
            verVentas.Show();
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

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}