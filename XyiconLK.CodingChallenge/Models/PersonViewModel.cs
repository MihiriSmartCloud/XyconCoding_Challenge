using System;
using System.Collections.Generic;

namespace XyiconLK.CodingChallenge.Models
{
    public class PersonViewModel
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public List<ForeignVisitViewModel> ForeignVisits { get; set; }
    }
}
