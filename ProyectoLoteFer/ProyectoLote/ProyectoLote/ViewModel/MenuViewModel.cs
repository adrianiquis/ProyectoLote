using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading; // Para Thread.CurrentPrincipal
using System.Threading.Tasks;
using ProyectoLote.Model; // Para UserModel
using ProyectoLote.Repositories; // Para IUserRepository

namespace ProyectoLote.View // Asegúrate que el namespace sea correcto
{
    public class MainViewModel : ViewModelBase
    {
        // Campos
        private UserModel _currentUser;
        private IUserRepository userRepository;

        // Propiedades (para enlazar a la Vista)
        public UserModel CurrentUser
        {
            get { return _currentUser; }
            set
            {
                _currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
                // Notificar cambio también para las propiedades derivadas
                OnPropertyChanged(nameof(CurrentUserDisplayName));
            }
        }

        public string CurrentUserDisplayName
        {
            get
            {
                if (CurrentUser != null && !string.IsNullOrEmpty(CurrentUser.nombre))
                    // Devuelve el Nombre del empleado si existe
                    return $"Bienvenido/a {CurrentUser.nombre}";
                else if (CurrentUser != null && !string.IsNullOrEmpty(CurrentUser.usuario))
                    // Si no hay nombre, muestra el usuario
                    return $"Bienvenido/a {CurrentUser.usuario}";
                else
                    // Mensaje por defecto si no se encuentra el usuario
                    return "Usuario no logueado";
            }
        }

        // Constructor
        public MainViewModel()
        {
            userRepository = new UserRepository();
            LoadCurrentUserData();
        }

        // Métodos
        private void LoadCurrentUserData()
        {
            // Obtiene el nombre de usuario del hilo principal (establecido en LoginViewModel)
            var username = Thread.CurrentPrincipal?.Identity?.Name;

            if (!string.IsNullOrEmpty(username))
            {
                // Busca los datos completos del empleado en la base de datos
                CurrentUser = userRepository.GetByUsername(username);
                // Si CurrentUser es null después de buscar, puedes poner un valor por defecto o loggear un error.
                if (CurrentUser == null)
                {
                    // Opcional: Manejar caso donde el usuario del Thread no está en la BD
                    Console.WriteLine($"Error: No se encontró el empleado con usuario '{username}' en la base de datos.");
                    // Podrías asignar un usuario temporal o dejarlo null y que CurrentUserDisplayName muestre el mensaje por defecto.
                    CurrentUser = new UserModel { usuario = username }; // Asigna al menos el nombre de usuario si no se encontró el nombre completo
                }
            }
            else
            {

                Console.WriteLine("Error: No hay usuario autenticado en el hilo actual.");
                CurrentUser = null;
            }

            OnPropertyChanged(nameof(CurrentUserDisplayName));
        }

    }
}