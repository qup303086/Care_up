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
using System.Web.WebPages;
using Care_UP.Models;
using Microsoft.Ajax.Utilities;

namespace Care_UP.Controllers
{
    public class AttendantsController : ApiController
    {
        private Model1 db = new Model1();



        [System.Web.Http.Route("AttendantDetails")]
        [System.Web.Http.HttpGet]
        //[JwtAuthFilter]
        public IHttpActionResult AttendantDetails(int Id)
        {
            //string token = Request.Headers.Authorization.Parameter;
            //Token jwtAuthUtil = new Token();
            //TokenPayload tokenPayload= jwtAuthUtil.GetToken(token);
            //if (tokenPayload.Identity!="a"|| tokenPayload.ID != Id )
            //{
            //    return Ok(new
            //    {
            //        message = "非法操作"
            //    });
            //}
            
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
                });
            }

            else
            {
                return Ok(new
                {
                    attendant,
                    cities,
                });
            }

        }

        [System.Web.Http.Route("AttendantDetailsLocation")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult AttendantDetailsLocation(int Id)
        {
            List<Locations> locations = db.Locations.Where(x => x.CityId == Id).ToList();
            return Ok(new
            {
                locations
            });
        }

        [System.Web.Http.Route("EditAttendantDetails")]
        [System.Web.Http.HttpPatch]
        public IHttpActionResult NewAttendantDetails()
        {
            
            var formdata = HttpContext.Current.Request;
            int AttendantID = Convert.ToInt32(formdata["Id"]);
            Attendants attendant = db.Attendants.Include(x => x.Locationses).Where(x => x.Id == AttendantID).FirstOrDefault();
            if (!formdata["Location"].IsNullOrWhiteSpace())
            {
                string[] formlocation = formdata["Location"].Split(',');
                int[] formlocationInt = new int[formlocation.Length];
                for (int i = 0; i < formlocation.Length; i++)
                {
                    formlocationInt[i] = Convert.ToInt32(formlocation[i]);
                }

                var NewLocationses = db.Locations.Where(x => formlocationInt.Contains(x.Id)).ToList();//搜尋 同SQL IN
            
                attendant.Locationses = NewLocationses;
            }
            else
            {
                return Ok(new { message = "未選擇地區" });
            }

            if (formdata.Files["Photo"] != null)
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
                else
                {
                    return Ok(new { message = "相片檔案格式不符" });
                }
            }
            else
            {
                if (attendant.Photo == null)
                {
                    return Ok(new { message = "未上傳照片" });
                }
            }

            if (formdata.Files["File"] != null)
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
                else
                {
                    return Ok(new { message = "證照檔案格式不符" });
                }
            }
            else
            {
                if (attendant.File == null)
                {
                    return Ok(new { message = "未上傳證照" });
                }
            }

            if (formdata["Name"] != null)
            {
                attendant.Name = formdata["Name"];
            }
            else
            {
                return Ok(new { message = "未填姓名" });
            }

            if (formdata["Salary"] != null)
            {
                attendant.Salary = Convert.ToInt32(formdata["Salary"]);
            }
            else
            {
                return Ok(new { message = "未填薪水" });
            }

            if (formdata["Account"] != null)
            {
                attendant.Account = formdata["Account"];
            }
            else
            {
                return Ok(new { message = "未填戶頭" });
            }

            if (formdata["Service"] != null)
            {
                attendant.Service = formdata["Service"];
            }
            else
            {
                return Ok(new { message = "未選擇能提供的服務項目" });
            }

            if (formdata["ServiceTime"] != null)
            {

                int serviceTime = Convert.ToInt32(formdata["ServiceTime"]);
                attendant.ServiceTime = (ServiceTime)serviceTime;

            }
            else
            {
                return Ok(new { message = "未選擇服務時段" });
            }

            if (formdata["Experience"] != null)
            {
                attendant.Experience = formdata["Experience"];
            }
            else
            {
                return Ok(new { message = "未填寫履歷" });
            }

            int status =Convert.ToInt32( formdata["Status"]);
            attendant.Status = (Whether) status;

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