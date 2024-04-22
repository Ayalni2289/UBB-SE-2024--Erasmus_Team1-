using System.Windows;

namespace Sesion
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Simulando una comprobación de credenciales de inicio de sesión
            string userEmail = emailTextBox.Text;
            string userPassword = passwordBox.Password;

            if (userEmail == "user@example.com" && userPassword == "123")
            {
                MessageBox.Show("Login successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                
                //Aqui habría que entrar en el inicio de la aplicación
            }
            else
            {
                MessageBox.Show("Login failed. Please check your email and password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ForgotPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            ForgotPasswordWindow forgotPasswordWindow = new ForgotPasswordWindow();
            forgotPasswordWindow.Show();
            Close();
        }

        private void SignUpHyperlink_Click(object sender, RoutedEventArgs e)
        {
            SignUpWindow signUpWindow = new SignUpWindow();
            signUpWindow.ShowDialog();
            this.Close(); 
        }






    }
}

