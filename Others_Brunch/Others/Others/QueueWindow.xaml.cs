using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Data.SqlClient;
using System.Collections.ObjectModel;

namespace Others
{
    public partial class QueueWindow : Window
    {
        private MediaPlayer mediaPlayer = new MediaPlayer();
        public ObservableCollection<Song> Queue { get; set; }
        public string CurrentSongDisplay { get; set; }

        public QueueWindow()
        {
            InitializeComponent();
            Queue = new ObservableCollection<Song>();
            CurrentSongDisplay = "Not playing"; // Default text when no song is playing
            QueueListBox.ItemsSource = Queue;
            LoadQueueFromDatabase();
            DataContext = this;
        }

        private void LoadQueueFromDatabase()
        {
            // Replace with your actual connection string
            string connectionString = @"Server=LAPTOP-82HME98S\SQLEXPRESS01;Database=BASE;Trusted_Connection=True"; 
            int playlistId = 1; // Assume that there is a playlist which acts as the queue

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sqlQuery = $"SELECT Songs.Title, Artists.Name FROM Songs INNER JOIN Artists ON Songs.ArtistID = Artists.ArtistID WHERE Songs.SongID IN (SELECT SongID FROM PlaylistSongs WHERE PlaylistID = {playlistId})";
                SqlCommand command = new SqlCommand(sqlQuery, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Song song = new Song
                        {
                            Title = reader.GetString(0),
                            Artist = reader.GetString(1)
                        };
                        Queue.Add(song);
                    }

                    if (Queue.Count == 0)
                    {
                        CurrentSongDisplay = "There are no songs in the queue";
                    }
                    else
                    {
                        CurrentSongDisplay = $"Currently playing: {Queue[0].Title} by {Queue[0].Artist}";
                    }
                }
            }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var song = button?.Tag as Song;
            if (song != null)
            {
                NowPlayingTextBlock.Text = $"Currently playing: {song.Title} by {song.Artist}";
                // Add your code to play the song
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var song = button?.Tag as Song;
            if (song != null)
            {
                // Replace with your actual connection string
                string connectionString = "Your Connection String Here";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    int playlistId = 1; // Your playlist/queue ID
                    string sqlQuery = $"DELETE FROM PlaylistSongs WHERE PlaylistID = {playlistId} AND SongID = {song.SongID}";

                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.ExecuteNonQuery();
                }

                Queue.Remove(song);

                if (Queue.Count == 0)
                {
                    NowPlayingTextBlock.Text = "There are no songs in the queue";
                }
            }
        }

        private void TextBlock_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var textBlock = sender as TextBlock;
            var song = textBlock?.DataContext as Song;
            if (song != null)
            {
                // Open feedback window for the song
                FeedbackWindow feedbackWindow = new FeedbackWindow(song);
                feedbackWindow.Show();
                this.Close();
            }
        }
    }

    
}



