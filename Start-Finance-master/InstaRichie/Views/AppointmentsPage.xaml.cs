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
    public sealed partial class AppointmentsPage : Page
    {
        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");
        int editID = -1; // this is to check when you click 'add' if it is updating an existing record or not. -1 means new entry;
        public AppointmentsPage()
        {
            this.InitializeComponent();

            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            /// Initializing a database
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            // Creating table
            conn.CreateTable<Appointments>();
            EventDate.Date = DateTime.Now; // gets current date and time
      
      
            
            Results();

        }
        
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            Results();
        }

        public void Results()
        {
            conn.CreateTable<Appointments>();
            var query = conn.Table<Appointments>();
            AppointmentList.ItemsSource = query.ToList();
            
            // AccountsListSel.ItemsSource = query1.ToList();
           // FromAccountsSel.ItemsSource = query1.ToList();
           // ToAccountSel.ItemsSource = query1.ToList();
        }


        private async void DelAppointment(object sender, RoutedEventArgs e)
        {
            MessageDialog ShowConf = new MessageDialog("Are you sure you want to delete this Appointment?", "Important");
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
                    int AppointmentID = ((Appointments)AppointmentList.SelectedItem).ID;
                    var querydel = conn.Query<Appointments>("DELETE FROM Appointments WHERE ID='" + AppointmentID + "'");
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

        private async void AddAppointment(object sender, RoutedEventArgs e)
        {
            // building the data elements to add to the database

            string EDay = EventDate.Date.Value.Day.ToString();
            
            string EMonth = EventDate.Date.Value.Month.ToString();
            string EYear = EventDate.Date.Value.Year.ToString();
            string extra = "";
            if (int.Parse(EDay) < 10)
            {
                extra = "0";
            }


             string FinalEventDate = extra + EDay + "/" + EMonth + "/" + EYear;
       
            string ELocation = EventLocation.Text.ToString();
            string EName = EventName.Text.ToString();

            string STime = StartTime.Time.ToString();
            string ETime = EndTime.Time.ToString();


            if (editID != -1) {

                conn.Update(new Appointments()
                {

                    ID = editID,
                    EventName = EName,
                    Location = ELocation,
                    EventDate = FinalEventDate,
                    StartTime = STime,
                    EndTime = ETime


                });



            } else
            {
                conn.Insert(new Appointments()
                {
                    EventName = EName,
                    Location = ELocation,
                    EventDate = FinalEventDate,
                    StartTime = STime,
                    EndTime = ETime

                });



            }
          

       



          


    

            Results();
        }

        private async void EditAppointment(object sender, RoutedEventArgs e)
        {

            // checks if data is null else inserts
            try
            {


                string DateToParse = ((Appointments)AppointmentList.SelectedItem).EventDate + " 12:00:00 AM";
                    
             

                editID = ((Appointments)AppointmentList.SelectedItem).ID;
                EventName.Text = ((Appointments)AppointmentList.SelectedItem).EventName;
                EventLocation.Text = ((Appointments)AppointmentList.SelectedItem).Location;
                EventDate.Date = DateTime.Parse(DateToParse);
               StartTime.Time = TimeSpan.Parse(((Appointments)AppointmentList.SelectedItem).StartTime);
                EndTime.Time = TimeSpan.Parse(((Appointments)AppointmentList.SelectedItem).EndTime);
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
