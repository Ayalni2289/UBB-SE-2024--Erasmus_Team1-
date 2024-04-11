using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using static Others.SearchWindow;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Media;

namespace Others
{
    public partial class PlaylistWindow : Window
    {
        private readonly string connectionString = @"Server=LAPTOP-82HME98S\SQLEXPRESS01;Database=BASE;Trusted_Connection=True";
        private int playlistID;

        public PlaylistWindow(int playlistID)
        {
            InitializeComponent();
            this.playlistID = playlistID;
            LoadSongsFromDatabase();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            // Crea una nueva instancia de la ventana QueueWindow y la muestra
            QueueWindow queueWindow = new QueueWindow();
            queueWindow.Show();
            this.Close();
        }
        private void LoadSongsFromDatabase()
        {
            List<Song> songs = new List<Song>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT s.Title, a.Name AS ArtistName FROM Songs s JOIN PlaylistSongs ps ON s.SongID = ps.SongID JOIN Artists a ON s.ArtistID = a.ArtistID WHERE ps.PlaylistID = @PlaylistID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PlaylistID", playlistID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                songs.Add(new Song
                                {
                                    
                                    Title = reader["Title"].ToString(),
                                    ArtistID = reader["ArtistName"].ToString()
                                });
                            }
                        }
                    }

                    SongsListBox.ItemsSource = songs;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading songs from database: {ex.Message}", "Error");
                }
            }
        }
    }
    public class Song
    {
        public string Title { get; set; }
        public string ArtistID { get; set; }
        public string Artist { get; set; }
        public string SongID { get; set; }
        public int ID { get; set; }

    }
    

}

