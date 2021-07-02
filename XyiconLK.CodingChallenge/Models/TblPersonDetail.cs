using System;
using System.Collections.Generic;

#nullable disable

namespace XyiconLK.CodingChallenge.Models
{
    public partial class TblPersonDetail
    {
        public TblPersonDetail()
        {
            TblForeignTours = new HashSet<TblForeignTour>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Birthdate { get; set; }
        public int Age { get; set; }

        public virtual ICollection<TblForeignTour> TblForeignTours { get; set; }
    }
}
