using System;
using System.Windows;

namespace Events
{
    public partial class Payment : Window
    {
        private string _ticketType;
        private int _quantity;
        private string _eventLocation; // Variable para almacenar la ubicación del evento
        private DateTime _eventDate; // Variable para almacenar la fecha del evento

        // Modificado para aceptar la ubicación y fecha del evento
        public Payment(string ticketType, int quantity, string eventLocation, DateTime eventDate)
        {
            InitializeComponent();
            _ticketType = ticketType;
            _quantity = quantity;
            _eventLocation = eventLocation;
            _eventDate = eventDate;
            LoadEventData(); // Llamar a la función para cargar los datos del evento
        }

        private void LoadEventData()
        {
            // Mostrar la ubicación y fecha del evento en el TextBlock correspondiente
            eventLocationDateTextBlock.Text = $"{_eventLocation} - {_eventDate.ToString("MMMM dd, yyyy")}";
        }

        private void ConfirmPurchaseButton_Click(object sender, RoutedEventArgs e)
        {
            // Aquí podrías validar los detalles de pago antes de proceder

            // Open the CompletePurchase window with event details
            CompletePurchase completePurchaseWindow = new CompletePurchase(_eventLocation, _eventDate, _ticketType, _quantity);
            completePurchaseWindow.Show();
            this.Close();
        }
    }
}




