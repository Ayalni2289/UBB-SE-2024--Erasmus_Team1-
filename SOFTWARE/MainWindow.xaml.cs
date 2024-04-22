using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using System.ComponentModel;
using System.Windows.Media;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace PLAYLIST
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Song> Songs { get; } = new ObservableCollection<Song>();
        private List<Song> songs = new List<Song>();

        private string playlistName;

        // Constructor que acepta el nombre de una playlist
        public MainWindow(string playlistName)
        {
            InitializeComponent();
            this.playlistName = playlistName;
            LoadSongsFromPlaylist();
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            LoadSongs();
        }
        private string connectionString = @"Data Source=DESKTOP-MS7L59C;Initial Catalog=Software;Integrated Security=True";

        private void LoadSongsFromPlaylist()
        {
            try
            {
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

                            // Aquí puedes agregar las canciones a tu ListBox songListBox
                            songListBox.Items.Add($"Title: {title}, Genre: {genre}, Duration: {duration}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading songs: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }




        private void LoadSongs()
        {
           

            string query = "SELECT s.Title, a.Name AS ArtistName FROM Songs s INNER JOIN Artists a ON s.ArtistID = a.ArtistID";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Song song = new Song
                        {
                            SongTitle = reader["Title"].ToString(), // Nombre de la columna que contiene los títulos de las canciones
                            ArtistName = reader["ArtistName"].ToString(), // Nombre de la columna que contiene los nombres de los artistas
                        };

                        Songs.Add(song);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading songs: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }




        private void Play_Click(object sender, RoutedEventArgs e)
        {
            var selectedSong = PlaylistListView.SelectedItem as Song;
            if (selectedSong != null)
            {
                MessageBox.Show($"Playing: {selectedSong.SongTitle} by {selectedSong.ArtistName}");
            }
            else
            {
                MessageBox.Show("Please select a song to play.");
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            var selectedSong = PlaylistListView.SelectedItem as Song;
            if (selectedSong != null)
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to remove {selectedSong.SongTitle}?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    Songs.Remove(selectedSong);
                    SaveChanges();
                }
            }
            else
            {
                MessageBox.Show("Please select a song to remove.");
            }
        }


        private void SaveChanges()
        {
            try
            {
                string filePath = "path_to_your_file.json";
                string json = JsonConvert.SerializeObject(Songs, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving changes: {ex.Message}");
            }
        }

        private void Shuffle_Click(object sender, RoutedEventArgs e)
        {
            var shuffledSongs = Songs.OrderBy(x => System.Guid.NewGuid()).ToList();
            Songs.Clear();
            foreach (var song in shuffledSongs)
            {
                Songs.Add(song);
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


        



        

        private void Share_Click(object sender, RoutedEventArgs e)
        {
            var playlistText = string.Join("\n", Songs.Select(song => $"Title: {song.SongTitle}, Artist: {song.ArtistName}"));
            Clipboard.SetText(playlistText);
            MessageBox.Show("Playlist copied to clipboard.");
        }

        private void OpenPlaylistListWindow_Click(object sender, RoutedEventArgs e)
        {
            // Crear una instancia de la ventana PlaylistListWindow
            var playlistListWindow = new PlaylistListWindow();

            // Mostrar la ventana
            playlistListWindow.Show();
        }

    }



    public class Song : INotifyPropertyChanged
    {
        private string songTitle;
        public string SongTitle
        {
            get => songTitle;
            set
            {
                if (songTitle != value)
                {
                    songTitle = value;
                    OnPropertyChanged(nameof(SongTitle));
                }
            }
        }

        private string artistName;
        public string ArtistName
        {
            get => artistName;
            set
            {
                if (artistName != value)
                {
                    artistName = value;
                    OnPropertyChanged(nameof(ArtistName));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

