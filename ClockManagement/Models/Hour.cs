using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace ClockManagement.Models
{
  public class Hour
  {
    public int id {get; set;}
    public DateTime? clock_in {get; set; }
    public DateTime? clock_out {get; set; }
    public int hours {get; set; }
    public int employee_id {get; set; }

    public Hour (int newEmployee_id, DateTime? clockIn = null, DateTime? clockOut = null, int newHours=0, int Id=0)
    {
      id = Id;
      hours = newHours;
      employee_id = newEmployee_id;
      clock_in = clockIn;
      clock_out = clockOut;
    }

    public override bool Equals(System.Object otherHour)
    {
      if (!(otherHour is Hour))
      {
        return false;
      }
      else
      {
        Hour newHour = (Hour) otherHour;
        bool areIdsEqual = (this.id == newHour.id);
        bool areClockInsEqual = (this.clock_in == newHour.clock_in);
        bool areClockOutsEqual = (this.clock_out == newHour.clock_out);
        bool areHoursEqual = (this.hours == newHour.hours);
        bool areEmployeeIdsEqual = (this.employee_id == newHour.employee_id);

        return (areIdsEqual && areEmployeeIdsEqual && areHoursEqual && areClockOutsEqual && areClockInsEqual);
      }
    }

    public override int GetHashCode()
    {
      return this.hours.GetHashCode();
    }

    public static List<Hour> GetAll()
      {
        List<Hour> allHours  = new List<Hour> ();
        MySqlConnection conn = DB.Connection();
        conn.Open();

        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = "SELECT * FROM employees_hours;";
        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        while(rdr.Read())
        {

          int employeeHours = rdr.GetInt32(3);
          int employeeId = rdr.GetInt32(4);
          DateTime employeeClockIn = rdr.GetDateTime(1);
          DateTime employeeClockOut = rdr.GetDateTime(2);
          int hourId = rdr.GetInt32(0);

          Hour newHour = new Hour(employeeId, employeeClockIn, employeeClockOut, employeeHours, hourId);
          allHours.Add(newHour);
        }

        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
        return allHours;
      }

      public void ClockIn(int id)
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();

        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO employee_hours (clock_in) VALUES (@clock_In) WHERE employee_id = @searchId;";
        cmd.Parameters.AddWithValue("@clock_In", DateTime.Now);
        cmd.Parameters.AddWithValue("@searchId", id);


        cmd.ExecuteNonQuery();
        id = (int) cmd.LastInsertedId;

        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
      }

      public void ClockOut(int id)

      {
        MySqlConnection conn = DB.Connection();
        conn.Open();

        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO employees_hours (clock_out) VALUES (@clock_Out) WHERE employee_id = @searchId;";
        cmd.Parameters.AddWithValue("@clock_Out", DateTime.Now);
        cmd.Parameters.AddWithValue("@searchId", id);


        cmd.ExecuteNonQuery();
        id = (int) cmd.LastInsertedId;

        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
      }

      public void Hours(int id)
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();

        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO employees_hours (hours) VALUES TIMESTAMPDIFF(MINUTE, 'clock_in', 'clock_out') WHERE employee_id = @searchId;";
        cmd.Parameters.AddWithValue("@searchId", id);


        cmd.ExecuteNonQuery();
        id = (int) cmd.LastInsertedId;

        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
      }

      public static Hour Find(int id)
       {
           MySqlConnection conn = DB.Connection();
           conn.Open();
           var cmd = conn.CreateCommand() as MySqlCommand;
           cmd.CommandText = @"SELECT * FROM employees_hours WHERE employee_id = (@searchId);";
           cmd.Parameters.AddWithValue("@searchId", id);

           var rdr = cmd.ExecuteReader() as MySqlDataReader;
           int Id = 0;
           DateTime? employeeClockIn = null;
           DateTime? employeeClockOut = null;
           int employeeHours = 0;
           int employeeId = 0;

           while(rdr.Read())
           {
             Id = rdr.GetInt32(0);
             employeeClockIn = rdr.GetDateTime(1);
             employeeClockOut = rdr.GetDateTime(2);
             employeeHours = rdr.GetInt32(3);
             employeeId = rdr.GetInt32(4);
           }
           Hour newHour = new Hour(employeeId, employeeClockIn, employeeClockOut, employeeHours, Id);
           conn.Close();
           if (conn != null)
           {
               conn.Dispose();
           }
           return newHour;
       }

      //  public void Save()
      //  {
      //    MySqlConnection conn = DB.Connection();
      //    conn.Open();
       //
      //    var cmd = conn.CreateCommand() as MySqlCommand;
      //    cmd.CommandText = @"INSERT INTO employees_hours (employee_id) VALUES (@searchId);";
       //
      //    MySqlParameter employeeName = new MySqlParameter();
      //    employeeName.ParameterName = "@searchId";
      //    employeeName.Value = this.id;
      //    cmd.Parameters.Add(employeeName);
       //
      //    cmd.ExecuteNonQuery();
      //    id = (int) cmd.LastInsertedId;
       //
      //    conn.Close();
      //    if (conn != null)
      //    {
      //      conn.Dispose();
      //    }
      //  }

       public void AddEmployee(Employee newEmployee)
       {
         MySqlConnection conn = DB.Connection();
         conn.Open();
         var cmd = conn.CreateCommand() as MySqlCommand;
         cmd.CommandText = @"INSERT INTO employees_hours (employee_id) VALUES (@EmployeeId)";
         cmd.Parameters.AddWithValue("@EmployeeId", newEmployee.id);
         cmd.ExecuteNonQuery();

         conn.Close();
         if (conn != null)
         {
           conn.Dispose();
         }
       }

  }
}
