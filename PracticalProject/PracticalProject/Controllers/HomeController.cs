using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using PracticalProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace PracticalProject.Controllers
{
    public class HomeController : Controller
    {
        Bill _Bill = new Bill();
        db_Connection Connstring = new db_Connection();
        MasterDetail _MasterDetail = new MasterDetail();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public JsonResult List()
        {
            return Json(_MasterDetail.ListAll(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult set_drop_down_list_Product(string db_table, string col_value, string col_text, string condition_field, string condition, string condition_field1, string condition1)
        {
            if (db_table != null)
                return Json(_MasterDetail.LoadDropDown(db_table, col_value, col_text, condition_field, condition, condition_field1, condition1), JsonRequestBehavior.AllowGet);
            else
                return Json("");
        }
             
        public JsonResult SaveDetails(Bill master, string[][] array, string[][] array1)
        {
            var ms = _MasterDetail.SaveMasterDetails(master,array, array1);
            return Json(ms, JsonRequestBehavior.AllowGet);
        }
              
        public JsonResult UpdateDetails(Bill master, string[][] array, string[][] array1)
        {
            var ms = _MasterDetail.UpdateMasterDetails(master, array, array1);
            return Json(ms, JsonRequestBehavior.AllowGet);           
        }

        public JsonResult GetData(string id)
        {
            if (id != null)
                return Json(_MasterDetail.GetData(id), JsonRequestBehavior.AllowGet);
            else
                return Json("");
        }

        public JsonResult DeleteDt(string id)
        {
            var ms = _MasterDetail.DeleteData(id);
            return Json(ms, JsonRequestBehavior.AllowGet);
        }

        public JsonResult show_details_data(string id)
        {
            if (id != null)
                return Json(_MasterDetail.GetDetailData(id), JsonRequestBehavior.AllowGet);
            else
                return Json("");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}