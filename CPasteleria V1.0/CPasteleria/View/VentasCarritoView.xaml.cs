// CPasteleria/View/VentasCarritoView.xaml.cs
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CPasteleria.CustomControls; // Si usas mensajes
using CPasteleria.Model;
using CPasteleria.Repositories;
using System.Collections.Generic;

namespace CPasteleria.View
{
    public partial class VentasCarritoView : Window
    {
        // ----- VERIFICACIÓN 1: Declaración ÚNICA del repositorio -----
        // Debe estar declarado UNA SOLA VEZ aquí, como miembro de la clase.
        private readonly ICarritoRepository carritoRepository;

        public VentasCarritoView()
        {
            InitializeComponent();
            // ----- VERIFICACIÓN 2: Inicialización SIN redeclarar tipo -----
            // Aquí se le asigna valor al miembro de la clase, NO se declara de nuevo.
            carritoRepository = new CarritoRepository();
            LoadCarrito();
        }

        private void LoadCarrito()
        {
            try
            {
                var items = carritoRepository.GetAll();

                // ----- VERIFICACIÓN 3: Nombre CORRECTO del ListView -----
                // Asegúrate que el ListView en tu VentasCarritoView.xaml tiene x:Name="Prueba"
                Prueba.ItemsSource = items;

                if (items == null || !items.Any())
                {
                    // Mensaje opcional
                }
            }
            catch (Exception ex)
            {
                CustomOkMessageBox.Show($"Error al cargar el carrito: {ex.Message}");
                // ----- VERIFICACIÓN 4: Nombre CORRECTO del ListView (en catch) -----
                Prueba.ItemsSource = null;
            }
        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            var venta = new RealizarVentaView();
            venta.Show();
            Close();
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
            Close();
        }
    }
}