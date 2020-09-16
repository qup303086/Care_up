using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Care_UP.Models;

namespace Care_UP.Controllers
{
    public class QuestionsController : ApiController
    {
        private Model1 db = new Model1();


        [Route("Quiz")]
        [HttpPost]
        public IHttpActionResult Quiz(Question quiz)
        {
            if (string.IsNullOrWhiteSpace(quiz.Quiz))
            {
                return Ok(new
                {
                    message = "提問沒填喔"
                });
            }
            quiz.InitDateTime =DateTime.Now;
            db.Questions.Add(quiz);
            db.SaveChanges();
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool QuestionExists(int id)
        {
            return db.Questions.Count(e => e.Id == id) > 0;
        }
    }
}