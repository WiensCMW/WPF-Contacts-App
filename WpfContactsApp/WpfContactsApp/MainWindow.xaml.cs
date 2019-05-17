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

                var variable = from c2 in _contacts
                               where (c2.Name != null && c2.Name.Contains(searchTextBox.Text))
                                   || (c2.Email != null && c2.Email.Contains(searchTextBox.Text))
                                   || (c2.Phone != null && c2.Phone.Contains(searchTextBox.Text))
                               orderby c2.Name
                               select c2;

                var res = variable.ToList();
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
                //List<Contact> filteredList = _contacts.Where(c => (!string.IsNullOrEmpty(c.Name) && c.Name.ToLower().Contains(filterValue.ToLower()))
                //                                        || (!string.IsNullOrEmpty(c.Email) && c.Email.ToLower().Contains(filterValue.ToLower()))
                //                                        || (!string.IsNullOrEmpty(c.Phone) && c.Phone.ToLower().Contains(filterValue.ToLower()))).ToList();
                //contactsListView.ItemsSource = filteredList;
                var variable = from c2 in _contacts
                               where (c2.Name != null && c2.Name.Contains(searchTextBox.Text))
                                   || (c2.Email != null && c2.Email.Contains(searchTextBox.Text))
                                   || (c2.Phone != null && c2.Phone.Contains(searchTextBox.Text))
                               orderby c2.Name
                               select c2;
                contactsListView.ItemsSource = variable.ToList();
            }
            else
            {
                contactsListView.ItemsSource = _contacts;
            }
        }
    }
}
