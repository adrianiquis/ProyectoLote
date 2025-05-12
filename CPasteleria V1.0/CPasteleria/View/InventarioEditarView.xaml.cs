// CPasteleria/View/InventarioEditarView.xaml.cs
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls; // <-- Añadir para SelectionChangedEventArgs
using CPasteleria.CustomControls;
using CPasteleria.Model;
using CPasteleria.Repositories;

namespace CPasteleria.View
{
    public partial class InventarioEditarView : Window
    {
        // Asume que tienes controles llamados:
        // ComboBox: seleccionNombre
        // TextBox: txtFieldPrecio, txtFieldExistencias
        // Button: btnAplicar, btnCancelar, btnMinimizar, btnCerrar

        private IPastelRepository _pastelRepository; // Guardar instancia del repositorio

        public InventarioEditarView()
        {
            InitializeComponent();
            _pastelRepository = new PastelRepository(); // Crear instancia una vez
            LoadPastelNames();
            // Añadir manejador para cuando cambie la selección del ComboBox
            seleccionNombre.SelectionChanged += SeleccionNombre_SelectionChanged;
        }

        private void LoadPastelNames()
        {
            try
            {
                var nombres = _pastelRepository.GetAllNames().ToList();
                seleccionNombre.ItemsSource = nombres;

                if (nombres.Any())
                {
                    seleccionNombre.SelectedIndex = 0; // Selecciona el primero
                }
                else
                {
                    btnAplicar.IsEnabled = false; // Deshabilitar si no hay pasteles
                    txtFieldPrecio.IsEnabled = false;
                    txtFieldExistencias.IsEnabled = false;
                    CustomOkMessageBox.Show("No hay pasteles en el inventario para editar.");
                }
            }
            catch (Exception ex)
            {
                CustomOkMessageBox.Show($"Error al cargar nombres de pastel: {ex.Message}");
                seleccionNombre.ItemsSource = null;
                btnAplicar.IsEnabled = false;
            }
        }

        private void SeleccionNombre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Se ejecuta cuando el usuario elige un pastel diferente en el ComboBox
            string nombreSeleccionado = seleccionNombre.SelectedItem as string;
            if (!string.IsNullOrEmpty(nombreSeleccionado))
            {
                try
                {
                    PastelModel pastel = _pastelRepository.GetByName(nombreSeleccionado);
                    if (pastel != null)
                    {
                        // Mostrar datos actuales en los TextBox
                        txtFieldPrecio.Text = pastel.Precio.ToString("F2"); // "F2" para formato con 2 decimales
                        txtFieldExistencias.Text = pastel.Existencias.ToString();
                        txtFieldPrecio.IsEnabled = true; // Habilitar edición
                        txtFieldExistencias.IsEnabled = true;
                        btnAplicar.IsEnabled = true;
                    }
                    else
                    {
                        // Deshabilitar si no se encontró (raro pero posible)
                        txtFieldPrecio.Text = "";
                        txtFieldExistencias.Text = "";
                        txtFieldPrecio.IsEnabled = false;
                        txtFieldExistencias.IsEnabled = false;
                        btnAplicar.IsEnabled = false;
                    }
                }
                catch (Exception ex)
                {
                    CustomOkMessageBox.Show($"Error al cargar datos del pastel: {ex.Message}");
                    txtFieldPrecio.IsEnabled = false;
                    txtFieldExistencias.IsEnabled = false;
                    btnAplicar.IsEnabled = false;
                }
            }
            else
            {
                // Limpiar y deshabilitar si no hay selección
                txtFieldPrecio.Text = "";
                txtFieldExistencias.Text = "";
                txtFieldPrecio.IsEnabled = false;
                txtFieldExistencias.IsEnabled = false;
                btnAplicar.IsEnabled = false;
            }
        }

        private void btnAplicar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string nombreSeleccionado = seleccionNombre.SelectedItem as string;
                if (string.IsNullOrEmpty(nombreSeleccionado))
                {
                    CustomOkMessageBox.Show("Seleccione un pastel para editar.");
                    return;
                }

                // Validar nuevos datos
                if (!decimal.TryParse(txtFieldPrecio.Text, out decimal nuevoPrecio) || nuevoPrecio <= 0)
                {
                    CustomOkMessageBox.Show("Ingrese un precio válido.");
                    return;
                }
                if (!int.TryParse(txtFieldExistencias.Text, out int nuevasExistencias) || nuevasExistencias < 0)
                {
                    CustomOkMessageBox.Show("Ingrese existencias válidas.");
                    return;
                }

                // Obtener ID del pastel
                PastelModel pastelOriginal = _pastelRepository.GetByName(nombreSeleccionado);
                if (pastelOriginal == null)
                {
                    CustomOkMessageBox.Show("Error: No se encontró el pastel original.");
                    return;
                }

                // Crear modelo actualizado
                var pastelActualizado = new PastelModel
                {
                    IDPastel = pastelOriginal.IDPastel, // Mantener el ID original
                    Nombre = nombreSeleccionado,       // El nombre no se edita aquí
                    Precio = nuevoPrecio,
                    Existencias = nuevasExistencias
                };

                // Guardar cambios
                _pastelRepository.Edit(pastelActualizado);

                CustomOkMessageBox.Show($"Pastel '{nombreSeleccionado}' actualizado.");

                // Opcional: Mantener la ventana o regresar
                var menuInventario = new MenuInventarioView();
                menuInventario.Show();
                Close();

            }
            catch (FormatException)
            {
                CustomOkMessageBox.Show("Error de formato en Precio o Existencias.");
            }
            catch (Exception ex)
            {
                CustomOkMessageBox.Show($"Error al editar pastel: {ex.Message}\n{ex.StackTrace}");
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