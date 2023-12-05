using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PracticalProject.Models
{
    public class db_Connection
    {
        public SqlConnection conn = new SqlConnection("Data Source=KABIR-PC; Initial Catalog=PracticalProjectDB; Persist Security Info=True;User ID=sa;Password=sa*1209"); //Localhost
        //-------------------------------------//
        public DataTable dt = new DataTable();
        public SqlCommand cmd = new SqlCommand();
        public SqlDataAdapter da = new SqlDataAdapter();

        public SqlConnection getcon
        {
            get { return conn; }
        }

        public DataTable SqlDataTable(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();
            return dt;
        }
        public void insert(dynamic qurry)
        {
            try
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(qurry, conn))
                {
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                conn.Close();
            }
            catch (SqlException ex)
            {
                conn.Close();
                string msg = "Insert Error:";
                msg += ex.Message;
            }
        }

        public void insert_details(dynamic qurry)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(qurry, conn))
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (SqlException ex)
            {
                string msg = "Insert Error:";
                msg += ex.Message;
            }
        }

      
        public void CallProcedure(Array sqlpra, string procidurname)  //Call Procedur whith Paramiter and Procidure Name
        {
            try
            {
                cmd = new SqlCommand(procidurname, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                if (sqlpra != null)
                {
                    cmd.Parameters.AddRange(sqlpra);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                conn.Close();
                throw ex;
            }

        }
        public void VisitingLog(string ReportName, string URL, string UserID, string Type, string ProjectURL)
        {
            string sql = @"INSERT INTO VisitingLog
                         (ReportName, PageURL, UserID,Type,ProjectURL)
                    VALUES (@ReportName, @PageURL, @UserID,@Type,@ProjectURL)";
            SqlCommand MyCommand = new SqlCommand(sql, conn);
            MyCommand.Parameters.AddWithValue("@ReportName", ReportName);
            MyCommand.Parameters.AddWithValue("@PageURL", ProjectURL);
            MyCommand.Parameters.AddWithValue("@UserID", UserID);
            MyCommand.Parameters.AddWithValue("@Type", Type);
            MyCommand.Parameters.AddWithValue("@ProjectURL", URL);
            conn.Open();
            MyCommand.ExecuteNonQuery();
            conn.Close();
        }
    }
}