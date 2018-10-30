using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    public class Profile
    {
        public int ProfileId { get; set; }

        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Preferred_Name { get; set; }
        public string Pic_Url { get; set; }


        //construction for foreign key
        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<Plays_Instrument> Plays_Instrument { get; set; }
        //public ICollection<Ensemble_Membership> Ensemble_Membership { get; set; }
    }
}
