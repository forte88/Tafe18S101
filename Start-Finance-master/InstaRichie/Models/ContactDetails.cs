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
        public int ID { get; set; }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CompanyName { get; set; }

        public string CompanyPhone { get; set; }
       
    }
}
