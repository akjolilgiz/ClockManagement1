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

      // [HttpGet("/signup")]
      // public ActionResult SignUp()
      // {
      //   return View();
      // }

      [HttpGet("/login")]
      public ActionResult Login()
      {
        return View();
      }

      [HttpPost("/login")]
      public ActionResult CreateLogIn(string userName, string password)
      {
        bool result = Employee.Login(userName, password);
        string resultString = "";
        if (result == true)
        {
          return View("Login");
        }
        else
        {
          resultString = "false";
        }
        return View(result);
      }

    }
}
