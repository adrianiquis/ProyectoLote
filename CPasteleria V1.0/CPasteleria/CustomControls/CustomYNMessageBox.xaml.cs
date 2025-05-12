using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CPasteleria.CustomControls
{
    /// <summary>
    /// Lógica de interacción para CustomYNMessageBox.xaml
    /// </summary>
    public partial class CustomYNMessageBox : Window
    {

        public bool? Resultado { get; private set; }

        public CustomYNMessageBox(string mensaje)
        {
            InitializeComponent();
            etMensaje.Content = mensaje;
        }

        private void btnSi_Click(object sender, RoutedEventArgs e)
        {
            Resultado = true;
            this.Close();
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            Resultado = false;
            this.Close();
        }

        public static bool? Show(string mensaje)
        {
            var box = new CustomYNMessageBox(mensaje);
            box.ShowDialog();
            return box.Resultado;
        }
    }
}
