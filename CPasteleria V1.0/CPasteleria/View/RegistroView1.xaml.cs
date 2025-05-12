using System;
using System.Windows;
using CPasteleria.CustomControls;
using CPasteleria.Model; // Necesario
using CPasteleria.Repositories; // Necesario
using System.Data.SqlClient; // Para capturar errores SQL
using System.Linq; 

namespace CPasteleria.View
{
    public partial class RegistroView1 : Window
    {
        private readonly IEmpleadoRepository empleadoRepository;
        // Variables para almacenar los datos pasados de la ventana anterior
        private string _usuario;
        private string _nombreCompleto;

        // Controles XAML asumidos en ESTA ventana:
        // TextBox: txtBoxRfc
        // CustomControls:PasswordBox: PasswordBoxContrasenia
        // Button: btnRegistrarse, btnIniciarSesion, btnRegresar, btnMinimizar, btnCerrar

        // Constructor modificado para recibir datos de RegistroView
        public RegistroView1(string usuario, string nombreCompleto)
        {
            InitializeComponent();
            empleadoRepository = new EmpleadoRepository();
            _usuario = usuario; // Guardar el usuario recibido
            _nombreCompleto = nombreCompleto; // Guardar el nombre recibido
        }

        private void btnRegistrarse_Click(object sender, RoutedEventArgs e)
        {
            // --- Recopilar datos ---
            string usuario = _usuario; // Usar el valor pasado
            string nombre = _nombreCompleto; // Usar el valor pasado
            string rfc = txtBoxRfc.Text;
            var securePassword = PasswordBoxContrasenia.Password;
            string contraseñaPlana = new System.Net.NetworkCredential(string.Empty, securePassword).Password;

            // --- Validación ---
            if (string.IsNullOrWhiteSpace(usuario)) { CustomOkMessageBox.Show("Usuario obligatorio"); return; }
            if (string.IsNullOrWhiteSpace(nombre)) { CustomOkMessageBox.Show("Nombre obligatorio."); return; }
            if (string.IsNullOrWhiteSpace(rfc)) { CustomOkMessageBox.Show("RFC obligatorio."); return; }
            if (string.IsNullOrWhiteSpace(contraseñaPlana)) { CustomOkMessageBox.Show("Contraseña obligatoria."); return; }
            if (rfc.Length != 13) { CustomOkMessageBox.Show("RFC Tamaño incorrecto"); return; }
            // -> SIN HASHING: No se valida complejidad de contraseña aquí

            // --- Crear Modelo ---
            var nuevoEmpleado = new EmpleadoModel
            {
                // Asumiendo que ID_Empleado NO es IDENTITY y necesitas generar uno.
                // Si SÍ es IDENTITY en tu BD, quita la línea de ID_Empleado.
                ID_Empleado = GenerarIdUnicoTemporal(), // Necesitas una forma de generar IDs únicos si no es IDENTITY
                Usuario = usuario,
                Nombre = nombre,
                RFC = rfc,
                Contraseña = contraseñaPlana // Guardar contraseña plana
            };

            // --- Guardar en BD ---
            try
            {
                if (empleadoRepository.GetByUsername(nuevoEmpleado.Usuario) != null)
                {
                    CustomOkMessageBox.Show($"El nombre de usuario '{nuevoEmpleado.Usuario}' ya está registrado.");
                    return;
                }
                // Aquí podrías añadir una verificación similar para el RFC si debe ser único en tu lógica.

                empleadoRepository.Add(nuevoEmpleado);
                CustomOkMessageBox.Show("Registro exitoso");

                // --- Navegar al Login ---
                var loginView = new InicioSesionView();
                loginView.Show();
                Close();

            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627) // Violación de UNIQUE constraint (podría ser Usuario o RFC si lo hiciste UNIQUE)
                {
                    CustomOkMessageBox.Show($"Error: El nombre de usuario '{nuevoEmpleado.Usuario}' o el RFC '{nuevoEmpleado.RFC}' ya existen.");
                }
                else if (ex.Number == 547) // Violación de Foreign Key o Check constraint (poco probable aquí)
                {
                    CustomOkMessageBox.Show($"Error de datos: {ex.Message}");
                }
                else if (ex.Number == 2628) // Dato demasiado largo
                {
                    CustomOkMessageBox.Show($"Error: Uno de los campos excede la longitud permitida.");
                }
                else
                {
                    CustomOkMessageBox.Show($"Error de base de datos ({ex.Number}): {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                CustomOkMessageBox.Show($"Error al registrar empleado: {ex.Message}\n{ex.StackTrace}");
            }
            finally
            {
                contraseñaPlana = null; // Limpiar contraseña de memoria
            }
        }

        // Función temporal para generar un ID si no es IDENTITY.
        // ¡ESTO NO ES RECOMENDABLE PARA PRODUCCIÓN! Deberías usar IDENTITY o GUIDs.
        private int GenerarIdUnicoTemporal()
        {
            // Intenta obtener el último ID + 1. ¡Esto puede fallar con concurrencia!
            try
            {
                var ultimoId = empleadoRepository.GetAll().Max(emp => (int?)emp.ID_Empleado) ?? 0;
                return ultimoId + 1;
            }
            catch
            {
                // Si falla o no hay empleados, usa un timestamp (menos fiable aún para unicidad)
                return (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            }
        }

        // --- Otros botones (Minimizar, Cerrar, IniciarSesion, Regresar) ---
        private void btnMinimizar_Click(object sender, RoutedEventArgs e)
        { this.WindowState = WindowState.Minimized; }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        { Close(); }

        private void btnIniciarSesion_Click(object sender, RoutedEventArgs e)
        {
            var inicioSesion = new InicioSesionView();
            inicioSesion.Show();
            Close();
        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            var inicioRegistro = new RegistroView();
            inicioRegistro.Show();
            Close();
        }
    }
}