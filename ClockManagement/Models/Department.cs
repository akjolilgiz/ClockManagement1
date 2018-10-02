using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace ClockManagement.Models
{
  public class Department
  {
    public string name {get; set;}
    public int id {get; set;}

    public Department (string departmentName, int departmentId=0)
    {
      name = departmentName;
      id = departmentId;
    }

    public override bool Equals(System.Object otherDepartment)
    {
      if (!(otherDepartment is Department))
      {
        return false;
      }
      else
      {
        Department newDepartment = (Department) otherDepartment;
        bool areIdsEqual = (this.id == newDepartment.id);
        bool areNamesEqual = (this.name == newDepartment.name);
        return (areIdsEqual && areNamesEqual);
      }
    }

    public override int GetHashCode()
    {
    return this.id.GetHashCode();
    }

    public static List<Department> GetAll()
    {
      List<Department> allDepartments  = new List<Department> ();
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = "SELECT * FROM departments;";

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        string departmentName = rdr.GetString(1);
        int departmentId = rdr.GetInt32(0);

        Department newDepartment = new Department(departmentName, departmentId);
        allDepartments.Add(newDepartment);
      }

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allDepartments;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO departments (name) VALUES (@name);";

      MySqlParameter departmentName = new MySqlParameter();
      departmentName.ParameterName = "@name";
      departmentName.Value = this.name;
      cmd.Parameters.Add(departmentName);

      cmd.ExecuteNonQuery();
      id = (int) cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static void Delete(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM departments WHERE id = @thisId;";

      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = id;
      cmd.Parameters.Add(thisId);

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM departments;";

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static Department Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM departments WHERE id = @searchId;";

      MySqlParameter SearchId = new MySqlParameter();
      SearchId.ParameterName = "@SearchId";
      SearchId.Value = id;
      cmd.Parameters.Add(SearchId);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      int departmentId = 0;
      string departmentName = "";

      while(rdr.Read())
      {
        departmentId = rdr.GetInt32(0);
        departmentName = rdr.GetString(1);
      }
      Department foundClass = new Department(departmentName, departmentId);

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return foundClass;
    }

    public void Edit(string departmentName)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE departments SET name = @newDepartmentName WHERE id = @searchId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = this.id;
      cmd.Parameters.Add(searchId);

      MySqlParameter newDepartmentName = new MySqlParameter();
      newDepartmentName.ParameterName = "@newDepartmentName";
      newDepartmentName.Value = departmentName;
      cmd.Parameters.Add(newDepartmentName);

      cmd.ExecuteNonQuery();
      this.name = departmentName;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void AddEmployee(Employee newEmployee)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO departments_employees (department_id, employee_id) VALUES (@DepartmentId, @EmployeeId);";

      MySqlParameter department_id = new MySqlParameter();
      department_id.ParameterName = "@DepartmentId";
      department_id.Value = id;
      cmd.Parameters.Add(department_id);

      MySqlParameter employee_id = new MySqlParameter();
      employee_id.ParameterName = "@EmployeeId";
      employee_id.Value = newEmployee.id;
      cmd.Parameters.Add(employee_id);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public List<Employee> GetEmployees()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT employees.* FROM departments
      JOIN departments_employees ON (departments.id = departments_employees.department_id)
      JOIN employees ON (departments_employees.employee_id = employees.id)
      WHERE departments.id = @DepartmentId;";

      MySqlParameter departmentIdParameter = new MySqlParameter();
      departmentIdParameter.ParameterName = "@DepartmentId";
      departmentIdParameter.Value = id;
      cmd.Parameters.Add(departmentIdParameter);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Employee> employees = new List<Employee>{};

      while(rdr.Read())
      {
        int employeeId = rdr.GetInt32(0);
        string employeeName = rdr.GetString(1);
        string employeeUsername = rdr.GetString(2);
        string employeeUserPassword = rdr.GetString(3);

        Employee newEmployee = new Employee(employeeName, employeeUsername, employeeUserPassword, employeeId);
        employees.Add(newEmployee);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return employees;
    }

    public static List<Department> SearchDepartment(string departmentName)
    {
      List<Department> allDepartments = new List<Department>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;

      cmd.CommandText = @"SELECT * FROM departments WHERE name LIKE @searchName;";

      cmd.Parameters.AddWithValue("@searchName", "%" + departmentName + "%");

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      while (rdr.Read())
      {
        int departmentId = rdr.GetInt32(0);
        string departmentsName = rdr.GetString(1);

        Department newDepartment = new Department (departmentsName, departmentId);
        allDepartments.Add(newDepartment);

      }
      conn.Close();
      if (conn !=null)
      {
        conn.Dispose();
      }
      return allDepartments;
    }
  }
}
