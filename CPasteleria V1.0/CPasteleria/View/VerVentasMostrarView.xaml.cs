// En CPasteleria/View/VerVentasMostrarView.xaml.cs
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CPasteleria.CustomControls;
using CPasteleria.Model;
using CPasteleria.Repositories; // Aunque no se usa directamente aquí, es bueno tenerlo por contexto
using System.Collections.Generic;

namespace CPasteleria.View
{
    public partial class VerVentasMostrarView : Window
    {
        // Constructor para múltiples ventas (ej. "Ver Todas")
        public VerVentasMostrarView(IEnumerable<VentaModel> ventasAMostrar)
        {
            InitializeComponent();
            CargarVentas(ventasAMostrar);
        }

        // Constructor para una sola venta (ej. "Buscar por ID")
        public VerVentasMostrarView(VentaModel ventaAMostrar)
        {
            InitializeComponent();
            if (ventaAMostrar != null)
            {
                // El XAML con el ItemTemplate y Expander manejará la visualización de los detalles.
                // Simplemente pasamos una lista que contiene solo esta venta.
                CargarVentas(new List<VentaModel> { ventaAMostrar });
            }
            else
            {
                CustomOkMessageBox.Show("No se proporcionó una venta para mostrar o la venta no tiene detalles.");
                verVentas.ItemsSource = null; // ListView en XAML
            }
        }

        private void CargarVentas(IEnumerable<VentaModel> ventas)
        {
            try
            {
                verVentas.ItemsSource = ventas; // ListView en XAML

                if (ventas == null || !ventas.Any())
                {
                    // Opcional: Mostrar un mensaje si no hay nada que mostrar,
                    // aunque el constructor ya maneja el caso de ventaAMostrar == null.
                    // CustomOkMessageBox.Show("No hay ventas para mostrar.");
                }
            }
            catch (Exception ex)
            {
                CustomOkMessageBox.Show($"Error al cargar las ventas en la lista: {ex.Message}");
                verVentas.ItemsSource = null;
            }
        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            // Regresa a la vista de selección (VerVentasView)
            var vistaAnterior = new VerVentasView();
            vistaAnterior.Show();
            this.Close();
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
            // Considera si al cerrar aquí debería ir a VerVentasView o MenuVentasView
            var vistaAnterior = new VerVentasView(); // O MenuVentasView()
            vistaAnterior.Show();
            this.Close();
        }
    }
}