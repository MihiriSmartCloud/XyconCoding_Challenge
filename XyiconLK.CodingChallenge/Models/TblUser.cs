using System;
using System.Collections.Generic;

#nullable disable

namespace XyiconLK.CodingChallenge.Models
{
    public partial class TblUser
    {
        public TblUser()
        {
            TblLogs = new HashSet<TblLog>();
        }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual ICollection<TblLog> TblLogs { get; set; }
    }
}
