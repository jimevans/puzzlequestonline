using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PuzzleQuestWeb.Models;
using System.Web.Security;
using System.Net.Mail;
using System.Web.Profile;
using System.Data.Entity.SqlServer;

namespace PuzzleQuestWeb.Controllers
{ 
    public class AdminController : Controller
    {
        private PuzzleQuestDbContext db = new PuzzleQuestDbContext();

        //
        // GET: /Admin/

        public ViewResult Index()
        {
            return View(db.UserQuests.ToList());
        }

        //
        // GET: /Admin/Details/5

        public ViewResult Details(int id)
        {
            UserQuest quest = db.UserQuests.Find(id);
            return View(quest);
        }

        //
        // GET: /Admin/Create

        public ActionResult Create()
        {
            MembershipUserCollection allUsers = Membership.GetAllUsers();
            UserQuest quest = new UserQuest
            {
                AllUsers = allUsers.Cast<MembershipUser>().Select(user => new SelectListItem
                {
                    Value = user.ProviderUserKey.ToString(),
                    Text = user.UserName
                })
            };

            return View(quest);
        } 

        //
        // POST: /Admin/Create

        [HttpPost]
        public ActionResult Create(UserQuest quest)
        {
            if (ModelState.IsValid)
            {
                db.UserQuests.Add(quest);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(quest);
        }
        
        //
        // GET: /Admin/Edit/5
 
        public ActionResult Edit(int id)
        {
            UserQuest quest = db.UserQuests.Find(id);
            quest.AllUsers = Membership.GetAllUsers().Cast<MembershipUser>().Select(user => new SelectListItem
            {
                Value = user.ProviderUserKey.ToString(),
                Text = user.UserName
            });
            return View(quest);
        }

        //
        // POST: /Admin/Edit/5

        [HttpPost]
        public ActionResult Edit(UserQuest quest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(quest);
        }

        //
        // GET: /Admin/Delete/5
 
        public ActionResult Delete(int id)
        {
            UserQuest quest = db.UserQuests.Find(id);
            return View(quest);
        }

        //
        // POST: /Admin/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            UserQuest quest = db.UserQuests.Find(id);
            db.UserQuests.Remove(quest);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AddPuzzle(int questId)
        {
            QuestPuzzle qp = new QuestPuzzle();
            qp.QuestId = questId;
            qp.Quest = db.UserQuests.Find(questId);
            qp.AllPuzzles = db.Puzzles.Select(puzzle => new SelectListItem
            {
                Value = SqlFunctions.StringConvert((double)puzzle.PuzzleId),
                Text = puzzle.PuzzleName + ": " + puzzle.PuzzleSolutionText
            });
            return View(qp);
        }

        [HttpPost]
        public ActionResult AddPuzzle(QuestPuzzle qp)
        {
            if (ModelState.IsValid)
            {
                var quest = db.UserQuests.Find(qp.QuestId);
                qp.QuestPuzzleOrder = quest.QuestPuzzles.Count() + 1;
                qp.PuzzleStatus = 0;
                qp.NextHintIndexToDisplay = 0;
                db.Entry(qp).State = EntityState.Added;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(qp);
        }

        public ActionResult Reset(int id)
        {
            UserQuest quest = db.UserQuests.Find(id);
            var puzzles = quest.QuestPuzzles.OrderBy(qp => qp.QuestPuzzleOrder);

            for (int i = 0; i < puzzles.Count(); i++)
            {
                QuestPuzzle puzzle =  quest.QuestPuzzles.ElementAt(i);
                puzzle.PuzzleStatus = 0;
                puzzle.StartTime = null;
                puzzle.EndTime = null;
                puzzle.NextHintIndexToDisplay = 0;
                db.Entry(puzzle).State = EntityState.Modified;
            }

            quest.QuestStatus = 0;
            db.Entry(quest).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        public ActionResult Activate(int id)
        {
            UserQuest quest = db.UserQuests.Find(id);
            var puzzles = quest.QuestPuzzles.OrderBy(qp => qp.QuestPuzzleOrder);
            if (puzzles.Count() > 0)
            {
                var puzzle = puzzles.ElementAt(0);
                puzzle.PuzzleStatus = 1;
                puzzle.StartTime = DateTime.Now;
                db.Entry(puzzle).State = EntityState.Modified;
            }

            quest.QuestStatus = 1;
            db.Entry(quest).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult SendMessage(string userId)
        {
            MembershipUser user = Membership.GetUser(Guid.Parse(userId));
            return View(user);
        }

        [HttpPost]
        public ActionResult SendMessage(string userId, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                MembershipUser user = Membership.GetUser(Guid.Parse(userId));
                ProfileBase profile = ProfileBase.Create(user.UserName, true);
                string destinationAddress = profile.GetPropertyValue("SmsEmailAddress").ToString();

                MailMessage mailMessage = new MailMessage("puzzlemaster@notreallyhere.com", destinationAddress, "Message from PuzzleMaster", message);
                SmtpClient client = new SmtpClient();
                client.Send(mailMessage);
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}