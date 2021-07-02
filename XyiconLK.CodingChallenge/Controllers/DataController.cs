using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using XyiconLK.CodingChallenge.Configuration;
using XyiconLK.CodingChallenge.Models;

using FluentAssertions.Common;

namespace XyiconLK.CodingChallenge.Controllers
{
    [Produces(MediaTypeNames.Application.Json)]

    [ApiController]
    public class DataController : Controller
    {
        [Route("api/people")]
        [HttpGet("{id}")]
        public JsonResult GetPerson(int id)
        {
            try
            {
                using (Code_challengeDBContext entities = new Code_challengeDBContext())
                {
                    var person = entities.TblPersonDetails.Find(id);
                    if (person == null)
                        return Json(new { success = false, status_code = 404, message = "Retrieve failed! User does not exist." });

                    var visits = entities.TblForeignTours.Where(x => x.VisitId == person.Id).ToList();
                    List<ForeignVisitViewModel> offers = new List<ForeignVisitViewModel>();

                    foreach (var offer in visits)
                    {
                        offers.Add(new ForeignVisitViewModel
                        {
                            City=offer.City,
                            VisitedYear=offer.VisitedYear,
                            Country=offer.Country
                        });
                    }
                    return Json(new
                    {
                        success = true,
                        status_code = 200,
                        user = new PersonViewModel()
                        {
                            FirstName = person.FirstName,
                            LastName = person.LastName,
                            FullName = person.FirstName + " " + person.LastName,
                            Email = person.Email,
                            Address = person.Address,
                            PhoneNumber = person.PhoneNumber,
                            BirthDate = person.Birthdate,
                            Age = person.Age,
                            ForeignVisits= offers
                        }

                    }); ; ;
                }
            }
            catch (Exception)
            {
                return Json(new { success = false, status_code = 400, message = "An error occured while retrieving the user." });
            }
        }
        [HttpGet]
        [Route("api/people")]
        public JsonResult GetAll()
        {
            try
            {
                using (Code_challengeDBContext entities = new Code_challengeDBContext())
                {
                    var person = entities.TblPersonDetails.ToList();
                    List<object> reponse = new List<object>();

                    if (person.Count == 0)
                        return Json(new { success = true, status_code = 200, message = "No person found!", branches_list = reponse });

                    foreach (var pDetails in person)
                        reponse.Add(new
                        {
                            pDetails.FirstName,
                            pDetails.LastName,
                            pDetails.FullName,
                            pDetails.Email,
                            pDetails.Address,
                            pDetails.PhoneNumber,
                            pDetails.Birthdate,
                            pDetails.Age,
                            foriegn_visit = entities.TblForeignTours.Where(e => e.UserId == pDetails.Id).Select(a => new ForeignVisitViewModel()
                            {
                                Country = a.Country,
                                VisitedYear = a.VisitedYear,
                                City = a.City

                            }).ToList()
                        });

                    return Json(new { success = true, status_code = 200, branches_list = reponse });
                }
            }
            catch (Exception)
            {

                return Json(new { success = false, status_code = 400, message = "An error occured while retrieving the user." });
            }
        }

        [HttpGet]
        [Route("api/about")]
        public JsonResult GetDeveloper()
        {
            try
            {
 
                var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

                var settings = config.Get<CodingChallengeConfiguration>();             
                List<object> reponse = new List<object>();

                List<DeveloperViewModel> developer = new List<DeveloperViewModel>();
                return Json(new
                {
                    success = true,
                    status_code = 200,
                    developer = new DeveloperViewModel()
                    {
                       Email=settings.Developer.Email,
                       Name=settings.Developer.Name,
                       Timestamp=DateTime.Now               
                    }

                });;;
            }
            catch (Exception)
            {       
                return Json(new { success = false, status_code = 400, message = "An error occured while retrieving the developer." });
            }
        }
    }
}
//    }
//            public ActionResult<List<PersonViewModel>> GetAll()
//            => new List<PersonViewModel> { };

//        public ActionResult<DeveloperViewModel> GetDeveloper()
//            => new DeveloperViewModel();
//    }
//}
