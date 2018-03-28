using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace StartFinance.Models
{
    public class ShoppingList
    {
        [PrimaryKey, AutoIncrement]
        public int ShoppingItemID { set; get; }
        [Unique, NotNull]
        public string ShopName { set; get; }
        [Unique, NotNull]
        public string NameOfItem { set; get; }

        public string ShoppingDate { set; get; }
        [NotNull]
        public double PriceQuote { set; get; }

    }
}
