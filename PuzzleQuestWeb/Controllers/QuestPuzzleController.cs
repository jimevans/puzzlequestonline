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
    public class QuestPuzzleController : Controller
    {
        private PuzzleQuestDbContext db = new PuzzleQuestDbContext();

        public ActionResult Details(int puzzleId, int questId)
        {
            QuestPuzzle questpuzzle = db.QuestPuzzles.Find(questId, puzzleId);
            if (!IsQuestPuzzleForCurrentUser(questpuzzle))
            {
                return RedirectToAction("Index", "User");
            }

            return View(questpuzzle);
        }

        public ActionResult Activate(int puzzleId, int questId)
        {
            QuestPuzzle questpuzzle = db.QuestPuzzles.Find(questId, puzzleId);
            if (!IsQuestPuzzleForCurrentUser(questpuzzle))
            {
                return RedirectToAction("Index", "User");
            }

            return View(questpuzzle);
        }

        [HttpPost]
        public ActionResult Activate(QuestPuzzle questPuzzle)
        {
            QuestPuzzle actual = db.QuestPuzzles.Find(questPuzzle.QuestId, questPuzzle.PuzzleId);
            string activationKey = this.Request.Form["ActivationKey"];
            if (ModelState.IsValid)
            {
                if (string.Compare(actual.Puzzle.PuzzleActivationCode, activationKey, true) == 0)
                {
                    actual.PuzzleStatus = 2;
                    actual.ActivationTime = DateTime.Now;
                    db.Entry(actual).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Current", actual);
                }
                else
                {
                    ModelState.AddModelError("", "The activation code is incorrect.");
                }
            }

            return View(actual);
        }

        public ActionResult Current(int puzzleId, int questId)
        {
            QuestPuzzle questpuzzle = db.QuestPuzzles.Find(questId, puzzleId);
            if (!IsQuestPuzzleForCurrentUser(questpuzzle))
            {
                return RedirectToAction("Index", "User");
            }

            return View(questpuzzle);
        }

        public ActionResult Solve(int puzzleId, int questId)
        {
            QuestPuzzle questpuzzle = db.QuestPuzzles.Find(questId, puzzleId);
            if (!IsQuestPuzzleForCurrentUser(questpuzzle))
            {
                return RedirectToAction("Index", "User");
            }

            return View(questpuzzle);
        }

        [HttpPost]
        public ActionResult Solve(QuestPuzzle questPuzzle)
        {
            QuestPuzzle actual = db.QuestPuzzles.Find(questPuzzle.QuestId, questPuzzle.PuzzleId);
            string solution = this.Request.Form["Solution"];
            if (ModelState.IsValid)
            {
                if (solution.ToLowerInvariant().Contains(actual.Puzzle.PuzzleSolutionKeyword))
                {
                    var action = "Index";
                    actual.PuzzleStatus = 3;
                    actual.EndTime = DateTime.Now;
                    db.Entry(actual).State = EntityState.Modified;

                    var questPuzzles = db.QuestPuzzles.Where(qp => qp.QuestId == questPuzzle.QuestId && qp.QuestPuzzleOrder > actual.QuestPuzzleOrder);
                    if (questPuzzles.Count() > 0)
                    {
                        var nextPuzzle = questPuzzles.OrderBy(qp => qp.QuestPuzzleOrder).First();
                        nextPuzzle.PuzzleStatus = 1;
                        nextPuzzle.StartTime = DateTime.Now;
                        db.Entry(nextPuzzle).State = EntityState.Modified;
                    }
                    else
                    {
                        var q = actual.Quest;
                        q.QuestStatus = 2;
                        db.Entry(q).State = EntityState.Modified;
                        action = "QuestFinish";
                    }

                    db.SaveChanges();
                    return RedirectToAction(action, "User");
                }
                else
                {
                    ModelState.AddModelError("", "The solution is incorrect.");
                }
            }

            return View(actual);
        }

        public ActionResult Hint(int puzzleId, int questId)
        {
            QuestPuzzle questpuzzle = db.QuestPuzzles.Find(questId, puzzleId);
            if (!IsQuestPuzzleForCurrentUser(questpuzzle))
            {
                return RedirectToAction("Index", "User");
            }

            if (ModelState.IsValid)
            {
                questpuzzle.NextHintIndexToDisplay++;
                db.Entry(questpuzzle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Current", questpuzzle);
            }

            return View(questpuzzle);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private bool IsQuestPuzzleForCurrentUser(QuestPuzzle questpuzzle)
        {
            return questpuzzle.Quest.UserId == (Guid)Membership.GetUser().ProviderUserKey;
        }
    }
}