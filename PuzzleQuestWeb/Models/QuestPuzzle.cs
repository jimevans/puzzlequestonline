using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PuzzleQuestWeb.Models
{
    public class QuestPuzzle
    {
        [Key, Column(Order = 1)]
        public int QuestId { get; set; }
        [Key, Column(Order = 2)]
        public int PuzzleId { get; set; }
        public int QuestPuzzleOrder { get; set; }
        public int NextHintIndexToDisplay { get; set; }
        public int PuzzleStatus { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? ActivationTime { get; set; }

        [ForeignKey("PuzzleId")]
        public virtual Puzzle Puzzle { get; set; }

        [ForeignKey("QuestId")]
        public virtual UserQuest Quest { get; set; }

        public virtual IEnumerable<SelectListItem> AllPuzzles { get; set; }

        public string PuzzleStatusText
        {
            get
            {
                switch (PuzzleStatus)
                {
                    case 1:
                        return "Pending Activation";
                    case 2:
                        return "In progress";
                    case 3:
                        return "Solved";
                    default:
                        return "Unavailable";
                }
            }
        }

        public TimeSpan ElapsedTime
        {
            get
            {
                if (StartTime.HasValue)
                {
                    if (EndTime.HasValue)
                    {
                        return EndTime.Value - StartTime.Value;
                    }
                    else
                    {
                        return DateTime.Now - StartTime.Value;
                    }
                }

                return TimeSpan.FromSeconds(0);
            }
        }

        public TimeSpan PenaltyTime
        {
            get
            {
                TimeSpan penalty = TimeSpan.FromMinutes(0);
                for (int i = 0; i < NextHintIndexToDisplay; i++)
                {
                    penalty += TimeSpan.FromMinutes(Math.Pow(2, i));
                }

                return penalty;
            }
        }

        public TimeSpan TotalTime
        {
            get { return ElapsedTime + PenaltyTime; }
        }
    }
}