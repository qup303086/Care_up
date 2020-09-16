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
            return Ok(new
            {
                message = "提問成功"
            });
        }

        [Route("AttendantsGetQuiz")]
        [HttpGet]
        public IHttpActionResult AttendantsGetQuiz(int id)
        {
            List<Question> quizList = db.Questions.Include(x=>x.QuestionAnswers).Where(x=>x.AttendantId==id).ToList();

            if (quizList.Count==0)
            {
                return Ok(new
                {
                    message = "還沒有提問喔"
                });
            }
           

            return Ok(quizList);
        }

        [Route("QuizReply")]
        [HttpPost]
        public IHttpActionResult QuizReply(QuestionAnswer questionAnswer)
        {
            if (string.IsNullOrWhiteSpace(questionAnswer.Answer))
            {
                return Ok(new
                {
                    message = "回覆沒填喔"
                });
            }
            questionAnswer.ReplyTime=DateTime.Now;
            db.QuestionAnswers.Add(questionAnswer);
            db.SaveChanges();
            return Ok(new
            {
                message = "已回覆"
            });
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