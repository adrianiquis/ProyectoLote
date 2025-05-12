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
    public partial class VentasAgregarView : Window
    {
        private readonly IPastelRepository pastelRepository;
        private readonly ICarritoRepository carritoRepository;

        public VentasAgregarView()
        {
            InitializeComponent();
            pastelRepository = new PastelRepository();
            carritoRepository = new CarritoRepository(); // Instanciar repositorio del carrito
            LoadPastelNames();
        }

        private void LoadPastelNames()
        {
            try
            {
                // Obtener solo pasteles con existencias > 0
                var pastelesDisponibles = pastelRepository.GetAll().Where(p => p.Existencias > 0).ToList();
                var nombresDisponibles = pastelesDisponibles.Select(p => p.Nombre).ToList();

                seleccionNombre.ItemsSource = nombresDisponibles;

                if (nombresDisponibles.Any())
                {
                    seleccionNombre.SelectedIndex = 0;
                    btnAgregar.IsEnabled = true;
                }
                else
                {
                    CustomOkMessageBox.Show("No hay pasteles disponibles en inventario.");
                    btnAgregar.IsEnabled = false;
                    txtBoxCantidad.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                CustomOkMessageBox.Show($"Error al cargar nombres de pastel: {ex.Message}");
                btnAgregar.IsEnabled = false;
                txtBoxCantidad.IsEnabled = false;
            }
        }


        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            string nombreSeleccionado = seleccionNombre.SelectedItem as string;
            string cantidadStr = txtBoxCantidad.Text;

            // Validaciones
            if (string.IsNullOrEmpty(nombreSeleccionado))
            {
                CustomOkMessageBox.Show("Seleccione un pastel.");
                return;
            }
            if (!int.TryParse(cantidadStr, out int cantidad) || cantidad <= 0)
            {
                CustomOkMessageBox.Show("Ingrese una cantidad válida (número entero mayor a cero).");
                return;
            }

            try
            {
                // Obtener datos del pastel seleccionado
                PastelModel pastel = pastelRepository.GetByName(nombreSeleccionado);
                if (pastel == null)
                {
                    CustomOkMessageBox.Show($"Error: No se encontró el pastel '{nombreSeleccionado}'.");
                    LoadPastelNames(); // Recargar lista por si acaso
                    return;
                }

                // Verificar existencias
                if (cantidad > pastel.Existencias)
                {
                    CustomOkMessageBox.Show($"No hay suficientes existencias de '{pastel.Nombre}'. Disponible: {pastel.Existencias}");
                    return;
                }

                // --- Lógica del Carrito ---
                // Verificar si el pastel ya está en el carrito
                var itemsCarrito = carritoRepository.GetAll().ToList();
                var itemExistente = itemsCarrito.FirstOrDefault(item => item.Nombre == pastel.Nombre);

                if (itemExistente != null)
                {
                    // Si ya existe, preguntar si desea sumar la cantidad o reemplazarla
                    var result = CustomYNMessageBox.Show($"'{pastel.Nombre}' ya está en el carrito ({itemExistente.Cantidad} uds). ¿Desea añadir {cantidad} más?");
                    if (result == true) // Añadir
                    {
                        int nuevaCantidad = itemExistente.Cantidad + cantidad;
                        // Verificar existencias totales
                        if (nuevaCantidad > pastel.Existencias)
                        {
                            CustomOkMessageBox.Show($"No puede agregar {cantidad} más. Total excedería existencias ({pastel.Existencias}).");
                            return;
                        }
                        carritoRepository.EditQuantity(pastel.Nombre, nuevaCantidad);
                        CustomOkMessageBox.Show($"{cantidad} '{pastel.Nombre}' añadido(s) al carrito. Total: {nuevaCantidad}.");
                    }
                    else if (result == false) // Reemplazar (si el usuario dice No, asumimos que quiere la nueva cantidad)
                    {
                        carritoRepository.EditQuantity(pastel.Nombre, cantidad);
                        CustomOkMessageBox.Show($"Cantidad de '{pastel.Nombre}' actualizada a {cantidad} en el carrito.");
                    }
                    else // Si cierra el messagebox (null), no hacer nada
                    {
                        return;
                    }
                }
                else
                {
                    // Si no existe, añadir nuevo item al carrito
                    var nuevoItemCarrito = new CarritoModel
                    {
                        Nombre = pastel.Nombre,
                        Precio = pastel.Precio, // Precio unitario
                        Cantidad = cantidad
                        // El Subtotal se calcula en el repositoriocd ..
                    };
                    carritoRepository.Add(nuevoItemCarrito);
                    CustomOkMessageBox.Show($"'{pastel.Nombre}' ({cantidad} uds) agregado al carrito.");
                }


                // Regresar a la vista de realizar venta
                var menuVentas = new RealizarVentaView();
                menuVentas.Show();
                Close();

            }
            catch (Exception ex)
            {
                CustomOkMessageBox.Show($"Error al agregar al carrito: {ex.Message}\n{ex.StackTrace}");
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