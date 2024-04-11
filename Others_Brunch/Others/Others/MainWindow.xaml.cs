using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Others
{


    public partial class SearchWindow : Window
    {
        // Reemplaza con tu cadena de conexión real
        private readonly string connectionString = @"Server=LAPTOP-82HME98S\SQLEXPRESS01;Database=BASE;Trusted_Connection=True";

        public SearchWindow()
        {
            InitializeComponent();
            LoadPlaylistsFromDatabase();
        }

        private void LoadPlaylistsFromDatabase()
        {
            List<Playlist> playlists = new List<Playlist>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT PlaylistID, Name, Description, CreationDate FROM Playlists";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                playlists.Add(new Playlist
                                {
                                    PlaylistID = Convert.ToInt32(reader["PlaylistID"]),
                                    Name = reader["Name"].ToString(),
                                    Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? "" : reader["Description"].ToString(),
                                    CreationDate = Convert.ToDateTime(reader["CreationDate"]),
                                    IsLiked = false,  // Asumiendo estado inicial
                                    IsFollowed = false  // Asumiendo estado inicial
                                });
                            }
                        }
                    }
                    PlaylistResultsListBox.ItemsSource = playlists;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading playlists from database: {ex.Message}", "Error");
                }
            }
        }

       
        public class Playlist
        {
            public int PlaylistID { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public DateTime CreationDate { get; set; }
            public bool IsLiked { get; set; }
            public bool IsFollowed { get; set; }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchQuery = SearchBar.Text.ToLower().Trim();
            List<Playlist> playlists = new List<Playlist>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT PlaylistID, Name, Description, CreationDate FROM Playlists WHERE LOWER(Name) LIKE @searchQuery";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Asegúrate de que el término de búsqueda esté correctamente formateado para SQL LIKE
                        command.Parameters.AddWithValue("@searchQuery", $"%{searchQuery}%");
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                playlists.Add(new Playlist
                                {
                                    PlaylistID = Convert.ToInt32(reader["PlaylistID"]),
                                    Name = reader["Name"].ToString(),
                                    Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? "" : reader["Description"].ToString(),
                                    CreationDate = Convert.ToDateTime(reader["CreationDate"]),
                                    IsLiked = false,
                                    IsFollowed = false
                                });
                            }
                        }
                    }

                    // Actualiza directamente la lista de resultados sin filtrar adicional en C#
                    PlaylistResultsListBox.ItemsSource = playlists;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error searching playlists: {ex.Message}", "Error");
                }
            }
        }
        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var playlist = button.DataContext as Playlist;
                if (playlist != null)
                {
                    // Abre la ventana de detalles con la información de la playlist
                    var detailsWindow = new PlaylistWindow(playlist.PlaylistID);
                    detailsWindow.Show();
                    this.Close();
                }
                else
                {
                    // El DataContext no es una instancia de Playlist o es null
                    MessageBox.Show("No se pudo obtener la información de la playlist.");
                }
            }
        }



    }




}




