using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace StartFinance.Models
{
    class Appointments
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [NotNull]
        public string EventName { get; set; }

     
        public string Location { get; set; }

        [NotNull]
        public string EventDate { get; set; }

       
        public string StartTime { get; set; }

     
        public string EndTime { get; set; }

    }
}
