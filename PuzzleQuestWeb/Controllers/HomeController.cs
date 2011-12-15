using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PuzzleQuestWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to the PuzzleMaster's domain.";
            if (Membership.GetAllUsers().Count == 0)
            {
                Membership.CreateUser("PuzzleMaster", "pqadmin");
                Roles.CreateRole("Administrator");
                Roles.AddUserToRole("PuzzleMaster", "Administrator");
            }

            return View();
        }
    }
}
