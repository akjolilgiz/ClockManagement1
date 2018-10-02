using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace ClockManagement.Models
{
  public class Employee
  {
    public int id {get; set;}
    public string name {get; set;}
    public string username {get; set;}
    public string password {get; set;}

    public Employee (string Name, string loginUsername, string loginPassword, int employeeId = 0)
    {
      id = employeeId;
      name = Name;
      username = loginUsername;
      password = loginPassword;
    }

    public override bool Equals(System.Object otherEmployee)
    {
      if (!(otherEmployee is Employee))
      {
        return false;
      }
      else
      {
        Employee newEmployee = (Employee) otherEmployee;
        bool areIdsEqual = (this.id == newEmployee.id);
        bool areDescriptionsEqual = (this.name == newEmployee.name);
        bool areUserNamesEqual = (this.username == newEmployee.username);
        bool arePasswordsEqual = (this.password == newEmployee.password);
        return (areIdsEqual && areDescriptionsEqual && areUserNamesEqual && arePasswordsEqual);
      }
    }

    public override int GetHashCode()
    {
      return this.name.GetHashCode();
    }

    public static List<Employee> GetAll()
    {
      List<Employee> allEmployees  = new List<Employee> ();
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = "SELECT * FROM employees;";

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int employeeId = rdr.GetInt32(0);
        string employeeName = rdr.GetString(1);
        string employeeUsername = rdr.GetString(2);
        string employeeUserPassword = rdr.GetString(3);


        Employee newEmployee = new Employee(employeeName, employeeUsername, employeeUserPassword, employeeId);
        allEmployees.Add(newEmployee);
      }

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allEmployees;
    }

    public void SignUp()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO employees (username, password) VALUES (@username, @password);";

      MySqlParameter employeeUsername = new MySqlParameter();
      employeeUsername.ParameterUsername = "@username";
      employeeUsername.Value = this.username;
      cmd.Parameters.Add(employeeUsername);

      MySqlParameter employeePassword = new MySqlParameter();
      employeePassword.ParameterPassword = "@password";
      employeePassword.Value = this.login;
      cmd.Parameters.Add(employeePassword);

      cmd.ExecuteNonQuery();
      id = (int) cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    // public static Employee Login(string username, string password)
    // {
    //   MySqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //   var cmd = conn.CreateCommand() as MySqlCommand;
    //   cmd.CommandText = @"SELECT * FROM employees WHERE username = @username AND password = @password;";
    //
    //   MySqlParameter username = new MySqlParameter();
    //   username.ParameterName = "@username";
    //   username.Value = username;
    //   cmd.Parameters.Add(username);
    //
    //   MySqlParameter password = new MySqlParameter();
    //   password.ParameterName = "@password";
    //   password.Value = password;
    //   cmd.Parameters.Add(password);
    //
    //   MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
    //
    //   int employeeId = 0;
    //   string employeeName = "";
    //   string username = "";
    //   string password = "";
    //
    //   while(rdr.Read())
    //   {
    //     employeeId = rdr.GetInt32(0);
    //     employeeName = rdr.GetString(1);
    //     username = rdr.GetString(2);
    //     password = rdr.GetString(3);
    //   }
    //   Employee foundEmployee = new Employee(employeeName, username, password, employeeId);
    //
    //   conn.Close();
    //   if (conn != null)
    //   {
    //     conn.Dispose();
    //   }
    //   return foundEmployee;
    // }
    public static void Delete(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM employees WHERE id = @thisId;";

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
      cmd.CommandText = @"DELETE FROM employees;";

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static Employee Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM employees WHERE id = @searchId;";

      MySqlParameter SearchId = new MySqlParameter();
      SearchId.ParameterName = "@SearchId";
      SearchId.Value = id;
      cmd.Parameters.Add(SearchId);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      int employeeId = 0;
      string employeeName = "";
      string username = "";
      string password = "";

      while(rdr.Read())
      {
        employeeId = rdr.GetInt32(0);
        employeeName = rdr.GetString(1);
        username = rdr.GetString(2);
        password = rdr.GetString(3);
      }
      Employee foundEmployee = new Employee(employeeName, username, password, employeeId);

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return foundEmployee;
    }

    public void Edit(string employeeName)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE employees SET name = @newEmployeeName WHERE id = @searchId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = this.id;
      cmd.Parameters.Add(searchId);

      MySqlParameter newEmployeeName = new MySqlParameter();
      newEmployeeName.ParameterName = "@newEmployeeName";
      newEmployeeName.Value = employeeName;
      cmd.Parameters.Add(newEmployeeName);

      cmd.ExecuteNonQuery();
      this.name = employeeName;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void AddDepartment(Department newDepartment)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO departments_employees (department_id, employee_id) VALUES (@DepartmentId, @EmployeeId);";

      MySqlParameter departmentId = new MySqlParameter();
      departmentId.ParameterName = "@DepartmentId";
      departmentId.Value = newDepartment.id;
      cmd.Parameters.Add(departmentId);

      MySqlParameter employeeId = new MySqlParameter();
      employeeId.ParameterName = "@EmployeeId";
      employeeId.Value = id;
      cmd.Parameters.Add(employeeId);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }


    public List<Department> GetDepartments()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT department_id FROM departments_employees WHERE employee_id = @employeeId;";

      MySqlParameter employeeIdParameter = new MySqlParameter();
      employeeIdParameter.ParameterName = "@employeeId";
      employeeIdParameter.Value = id;
      cmd.Parameters.Add(employeeIdParameter);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      List<int> departmentIds = new List<int> {};
      while(rdr.Read())
      {
        int departmentId = rdr.GetInt32(0);
        departmentIds.Add(departmentId);
      }
      rdr.Dispose();

      List<Department> departments = new List<Department> {};
      foreach (int departmentId in departmentIds)
      {
        var departmentQuery = conn.CreateCommand() as MySqlCommand;
        departmentQuery.CommandText = @"SELECT * FROM departments WHERE id = @DepartmentId;";

        MySqlParameter departmentIdParameter = new MySqlParameter();
        departmentIdParameter.ParameterName = "@DepartmentId";
        departmentIdParameter.Value = departmentId;
        departmentQuery.Parameters.Add(departmentIdParameter);

        var departmentQueryRdr = departmentQuery.ExecuteReader() as MySqlDataReader;
        while(departmentQueryRdr.Read())
        {
          int thisDepartmentId = departmentQueryRdr.GetInt32(0);
          string departmentName = departmentQueryRdr.GetString(1);
          Department foundDepartment = new Department(departmentName, thisDepartmentId);
          departments.Add(foundDepartment);
        }
        departmentQueryRdr.Dispose();
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return departments;
    }

    public static List<Employee> SearchEmployee(string employeeName)
    {
      List<Employee> allEmployees = new List<Employee>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;

      cmd.CommandText = @"SELECT * FROM employees WHERE name LIKE @searchName;";

      cmd.Parameters.AddWithValue("@searchName", "%" + employeeName + "%");

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      while (rdr.Read())
      {
        int employeeId = rdr.GetInt32(0);
        string employeesName = rdr.GetString(1);
        string employeesUserName = rdr.GetString(2);
        string employeesUserPassword = rdr.GetString(3);

        Employee newEmployee = new Employee (employeesName, employeesUserName, employeesUserPassword, employeeId);
        allEmployees.Add(newEmployee);

      }
      conn.Close();
      if (conn !=null)
      {
        conn.Dispose();
      }
      return allEmployees;
    }

    public static bool Login(string userName, string password)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;

      cmd.CommandText = @"SELECT * FROM employees WHERE username = @searchName AND password = @searchPassword;";

      cmd.Parameters.AddWithValue("@searchName", userName);
      cmd.Parameters.AddWithValue("@searchPassword", password);

      MySql.Data.MySqlClient.MySqlDataAdapter MyAdapter = new MySql.Data.MySqlClient.MySqlDataAdapter(cmd);
      DataSet dataset = new DataSet();
      MyAdapter.Fill(dataset);
      if (dataset.Tables(0).Rows.Count > 0)
      {
      // Login Pass
      return true;
      }
      else
      {
      //Login fail
      return false;
      }
    }

      // string StrQuery1 = "Select count from table1 where userID='" + @userName + "' and password='" + txtPassword.Text + "'";
      // MySql.Data.MySqlClient.MySqlCommand myCommand = new MySql.Data.MySqlClient.MySqlCommand(strQuery, MySqlConn);
      // MySql.Data.MySqlClient.MySqlDataAdapter MyAdapter = new MySql.Data.MySqlClient.MySqlDataAdapter(myCommand);
      // DataSet ds = new DataSet();
      // MyAdapter.Fill(ds);
      // if (ds.Tables(0).Rows.Count > 0)
      // {
      // // Login Pass
      // }
      // else
      // {
      // //Login fail
      // }





    // public static int Login(string userName, string password)
    // {
    //   MySqlConnection conn = DB.Connection();
    //   conn.Open();
    //   MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
    //
    //   cmd.CommandText = @"SELECT * FROM users WHERE user_name = @userName;";
    //
    //   MySqlParameter userNameParameter = new MySqlParameter();
    //   userNameParameter.ParameterName = "@userName";
    //   userNameParameter.Value = userName;
    //   cmd.Parameters.Add(userNameParameter);
    //
    //   MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
    //
    //   int id = 0;
    //   string databasePassword = "";
    //   int sessionId = 0;
    //   string databaseUserName = "";
    //
    //   while (rdr.Read())
    //   {
    //     id = rdr.GetInt32(0);
    //     databaseUserName = rdr.GetString(4);
    //     databasePassword = rdr.GetString(5);
    //   }
    //
    //   rdr.Dispose();
    //   if (password == databasePassword && userName == databaseUserName)
    //   {
    //     cmd.CommandText = @"INSERT INTO sessions (user_id, session_id) VALUES (@userId, @sessionId);";
    //
    //     MySqlParameter thisIdParameter = new MySqlParameter();
    //     thisIdParameter.ParameterName = "@userId";
    //     thisIdParameter.Value = id;
    //     cmd.Parameters.Add(thisIdParameter);
    //
    //     MySqlParameter sessionIdParameter = new MySqlParameter();
    //     sessionIdParameter.ParameterName = "@sessionId";
    //     Random newRandom = new Random();
    //     int randomNumber = newRandom.Next(100000000);
    //     sessionIdParameter.Value = randomNumber;
    //     cmd.Parameters.Add(sessionIdParameter);
    //
    //     cmd.ExecuteNonQuery();
    //     sessionId = randomNumber;
    //   }
    //   else
    //   {
    //
    //   }
    //   conn.Close();
    //   if (!(conn == null))
    //   {
    //     conn.Dispose();
    //   }
    //
    //   return sessionId;
    // }
  }
}
