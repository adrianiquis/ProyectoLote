using System;
using System.Linq;
using System.Windows;
using CPasteleria.CustomControls;
using CPasteleria.Model;
using CPasteleria.Repositories;
using System.Collections.Generic;
using System.IO; // Necesario para Path y File
// Ya no se necesita QuestPDF ni CPasteleria.Documents
using System.Diagnostics; // Para Process.Start
using System.Text; // Necesario para StringBuilder

namespace CPasteleria.View
{
    public partial class CrearTicketView : Window
    {
        private readonly IVentaRepository ventaRepository;
        // private readonly IDetalleVentaRepository detalleVentaRepository; // No se usa si GetById carga detalles

        public CrearTicketView()
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
                seleccionTicket.ItemsSource = ids;
                if (ids.Any())
                {
                    seleccionTicket.SelectedIndex = 0;
                    btnCrear.IsEnabled = true;
                }
                else
                {
                    CustomOkMessageBox.Show("No hay ventas registradas para generar tickets.");
                    btnCrear.IsEnabled = false;
                    seleccionTicket.ItemsSource = null;
                }
            }
            catch (Exception ex)
            {
                CustomOkMessageBox.Show($"Error al cargar IDs de venta: {ex.Message}");
                btnCrear.IsEnabled = false;
            }
        }

        private void btnCrear_Click(object sender, RoutedEventArgs e)
        {
            if (seleccionTicket.SelectedItem == null || !(seleccionTicket.SelectedItem is int idVentaSeleccionada))
            {
                CustomOkMessageBox.Show("Seleccione un ID de venta válido.");
                return;
            }

            try
            {
                VentaModel venta = ventaRepository.GetById(idVentaSeleccionada);
                if (venta == null || venta.Detalles == null || !venta.Detalles.Any())
                {
                    CustomOkMessageBox.Show($"No se encontraron datos completos o detalles para la venta #{idVentaSeleccionada}. No se puede generar el ticket.");
                    return;
                }

                // --- CONSTRUCCIÓN DEL CONTENIDO DEL TICKET ---
                var ticketInfo = new StringBuilder(); // Usar StringBuilder
                ticketInfo.AppendLine("      PASTELERÍA \"EL DULCE ENCANTO\""); // Nombre de tu pastelería
                ticketInfo.AppendLine("          R.F.C: XAXX010101000"); // RFC
                ticketInfo.AppendLine("  Calle Ficticia 123, Colonia Centro"); // Dirección
                ticketInfo.AppendLine("         Guadalupe, Zacatecas");
                ticketInfo.AppendLine("           Tel: (492) 123-4567"); // Teléfono
                ticketInfo.AppendLine("================================================");
                ticketInfo.AppendLine($"TICKET DE VENTA #{venta.IDVenta}");
                ticketInfo.AppendLine("================================================");
                ticketInfo.AppendLine($"Fecha: {venta.Fecha:dd/MM/yyyy HH:mm}");
                ticketInfo.AppendLine($"Vendedor: {venta.NombreEmpleado}");
                ticketInfo.AppendLine("------------------------------------------------");
                ticketInfo.AppendLine("Cant. Producto                P.Unit. Subtotal");
                ticketInfo.AppendLine("------------------------------------------------");

                foreach (var detalle in venta.Detalles)
                {
                    // Formatear cada línea para que se alinee mejor en un TXT
                    string productoNombre = detalle.Nombre.PadRight(25).Substring(0, Math.Min(detalle.Nombre.Length, 25)); // Limitar y rellenar nombre
                    string cantidad = detalle.Cantidad.ToString().PadLeft(3);
                    string precioUnit = $"{detalle.PrecioUnitario:C2}".PadLeft(10);
                    string subtotal = $"{detalle.Subtotal:C2}".PadLeft(10);
                    ticketInfo.AppendLine($"{cantidad} {productoNombre} {precioUnit} {subtotal}");
                }
                ticketInfo.AppendLine("------------------------------------------------");
                ticketInfo.AppendLine($"TOTAL: {venta.Total,36:C2}"); // Alinear total a la derecha
                ticketInfo.AppendLine("================================================");
                ticketInfo.AppendLine("           ¡Gracias por su compra!");
                ticketInfo.AppendLine("================================================");
                // --- FIN CONSTRUCCIÓN DEL CONTENIDO ---

                // Definir la ruta y nombre del archivo
                string fileName = $"Ticket_Venta_{venta.IDVenta}_{DateTime.Now:yyyyMMddHHmmss}.txt"; // Cambiado a .txt
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

                // Guardar el contenido en el archivo TXT
                File.WriteAllText(filePath, ticketInfo.ToString());

                CustomOkMessageBox.Show($"Ticket TXT generado exitosamente:\n{filePath}");

                var result = CustomYNMessageBox.Show("¿Desea abrir la carpeta donde se guardó el ticket?");
                if (result == true)
                {
                    try
                    {
                        // Abre el explorador y selecciona el archivo
                        Process.Start("explorer.exe", $"/select,\"{filePath}\"");
                    }
                    catch (Exception exOpen)
                    {
                        CustomOkMessageBox.Show($"No se pudo abrir la carpeta: {exOpen.Message}");
                    }
                }

                var menuVentas = new MenuVentasView();
                menuVentas.Show();
                Close();
            }
            catch (Exception ex)
            {
                CustomOkMessageBox.Show($"Error al generar el ticket TXT: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            var menuVentas = new MenuVentasView();
            menuVentas.Show();
            Close();
        }
        private void btnMinimizar_Click(object sender, RoutedEventArgs e)
        { this.WindowState = WindowState.Minimized; }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        { Close(); }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                DragMove();
        }
    }
}
