using SQLite;
using System.Collections.Generic;
using System.Linq;
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
        private List<Contact> _contacts;

        public MainWindow()
        {
            InitializeComponent();
            _contacts = new List<Contact>();

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
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabasePath))
            {
                conn.CreateTable<Contact>();
                _contacts = conn.Table<Contact>().ToList().OrderBy(c => c.Name).ToList();
            }

             // Assign contacts list to ListView's ItemsSource
            contactsListView.ItemsSource = _contacts;

            FilterListBox(searchTextBox.Text);
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox searchTextBox = sender as TextBox;

            FilterListBox(searchTextBox.Text);
        }

        private void FilterListBox(string filterValue)
        {
            if (!string.IsNullOrEmpty(filterValue))
            {
                // Search all Contact properties for passed in filterValue
                List<Contact> filteredList = (from c in _contacts
                                    where (c.Name != null && c.Name.ToLower().Contains(filterValue.ToLower()))
                                        || (c.Email != null && c.Email.ToLower().Contains(filterValue.ToLower()))
                                        || (c.Phone != null && c.Phone.ToLower().Contains(filterValue.ToLower()))
                                    orderby c.Name
                                    select c).ToList();

                // Assign filtered results to ListView
                contactsListView.ItemsSource = filteredList;
            }
            else
            {
                // No filterValue specified, so assign the entire _contacts list to ListView
                contactsListView.ItemsSource = _contacts;
            }
        }

        private void ContactsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Contact selectedContact = (Contact)contactsListView.SelectedItem;
            if (selectedContact != null)
            {
                ContactDetailsWindow contactDetails = new ContactDetailsWindow(selectedContact) { Owner = this };
                contactDetails.ShowDialog();
            }

            ReadDatabase();
            FilterListBox(searchTextBox.Text);
        }
    }
}
