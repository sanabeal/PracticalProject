using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace PracticalProject.Models
{
    public class MasterDetail
    {
        SqlTransaction transaction1;
        db_Connection Connstring = new db_Connection();
        public List<Bill> ListAll()
        {
            List<Bill> details_data = new List<Bill>();
            using (SqlConnection con = Connstring.getcon)
            {
                con.Open();
                using (SqlCommand sql_cmnd = new SqlCommand("spMasterDetail", con))
                {
                    sql_cmnd.CommandType = CommandType.StoredProcedure;
                    sql_cmnd.Parameters.AddWithValue("@ActionType", "GetAllMasterData");
                    sql_cmnd.Parameters.AddWithValue("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    SqlDataReader reader = sql_cmnd.ExecuteReader();
                    while (reader.Read())
                    {
                        Bill col_data = new Bill();
                        col_data.BillMasterID = reader.GetInt32(0);
                        col_data.BillDate = reader.GetString(1);
                        col_data.CustomerName = reader.GetString(2);
                        col_data.ContactNo = reader.GetString(3);

                        details_data.Add(col_data);
                    }
                }
            }
            return details_data;
        }
        
        public string SaveMasterDetails(Bill master, string[][] array, string[][] array1)
        {
            Connstring.conn.Open();
            transaction1 = Connstring.conn.BeginTransaction();
            try
            {
                SqlCommand MyCommand = new SqlCommand("spMasterDetail", Connstring.conn, transaction1);
                MyCommand.CommandType = CommandType.StoredProcedure;
                MyCommand.Parameters.AddWithValue("@ActionType", "SaveOrUpdateMasterData");
                MyCommand.Parameters.AddWithValue("@BillDate", master.BillDate);
                MyCommand.Parameters.AddWithValue("@CustomerName", master.CustomerName);
                MyCommand.Parameters.AddWithValue("@ContactNo", master.ContactNo);
                MyCommand.Parameters.AddWithValue("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

                MyCommand.ExecuteNonQuery();
                string id = MyCommand.Parameters["@id"].Value.ToString();

                //-----------Save Detail------//
                string result = string.Empty;
                DataTable dt = new DataTable();
                dt.Columns.Add("ItemID");
                dt.Columns.Add("UnitPrice");
                dt.Columns.Add("ItemQty");
                dt.Columns.Add("TotalPrice");
                //-------------Set Data Tbale Value----------------//
                foreach (var arr in array)
                {
                    DataRow dr = dt.NewRow();
                    dr["ItemID"] = arr[0];
                    dr["UnitPrice"] = arr[1];
                    dr["ItemQty"] = arr[2];
                    dr["TotalPrice"] = arr[3];
                    dt.Rows.Add(dr);
                }
                //--------------Insert Details Data---------------//
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SqlCommand MyCommandDetail = new SqlCommand("spMasterDetail", Connstring.conn, transaction1);
                    MyCommandDetail.CommandType = CommandType.StoredProcedure;
                    MyCommandDetail.Parameters.AddWithValue("@ActionType", "SaveOrUpdateDetailData");
                    MyCommandDetail.Parameters.AddWithValue("@BillMasterID", id.ToString());
                    MyCommandDetail.Parameters.AddWithValue("@ItemID", dt.Rows[i]["ItemID"].ToString().Trim());
                    MyCommandDetail.Parameters.AddWithValue("@UnitPrice", dt.Rows[i]["UnitPrice"].ToString().Trim());
                    MyCommandDetail.Parameters.AddWithValue("@ItemQty", dt.Rows[i]["ItemQty"].ToString().Trim());
                    MyCommandDetail.Parameters.AddWithValue("@TotalPrice", dt.Rows[i]["TotalPrice"].ToString().Trim());
                    MyCommandDetail.Parameters.AddWithValue("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    MyCommandDetail.ExecuteNonQuery();
                }

                transaction1.Commit();
                Connstring.conn.Close();

            }
            catch (Exception ex)
            {
                transaction1.Rollback();
                Connstring.conn.Close();
            }

            //-------------------- Start ADO.Net with Transction -----------------------------------//
            #region
            //Connstring.conn.Open();
            //transaction1 = Connstring.conn.BeginTransaction();
            //try
            //{
            //    //-----------Save Master------//
            //    string sql = @"INSERT  INTO  BillMaster(BillDate, CustomerName, ContactNo) output INSERTED.BillMasterID VALUES (@BillDate, @CustomerName, @ContactNo)";
            //    SqlCommand MyCommand = new SqlCommand(sql, Connstring.conn, transaction1);
            //    MyCommand.Parameters.AddWithValue("@BillDate", master.BillDate);

            //    MyCommand.Parameters.AddWithValue("@CustomerName", master.CustomerName);
            //    MyCommand.Parameters.AddWithValue("@ContactNo", master.ContactNo);

            //    int ID = (int)MyCommand.ExecuteScalar();

            //    //-----------Save Detail------//
            //    string result = string.Empty;
            //    DataTable dt = new DataTable();
            //    dt.Columns.Add("ItemID");
            //    dt.Columns.Add("UnitPrice");
            //    dt.Columns.Add("ItemQty");
            //    dt.Columns.Add("TotalPrice");
            //    //-------------Set Data Tbale Value----------------//
            //    foreach (var arr in array)
            //    {
            //        DataRow dr = dt.NewRow();
            //        dr["ItemID"] = arr[0];
            //        dr["UnitPrice"] = arr[1];
            //        dr["ItemQty"] = arr[2];
            //        dr["TotalPrice"] = arr[3];
            //        dt.Rows.Add(dr);
            //    }
            //    //--------------Insert Details Data---------------//
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        string sqldetails = @"INSERT INTO BillChilds(BillMasterID, ItemID, UnitPrice, ItemQty, TotalPrice) VALUES(@BillMasterID, @ItemID, @UnitPrice, @ItemQty, @TotalPrice)";
            //        SqlCommand MyCommandDetail = new SqlCommand(sqldetails, Connstring.conn, transaction1);
            //        MyCommandDetail.Parameters.AddWithValue("@BillMasterID", ID);
            //        MyCommandDetail.Parameters.AddWithValue("@ItemID", dt.Rows[i]["ItemID"].ToString().Trim());
            //        MyCommandDetail.Parameters.AddWithValue("@UnitPrice", dt.Rows[i]["UnitPrice"].ToString().Trim());
            //        MyCommandDetail.Parameters.AddWithValue("@ItemQty", dt.Rows[i]["ItemQty"].ToString().Trim());
            //        MyCommandDetail.Parameters.AddWithValue("@TotalPrice", dt.Rows[i]["TotalPrice"].ToString().Trim());
            //        MyCommandDetail.ExecuteNonQuery(); 
            //    }
            //    transaction1.Commit();
            //    Connstring.conn.Close();

            //}
            //catch (Exception ex)
            //{
            //    transaction1.Rollback();
            //    Connstring.conn.Close();
            //}
            #endregion
            //-------------------- END ADO.Net with Transction -----------------------------------//

            return "";
        }
                
        public object UpdateMasterDetails(Bill master, string[][] array, string[][] array1)
        {
            Connstring.conn.Open();
            transaction1 = Connstring.conn.BeginTransaction();
            try
            {
                SqlCommand MyCommand = new SqlCommand("spMasterDetail", Connstring.conn, transaction1);
                MyCommand.CommandType = CommandType.StoredProcedure;
                MyCommand.Parameters.AddWithValue("@ActionType", "SaveOrUpdateMasterData");
                MyCommand.Parameters.AddWithValue("@BillDate", master.BillDate);
                MyCommand.Parameters.AddWithValue("@BillMasterID", master.BillMasterID);
                MyCommand.Parameters.AddWithValue("@CustomerName", master.CustomerName);
                MyCommand.Parameters.AddWithValue("@ContactNo", master.ContactNo);
                MyCommand.Parameters.AddWithValue("@id", SqlDbType.Int).Direction = ParameterDirection.Output;                              
                MyCommand.ExecuteNonQuery();
           
                string result = string.Empty;
                DataTable dt = new DataTable();
                dt.Columns.Add("ItemID");
                dt.Columns.Add("UnitPrice");
                dt.Columns.Add("ItemQty");
                dt.Columns.Add("TotalPrice");
                dt.Columns.Add("remove_id");
                //-------------Set Data Tbale Value----------------//
                foreach (var arr in array)
                {
                    DataRow dr = dt.NewRow();
                    dr["ItemID"] = arr[0];
                    dr["UnitPrice"] = arr[1];
                    dr["ItemQty"] = arr[2];
                    dr["TotalPrice"] = arr[3];
                    dr["remove_id"] = arr[5];
                    dt.Rows.Add(dr);
                }

                //----------Delete Removed Data----------------//
                string[] new_details_data = array1[1][1].Split('#');
                foreach (var s_data in new_details_data)
                {
                    if (s_data != "")
                    {
                        SqlCommand MyCommandDelete = new SqlCommand("spMasterDetail", Connstring.conn, transaction1);
                        MyCommandDelete.CommandType = CommandType.StoredProcedure;
                        MyCommandDelete.Parameters.AddWithValue("@ActionType", "DeleteDetailData");
                        MyCommandDelete.Parameters.AddWithValue("@BillDetailsID", s_data);
                        MyCommandDelete.Parameters.AddWithValue("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                        MyCommandDelete.ExecuteNonQuery();                        
                    }
                }

                //-------------Update/Insert New Data----------//
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["remove_id"].ToString().Trim() == "")
                    {
                        SqlCommand MyCommandDetail = new SqlCommand("spMasterDetail", Connstring.conn, transaction1);
                        MyCommandDetail.CommandType = CommandType.StoredProcedure;
                        MyCommandDetail.Parameters.AddWithValue("@ActionType", "SaveOrUpdateDetailData");
                        MyCommandDetail.Parameters.AddWithValue("@BillMasterID", master.BillMasterID);
                        MyCommandDetail.Parameters.AddWithValue("@ItemID", dt.Rows[i]["ItemID"].ToString().Trim());
                        MyCommandDetail.Parameters.AddWithValue("@UnitPrice", dt.Rows[i]["UnitPrice"].ToString().Trim());
                        MyCommandDetail.Parameters.AddWithValue("@ItemQty", dt.Rows[i]["ItemQty"].ToString().Trim());
                        MyCommandDetail.Parameters.AddWithValue("@TotalPrice", dt.Rows[i]["TotalPrice"].ToString().Trim());
                        MyCommandDetail.Parameters.AddWithValue("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                        MyCommandDetail.ExecuteNonQuery();
                    }
                    else
                    {
                        SqlCommand MyCommandDetail = new SqlCommand("spMasterDetail", Connstring.conn, transaction1);
                        MyCommandDetail.CommandType = CommandType.StoredProcedure;
                        MyCommandDetail.Parameters.AddWithValue("@ActionType", "SaveOrUpdateDetailData");
                        MyCommandDetail.Parameters.AddWithValue("@BillMasterID", master.BillMasterID);
                        MyCommandDetail.Parameters.AddWithValue("@BillDetailsID", dt.Rows[i]["remove_id"].ToString().Trim());
                        MyCommandDetail.Parameters.AddWithValue("@ItemID", dt.Rows[i]["ItemID"].ToString().Trim());
                        MyCommandDetail.Parameters.AddWithValue("@UnitPrice", dt.Rows[i]["UnitPrice"].ToString().Trim());
                        MyCommandDetail.Parameters.AddWithValue("@ItemQty", dt.Rows[i]["ItemQty"].ToString().Trim());
                        MyCommandDetail.Parameters.AddWithValue("@TotalPrice", dt.Rows[i]["TotalPrice"].ToString().Trim());
                        MyCommandDetail.Parameters.AddWithValue("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                        MyCommandDetail.ExecuteNonQuery();                        
                    }
                }

                transaction1.Commit();
                Connstring.conn.Close();

            }
            catch (Exception ex)
            {
                transaction1.Rollback();
                Connstring.conn.Close();
            }
            return "";
            //  return Json("", JsonRequestBehavior.AllowGet);
        }
        
        public List<ListItem> LoadDropDown(string db_table, string col_value, string col_text, string condition_field, string condition, string condition_field1, string condition1)
        {
            string sql;
            if (condition != "" && condition_field != "")
            {
                sql = "SELECT " + col_value + ", " + col_text + " FROM " + db_table + " WHERE " + condition_field + "='" + condition + "'";
            }
            else
            {
                sql = "SELECT " + col_value + ", " + col_text + " FROM " + db_table;
            }

            using (SqlConnection con = Connstring.getcon)
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    List<ListItem> customers = new List<ListItem>();
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customers.Add(new ListItem
                            {
                                Value = reader[col_value].ToString(),
                                Text = reader[col_text].ToString()
                            });
                        }
                    }
                    con.Close();
                    return customers;
                }
            }
        }

        public object GetData(string id)
        {
            List<Bill> details_data = new List<Bill>();
            using (SqlConnection con = Connstring.getcon)
            {
                con.Open();
                using (SqlCommand sql_cmnd = new SqlCommand("spMasterDetail", con))
                {
                    sql_cmnd.CommandType = CommandType.StoredProcedure;
                    sql_cmnd.Parameters.AddWithValue("@ActionType", "GetMasterDataByID");
                    sql_cmnd.Parameters.AddWithValue("@BillMasterID", id);
                    sql_cmnd.Parameters.AddWithValue("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    SqlDataReader reader = sql_cmnd.ExecuteReader();
                    while (reader.Read())
                    {
                        Bill col_data = new Bill();
                        col_data.BillMasterID = reader.GetInt32(0);
                        col_data.BillDate = reader.GetString(1);
                        col_data.CustomerName = reader.GetString(2);
                        col_data.ContactNo = reader.GetString(3);

                        details_data.Add(col_data);
                    }
                }
            }
            return details_data;

        }

        public List<Bill> GetDetailData(string id)
        {
            List<Bill> details_data_2 = new List<Bill>();
            using (SqlConnection con = Connstring.getcon)
            {
                con.Open();
                using (SqlCommand sql_cmnd = new SqlCommand("spMasterDetail", con))
                {
                    sql_cmnd.CommandType = CommandType.StoredProcedure;
                    sql_cmnd.Parameters.AddWithValue("@ActionType", "GetDetailDataByID");
                    sql_cmnd.Parameters.AddWithValue("@BillMasterID", id);
                    sql_cmnd.Parameters.AddWithValue("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    SqlDataReader reader = sql_cmnd.ExecuteReader();
                    while (reader.Read())
                    {
                        Bill col_data_2 = new Bill();
                        col_data_2.BillDetailsID = reader.GetInt32(0);
                        col_data_2.ItemID = reader.GetInt32(1);
                        col_data_2.UnitPrice = reader.GetDecimal(2);
                        col_data_2.ItemQty = reader.GetDecimal(3);
                        col_data_2.TotalPrice = reader.GetDecimal(4);

                        details_data_2.Add(col_data_2);
                    }
                }
            }
            return details_data_2;
        }

        internal object DeleteData(string id)
        {
            //Connstring.conn.Open();
            //transaction1 = Connstring.conn.BeginTransaction();

            //try
            //{

            //    SqlCommand MyCommandDelete = new SqlCommand("spMasterDetail", Connstring.conn, transaction1);
            //    MyCommandDelete.CommandType = CommandType.StoredProcedure;
            //    MyCommandDelete.Parameters.AddWithValue("@ActionType", "DeleteDetailDataByMasterID");
            //    MyCommandDelete.Parameters.AddWithValue("@BillMasterID", id);
            //    MyCommandDelete.Parameters.AddWithValue("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
            //    MyCommandDelete.ExecuteNonQuery();

            //    SqlCommand MyCommandMDelete = new SqlCommand("spMasterDetail", Connstring.conn, transaction1);
            //    MyCommandMDelete.CommandType = CommandType.StoredProcedure;
            //    MyCommandMDelete.Parameters.AddWithValue("@ActionType", "DeleteMasterData");
            //    MyCommandMDelete.Parameters.AddWithValue("@BillMasterID", id);
            //    MyCommandMDelete.Parameters.AddWithValue("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
            //    MyCommandMDelete.ExecuteNonQuery();

            //    transaction1.Commit();
            //    Connstring.conn.Close();

            //}
            //catch (Exception ex)
            //{
            //    transaction1.Rollback();
            //    Connstring.conn.Close();
            //}
            //return "";

            
            string sql = @"Delete from BillChilds where BillMasterID=" + id + "";
            SqlCommand MyCommand = new SqlCommand(sql, Connstring.conn);
            Connstring.conn.Open();
            int reslt = MyCommand.ExecuteNonQuery();
            Connstring.conn.Close();

            if (reslt > 0)
            {
                string sqls = @"Delete from BillMaster where BillMasterID=" + id + "";
                SqlCommand MyCommands = new SqlCommand(sqls, Connstring.conn);
                Connstring.conn.Open();
                int reslts = MyCommands.ExecuteNonQuery();
                Connstring.conn.Close();
            }

            return "Success";
        }
    }
}