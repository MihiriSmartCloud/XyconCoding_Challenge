using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XyiconLK.CodingChallenge.Models;

namespace XyiconLK.CodingChallenge.Common
{
    public class Log
    {
        /// <summary>
        /// Save data modification details to the update table
        /// </summary>
        /// <param name="id">Represents references</param>
        /// <param name="table">Represents the reference table</param>
        /// <param name="actionType">Reference the table modification type</param>
        public static void Update(string id, string table, ActionType actionType, int user)
        {
            try
            {
                //Log
                Code_challengeDBContext db = new Code_challengeDBContext();
                TblLog update = new TblLog
                {
                    TableName = table,
                    RecordId = int.Parse(id),
                    EventDateTime = DateTime.Now,
                    EventType = System.Enum.GetName(typeof(ActionType), actionType),
                    UserId = user
                };
                db.TblLogs.Add(update);
                db.SaveChanges();
            }
            catch (Exception)
            {
                Console.WriteLine("An error occured while updating the event log.");
            }
        }

    }
}
