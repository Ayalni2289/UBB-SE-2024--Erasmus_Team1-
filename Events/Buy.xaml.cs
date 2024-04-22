using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;

namespace Events
{
    public partial class Buy : Window
    {
        private string _connectionString = @"Data Source=LAPTOP-82HME98S\SQLEXPRESS01;Initial Catalog=BASE;Integrated Security=True;";
        private string _location;
        private DateTime _eventDate;
        private int _eventId; // Assume you pass the event ID to this window
        private List<TicketType> _ticketTypes;

        public Buy(string location, DateTime eventDate, int eventId)
        {
            InitializeComponent();
            _location = location;
            _eventDate = eventDate;
            _eventId = eventId; // Store the event ID
            LoadEventData();
        }

        private void LoadEventData()
        {
            // Set location and date text blocks
            locationTextBlock.Text = $"Location: {_location}";
            dateTextBlock.Text = $"Date: {_eventDate.ToString("MMMM dd, yyyy")}";

            // Crear los tipos de entradas directamente
            _ticketTypes = new List<TicketType>
            {
                new TicketType("Normal", 50), 
                new TicketType("VIP", 100)    
            };

            // Asignar los tipos de entradas al ComboBox
            ticketTypeComboBox.ItemsSource = _ticketTypes;
            ticketTypeComboBox.DisplayMemberPath = "DisplayInfo";
            ticketTypeComboBox.SelectedIndex = 0;
        }


        
        private void PurchaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (ticketTypeComboBox.SelectedItem is TicketType selectedTicketType)
            {
                if (!int.TryParse(quantityTextBox.Text, out int quantity) || quantity <= 0)
                {
                    MessageBox.Show("Please enter a valid number of tickets.");
                    return;
                }

                Payment paymentWindow = new Payment(selectedTicketType.Type, quantity, _location, _eventDate);
                paymentWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select a ticket type.");
            }
        }
    }
}



