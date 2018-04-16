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
    public sealed partial class PersonalInfoPage : Page
    {
        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");
        int editID = -1; // this is to check when you click 'add' if it is updating an existing record or not. -1 means new entry;
        public PersonalInfoPage()
        {
            this.InitializeComponent();

            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            /// Initializing a database
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            // Creating table
            conn.CreateTable<PersonalInfo>();
            dob.Date = DateTime.Now;
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
            conn.CreateTable<PersonalInfo>();
            var query = conn.Table<PersonalInfo>();
            PersonalInfoList.ItemsSource = query.ToList();
        }

        private async void DelInfo(object sender, RoutedEventArgs e)
        {
            MessageDialog ShowConf = new MessageDialog("Are you sure you want to delete this Personal Info?", "Important");
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
                    int PersonalID = ((PersonalInfo)PersonalInfoList.SelectedItem).PersonalID;
                    var querydel = conn.Query<PersonalInfo>("DELETE FROM PersonalInfo WHERE ID='" + PersonalID + "'");
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


        //Add Personal Info
        private async void AddInfo(object sender, RoutedEventArgs e)
        {
            // building the data elements to add to the database

            string PDay = dob.Date.Value.Day.ToString();

            string PMonth = dob.Date.Value.Month.ToString();
            string PYear = dob.Date.Value.Year.ToString();
            string extra = "";
            if (int.Parse(PDay) < 10)
            {
                extra = "0";
            }


            string FinalDOB = extra + PDay + "/" + PMonth + "/" + PYear;

            string FName = firstName.ToString();
            string LName = lastName.ToString();
            string PersonGender = gender.ToString();
            string PersonalEmail = email.ToString();
            string Phone = personalPhone.ToString();

            if (editID != -1)
            {

                conn.Update(new PersonalInfo()
                {
                    PersonalID = editID,
                    FirstName = FName,
                    LastName = LName,
                    DOB = FinalDOB,
                    Gender = PersonGender,
                    Email = PersonalEmail,
                    MobilePhone = Phone
                });

                editID = -1; // this should reset the edit check back to zero

            }
            else
            {
                conn.Insert(new PersonalInfo()
                {
                    PersonalID = editID,
                    FirstName = FName,
                    LastName = LName,
                    DOB = FinalDOB,
                    Gender = PersonGender,
                    Email = PersonalEmail,
                    MobilePhone = Phone

                });



            }

            Results();
        }


        //EditPersonalInfo
        private async void EditInfo(object sender, RoutedEventArgs e)
        {

            // checks if data is null else inserts
            try
            {


                string DateToParse = ((PersonalInfo)PersonalInfoList.SelectedItem).DOB + " 12:00:00 AM";



                editID = ((PersonalInfo)PersonalInfoList.SelectedItem).PersonalID;
                firstName.Text = ((PersonalInfo)PersonalInfoList.SelectedItem).FirstName;
                lastName.Text = ((PersonalInfo)PersonalInfoList.SelectedItem).LastName;
                dob.Date = DateTime.Parse(DateToParse);
                gender.Text = ((PersonalInfo)PersonalInfoList.SelectedItem).Gender;
                email.Text = ((PersonalInfo)PersonalInfoList.SelectedItem).Email;
                personalPhone.Text = ((PersonalInfo)PersonalInfoList.SelectedItem).MobilePhone;
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
