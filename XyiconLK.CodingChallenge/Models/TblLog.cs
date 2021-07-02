using System;
using System.Collections.Generic;

#nullable disable

namespace XyiconLK.CodingChallenge.Models
{
    public partial class TblLog
    {
        public int LogId { get; set; }
        public string TableName { get; set; }
        public int RecordId { get; set; }
        public DateTime EventDateTime { get; set; }
        public int UserId { get; set; }
        public string EventType { get; set; }

        public virtual TblUser User { get; set; }
    }
}
