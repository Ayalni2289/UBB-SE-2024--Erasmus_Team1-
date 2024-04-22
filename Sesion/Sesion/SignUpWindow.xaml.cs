using System.Windows;

namespace Sesion
{
    public partial class SignUpWindow : Window
    {
        public SignUpWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            Close(); // Cerrar la ventana actual después de abrir la ventana de inicio de sesión
        }



        

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // La clase MailAddress también puede validar el formato de la dirección de correo electrónico.
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

    private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameTextBox.Text;
            string email = emailTextBox.Text;
            string password = passwordBox.Password;
            string confirmPassword = confirmPasswordBox.Password;

            if (IsValidEmail(email) && password == confirmPassword)
            {
                DatabaseManager.SaveUser(username, email, password);
                MessageBox.Show("Usuario registrado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Por favor, introduce información válida.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}

