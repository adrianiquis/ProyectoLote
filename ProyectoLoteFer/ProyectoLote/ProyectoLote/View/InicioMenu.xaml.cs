using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ProyectoLote.Repositories;
using ProyectoLote.View;
using ProyectoLote.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ProyectoLote
{
    public partial class InicioMenu : Window
    {
        private readonly CarroRepository _carroRepository;

        public InicioMenu()
        {
            InitializeComponent();
            _carroRepository = new CarroRepository();
        }

        private void InitializeComponent()
        {
            throw new NotImplementedException();
        }

        private void btnInventario_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IEnumerable<CarroModel> listaCarros = _carroRepository.GetAll();

                if (listaCarros == null || !listaCarros.Any())
                {
                    MessageBox.Show("No hay autos en inventario para mostrar.");
                    return;
                }

                InventarioView inventario = new InventarioView(listaCarros);
                inventario.Show();
                this.Close(); // Cierra el menú actual si así deseas
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el inventario: {ex.Message}");
            }
        }

        private void btnMenuVentas_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Venta menuVentas = new Venta();
                menuVentas.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo abrir el menú de ventas: {ex.Message}");
            }
        }

        private void btnMenuInicio_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow main = new MainWindow();
                main.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo regresar al inicio: {ex.Message}");
            }
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
