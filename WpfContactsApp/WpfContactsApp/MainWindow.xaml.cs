using SQLite;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
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
            //NewContactWindow newContactWindow = new NewContactWindow() { Owner = this };
            //newContactWindow.ShowDialog();

            try
            {
                //ReadDatabase();
                for (int i = 0; i < 1000000; i++)
                {
                    using (SQLiteConnection conn = new SQLiteConnection(App.DatabasePath))
                    {
                        conn.CreateTable<Contact>();

                        Contact newContact = new Contact()
                        {
                            Name = $"Billy the {i + 1}",
                            Email = $"billy{i + 1}@gmail.com",
                            Phone = i.ToString()
                        };
                        conn.Insert(newContact);
                    }

                    using (SQLiteConnection conn = new SQLiteConnection(App.DatabasePath))
                    {
                        conn.CreateTable<Contact>();
                        List<Contact> contacts = conn.Table<Contact>().ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ReadDatabase()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabasePath))
            {
                conn.CreateTable<Contact>();
                List<Contact> contacts = conn.Table<Contact>().ToList();
            }
        }
    }
}
