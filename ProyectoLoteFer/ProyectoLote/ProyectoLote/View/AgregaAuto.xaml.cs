using ProyectoLote.Model;
using ProyectoLote.Repositories;
using System;
using System.Windows;

namespace ProyectoLote.View
{
    public partial class AgregaAuto : Window
    {
        private readonly ICarroRepository carroRepository;

        public AgregaAuto()
        {
            InitializeComponent();
            carroRepository = new CarroRepository(); // o usa inyección de dependencias si estás usando un contenedor
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMarca.Text) ||
                    string.IsNullOrWhiteSpace(txtModelo.Text) ||
                    string.IsNullOrWhiteSpace(txtAño.Text))
                {
                    MessageBox.Show("Por favor, complete todos los campos.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Generar un VIN aleatorio de 17 caracteres (simplificación)
                var vin = Guid.NewGuid().ToString("N").Substring(0, 17).ToUpper();

                var nuevoAuto = new CarroModel
                {
                    Vin = vin,
                    Marca = txtMarca.Text.Trim(),
                    Modelo = txtModelo.Text.Trim(),
                    Color = "Negro", // Podrías agregar un TextBox para color si lo deseas
                    Existencia = true,
                    ClaveVendedor = 1, // Valor fijo o puedes pasarlo desde otra parte del sistema
                    Precio = 150000 // Podrías agregar otro campo para permitir ingresar precio
                };

                carroRepository.Add(nuevoAuto);

                MessageBox.Show("Auto registrado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close(); // Cierra la ventana tras agregar
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al registrar el auto: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnMinimizar_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

