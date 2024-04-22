
using System.Net.Mail;
using System.Windows;

namespace Sesion
{
    public partial class ForgotPasswordWindow : Window
    {
        public ForgotPasswordWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            Close(); // Cerrar la ventana actual después de abrir la ventana de inicio de sesión
        }

        private void SendResetLinkButton_Click(object sender, RoutedEventArgs e)
        {
            string email = emailTextBox.Text;

            if (IsValidEmail(email))
            {
                // Enviar el correo electrónico con el enlace de restablecimiento aquí
                MessageBox.Show("Se ha enviado un correo con el enlace para restablecer la contraseña.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Por favor, introduce una dirección de correo electrónico válida.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                MailAddress mailAddress = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
