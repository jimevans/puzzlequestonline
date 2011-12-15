using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PuzzleQuestWeb.Models;
using System.Web.Security;

namespace PuzzleQuestWeb.Controllers
{ 
    public class UserController : Controller
    {
        private PuzzleQuestDbContext db = new PuzzleQuestDbContext();

        //
        // GET: /User/

        public ActionResult Index()
        {
            Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
            var userQuests = db.UserQuests.Where((q) => q.UserId == userId && (q.QuestStatus == 0 || q.QuestStatus == 1));
            if (userQuests.Count() > 0)
            {
                var quest = userQuests.First();
                if (quest.QuestStatus == 0)
                {
                    return RedirectToAction("QuestPending");
                }

                return RedirectToAction("QuestDetails", new { id = quest.QuestId });
            }

            return RedirectToAction("History");
        }

        public ViewResult History()
        {
            Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
            var userQuests = db.UserQuests.Where((q) => q.UserId == userId && q.QuestStatus == 2);
            return View(userQuests.ToList());
        }

        //
        // GET: /User/Details/5

        public ActionResult QuestDetails(int id)
        {
            Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
            UserQuest quest = db.UserQuests.Find(id);
            if (quest.UserId != userId)
            {
                return RedirectToAction("Index");
            }

            return View(quest);
        }

        public ViewResult QuestPending()
        {
            return View();
        }

        public ViewResult QuestFinish()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}