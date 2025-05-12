using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CPasteleria.CustomControls;
using CPasteleria.Repositories;
using CPasteleria.Model; // Necesario

namespace CPasteleria.View
{
    public partial class VentasQuitarView : Window
    {
        private readonly ICarritoRepository carritoRepository;

        public VentasQuitarView()
        {
            InitializeComponent();
            carritoRepository = new CarritoRepository();
            LoadCartItems();
        }

        private void LoadCartItems()
        {
            try
            {
                var items = carritoRepository.GetAll().ToList();
                var nombresItems = items.Select(i => $"{i.Nombre} ({i.Cantidad} uds)").ToList(); // Mostrar nombre y cantidad

                seleccionNombre.ItemsSource = nombresItems;

                if (nombresItems.Any())
                {
                    seleccionNombre.SelectedIndex = 0;
                    btnQuitar.IsEnabled = true;
                }
                else
                {
                    CustomOkMessageBox.Show("El carrito está vacío.");
                    btnQuitar.IsEnabled = false;
                    seleccionNombre.ItemsSource = null;
                }
            }
            catch (Exception ex)
            {
                CustomOkMessageBox.Show($"Error al cargar items del carrito: {ex.Message}");
                btnQuitar.IsEnabled = false;
            }
        }


        private void btnQuitar_Click(object sender, RoutedEventArgs e)
        {
            string itemSeleccionadoCompleto = seleccionNombre.SelectedItem as string;

            if (string.IsNullOrEmpty(itemSeleccionadoCompleto))
            {
                CustomOkMessageBox.Show("Seleccione un item para quitar.");
                return;
            }

            // Extraer el nombre del pastel del string (asumiendo formato "Nombre (Cantidad uds)")
            string nombrePastel = itemSeleccionadoCompleto.Split('(')[0].Trim();


            var result = CustomYNMessageBox.Show($"¿Está seguro de quitar '{nombrePastel}' del carrito?");
            if (result != true)
            {
                return; // No hacer nada si el usuario cancela
            }

            try
            {
                carritoRepository.Remove(nombrePastel);
                CustomOkMessageBox.Show($"'{nombrePastel}' quitado del carrito.");

                // Regresar a la vista de realizar venta
                var ventas = new RealizarVentaView();
                ventas.Show();
                Close();
            }
            catch (Exception ex)
            {
                CustomOkMessageBox.Show($"Error al quitar del carrito: {ex.Message}\n{ex.StackTrace}");
            }
        }


        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            var ventas = new RealizarVentaView();
            ventas.Show();
            Close();
        }

        // Mantener si quieres mover la ventana
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnMinimizar_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}