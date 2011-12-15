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
    public class PuzzleController : Controller
    {
        private PuzzleQuestDbContext db = new PuzzleQuestDbContext();

        //
        // GET: /Puzzle/

        public ViewResult Index()
        {
            return View(db.Puzzles.ToList());
        }

        //
        // GET: /Puzzle/Details/5

        public ViewResult Details(int id)
        {
            Puzzle puzzle = db.Puzzles.Find(id);
            return View(puzzle);
        }

        //
        // GET: /Puzzle/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Puzzle/Create

        [HttpPost]
        public ActionResult Create(Puzzle puzzle)
        {
            if (ModelState.IsValid)
            {
                db.Puzzles.Add(puzzle);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(puzzle);
        }
        
        //
        // GET: /Puzzle/Edit/5
 
        public ActionResult Edit(int id)
        {
            Puzzle puzzle = db.Puzzles.Find(id);
            return View(puzzle);
        }

        //
        // POST: /Puzzle/Edit/5

        [HttpPost]
        public ActionResult Edit(Puzzle puzzle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(puzzle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(puzzle);
        }

        //
        // GET: /Puzzle/Delete/5
 
        public ActionResult Delete(int id)
        {
            Puzzle puzzle = db.Puzzles.Find(id);
            return View(puzzle);
        }

        //
        // POST: /Puzzle/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Puzzle puzzle = db.Puzzles.Find(id);
            db.Puzzles.Remove(puzzle);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}