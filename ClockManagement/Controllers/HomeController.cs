using Microsoft.AspNetCore.Mvc;
using ClockManagement.Models;
using System.Collections.Generic;

namespace ClockManagement.Controllers
{
    public class HomeController : Controller
    {
      [HttpGet("/")]
      public ActionResult Index()
      {
        return View();
      }

      [HttpPost("/")]
      public ActionResult LogIn(string userName, string password)
      {
        bool result = Employee.LogIn(userName, password);
        string result = "";
        if (result == true)
        {
          result = "true";
        }
        else
        {
          result = "false";
        }
        return View(result);
      }
    }
}
