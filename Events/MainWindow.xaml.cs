using System;
using System.Windows;

namespace Events
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GoToEvents_Click(object sender, RoutedEventArgs e)
        {
            // Aquí abres la ventana de eventos
            Event eventWindow = new Event();
            eventWindow.Show();

            // Opcional: Puedes cerrar la ventana actual si es necesario
            this.Close();
        }
    }
}

