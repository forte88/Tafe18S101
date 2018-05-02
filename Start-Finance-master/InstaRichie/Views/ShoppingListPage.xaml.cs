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
using StartFinance.Models;
using Windows.UI.Popups;
using SQLite.Net;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StartFinance.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShoppingListPage : Page
    {

        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");

        public ShoppingListPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            /// Initializing a database
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            // Creating table
            Results();
        }

        public void Results()
        {
            conn.CreateTable<ShoppingList>();
            var query1 = conn.Table<ShoppingList>();
            ShoppingListView.ItemsSource = query1.ToList();
        }

        private async void AddShop_Click(object sender, RoutedEventArgs e)
        {
            string day = shopingDatePicker.Date.Value.Day.ToString();
            string month = shopingDatePicker.Date.Value.Month.ToString();
            string year = shopingDatePicker.Date.Value.Year.ToString();
            string theDate = day + "/" + month + "/" + year;

            try
            {
                if (shopNameTxt.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("Please Enter the Shops Name");
                    await dialog.ShowAsync();
                }
                else if (nameOfItemTxt.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("Please Enter the Item Name");
                    await dialog.ShowAsync();
                }
                else if (shopingDatePicker.Date.Value.Day.ToString() == null)
                {
                    MessageDialog dialog = new MessageDialog("Please Enter the Date");
                    await dialog.ShowAsync();
                }

                else if (priceQuotedTxt.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("Please Enter the Value of the Item");
                    await dialog.ShowAsync();
                }
                else
                {
                    double tempQuoted = Convert.ToDouble(priceQuotedTxt.Text);
                    conn.CreateTable<ShoppingList>();
                    conn.Insert(new ShoppingList
                    {                        
                        ShopName = shopNameTxt.Text.ToString(),
                        NameOfItem = nameOfItemTxt.Text.ToString(),
                        ShoppingDate = theDate,
                        PriceQuote = tempQuoted
                    });
                    Results();
                }
            }
            catch (Exception ex)
            {
                if (ex is FormatException)
                {
                    MessageDialog dialog = new MessageDialog("You forgot to enter the Amount or entered an invalid Amount", "Oops..!");
                    await dialog.ShowAsync();
                }
                else if (ex is SQLiteException)
                {
                    MessageDialog dialog = new MessageDialog("Wish Name already exist, Try Different Name", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    /// no idea
                }
            }
           
        }

        private async void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int AccSelection = ((ShoppingList)ShoppingListView.SelectedItem).ShoppingItemID;
                if (AccSelection == 0)
                {
                    MessageDialog dialog = new MessageDialog("Not selected the Item", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    conn.CreateTable<ShoppingList>();
                    var query1 = conn.Table<ShoppingList>();
                    var query3 = conn.Query<ShoppingList>("DELETE FROM ShoppingList WHERE ShoppingItemID ='" + AccSelection + "'");
                    ShoppingListView.ItemsSource = query1.ToList();
                }
            }
            catch (NullReferenceException)
            {
                MessageDialog dialog = new MessageDialog("Not selected the Item", "Oops..!");
                await dialog.ShowAsync();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Results();
        }
    }
}
