using Microsoft.AspNetCore.Mvc;
using ClockManagement.Models;
using System.Collections.Generic;

namespace ClockManagement.Controllers
{
  public class EmployeeController : Controller
  {
    [HttpGet("/employees")]
    public ActionResult Index()
    {
      Dictionary<string, object> model = new Dictionary <string, object>();
      List<Employee> allEmployees = Employee.GetAll();
      List<Department> allDepartments = Department.GetAll();
      model.Add("allEmployees", allEmployees);
      model.Add("allDepartments", allDepartments);
      return View(model);
    }

    [HttpGet("employees/new")]
    public ActionResult CreateForm()
    {
      Dictionary<string, object> model = new Dictionary <string, object>();
      List<Department> allDepartments = Department.GetAll();
      List<Employee> allEmployees = Employee.GetAll();
      model.Add("allDepartments",allDepartments);
      model.Add("allEmployees",allEmployees);
      return View(model);
    }

    [HttpPost("/employees")]
    public ActionResult Create(int employeeDepartmentId, string employeeName, string employeeUserName, string employeeUserPassword)
    {
      Employee newEmployee = new Employee(employeeName, employeeUserName, employeeUserPassword);
      newEmployee.Save();
      Department foundDepartment = Department.Find(employeeDepartmentId);
      newEmployee.AddDepartment(foundDepartment);

      return RedirectToAction("Index");
    }

    [HttpGet("/employees/delete")]
    public ActionResult DeleteAll()
    {
      Department.DeleteAll();
      return RedirectToAction("Index");
    }

    [HttpGet("employees/{employeeId}")]
    public ActionResult Details(int employeeId)
    {
      Dictionary<string, object> model = new Dictionary <string, object>();
      Employee selectedEmployee = Employee.Find(employeeId);
      List<Department> employeeDepartments = selectedEmployee.GetDepartments();
      List<Department> allDepartments = Department.GetAll();
      model.Add("selectedEmployee", selectedEmployee);
      model.Add("employeeDepartments", employeeDepartments);
      model.Add("allDepartments", allDepartments);
      return View(model);
    }

    [HttpPost("/employees/{employeeId}")]
    public ActionResult AddEmployee(int DepartmentId, int employeeId)
    {
      Department foundDepartment = Department.Find(DepartmentId);
      Employee foundEmployee = Employee.Find(employeeId);
      foundEmployee.AddDepartment(foundDepartment);
      return RedirectToAction("Details", new {employeeId = foundEmployee.id});
    }


    [HttpGet("/employees/{employeeId}/update")]
    public ActionResult UpdateForm(int employeeId)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      List<Department> allDepartments = Department.GetAll();
      Employee selectedEmployee = Employee.Find(employeeId);
      model.Add("allDepartments", allDepartments);
      model.Add("employee", selectedEmployee);
      return View(model);
    }

    [HttpPost("/employees/{employeeId}/update")]
    public ActionResult Update(int employeeId, string newName)
    {
      Employee thisEmployee = Employee.Find(employeeId);
      thisEmployee.Edit(newName);
      return RedirectToAction("Details", new {employeeId = thisEmployee.id});
    }

    [HttpGet("/employees/{employeeId}/delete")]
    public ActionResult Delete(int employeeId)
    {
      Employee thisEmployee = Employee.Find(employeeId);
      Employee.Delete(employeeId);
      return RedirectToAction("Index");
    }

    [HttpPost("/employees/search")]
    public ActionResult Search()
    {
      string userInput = Request.Form["searched"];
      string searchedEmployee =
      char.ToUpper(userInput[0]) + userInput.Substring(1);
      List<Employee> foundEmployees = Employee.SearchEmployee(searchedEmployee);
      return View(foundEmployees);
    }
    }
}
