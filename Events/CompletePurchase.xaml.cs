using Events;
using System.Windows;

namespace Events
{
    public partial class CompletePurchase : Window
    {
        private string _eventLocation;
        private DateTime _eventDate;
        private string _ticketType;
        private int _quantity;
        public CompletePurchase(string eventLocation, DateTime eventDate, string ticketType, int quantity)
        {
            InitializeComponent();
            _eventLocation = eventLocation;
            _eventDate = eventDate;
            _ticketType = ticketType;
            _quantity = quantity;
        }

        private void BackToEventsButton_Click(object sender, RoutedEventArgs e)
        {
            // Open/connect the Event window
            Event eventWindow = new Event();
            eventWindow.Show();

            // Close the CompletePurchase window
            this.Close();
        }
    }
}

