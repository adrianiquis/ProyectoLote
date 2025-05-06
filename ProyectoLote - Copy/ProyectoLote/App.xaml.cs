using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ProyectoLote.View;

namespace ProyectoLote
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var inicioView = new InicioView();
            inicioView.Show();

            inicioView.LoginRequested += (s, ev) =>
            {
                var loginView = new LoginView();
                loginView.Show();
                inicioView.Close();
            };

           /* inicioView.RegisterRequested += (s, ev) =>
            {
                var registerView = new RegistrarView();
                registerView.Show();
                inicioView.Close();
            };*/
        }
    }
}
