using System;
using System.Collections.Generic;
using System.Windows;
using System.Data.SqlClient;

namespace PLAYLIST
{
    public partial class PlaylistListWindow : Window
    {
        private string connectionString = @"Data Source=LAPTOP-82HME98S\SQLEXPRESS01;Initial Catalog=BASE;Integrated Security=True";

        // Constructor de tu clase
        public PlaylistListWindow()
        {
            InitializeComponent();
            LoadPlaylists(); // Llama a LoadPlaylists cuando la ventana se inicializa
        }

        private void LoadPlaylists()
        {
            
            List<string> playlists = new List<string>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Name FROM Playlists"; // Asume que quieres cargar los nombres de las playlists

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            playlists.Add(reader["Name"].ToString());
                        }
                    }
                }

                // Asume que tienes un ListBox o similar para mostrar las playlists
                // Reemplaza 'PlaylistListBox' con el nombre real de tu control en XAML
                PlaylistListBox.ItemsSource = playlists;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading playlists: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddPlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            // Crear una instancia de la ventana AddPlaylistWindow
            var addPlaylistWindow = new AddPlaylistWindow();

            // Mostrar la ventana de manera modal. Usa Show() si prefieres una ventana no modal.
            addPlaylistWindow.ShowDialog();
        }


        private void DeletePlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedPlaylistName = PlaylistListBox.SelectedItem as string;
            if (string.IsNullOrEmpty(selectedPlaylistName))
            {
                MessageBox.Show("Please select a playlist to delete.");
                return;
            }

            var result = MessageBox.Show($"Are you sure you want to delete '{selectedPlaylistName}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "DELETE FROM Playlists WHERE Name = @Name";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Name", selectedPlaylistName);
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Playlist deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                                LoadPlaylists(); // Recargar la lista de playlists
                            }
                            else
                            {
                                MessageBox.Show("Playlist not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting playlist: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void OpenPlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedPlaylistName = PlaylistListBox.SelectedItem as string;
            if (string.IsNullOrEmpty(selectedPlaylistName))
            {
                MessageBox.Show("Por favor, selecciona una lista de reproducción para abrir.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Suponiendo que EditPlaylistWindow acepta el nombre de la playlist como parámetro en su constructor
            var editPlaylistWindow = new EditPlaylistWindow(selectedPlaylistName);
            editPlaylistWindow.Show();
        }




       
    }
}
