using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Security;
using System.Security.Principal; // Necesario para GenericPrincipal, GenericIdentity
using System.Threading;         // Necesario para Thread
using System.Windows.Input;
using CPasteleria.Repositories;
using CPasteleria.Model;
using CPasteleria.CustomControls; // Asegúrate de tener este using para CustomOkMessageBox

namespace CPasteleria.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        // Campos
        private string _username;
        private SecureString _password;
        // Ya no necesitaremos ErrorMessage como propiedad pública enlazada a un TextBlock
        // private string _errorMessage;
        private bool _isViewVisible = true;
        private IEmpleadoRepository empleadoRepository;

        // Evento para notificar a la Vista sobre un inicio de sesión exitoso
        public event EventHandler LoginSuccess;

        // Propiedades
        public string Username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged(nameof(Username)); }
        }

        public SecureString Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(nameof(Password)); }
        }

        /* Ya no se necesita esta propiedad si el error se muestra en un MessageBox
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; OnPropertyChanged(nameof(ErrorMessage)); }
        }
        */

        public bool IsViewVisible
        {
            get { return _isViewVisible; }
            set { _isViewVisible = value; OnPropertyChanged(nameof(IsViewVisible)); }
        }

        // Comandos
        public ICommand LoginCommand { get; }

        // Constructor
        public LoginViewModel()
        {
            empleadoRepository = new EmpleadoRepository();
            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
        }

        // Métodos privados para el comando
        private bool CanExecuteLoginCommand(object obj)
        {
            bool isUsernameValid = !string.IsNullOrWhiteSpace(Username) && Username.Length >= 3;
            bool isPasswordValid = Password != null && Password.Length >= 3;

            return isUsernameValid && isPasswordValid;
        }

        private void ExecuteLoginCommand(object obj)
        {
            var isValidUser = empleadoRepository.AuthenticateUser(new NetworkCredential(Username, Password));

            if (isValidUser)
            {
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(Username), null);
                OnLoginSuccess();
            }
            else
            {
                // En lugar de asignar a ErrorMessage, mostramos el CustomOkMessageBox
                CustomOkMessageBox.Show("Credenciales incorrectas");
            }
        }

        // Método protegido para disparar el evento LoginSuccess
        protected virtual void OnLoginSuccess()
        {
            LoginSuccess?.Invoke(this, EventArgs.Empty);
        }
    }
}