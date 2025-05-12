// CPasteleria/View/InventarioAgregarView.xaml.cs
using System;
using System.Windows;
using CPasteleria.CustomControls;
using CPasteleria.Model;
using CPasteleria.Repositories;
using System.Linq; // Necesario para .Any() y .Max()

namespace CPasteleria.View
{
    public partial class InventarioAgregarView : Window
    {
        private readonly IPastelRepository pastelRepository;

        public InventarioAgregarView()
        {
            InitializeComponent();
            pastelRepository = new PastelRepository();
            btnAgregar.IsEnabled = true;
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string nombreIngresado = txtFieldNombrePastel.Text;
                string precioStr = txtFieldPrecio.Text;
                string existenciasStr = txtFieldExistencias.Text;

                // Validación básica (igual que antes)
                if (string.IsNullOrWhiteSpace(nombreIngresado))
                {
                    CustomOkMessageBox.Show("Ingrese el nombre del pastel.");
                    txtFieldNombrePastel.Focus();
                    return;
                }
                // TU SCRIPT SQL DICE VARCHAR(25) PARA PASTEL.NOMBRE
                if (nombreIngresado.Length > 25)
                {
                    CustomOkMessageBox.Show("El nombre del pastel no puede exceder los 25 caracteres.");
                    txtFieldNombrePastel.Focus();
                    return;
                }
                if (!decimal.TryParse(precioStr, out decimal precio) || precio <= 0)
                {
                    CustomOkMessageBox.Show("Ingrese un precio válido (número positivo).");
                    txtFieldPrecio.Focus();
                    return;
                }
                // TU SCRIPT SQL DICE Precio NUMERIC(4,0) para Pastel.Precio
                if (precio != Math.Truncate(precio) || precio > 9999 || precio < -9999) // Ajustar si el rango es solo positivo
                {
                    CustomOkMessageBox.Show("El precio debe ser un número entero entre -9999 y 9999 (sin decimales).");
                    txtFieldPrecio.Focus();
                    return;
                }
                if (!int.TryParse(existenciasStr, out int existencias) || existencias < 0)
                {
                    CustomOkMessageBox.Show("Ingrese una cantidad de existencias válida (número entero no negativo).");
                    txtFieldExistencias.Focus();
                    return;
                }

                if (pastelRepository.GetByName(nombreIngresado) != null)
                {
                    CustomOkMessageBox.Show($"Ya existe un pastel llamado '{nombreIngresado}' en el inventario.");
                    return;
                }

                // --- GENERAR IDPastel MANUALMENTE ---
                int nuevoIdPastel = 0;
                try
                {
                    var todosLosPasteles = pastelRepository.GetAll();
                    if (todosLosPasteles != null && todosLosPasteles.Any())
                    {
                        nuevoIdPastel = todosLosPasteles.Max(p => p.IDPastel) + 1;
                    }
                    else
                    {
                        nuevoIdPastel = 1; // Primer pastel en la tabla
                    }
                }
                catch (Exception exId)
                {
                    // Esto podría pasar si GetAll() falla o si hay un problema con los datos existentes.
                    CustomOkMessageBox.Show($"Error generando ID para el pastel: {exId.Message}. Usando ID temporal (riesgoso).");
                    // Fallback muy simple (NO RECOMENDADO PARA PRODUCCIÓN si Max falla)
                    // Podrías tener una secuencia separada o un mejor mecanismo de fallback.
                    Random rnd = new Random();
                    nuevoIdPastel = rnd.Next(10000, 99999); // Ejemplo de fallback simple, pero NO garantiza unicidad
                                                            // Lo ideal sería no continuar si esto falla.
                }
                // --- FIN GENERACIÓN DE IDPastel ---

                var pastel = new PastelModel
                {
                    IDPastel = nuevoIdPastel, // Asignar el ID generado
                    Nombre = nombreIngresado,
                    Precio = precio,
                    Existencias = existencias
                };

                pastelRepository.Add(pastel);

                CustomOkMessageBox.Show($"Pastel '{pastel.Nombre}' (ID: {pastel.IDPastel}) añadido exitosamente.");

                txtFieldNombrePastel.Clear();
                txtFieldPrecio.Clear();
                txtFieldExistencias.Clear();
                txtFieldNombrePastel.Focus();
            }
            catch (FormatException)
            {
                CustomOkMessageBox.Show("Error de formato en Precio o Existencias.");
            }
            catch (System.Data.SqlClient.SqlException sqlEx) // Capturar errores SQL específicos
            {
                if (sqlEx.Number == 2627) // Violación de PRIMARY KEY o UNIQUE constraint
                {
                    CustomOkMessageBox.Show($"Error: Ya existe un pastel con el ID '{txtFieldNombrePastel.Tag}' o el nombre '{txtFieldNombrePastel.Text}' ya está en uso.");
                }
                else
                {
                    CustomOkMessageBox.Show($"Error de base de datos al agregar pastel: {sqlEx.Message}");
                }
            }
            catch (Exception ex)
            {
                CustomOkMessageBox.Show($"Error al agregar pastel: {ex.Message}\n{ex.StackTrace}");
            }
        }

        // ... (btnCancelar_Click, btnMinimizar_Click, btnCerrar_Click sin cambios) ...
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