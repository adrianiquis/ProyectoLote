// CPasteleria/CustomControls/BindablePasswordBox.xaml.cs
using System;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Runtime.InteropServices;

namespace CPasteleria.CustomControls
{
    public partial class BindablePasswordBox : UserControl
    {
        private bool _isPasswordChanging;

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register(
                nameof(Password),
                typeof(SecureString),
                typeof(BindablePasswordBox),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnPasswordPropertyChanged
                )
            );

        public SecureString Password
        {
            get { return (SecureString)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        public BindablePasswordBox()
        {
            InitializeComponent();
            // Asumiendo que el PasswordBox interno se llama 'txtContrasenia' en tu BindablePasswordBox.xaml
            if (this.FindName("txtContrasenia") is PasswordBox internalPasswordBox)
            {
                internalPasswordBox.PasswordChanged += OnInternalPasswordChanged;
            }
            // Si tu PasswordBox en BindablePasswordBox.xaml tiene un x:Name diferente, actualiza "txtContrasenia" aquí.
            // Por ejemplo, si tu BindablePasswordBox.xaml es solo:
            // <UserControl ...>
            //    <PasswordBox x:Name="passwordBoxElement" />
            // </UserControl>
            // Entonces sería:
            // passwordBoxElement.PasswordChanged += OnInternalPasswordChanged;
            // En tu primer mensaje, el PasswordBox interno en BindablePasswordBox.xaml se llamaba "txtContrasenia",
            // por lo que:
            // txtContrasenia.PasswordChanged += OnInternalPasswordChanged;
        }

        private static void OnPasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as BindablePasswordBox;
            // Asegúrate de que el PasswordBox interno exista y tenga el nombre correcto.
            if (control != null && control.FindName("txtContrasenia") is PasswordBox internalPasswordBox && !control._isPasswordChanging)
            {
                control._isPasswordChanging = true;
                internalPasswordBox.Password = ConvertToUnsecureString((SecureString)e.NewValue);
                control._isPasswordChanging = false;
            }
        }

        private void OnInternalPasswordChanged(object sender, RoutedEventArgs e)
        {
            // (sender as PasswordBox) es el PasswordBox interno
            PasswordBox internalPasswordBox = sender as PasswordBox;
            if (internalPasswordBox != null && !_isPasswordChanging)
            {
                _isPasswordChanging = true;
                Password = internalPasswordBox.SecurePassword.Copy();
                _isPasswordChanging = false;
            }
        }

        private static string ConvertToUnsecureString(SecureString securePassword)
        {
            if (securePassword == null) return string.Empty;
            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                if (unmanagedString != IntPtr.Zero)
                    Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}