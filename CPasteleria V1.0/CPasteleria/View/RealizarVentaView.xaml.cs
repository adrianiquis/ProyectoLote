using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CPasteleria.CustomControls;
using CPasteleria.Model;
using CPasteleria.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Data.SqlClient; // Para capturar SqlException

namespace CPasteleria.View
{
    public partial class RealizarVentaView : Window
    {
        private readonly ICarritoRepository carritoRepository;
        private readonly IVentaRepository ventaRepository;
        private readonly IDetalleVentaRepository detalleVentaRepository;
        private readonly IPastelRepository pastelRepository;
        private readonly IEmpleadoRepository empleadoRepository;

        public RealizarVentaView()
        {
            InitializeComponent();
            carritoRepository = new CarritoRepository();
            ventaRepository = new VentaRepository();
            detalleVentaRepository = new DetalleVentaRepository();
            pastelRepository = new PastelRepository();
            empleadoRepository = new EmpleadoRepository();
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            var agregar = new VentasAgregarView();
            agregar.Show();
            Close();
        }

        private void btnQuitar_Click(object sender, RoutedEventArgs e)
        {
            var quitar = new VentasQuitarView();
            quitar.Show();
            Close();
        }

        private void btnModCantidad_Click(object sender, RoutedEventArgs e)
        {
            var cantidad = new VentasCantidadView();
            cantidad.Show();
            Close();
        }

        private void btnCarrito_Click(object sender, RoutedEventArgs e)
        {
            var carrito = new VentasCarritoView();
            carrito.Show();
            Close();
        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e) // Botón "Terminar Venta"
        {
            IEnumerable<CarritoModel> itemsCarrito;
            try
            {
                itemsCarrito = carritoRepository.GetAll().ToList();
            }
            catch (Exception ex)
            {
                CustomOkMessageBox.Show($"Error al acceder al carrito: {ex.Message}");
                return;
            }

            if (!itemsCarrito.Any()) // Si el carrito está vacío
            {
                var resultSalirVacio = CustomYNMessageBox.Show("El carrito está vacío. ¿Desea regresar al menú de ventas?");
                if (resultSalirVacio == true)
                {
                    var menuVentas = new MenuVentasView();
                    menuVentas.Show();
                    this.Close();
                }
                // Si es false o null, no hace nada y se queda en la vista actual.
                return;
            }

            // Si el carrito NO está vacío, proceder a finalizar la venta
            var resultFinalizar = CustomYNMessageBox.Show("¿Desea finalizar y registrar la venta actual?");
            if (resultFinalizar == true)
            {
                // Validar existencias ANTES de proceder
                bool stockSuficiente = true;
                foreach (var itemCarrito in itemsCarrito)
                {
                    try
                    {
                        PastelModel pastelEnInventario = pastelRepository.GetByName(itemCarrito.Nombre);
                        if (pastelEnInventario == null)
                        {
                            CustomOkMessageBox.Show($"Error crítico: El pastel '{itemCarrito.Nombre}' en el carrito no se encuentra en el inventario.");
                            stockSuficiente = false;
                            break;
                        }
                        if (itemCarrito.Cantidad > pastelEnInventario.Existencias)
                        {
                            CustomOkMessageBox.Show($"No hay suficientes existencias para '{itemCarrito.Nombre}'. Solicitado: {itemCarrito.Cantidad}, Disponible: {pastelEnInventario.Existencias}. Por favor, ajuste el carrito.");
                            stockSuficiente = false;
                            break;
                        }
                    }
                    catch (Exception exStock)
                    {
                        CustomOkMessageBox.Show($"Error al verificar stock para '{itemCarrito.Nombre}': {exStock.Message}");
                        stockSuficiente = false;
                        break;
                    }
                }

                if (!stockSuficiente)
                {
                    return; // No continuar si no hay stock
                }

                // Proceder con la venta
                try
                {
                    string usuarioActual = Thread.CurrentPrincipal?.Identity?.Name;
                    if (string.IsNullOrEmpty(usuarioActual))
                    {
                        var empleado = empleadoRepository.GetAll().FirstOrDefault();
                        if (empleado != null) usuarioActual = empleado.Usuario;
                        else throw new Exception("No se pudo determinar el empleado actual para la venta.");
                    }

                    var nuevaVenta = new VentaModel
                    {
                        // IDVenta será generado por la BD si es IDENTITY
                        Usuario = usuarioActual,
                        Fecha = DateTime.Now.Date, // Solo la fecha
                        Total = carritoRepository.GetTotal()
                    };

                    int idNuevaVenta = ventaRepository.Add(nuevaVenta); // Esto ya devuelve el ID si está configurado con OUTPUT

                    foreach (var itemCarrito in itemsCarrito)
                    {
                        var detalle = new DetalleVentaModel
                        {
                            // IDDetalle será generado por la BD si es IDENTITY
                            IDVenta = idNuevaVenta,
                            Nombre = itemCarrito.Nombre,
                            PrecioUnitario = itemCarrito.Precio,
                            Cantidad = itemCarrito.Cantidad,
                            Subtotal = itemCarrito.Subtotal
                        };
                        detalleVentaRepository.Add(detalle);

                        PastelModel pastel = pastelRepository.GetByName(itemCarrito.Nombre); // Volver a obtener por si acaso
                        if (pastel != null)
                        {
                            pastelRepository.UpdateStock(pastel.IDPastel, itemCarrito.Cantidad);
                        }
                        else
                        {
                            Console.WriteLine($"ADVERTENCIA SEVERA: El pastel '{itemCarrito.Nombre}' existía para la validación de stock pero no para la actualización. Esto no debería pasar.");
                            // Considera cómo manejar este caso extremo. Quizás no continuar con la venta o loggear un error crítico.
                        }
                    }

                    carritoRepository.Clear();
                    CustomOkMessageBox.Show($"Venta #{idNuevaVenta} realizada y registrada exitosamente.");

                    var menuVentasView = new MenuVentasView();
                    menuVentasView.Show();
                    this.Close();
                }
                catch (SqlException sqlEx) // Capturar errores específicos de SQL
                {
                    CustomOkMessageBox.Show($"Error de base de datos al finalizar la venta: {sqlEx.Message}\nNúmero de error: {sqlEx.Number}\n{sqlEx.StackTrace}");
                }
                catch (Exception ex)
                {
                    CustomOkMessageBox.Show($"Error al finalizar la venta: {ex.Message}\n{ex.StackTrace}");
                }
            }
            // Si el usuario presiona "No" o cierra el diálogo de finalizar venta, no se hace nada y permanece en la vista.
        }

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
            var items = carritoRepository.GetAll();
            if (items != null && items.Any()) // Verificar que items no sea null
            {
                var result = CustomYNMessageBox.Show("Tiene productos en el carrito. ¿Está seguro de cancelar la venta actual y cerrar esta ventana?");
                if (result == true)
                {
                    try
                    {
                        carritoRepository.Clear();
                        CustomOkMessageBox.Show("Carrito limpiado. Venta cancelada.");
                    }
                    catch (Exception ex) { CustomOkMessageBox.Show("Error al limpiar carrito: " + ex.Message); }

                    // Regresar al menú de ventas en lugar de solo cerrar
                    var menuVentas = new MenuVentasView();
                    menuVentas.Show();
                    this.Close();
                }
                // Si dice No, no cerrar
            }
            else
            {
                // Si el carrito está vacío, simplemente regresar al menú de ventas
                var menuVentas = new MenuVentasView();
                menuVentas.Show();
                this.Close();
            }
        }
    }
}