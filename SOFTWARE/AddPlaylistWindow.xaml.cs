using System;
using System.Data.SqlClient;
using System.Windows;

namespace PLAYLIST
{
    public partial class AddPlaylistWindow : Window
    {
        public AddPlaylistWindow()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Verificación básica de campos no vacíos
            if (string.IsNullOrWhiteSpace(txtPlaylistName.Text) || string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("Please fill in all the fields.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-MS7L59C;Initial Catalog=Software;Integrated Security=True"))
                {
                    conn.Open();
                    string sql = "INSERT INTO Playlists (OwnerID, Name, Description, CreationDate) VALUES (@OwnerID, @Name, @Description, @CreationDate)";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Aquí debes reemplazar el valor de @OwnerID con el ID real del propietario de la playlist.
                        // Esto podría venir de la sesión del usuario o una selección en la interfaz de usuario.
                        cmd.Parameters.AddWithValue("@OwnerID", 1); // Ejemplo con '1' como ID del propietario
                        cmd.Parameters.AddWithValue("@Name", txtPlaylistName.Text);
                        cmd.Parameters.AddWithValue("@Description", txtDescription.Text);
                        cmd.Parameters.AddWithValue("@CreationDate", DateTime.Now);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Playlist added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close(); // Opcional: Cierra la ventana después de la inserción exitosa
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to add playlist: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

