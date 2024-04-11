using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Others
{
    public partial class FeedbackWindow : Window
    {
        public ObservableCollection<string> PreviousComments { get; set; }
        private readonly string connectionString = @"Server=LAPTOP-82HME98S\SQLEXPRESS01;Database=BASE;Trusted_Connection=True";
        private int songID;
      
        private int userRating = 0;

        public FeedbackWindow(Song song)
        {
            int songID = song.ID;
            InitializeComponent();
            this.songID = songID;
            PreviousComments = new ObservableCollection<string>();
            LoadSongDetails();
            LoadPreviousComments();
            DataContext = this;
        }

        private void LoadSongDetails()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
                    SELECT Songs.Title, Artists.Name, Albums.CoverArt
                    FROM Songs
                    INNER JOIN Artists ON Songs.ArtistID = Artists.ArtistID
                    INNER JOIN Albums ON Songs.AlbumID = Albums.AlbumID
                    WHERE Songs.SongID = @SongID";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@SongID", songID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            SongNameTextBlock.Text = reader.GetString(0);
                            ArtistNameTextBlock.Text = reader.GetString(1);
                            string coverArtPath = reader.GetString(2);

                            AlbumCoverImage.Source = new BitmapImage(new Uri(coverArtPath, UriKind.Absolute));
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading song details: " + ex.Message);
                }
            }
        }

        private void LoadPreviousComments()
        {
            PreviousComments.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT Comment, Username FROM Feedback INNER JOIN Users ON Feedback.UserID = Users.UserID WHERE SongID = @SongID ORDER BY DateAndTime DESC";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@SongID", songID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string comment = reader.GetString(0);
                            string username = reader.GetString(1);
                            PreviousComments.Add(username + ": " + comment);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading previous comments: " + ex.Message);
                }
            }
        }

        private void StarButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                userRating = Convert.ToInt32(button.Tag);
                UpdateStarRatingDisplay(userRating);
            }
        }

        private void UpdateStarRatingDisplay(int rating)
        {
            // Assuming you have a StackPanel with the name 'StarsPanel' that contains the star buttons.
            StackPanel starsPanel = this.FindName("StarsPanel") as StackPanel;
            if (starsPanel != null)
            {
                foreach (Button starButton in starsPanel.Children)
                {
                    int starRating = Convert.ToInt32(starButton.Tag);
                    starButton.Foreground = starRating <= rating ? Brushes.Yellow : Brushes.Gray;
                }
            }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string comment = CommentTextBox.Text.Trim();
            if (string.IsNullOrEmpty(comment))
            {
                MessageBox.Show("Please enter a comment before submitting.", "Input Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
                    INSERT INTO Feedback (UserID, SongID, Rating, Comment, DateAndTime)
                    VALUES (@UserID, @SongID, @Rating, @Comment, @DateAndTime)";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@UserID", "SELECT UserID FROM Users");
                    command.Parameters.AddWithValue("@SongID", songID);
                    command.Parameters.AddWithValue("@Rating", userRating);
                    command.Parameters.AddWithValue("@Comment", comment);
                    command.Parameters.AddWithValue("@DateAndTime", DateTime.Now);

                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Your feedback has been submitted successfully.");
                        PreviousComments.Insert(0, comment); // Add the comment to the top of the list
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error submitting feedback: " + ex.Message);
                }
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the current window and open the MainWindow
            var mainWindow = new SearchWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}

