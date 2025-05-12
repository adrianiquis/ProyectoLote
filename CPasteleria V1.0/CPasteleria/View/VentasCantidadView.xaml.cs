using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls; // Necesario para SelectionChanged
using System.Windows.Input;
using CPasteleria.CustomControls;
using CPasteleria.Repositories;
using CPasteleria.Model;
using System.Collections.Generic;

namespace CPasteleria.View
{
    public partial class VentasCantidadView : Window
    {
        private readonly ICarritoRepository carritoRepository;
        private readonly IPastelRepository pastelRepository; // Necesario para verificar existencias
        private List<CarritoModel> _itemsCarrito; // Para guardar los items cargados

        public VentasCantidadView()
        {
            InitializeComponent();
            carritoRepository = new CarritoRepository();
            pastelRepository = new PastelRepository();
            LoadCartItems();
            // Añadir manejador para cambio de selección
            seleccionNombre.SelectionChanged += SeleccionNombre_SelectionChanged;
        }

        private void LoadCartItems()
        {
            try
            {
                _itemsCarrito = carritoRepository.GetAll().ToList(); // Guardar items
                var nombresItems = _itemsCarrito.Select(i => i.Nombre).ToList();

                seleccionNombre.ItemsSource = nombresItems;

                if (nombresItems.Any())
                {
                    seleccionNombre.SelectedIndex = 0; // Seleccionar el primero
                    // Actualizar cantidad inicial (se hará en SelectionChanged)
                    btnAplicar.IsEnabled = true;
                    txtBoxCantidad.IsEnabled = true;
                }
                else
                {
                    CustomOkMessageBox.Show("El carrito está vacío.");
                    btnAplicar.IsEnabled = false;
                    txtBoxCantidad.IsEnabled = false;
                    seleccionNombre.ItemsSource = null;
                }
            }
            catch (Exception ex)
            {
                CustomOkMessageBox.Show($"Error al cargar items del carrito: {ex.Message}");
                btnAplicar.IsEnabled = false;
                txtBoxCantidad.IsEnabled = false;
            }
        }

        private void SeleccionNombre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string nombreSeleccionado = seleccionNombre.SelectedItem as string;
            if (!string.IsNullOrEmpty(nombreSeleccionado) && _itemsCarrito != null)
            {
                var item = _itemsCarrito.FirstOrDefault(i => i.Nombre == nombreSeleccionado);
                if (item != null)
                {
                    txtBoxCantidad.Text = item.Cantidad.ToString(); // Mostrar cantidad actual
                }
            }
            else
            {
                txtBoxCantidad.Text = ""; // Limpiar si no hay selección
            }
        }


        private void btnAplicar_Click(object sender, RoutedEventArgs e)
        {
            string nombreSeleccionado = seleccionNombre.SelectedItem as string;
            string cantidadStr = txtBoxCantidad.Text;

            // Validaciones
            if (string.IsNullOrEmpty(nombreSeleccionado))
            {
                CustomOkMessageBox.Show("Seleccione un item del carrito.");
                return;
            }
            if (!int.TryParse(cantidadStr, out int nuevaCantidad) || nuevaCantidad <= 0)
            {
                // Permitir cantidad 0 podría interpretarse como quitar el item
                if (nuevaCantidad == 0)
                {
                    var result = CustomYNMessageBox.Show($"Establecer la cantidad a 0 quitará '{nombreSeleccionado}' del carrito. ¿Continuar?");
                    if (result != true) return;
                    // Continuar para quitar
                }
                else
                {
                    CustomOkMessageBox.Show("Ingrese una cantidad válida (número entero mayor o igual a cero).");
                    return;
                }
            }

            try
            {
                // Verificar existencias si la cantidad es > 0
                if (nuevaCantidad > 0)
                {
                    PastelModel pastel = pastelRepository.GetByName(nombreSeleccionado);
                    if (pastel == null)
                    {
                        CustomOkMessageBox.Show($"Error: No se encontró el pastel '{nombreSeleccionado}' en el inventario.");
                        return;
                    }
                    if (nuevaCantidad > pastel.Existencias)
                    {
                        CustomOkMessageBox.Show($"No hay suficientes existencias de '{pastel.Nombre}'. Disponible: {pastel.Existencias}");
                        return;
                    }
                }

                // Aplicar cambio
                if (nuevaCantidad == 0)
                {
                    carritoRepository.Remove(nombreSeleccionado); // Quitar si la cantidad es 0
                    CustomOkMessageBox.Show($"'{nombreSeleccionado}' quitado del carrito.");
                }
                else
                {
                    carritoRepository.EditQuantity(nombreSeleccionado, nuevaCantidad);
                    CustomOkMessageBox.Show($"Cantidad de '{nombreSeleccionado}' modificada a {nuevaCantidad}.");
                }


                // Regresar a la vista de realizar venta
                var menuVentas = new RealizarVentaView();
                menuVentas.Show();
                Close();
            }
            catch (Exception ex)
            {
                CustomOkMessageBox.Show($"Error al modificar cantidad: {ex.Message}\n{ex.StackTrace}");
            }
        }


        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            var menuVentas = new RealizarVentaView();
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

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}