using Microsoft.AspNetCore.Mvc;
using MQTT.Infrastructure.DAL;
using MQTT.Infrastructure.Models.DTO;
using MQTT.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MQTT.Web.Controllers
{
    public class CommandsController : Controller
    {
        private readonly string _connectionString = AppSettings.Instance.Configuration["connectionString"].ToString();
        private General _objGeneral;
        private static List<MessageTypeFieldDTO> _validFields;
        private static List<MessageTypeFieldDTO> _columnsSearch;
        private General DBAccess { get => _objGeneral; set => _objGeneral = value; }

        public CommandsController()
        {
            DBAccess = new General(_connectionString);
        }


        public IActionResult Index()
        {
            _validFields = ValidFieldsDAL.GetListValidFields(DBAccess, "Commands");
            _columnsSearch = _validFields.Where(f => f.SearchType).ToList();
            var columnsToHide = _validFields.Where(f => f.PrimaryType).Select(f => new { name = f.Name }).ToList();

            ViewBag.columnsSearch = JsonConvert.SerializeObject((_columnsSearch));
            ViewBag.columnsToHide = JsonConvert.SerializeObject((columnsToHide));
            return View();
        }

        public IActionResult Search(DateTime startDate, DateTime endDate, MessageTypeFieldDTO messageField = null, string value = null)
        {
            try
            {
                var result = SearchMessage(startDate, endDate, messageField, value);

                List<FilterViewModel> fields = new List<FilterViewModel>();

                foreach (var item in _columnsSearch)
                {
                    var data = result.AsEnumerable()
                        .Select(f => new { value = f.Field<dynamic>(item.Name).ToString() }).ToList();

                    var grouped = data.GroupBy(f => f.value).Select(f => new FilterViewModel
                    {
                        Id = item.Id,
                        NameField = item.Name,
                        Value = f.Key
                    }).ToList();
                    fields.AddRange(grouped);
                }

                var json = Json(
                    new
                    {
                        dataMessages = result,
                        filters = fields
                    }

                );
                return json;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable SearchMessage(DateTime startDate, DateTime endDate, MessageTypeFieldDTO messageField, string value)
        {
            try
            {
                DBAccess = new General(_connectionString);
                DataTable dtResult = CommandsDAL.SearchMessages(DBAccess, startDate, endDate, messageField, value);

                //foreach (DataColumn item in dtResult.Columns)
                //{
                //    var customName = _validFields.Where(v => v.Name.Equals(item.ColumnName)).Select(v => v.CustomName).FirstOrDefault();
                //    if (!string.IsNullOrEmpty(customName))
                //    {
                //        item.ColumnName = customName;
                //    }
                //}

                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
