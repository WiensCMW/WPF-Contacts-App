using SQLite;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WpfContactsApp.Classes;

namespace WpfContactsApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ReadDatabase();
        }

        private void buttonNewContact_Click(object sender, RoutedEventArgs e)
        {
            NewContactWindow newContactWindow = new NewContactWindow() { Owner = this };
            newContactWindow.ShowDialog();

            ReadDatabase();
        }

        private void ReadDatabase()
        {
            List<Contact> contacts = new List<Contact>();
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabasePath))
            {
                conn.CreateTable<Contact>();
                contacts = conn.Table<Contact>().ToList();
            }

             // Assign contacts list to ListView's ItemsSource
            contactsListView.ItemsSource = contacts;
        }
    }
}
