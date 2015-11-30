using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Security;
using System.Web.Mvc;

namespace PuzzleQuestWeb.Models
{
    public class UserQuest
    {
        [Key]
        public int QuestId { get; set; }
        public string QuestName { get; set; }
        public int QuestStatus { get; set; }
        public Guid UserId { get; set; }
        public virtual ICollection<QuestPuzzle> QuestPuzzles { get; set; }

        public virtual IEnumerable<SelectListItem> AllUsers { get; set; }

        public string UserName
        {
            get
            {
                MembershipUser user = Membership.GetUser(UserId);
                if (user == null)
                {
                    return "User not found";
                }

                return user.UserName;
            }
        }

        public string QuestStatusText
        {
            get
            {
                switch (QuestStatus)
                {
                    case 1:
                        return "In Progress";
                    case 2:
                        return "Completed";
                    default:
                        return "Pending";
                }
            }
        }

        public TimeSpan PenaltyTime
        {
            get
            {
                TimeSpan penalty = TimeSpan.FromSeconds(0);
                if (QuestPuzzles != null)
                {
                    foreach (QuestPuzzle puzzle in QuestPuzzles)
                    {
                        penalty += puzzle.PenaltyTime;
                    }
                }

                return penalty;
            }
        }

        public TimeSpan ElapsedTime
        {
            get
            {
                TimeSpan elapsed = TimeSpan.FromSeconds(0);
                if (QuestPuzzles != null)
                {
                    foreach (QuestPuzzle puzzle in QuestPuzzles)
                    {
                        elapsed += puzzle.ElapsedTime;
                    }
                }

                return elapsed;
            }
        }

        public TimeSpan TotalTime
        {
            get
            {
                return ElapsedTime + PenaltyTime;
            }
        }
    }
}