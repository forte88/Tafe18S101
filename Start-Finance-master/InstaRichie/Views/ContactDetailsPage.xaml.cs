using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SQLite;
using StartFinance.Models;
using Windows.UI.Popups;
using SQLite.Net;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StartFinance.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ContactDetailsPage : Page
    {
        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");
        int editID = -1; // this is to check when you click 'add' if it is updating an existing record or not. -1 means new entry;
        public ContactDetailsPage()
        {
            this.InitializeComponent();

            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            /// Initializing a database
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            // Creating table
            conn.CreateTable<ContactDetails>();
            Results();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            Results();
        }

        public void Results()
        {
            // Creating table
            conn.CreateTable<ContactDetails>();
            var query = conn.Table<ContactDetails>();
            ContactDetailsList.ItemsSource = query.ToList();
        }
        //Delete Contact Details
        private async void DelDetails(object sender, RoutedEventArgs e)
        {
            MessageDialog ShowConf = new MessageDialog("Are you sure you want to delete this Contact Detail?", "Important");
            ShowConf.Commands.Add(new UICommand("Yes, Delete")
            {
                Id = 0
            });
            ShowConf.Commands.Add(new UICommand("Cancel")
            {
                Id = 1
            });
            ShowConf.DefaultCommandIndex = 0;
            ShowConf.CancelCommandIndex = 1;

            var result = await ShowConf.ShowAsync();
            if ((int)result.Id == 0)
            {
                // checks if data is null else inserts
                try
                {
                    int ContactID = ((ContactDetails)ContactDetailsList.SelectedItem).ContactID;
                    var querydel = conn.Query<ContactDetails>("DELETE FROM ContactDetails WHERE ContactID='" + ContactID + "'");
                    Results();
                }
                catch (NullReferenceException)
                {
                    MessageDialog ClearDialog = new MessageDialog("Please select the item to Delete", "Oops..!");
                    await ClearDialog.ShowAsync();
                }
            }
            else
            {
                //
            }
        }


        //Add Contact Details
        private void AddDetails(object sender, RoutedEventArgs e)
        {
            // building the data elements to add to the database
            
            string FName = firstName.Text.ToString();
            string LName = lastName.Text.ToString();
            string CName = companyName.Text.ToString();
            string CPhone = companyPhone.Text.ToString();

            if (editID != -1)
            {

                conn.Update(new ContactDetails()
                {
                    ContactID = editID,
                    FirstName = FName,
                    LastName = LName,
                    CompanyName = CName,
                    CompanyPhone = CPhone
                });

                editID = -1; // this should reset the edit check back to zero

            }
            else
            {
                conn.Insert(new ContactDetails()
                {
                    FirstName = FName,
                    LastName = LName,
                    CompanyName = CName,
                    CompanyPhone = CPhone

                });
            }

            Results();
        }


        //Edit Contact Details
        private async void EditDetails(object sender, RoutedEventArgs e)
        {

            // checks if data is null else inserts
            try
            {





                editID = ((ContactDetails)ContactDetailsList.SelectedItem).ContactID;
                firstName.Text = ((ContactDetails)ContactDetailsList.SelectedItem).FirstName;
                lastName.Text = ((ContactDetails)ContactDetailsList.SelectedItem).LastName;
                companyName.Text = ((ContactDetails)ContactDetailsList.SelectedItem).CompanyName;
                companyPhone.Text = ((ContactDetails)ContactDetailsList.SelectedItem).CompanyPhone;
                Results();
            }
            catch (NullReferenceException)
            {
                MessageDialog ClearDialog = new MessageDialog("Please select the item to Edit", "Oops..!");
                await ClearDialog.ShowAsync();
            }





        }

    }
}
