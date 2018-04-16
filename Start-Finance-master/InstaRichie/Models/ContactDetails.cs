using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace StartFinance.Models
{
    public class ContactDetails
    {
        [PrimaryKey, AutoIncrement]
        public int ContactID { get; set; }

        public string FirstName { get; set; }

       
        public string LastName { get; set; }

        [NotNull]
        public string CompanyName { get; set; }


        public string CompanyPhone { get; set; }
       
    }
}
