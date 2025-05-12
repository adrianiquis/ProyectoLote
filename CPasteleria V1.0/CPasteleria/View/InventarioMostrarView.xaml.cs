// CPasteleria/View/InventarioMostrarView.xaml.cs
using System;
using System.Collections.Generic; // <-- Añadir
using System.Linq; // <-- Añadir
using System.Windows;
using CPasteleria.CustomControls; // <-- Añadir si usas mensajes
using CPasteleria.Model;
using CPasteleria.Repositories;

namespace CPasteleria.View
{
    public partial class InventarioMostrarView : Window
    {
        // Asume controles:
        // ListView: Inventario (asegúrate que en XAML tenga GridView con columnas enlazadas
        //                 ej: <GridViewColumn Header="Nombre" DisplayMemberBinding="{Binding Nombre}" />)
        // Button: btnRegresar, btnMinimizar, btnSalir

        // Constructor modificado para aceptar la lista
        public InventarioMostrarView(IEnumerable<PastelModel> pastelesAMostrar)
        {
            InitializeComponent();
            LoadInventario(pastelesAMostrar);
        }

        private void LoadInventario(IEnumerable<PastelModel> pasteles)
        {
            try
            {
                // Asigna la lista recibida al ItemsSource del ListView
                Inventario.ItemsSource = pasteles;

                // Mensaje si la lista está vacía
                if (pasteles == null || !pasteles.Any())
                {
                    CustomOkMessageBox.Show("No se encontraron pasteles para mostrar.");
                    // Podrías deshabilitar botones o cerrar la ventana si lo deseas
                }
            }
            catch (Exception ex)
            {
                CustomOkMessageBox.Show($"Error al mostrar inventario: {ex.Message}");
                Inventario.ItemsSource = null; // Limpiar en caso de error
            }
        }

        // Los botones Anterior/Siguiente no tienen sentido si mostramos una lista completa
        // private void btnAnterior_Click(object sender, RoutedEventArgs e) { }
        // private void btnSiguiente_Click(object sender, RoutedEventArgs e) { }

        private void btnMinimizar_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            // Considera si 'Salir' debe cerrar la app o ir al menú anterior
            // Close();
            // O regresar a la pantalla de búsqueda/menú inventario
            var buscarView = new InventarioBuscarView(); // O MenuInventarioView si prefieres
            buscarView.Show();
            Close();
        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            // Regresa a la pantalla anterior (menú buscar o menú inventario)
            var buscarView = new InventarioBuscarView(); // O MenuInventarioView
            buscarView.Show();
            Close();
        }
    }
}