using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using Events;

namespace Events
{
    public partial class Event : Window
    {
        private string _connectionString = @"Data Source=LAPTOP-82HME98S\SQLEXPRESS01;Initial Catalog=BASE;Integrated Security=True;";


        public Event()
        {
            InitializeComponent();
            LoadArtists();
        }

        private void LoadArtists()
        {
            List<ArtistModel> artists = new List<ArtistModel>();
            string query = "SELECT ArtistID, Name FROM Artists";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        artists.Add(new ArtistModel
                        {
                            ArtistID = (int)reader["ArtistID"],
                            Name = reader["Name"].ToString()
                        });
                    }
                    ArtistsComboBox.ItemsSource = artists;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar artistas: {ex.Message}");
                }
            }
        }

        private void ArtistsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ArtistsComboBox.SelectedItem is ArtistModel selectedArtist)
            {
                LoadEventsForSelectedArtist(selectedArtist.ArtistID);
            }
        }

        private void LoadEventsForSelectedArtist(int artistId)
        {
            List<EventModel> events = new List<EventModel>();
            string query = $"SELECT Name, Location, DateTime FROM Events WHERE ArtistID = {artistId}";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        events.Add(new EventModel
                        {
                            Name = reader["Name"].ToString(),
                            Location = reader["Location"].ToString(),
                            DateTime = Convert.ToDateTime(reader["DateTime"])
                        });
                    }
                    EventsListBox.ItemsSource = events;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar eventos: {ex.Message}");
                }
            }
        }

        private void BuyTickets_Click(object sender, RoutedEventArgs e)
        {
            /// Assuming that you have selected an event to pass to the Buy window.
            if (EventsListBox.SelectedItem is EventModel selectedEvent)
            {
                // You need to retrieve the list of TicketTypes for the selected event here.
                // For example:
                List<TicketType> ticketTypes = GetTicketTypesForEvent(selectedEvent.EventID);

                // Correct the constructor call by passing all required arguments.
                Buy buyWindow = new Buy(selectedEvent.Location, selectedEvent.DateTime, selectedEvent.EventID);
                buyWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select an event first.");
            }
        }

        private List<TicketType> GetTicketTypesForEvent(int eventId)
        {
            List<TicketType> ticketTypes = new List<TicketType>();
            string query = "SELECT DISTINCT TicketType, Price FROM Tickets WHERE EventID = @EventID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@EventID", eventId);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string type = reader["TicketType"].ToString();
                            decimal price = (decimal)reader["Price"];
                            ticketTypes.Add(new TicketType(type, price));
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar tipos de tickets: {ex.Message}");
                }
            }

            return ticketTypes;
        }
    }

    public class ArtistModel
    {
        public int ArtistID { get; set; }
        public string Name { get; set; }
    }

    public class EventModel
    {
        public int EventID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime DateTime { get; set; }
    }
}

