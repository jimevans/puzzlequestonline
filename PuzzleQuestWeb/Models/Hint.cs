using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PuzzleQuestWeb.Models
{
    public class Hint
    {
        public int HintId { get; set; }
        [AllowHtml]
        public string HintText { get; set; }
        public int HintOrder { get; set; }
        public bool SolutionWarning { get; set; }
        public int PuzzleId { get; set; }
    }
}