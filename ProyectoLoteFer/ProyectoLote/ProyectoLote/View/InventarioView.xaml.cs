using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ProyectoLote.Model; 
using ProyectoLote.CustomControl;

namespace ProyectoLote.View
{
    public partial class InventarioView : Window
    {
        public InventarioView(IEnumerable<CarroModel> carrosAMostrar)
        {
            InitializeComponent();
            LoadInventario(carrosAMostrar);
        }

        private void LoadInventario(IEnumerable<CarroModel> carritos)
        {
            try
            {
                if (carritos == null || !carritos.Any())
                {
                    MessageBox.Show("No hay carros para mostrar.");
                    Inventario.ItemsSource = null;
                }
                else
                {
                    Inventario.ItemsSource = carritos;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al mostrar inventario: {ex.Message}");
                Inventario.ItemsSource = null; // Limpiar en caso de error
            }
        }

        private void btnMinimizar_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            RegresarABusqueda();
        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            RegresarABusqueda();
        }

        //Metodo para regresar a buscar un carrito y no tener que colocarlo en cada situación
        private void RegresarABusqueda()
        {
            try
            {
                var buscarView = new BuscarAuto(); // O MenuInventarioView si corresponde
                buscarView.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo regresar: {ex.Message}");
            }
        }
    }
}