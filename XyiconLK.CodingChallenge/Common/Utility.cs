using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using XyiconLK.CodingChallenge.Models;

namespace XyiconLK.CodingChallenge.Common
{
    public class Utility
    {
        private static Utility utilities;

        private Utility()
        {
            utilities = null;
        }

        public static Utility getInstance() => utilities ?? (utilities = new Utility());


        // Validates email addresses
        public bool ValidateEmail(string email) => Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);


        // Validates usernames
        public bool ValidateUsername(string username) => Regex.Match(username, @"^[a-zA-Z][a-zA-Z0-9]{5,11}$").Success;


        // Validates passwords
        public bool ValidatePassword(string password) => Regex.IsMatch(password, @".{4,8}$", RegexOptions.IgnoreCase);
        //public bool ValidatePassword(string password) => Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{4,8}$", RegexOptions.IgnoreCase);

        // Validates contact numbers
        public bool ValidateContactNumber(string contactNumber) => Regex.Match(contactNumber, @"^\d{7,12}$").Success;

        // Encodes the password 
        public string CalculateHash(string password) => System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));

        // Decodes the password
        public string DecodeFrom64(string encodedData) => System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(encodedData));

        // Checks if the user exists
        public Boolean IsValidUser(string username)
        {
            using (Code_challengeDBContext entities = new Code_challengeDBContext())
            {
                TblUser selectedUser = entities.TblUsers.FirstOrDefault(e => e.Email == username);
                return (selectedUser != null) ? true : false;
            }
        }

        // Checks if the entered password matches the exact password
        public bool IsValidPassword(string username, string password)
        {
            using (Code_challengeDBContext entities = new Code_challengeDBContext())
            {
                // Get the encrypted password of the user from the db
                // Decrypt the db password
                string decryptedPwd = Utility.getInstance().DecodeFrom64(GetUserData(username));

                // Validate passwords - compare the two passwords
                return password.Equals(decryptedPwd) ? true : false;
            }
        }

        // Get password of the user from the db
        public string GetUserData(string username, bool isPwd = true, bool isUserId = false)
        {
            using (Code_challengeDBContext entities = new Code_challengeDBContext())
            {
                if (isPwd)
                {
                    TblUser selectedUser = entities.TblUsers.FirstOrDefault(e => e.Email == username);
                    return (selectedUser != null) ? selectedUser.Password : null;
                }
                else if (isUserId)
                {
                    TblUser selectedUser = entities.TblUsers.FirstOrDefault(e => e.Email == username);
                    return (selectedUser != null) ? selectedUser.UserId.ToString() : null;
                }

                return null;
            }
        }
    }
}
