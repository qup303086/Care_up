using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using Care_UP.Models;

namespace Care_UP.Controllers
{
    public class AttendantsController : ApiController
    {
        private Model1 db = new Model1();

        [System.Web.Http.Route("City")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult City(int Id)
        {
            List<Cities> cities = db.Cities.ToList();
            List<Locations> locationses = db.Locations.ToList();

            List<Attendants> attendant = db.Attendants.Include(x => x.Locationses)
                .Where(x => x.Locationses.Where(y => y.CityId == Id).Count() > 0).ToList();

            return Ok(new
            {
                attendant,
                cities,
                locationses
            });
        }

        [System.Web.Http.Route("AttendantDetails")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult AttendantDetails(int Id)
        {
            List<Cities> cities = db.Cities.ToList();
            List<Locations> locationses = db.Locations.ToList();

            Attendants attendant = db.Attendants.Include(x => x.Locationses).Where(x => x.Id == Id).FirstOrDefault();

            if (attendant.Locationses.Count == 0)
            {
                return Ok(new
                {
                    attendant,
                    city = "未選擇城市",
                    location = "未指定地區",
                    cities,
                    locationses
                });
            }

            else
            {
                return Ok(new
                {
                    attendant,
                    cities,
                    locationses
                });
            }

        }

        [System.Web.Http.Route("EditAttendantDetails")]
        [System.Web.Http.HttpPatch]
        public IHttpActionResult NewAttendantDetails()
        {
            var formdata = HttpContext.Current.Request;
            int AttendantID = Convert.ToInt32(formdata["Id"]);
            Attendants attendant = db.Attendants.Include(x => x.Locationses).Where(x => x.Id == AttendantID).FirstOrDefault();

            string[] formlocation = formdata["Location"].Split(',');
            ICollection<Locations> NewLocationses = new List<Locations>();
            foreach (string item in formlocation)
            {
                Locations locations = db.Locations.Find(Convert.ToInt32(item));
                NewLocationses.Add(locations);
            }
            attendant.Locationses = NewLocationses;

            if (formdata.Files["Photo"].FileName != null)
            {
                string photo = "";
                string fileExtension = Path.GetExtension(formdata.Files["Photo"].FileName).ToLower();
                string[] Extension = { ".jpeg", ".jpg", ".png", ".gif", };
                bool photoOK = false;
                for (int i = 0; i < Extension.Length; i++)
                {
                    if (fileExtension == Extension[i])
                    {
                        photoOK = true;
                        break;
                    }
                }
                if (photoOK)
                {
                    if (attendant.Photo != null)
                    {
                        string oldpath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath + "Uploads", attendant.Photo);
                        File.Delete(oldpath);
                    }
                    photo = AttendantID + "_p_" + DateTime.Now.ToString("yyyyMMddHHmm") + fileExtension;
                    string path = Path.Combine(HostingEnvironment.ApplicationPhysicalPath + "Uploads", photo);
                    formdata.Files["Photo"].SaveAs(path);
                    attendant.Photo = photo;
                }
            }
            if (formdata.Files["File"].FileName != null)
            {
                string file = "";
                string fileExtension = Path.GetExtension(formdata.Files["File"].FileName).ToLower();
                string[] Extension = { ".jpeg", ".jpg", ".pdf" };
                bool fileOK = false;
                for (int i = 0; i < Extension.Length; i++)
                {
                    if (fileExtension == Extension[i])
                    {
                        fileOK = true;
                        break;
                    }
                }
                if (fileOK)
                {
                    if (attendant.File != null)
                    {
                        string oldpath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath + "Uploads", attendant.File);
                        File.Delete(oldpath);
                    }
                    file = AttendantID + "_f_" + DateTime.Now.ToString("yyyyMMddHHmm") + fileExtension;
                    string path = Path.Combine(HostingEnvironment.ApplicationPhysicalPath + "Uploads", file);
                    formdata.Files["File"].SaveAs(path);
                    attendant.File = file;
                }
            }
            attendant.Name = formdata["Name"];
            attendant.Account = formdata["Account"];
            attendant.Salary = Convert.ToInt32(formdata["Salary"]);
            attendant.Account = formdata["Account"];
            attendant.Service = formdata["Service"];
            attendant.ServiceTime = formdata["ServiceTime"];
            attendant.Experience = formdata["Experience"];
            attendant.StartDateTime = Convert.ToDateTime(formdata["StartDateTime"]);
            attendant.EndDateTime = Convert.ToDateTime(formdata["EndDateTime"]);
            attendant.Status = formdata["Status"];
            attendant.EditDate = DateTime.Now;

            db.SaveChanges();
            return Ok(new { message = "更新資料成功" });
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AttendantsExists(int id)
        {
            return db.Attendants.Count(e => e.Id == id) > 0;
        }
    }
}