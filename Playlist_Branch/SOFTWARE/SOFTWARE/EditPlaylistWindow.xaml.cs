using PLAYLIST;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Media;

namespace PLAYLIST
{
    public partial class EditPlaylistWindow : Window
    {
        private string playlistName;
        private ObservableCollection<Song> playlistSongs = new ObservableCollection<Song>();
        public ObservableCollection<Song> Songs { get; } = new ObservableCollection<Song>();
        private List<Song> songs = new List<Song>();

        public EditPlaylistWindow(string playlistName)
        {
            InitializeComponent();
            this.playlistName = playlistName;
            LoadPlaylistData();
        }

        private void LoadPlaylistData()
        {
            try
            {
                string connectionString = @"Data Source=LAPTOP-82HME98S\SQLEXPRESS01;Initial Catalog=BASE;Integrated Security=True";
                string query = @"SELECT Songs.Title, Songs.Genre, Songs.Duration
                             FROM Songs
                             JOIN PlaylistSongs ON Songs.SongID = PlaylistSongs.SongID
                             JOIN Playlists ON PlaylistSongs.PlaylistID = Playlists.PlaylistID
                             WHERE Playlists.Name = @PlaylistName";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PlaylistName", playlistName);

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            string title = reader["Title"].ToString();
                            string genre = reader["Genre"].ToString();
                            TimeSpan duration = TimeSpan.Parse(reader["Duration"].ToString());

                            playlistSongs.Add(new Song { SongTitle = title, ArtistName = genre });
                        }
                    }
                }

                // Enlaza la lista de canciones de la playlist al ListView
                PlaylistListView.ItemsSource = playlistSongs;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la playlist: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddSong_Click(object sender, RoutedEventArgs e)
        {
            var selectSongsWindow = new SelectSongsWindow();
            if (selectSongsWindow.ShowDialog() == true) // Muestra la ventana y espera a que el usuario termine
            {
                var selectedSongs = selectSongsWindow.SelectedSongs;
                foreach (var song in selectedSongs)
                {
                    Songs.Add(song); // Asume que Songs es tu ObservableCollection<Song> visible en la UI
                    SaveChanges(); // Guarda los cambios, si es necesario
                }
            }
        }

        private void SaveChanges()
        {
            try
            {
                string connectionString = @"Data Source=LAPTOP-82HME98S\SQLEXPRESS01;Initial Catalog=BASE;Integrated Security=True";

                // Conexión con la base de datos
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Insertar las nuevas canciones en la playlist en la base de datos
                    foreach (var song in Songs)
                    {
                        string insertQuery = @"INSERT INTO PlaylistSongs (PlaylistID, SongID) 
                                       VALUES ((SELECT PlaylistID FROM Playlists WHERE Name = @PlaylistName), 
                                               (SELECT SongID FROM Songs WHERE Title = @SongTitle))";
                        using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@PlaylistName", playlistName);
                            insertCommand.Parameters.AddWithValue("@SongTitle", song.SongTitle);
                            insertCommand.ExecuteNonQuery();
                        }
                    }
                }

                MessageBox.Show("Changes saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving changes: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RemoveSong_Click(object sender, RoutedEventArgs e)
        {
            var selectedSong = PlaylistListView.SelectedItem as Song;

            if (selectedSong != null)
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to remove '{selectedSong.SongTitle}'?", "Confirm Removal", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        string connectionString = @"Data Source=LAPTOP-82HME98S\SQLEXPRESS01;Initial Catalog=BASE;Integrated Security=True";
                        string deleteQuery = @"DELETE FROM PlaylistSongs 
                                       WHERE PlaylistID = (SELECT PlaylistID FROM Playlists WHERE Name = @PlaylistName) 
                                         AND SongID = (SELECT SongID FROM Songs WHERE Title = @SongTitle)";

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection))
                            {
                                deleteCommand.Parameters.AddWithValue("@PlaylistName", playlistName);
                                deleteCommand.Parameters.AddWithValue("@SongTitle", selectedSong.SongTitle);
                                deleteCommand.ExecuteNonQuery();
                            }
                        }

                        // Remove the song from the playlist in the UI
                        playlistSongs.Remove(selectedSong);

                        MessageBox.Show("Song removed successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error removing song: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a song to remove.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


    }
}