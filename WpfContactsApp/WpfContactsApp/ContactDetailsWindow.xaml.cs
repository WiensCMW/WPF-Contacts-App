using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SQLite;
using WpfContactsApp.Classes;

namespace WpfContactsApp
{
    /// <summary>
    /// Interaction logic for ContactDetailsWindow.xaml
    /// </summary>
    public partial class ContactDetailsWindow : Window
    {
        private Contact _contact;

        public ContactDetailsWindow(Contact contact)
        {
            InitializeComponent();

            _contact = contact;

            nameTextBox.Text = _contact.Name;
            emailTextBox.Text = _contact.Email;
            phoneNumberTextBox.Text = _contact.Phone;
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            _contact.Name = nameTextBox.Text;
            _contact.Email = emailTextBox.Text;
            _contact.Phone = phoneNumberTextBox.Text;

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabasePath))
            {
                // Creates table of type <T> if it doesn't exist. If it exists, it does nothing.
                conn.CreateTable<Contact>();

                // Update object in table
                conn.Update(_contact);
            }

            Close();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabasePath))
            {
                conn.CreateTable<Contact>();
                conn.Delete(_contact);
            }

            Close();
        }
    }
}
