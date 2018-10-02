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
        bool result = Employee.Login(userName, password);
        string resultString = "";
        if (result == true)
        {
          resultString = "true";
        }
        else
        {
          resultString = "false";
        }
        return View(result);
      }
    }
}
