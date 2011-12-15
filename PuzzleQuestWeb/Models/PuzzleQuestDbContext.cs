using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace PuzzleQuestWeb.Models
{
    public class PuzzleQuestDbContext : DbContext
    {
        public DbSet<UserQuest> UserQuests { get; set; }
        public DbSet<QuestPuzzle> QuestPuzzles { get; set; }
        public DbSet<Puzzle> Puzzles { get; set; }
        public DbSet<Hint> Hints { get; set; }
    }
}