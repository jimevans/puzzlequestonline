using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PuzzleQuestWeb.Models
{
    public class Puzzle
    {
        public int PuzzleId { get; set; }
        public string PuzzleName { get; set; }
        public int PuzzleType { get; set; }
        [AllowHtml]
        public string PuzzleText { get; set; }
        public string PuzzleSolutionKeyword { get; set; }
        public string PuzzleSolutionText { get; set; }
        public string PuzzleResourceFileName { get; set; }
        public string PuzzleActivationCode { get; set; }
        public virtual ICollection<Hint> Hints { get; set; }
        public virtual ICollection<QuestPuzzle> QuestPuzzles { get; set; }
    }
}