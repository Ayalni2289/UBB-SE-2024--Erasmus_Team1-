using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace PLAYLIST
{
    public partial class SelectSongsWindow : Window
    {
        public List<Song> SelectedSongs { get; private set; } = new List<Song>();

        public SelectSongsWindow()
        {
            InitializeComponent();
            LoadAvailableSongs();
        }

        private void LoadAvailableSongs()
        {
            try
            {
                // Conecta con la base de datos y ejecuta la consulta para obtener las canciones disponibles
                string connectionString = @"Data Source=LAPTOP-82HME98S\SQLEXPRESS01;Initial Catalog=BASE;Integrated Security=True";
                string query = "SELECT s.Title, a.Name AS ArtistName FROM Songs s INNER JOIN Artists a ON s.ArtistID = a.ArtistID";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Song song = new Song
                        {
                            SongTitle = reader["Title"].ToString(),
                            ArtistName = reader["ArtistName"].ToString()
                        };

                        AvailableSongsListView.Items.Add(song);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading available songs: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddSongs_Click(object sender, RoutedEventArgs e)
        {
            foreach (Song song in AvailableSongsListView.SelectedItems)
            {
                SelectedSongs.Add(song);
            }
            this.DialogResult = true; // Cierra la ventana y señala que se seleccionaron canciones
        }
    }
}
