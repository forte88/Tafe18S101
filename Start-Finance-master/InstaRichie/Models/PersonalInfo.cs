using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace StartFinance.Models
{
    class PersonalInfo
    {
        [PrimaryKey, AutoIncrement]
        public int PersonalID { get; set;}

       
        public string FirstName { get; set; }

        
        public string LastName { get; set; }

       
        public string DOB { get; set; }

       
        public string Gender { get; set; }
        
        public string Email { get; set; }

        
        public string MobilePhone { get; set; }
    }
}
