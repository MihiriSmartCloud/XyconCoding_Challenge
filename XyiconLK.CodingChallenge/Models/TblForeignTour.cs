using System;
using System.Collections.Generic;

#nullable disable

namespace XyiconLK.CodingChallenge.Models
{
    public partial class TblForeignTour
    {
        public int VisitId { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public int VisitedYear { get; set; }
        public int UserId { get; set; }

        public virtual TblPersonDetail User { get; set; }
    }
}
