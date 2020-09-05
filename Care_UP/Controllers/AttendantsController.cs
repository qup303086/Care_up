using System;
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

        // GET: api/Attendants
        public IQueryable<Attendants> GetAttendants()
        {
            return db.Attendants;
        }

        [System.Web.Http.Route("AttendantDetails")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult AttendantDetails(int Id)
        {
            Attendants attendant = db.Attendants.FirstOrDefault(x => x.Id == Id);
            return Ok(new { attendant });
        }

        [System.Web.Http.Route("EditAttendantDetails")]
        [System.Web.Http.HttpPatch]
        public IHttpActionResult NewAttendantDetails()
        {
            var formdata = HttpContext.Current.Request;
            int AttendantID = Convert.ToInt32(formdata["Id"]);
            Attendants attendants = db.Attendants.Find(AttendantID);
            string photo = "";
            string file = "";
            if (formdata.Files["Photo"].FileName != null)
            {
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
                    string oldpath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath + "Uploads", attendants.Photo);
                    File.Delete(oldpath);

                    photo = AttendantID+"_p_"+ DateTime.Now.ToString("yyyyMMddHHmm")+ fileExtension;
                    string path = Path.Combine(HostingEnvironment.ApplicationPhysicalPath + "Uploads",photo);
                    formdata.Files["Photo"].SaveAs(path);
                    attendants.Photo = photo;
                }
            }
            if (formdata.Files["File"].FileName != null)
            {
                string fileExtension = Path.GetExtension(formdata.Files["File"].FileName).ToLower();
                string[] Extension = { ".jpeg", ".jpg", ".pdf"};
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
                    string oldpath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath + "Uploads", attendants.File);
                    File.Delete(oldpath);

                    file = AttendantID + "_f_" + DateTime.Now.ToString("yyyyMMddHHmm") + fileExtension;
                    string path = Path.Combine(HostingEnvironment.ApplicationPhysicalPath + "Uploads",file);
                    formdata.Files["File"].SaveAs(path);
                    attendants.File = file;
                }
            }
            attendants.Name = formdata["Name"];
            attendants.Account = formdata["Account"];
            attendants.Salary = formdata["Salary"];
            attendants.Account = formdata["Account"];
            attendants.Service = formdata["Service"];
            attendants.ServiceTime = formdata["ServiceTime"];
            attendants.Experience = formdata["Experience"];
            attendants.StartDateTime = Convert.ToDateTime(formdata["StartDateTime"]);
            attendants.EndDateTime = Convert.ToDateTime(formdata["EndDateTime"]);
            attendants.Status = formdata["Status"];
            attendants.EditDate = DateTime.Now;
            
            db.SaveChanges();
            return Ok(new{message = ModelState});
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