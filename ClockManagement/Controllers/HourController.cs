using Microsoft.AspNetCore.Mvc;
using ClockManagement.Models;
using System.Collections.Generic;

namespace ClockManagement.Controllers
{
  public class HourController : Controller
  {
    [HttpGet("/hours")]
    public ActionResult Index()
    {
      List<Hour> allHours = Hour.GetAll();
      return View(allHours);
    }

    [HttpPost("employees/{id}/clockIn")]
    public ActionResult clockIn(int id)
    {
      Dictionary<string, object> model = new Dictionary <string, object>();
      Employee foundEmployee = Employee.Find(id);
      Hour newHour = Hour.Find(foundEmployee.id);
      newHour.ClockIn(foundEmployee.id);
      model.Add("foundEmployee",foundEmployee);
      model.Add("newHour", newHour);
      return RedirectToAction("~/Views/Employee/Details", new {employeeId = foundEmployee.id});
    }

    [HttpPost("employees/{id}/clockOut")]
    public ActionResult clockOut(int id)
    {
      Dictionary<string, object> model = new Dictionary <string, object>();
      Employee foundEmployee = Employee.Find(id);
      Hour newHour = Hour.Find(foundEmployee.id);
      newHour.ClockOut(foundEmployee.id);
      newHour.Hours(foundEmployee.id);
      model.Add("foundEmployee",foundEmployee);
      model.Add("newHour", newHour);
      return RedirectToAction("Details");
    }

  }
}
