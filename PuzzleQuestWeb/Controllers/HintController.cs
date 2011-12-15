using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PuzzleQuestWeb.Models;

namespace PuzzleQuestWeb.Controllers
{ 
    public class HintController : Controller
    {
        private PuzzleQuestDbContext db = new PuzzleQuestDbContext();

        //
        // GET: /Hint/

        public ViewResult Index()
        {
            return View(db.Hints.ToList());
        }

        //
        // GET: /Hint/Details/5

        public ViewResult Details(int id)
        {
            Hint hint = db.Hints.Find(id);
            return View(hint);
        }

        //
        // GET: /Hint/Create

        public ActionResult Create(int puzzleId)
        {
            Hint hint = new Hint
            {
                PuzzleId = puzzleId,
            };

            return View(hint);
        } 

        //
        // POST: /Hint/Create

        [HttpPost]
        public ActionResult Create(Hint hint)
        {
            if (ModelState.IsValid)
            {
                var hintCount = db.Puzzles.Find(hint.PuzzleId).Hints.Count();
                hint.HintOrder = hintCount + 1;
                db.Hints.Add(hint);
                db.SaveChanges();
                return RedirectToAction("Details", "Puzzle", new { id = hint.PuzzleId });  
            }

            return View(hint);
        }
        
        //
        // GET: /Hint/Edit/5
 
        public ActionResult Edit(int id)
        {
            Hint hint = db.Hints.Find(id);
            return View(hint);
        }

        //
        // POST: /Hint/Edit/5

        [HttpPost]
        public ActionResult Edit(Hint hint)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hint).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Puzzle", new { id = hint.PuzzleId });
            }

            return View(hint);
        }

        //
        // GET: /Hint/Delete/5
 
        public ActionResult Delete(int id)
        {
            Hint hint = db.Hints.Find(id);
            return View(hint);
        }

        //
        // POST: /Hint/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Hint hint = db.Hints.Find(id);
            db.Hints.Remove(hint);
            db.SaveChanges();
            return RedirectToAction("Details", "Puzzle", new { id = hint.PuzzleId });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}