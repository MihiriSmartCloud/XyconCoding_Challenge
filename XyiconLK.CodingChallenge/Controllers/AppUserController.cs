using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using XyiconLK.CodingChallenge.Common;
using XyiconLK.CodingChallenge.Models;

namespace XyiconLK.CodingChallenge.Controllers
{
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/users")]
    [ApiController]
    public class AppUserController : Controller
    {

        /// <summary>
        /// Get selected user
        /// </summary>
        /// <remarks>
        /// Get the details of a particular user using the user id
        /// </remarks>
        /// <param name="id">Refers to the user id</param>
        /// <returns></returns>
        // GET: api/users/5
        [HttpGet("{id}")]
        public JsonResult GetUser(int id)
        {
            try
            {
                using (Code_challengeDBContext entities = new Code_challengeDBContext())
                {
                    var user = entities.TblUsers.Find(id);
                    if (user == null)
                        return Json(new { success = false, status_code = 404, message = "Retrieve failed! User does not exist." });

                    return Json(new
                    {
                        success = true,
                        status_code = 200,
                        user = new
                        {
                            user.UserId,
                            user.FirstName,
                            user.LastName,
                            user.MobileNo,
                            user.Email,
                            user.Password,
                            user.CreatedBy,
                            user.CreatedOn,
                            user.UpdatedBy,
                            user.UpdatedOn
                        }
                    });
                }
            }
            catch (Exception)
            {
                return Json(new { success = false, status_code = 400, message = "An error occured while retrieving the user." });
            }
        }


        /// <summary>
        /// Get user details
        /// </summary>
        /// <remarks>
        /// Get the details of a particular user using the email address
        /// </remarks>
        /// <returns></returns>
        //GET api/users/getuserbyemail/abc@gmail.com
        [HttpGet]
        [Route("getuserbyemail/{email}")]
        public JsonResult Get(string email)
        {
            try
            {
                // Validate email
                if (string.IsNullOrEmpty(email) && !Utility.getInstance().ValidateEmail(email.Trim()))
                    return Json(new { success = false, status_code = 400, message = "Retrieve failed! Received invalid email." });

                using (Code_challengeDBContext entities = new Code_challengeDBContext())
                {
                    var user = entities.TblUsers.FirstOrDefault(e => e.Email.Trim() == email.Trim());
                    if (user != null)
                    {
                        return Json(new
                        {
                            success = true,
                            status_code = 200,
                            message = "User retrieved successfully!",
                            User = new
                            {
                                user.UserId,
                                user.FirstName,
                                user.LastName,
                                user.MobileNo,
                                user.Email,
                                user.Password,
                                user.CreatedBy,
                                user.CreatedOn,
                                user.UpdatedBy,
                                user.UpdatedOn
                            }
                        });
                    }
                    else
                        return Json(new { success = false, status_code = 404, message = "Retrieve failed! User does not exist." });
                }
            }
            catch (Exception)
            {
                return Json(new { success = false, status_code = 400, message = "An error occured while retrieving the user." });
            }
        }



        /// <summary>
        /// Add a new user
        /// </summary>
        /// <remarks>
        /// Create a new user with the relevant details
        /// </remarks>
        /// <returns></returns>
        // POST: api/users
        [HttpPost]
        [Consumes("application/json")]
        public JsonResult CreateUser([FromBody] JObject user_details)
        {
            try
            {
                string firstName = user_details["FirstName"].ToString().Trim();
                string lastName = user_details["LastName"].ToString().Trim();
                string mobileNo = user_details["MobileNo"].ToString().Trim();
                string email = user_details["Email"].ToString().Trim();
                string password = user_details["Password"].ToString().Trim();
                //string createdBy = user_details["CreatedBy"].ToString().Trim();
               

                using (Code_challengeDBContext entities = new Code_challengeDBContext())
                {

                    // Validates the mobile no
                    if (mobileNo != null && (!Utility.getInstance().ValidateContactNumber(mobileNo)))
                        return Json(new { success = false, status_code = 400, message = "Received invalid mobile number!" });

                    // Validates the email
                    if (email != null && !Utility.getInstance().ValidateEmail(email))
                        return Json(new { success = false, status_code = 400, message = "Received invalid email!" });

                    // Validates the password
                    if (password != null && !Utility.getInstance().ValidatePassword(password))
                        return Json(new { success = false, status_code = 400, message = "Received invalid password!" });


         
                    string hashedPwd = null; 
                    hashedPwd = Utility.getInstance().CalculateHash(password);
                    using (var transaction = entities.Database.BeginTransaction())
                    {
                        TblUser user = new TblUser
                        {

                            FirstName = firstName,
                            LastName = lastName,
                            MobileNo = mobileNo,
                            Email = email,
                            Password = hashedPwd,
                            CreatedBy = email,
                            CreatedOn = DateTime.Now,
                        };

                        entities.TblUsers.Add(user);
                        entities.SaveChanges();

                        //int userId = entities.TblUsers.Where(x => x.CreatedBy == user.CreatedBy).Select(x => x.UserId).FirstOrDefault();
                        Log.Update(user.UserId.ToString(), typeof(TblUser).Name, ActionType.INSERT,user.UserId);
                        transaction.Commit();

                        return Json(new { success = true, status_code = 201, message = "User created successfully!" });
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { success = false, status_code = 400, message = "An error occured while creating the user." });
            }
        }



        /// <summary>
        /// Update an existing user
        /// </summary>
        /// <remarks>
        /// Update the details of an existing user using the user id
        /// </remarks>
        /// <param name="id">Refers to the user id</param>
        /// <returns></returns>
        // PUT: api/users/1
        [HttpPut("{id}")]
        [Consumes("application/json")]
        public JsonResult UpdateUser(int id, [FromBody] JObject user_details)
        {
            try
            {
                string firstName = user_details["FirstName"].ToString().Trim();
                string lastName = user_details["LastName"].ToString().Trim();
                string mobileNo = user_details["MobileNo"].ToString().Trim();
                string email = user_details["Email"].ToString().Trim();
                string currentPassword = user_details["current_password"].ToString().Trim();
                string newPassword = user_details["new_password"].ToString().Trim();
                string confirmPassword = user_details["confirm_password"].ToString().Trim();


                using (Code_challengeDBContext entities = new Code_challengeDBContext())
                {
                    // Check if a user with the specified id exist
                    var entity = entities.TblUsers.FirstOrDefault(e => e.UserId == id);
                    if (entity == null)
                        return Json(new { success = false, status_code = 404, message = "Update failed! User does not exist." });

                    // Validates the email
                    if (email != null && !Utility.getInstance().ValidateEmail(email.Trim()))
                        return Json(new { success = false, status_code = 400, message = "Received invalid email!" });

                    // Validates the mobile no
                    if (mobileNo != null && (!Utility.getInstance().ValidateContactNumber(mobileNo)
                        //|| !Utility.getInstance().ValidateCountryCode(countryCode))
                        ))
                        return Json(new { success = false, status_code = 400, message = "Received invalid mobile number!" });


                    // Get the password of the user from the database
                    string password = entities.TblUsers.Where(x => x.UserId == id).Select(x => x.Password).First();

                    // Check if the entered current password of the user is correct
                    if (currentPassword != null && (Utility.getInstance().DecodeFrom64(password) != currentPassword))
                        return Json(new { success = false, status_code = 400, message = "Update failed! Received incorrect password." });

                    // Validates the password format
                    if (newPassword != null && !Utility.getInstance().ValidatePassword(newPassword))
                        return Json(new { success = false, status_code = 400, message = "Update failed! Received invalid password." });

                    // Check if the values of password & confirm password fields are the same
                    if (!(confirmPassword != null && newPassword == confirmPassword))
                        return Json(new { success = false, status_code = 400, message = "Update failed! Passwords do not match." });


                    using (var transaction = entities.Database.BeginTransaction())
                    {

                        entity.FirstName = user_details["FirstName"].ToString().Trim();
                        entity.LastName = user_details["LastName"].ToString().Trim();
                        entity.MobileNo = user_details["MobileNo"].ToString().Trim();
                        entity.Email = user_details["Email"].ToString().Trim();
                        entity.Password = Utility.getInstance().CalculateHash(newPassword);
                        entity.UpdatedBy = user_details["UpdatedBy"].ToString().Trim();
                        entity.UpdatedOn = DateTime.Now;
                        entities.SaveChanges();

                        //int userId = entities.TblUsers.Where(x => x.UpdatedBy == entity.UpdatedBy).Select(x => x.UserId).FirstOrDefault();
                        Log.Update(id.ToString(), typeof(TblUser).Name, ActionType.UPDATE, id);
                        transaction.Commit();

                        return Json(new { success = true, status_code = 200, message = "User updated successfully!" });
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, status_code = 400, Ex = e, message = "An error occured while updating the user." });
            }
        }



        /// <summary>
        /// Delete an existing user
        /// </summary>
        /// <remarks>
        /// Delete an existing user using the user id
        /// </remarks>
        /// <param name="id">Refers to the user id</param>
        /// <param name="firebase_id">Refers to the firebase id of the currently logged in user.</param>
        /// <returns></returns>
        // DELETE: api/users/1
        [HttpDelete("{id}")]
        public JsonResult DeleteUser(int id)
        {
            try
            {
                using (Code_challengeDBContext entities = new Code_challengeDBContext())
                {
                    var entity = entities.TblUsers.FirstOrDefault(e => e.UserId == id);
                    if (entity == null)
                        return Json(new { success = false, status_code = 404, message = "Delete failed! User does not exist." });
                    if (!entities.TblUsers.Any(e => e.UserId ==id ))
                        return Json(new { success = false, status_code = 404, message = "Delete failed! An user with the given firebase id does not exist." });

                    else
                    {
                        using (var transaction = entities.Database.BeginTransaction())
                        {
                            entities.SaveChanges();

                            //int userId = entities.TblUsers.Where(x => x.UserId == id).Select(x => x.UserId).FirstOrDefault();
                            Log.Update(id.ToString(), typeof(TblUser).Name, ActionType.DELETE, id);
                            transaction.Commit();

                            return Json(new { success = true, status_code = 200, message = "User deleted successfully!" });
                        }
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { success = false, status_code = 400, message = "An error occured while deleting the user." });
            }
        }

        /// <summary>
        /// Reset user password
        /// </summary>
        /// <remarks>
        /// Reset the password of a user, using the email
        /// </remarks>
        /// <returns></returns>
        //PATCH api/users
        [HttpPatch]
        public JsonResult ResetPasswordByEmail([FromBody] JObject user_details)
        {
            try
            {
                string email = user_details["Email"].ToString().Trim();
                string newPassword = user_details["new_password"].ToString().Trim();
                string confirmPassword = user_details["confirm_password"].ToString().Trim();

                using (Code_challengeDBContext entities = new Code_challengeDBContext())
                {
                    // Validates the email format
                    if (email != null && !Utility.getInstance().ValidateEmail(email))
                        return Json(new { success = false, status_code = 400, message = "Update failed! Received invalid email." });

                    // Check if the user exists
                    var entity = entities.TblUsers.FirstOrDefault(e => e.Email.Trim() == email);
                    if (entity == null)
                        return Json(new { success = false, status_code = 400, message = "Update failed! User does not exist." });

                    // Validates the password format
                    if (newPassword != null && !Utility.getInstance().ValidatePassword(newPassword))
                        return Json(new { success = false, status_code = 400, message = "Update failed! Received invalid password." });

                    // Check if the values of password & confirm password fields are the same
                    if (!(confirmPassword != null && newPassword == confirmPassword))
                        return Json(new { success = false, status_code = 400, message = "Update failed! Passwords do not match." });

                    else
                    {
                        using (var transaction = entities.Database.BeginTransaction())
                        {
                            // Update password field
                            entity.Password = Utility.getInstance().CalculateHash(newPassword);
                            entities.SaveChanges();

                            Log.Update(entity.UserId.ToString(), typeof(TblUser).Name, ActionType.RESET_PWD, int.Parse(user_details["UpdatedBy"].ToString().Trim()));
                            transaction.Commit();

                            return Json(new { success = true, status_code = 200, message = "Password reset successfully!" });
                        }
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { success = false, status_code = 400, message = "An error occured while updating the password." });
            }
        }


        /// <summary>
        /// User login
        /// </summary>
        /// <remarks>
        /// Login to the application using the login credentials
        /// </remarks>
        /// <returns></returns>
        // POST api/User/Login  
        [HttpPost]
        [Consumes("application/json")]
        public JsonResult Login([FromBody] JObject credentials)
        {
            try
            {
                string email = credentials["Email"].ToString();
                string password = credentials["Password"].ToString();

                // Validates user email

                if (email != null && !Utility.getInstance().ValidateEmail(email))
                    return Json(new { success = false, status_code = 400, message = "Received invalid email!" });
                //if (!Utilities.getInstance().ValidateEmail(email))
                    //return Messages.GetInstance().HandleException("Login failed! Received invalid email address.", isLogin: true);
                else
                {
                    // Checks if the user exists
                    if (Utility.getInstance().IsValidUser(email))
                    {
                        using (Code_challengeDBContext entities = new Code_challengeDBContext())
                        {
                            // Validates the password format
                            if (!Utility.getInstance().ValidatePassword(password))                               
                                return Json(new { success = false, status_code = 400, message = "Login failed! Received invalid password. Hint: password should contain atleast 8 characters including letters, digits & special characters." });
                            // Validates the user password
                            if (Utility.getInstance().IsValidPassword(email, password))
                            {
                                int user_id = int.Parse(Utility.getInstance().GetUserData(email, false, true));

                                // Update log information
                               
                                Log.Update(user_id.ToString(), typeof(TblUser).Name, ActionType.INSERT, user_id);

                                return Json(new { success = true, status_code = 200, message = "User Login successfully!" });
                            }
                            else
                            return Json(new { success = false, status_code = 400, message = "Login failed! Received invalid credentials." });
                        }
                    }
                    else
                    return Json(new { success = false, status_code = 400, message = "Login failed! User does not exist.." });
                }
            }
            catch (Exception)
            {
                return Json(new { success = false, status_code = 400, message = "An error occured while login." });
            }
        }
    }
}
