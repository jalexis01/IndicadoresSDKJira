using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly string _connectionString = AppSettings.Instance.Configuration["connectionString"].ToString();
        private General _objGeneral;
        private static List<MessageTypeFieldDTO> _validFields;
        private static List<MessageTypeFieldDTO> _columnsSearch;
        private static List<SettingDTO> _settings;
        private General DBAccess { get => _objGeneral; set => _objGeneral = value; }

        public MessagesController()
        {
            DBAccess = new General(_connectionString);
        }


        public IActionResult Index()
        {
            // Obtiene la identidad del usuario actual
            var identity = User.Identity as System.Security.Claims.ClaimsIdentity;

            // Verifica si el usuario tiene el rol de "Administrador"
            if (identity != null && identity.HasClaim(System.Security.Claims.ClaimTypes.Name, "admin@admin.com"))
            {
                ViewBag.Menu = "admin";
            }
            else
            {
                ViewBag.Menu = "user";
            }
            _validFields = ValidFieldsDAL.GetListValidFields(DBAccess, "Messages");
            _settings = General.GetSettings(DBAccess);

            _columnsSearch = _validFields.Where(f => f.SearchType).ToList();
            var columnsToHide = _validFields.Where(f => f.Name == "NA").Select(b => new { name = b.Name }).ToList();

            int idParent = _settings.Where(s => s.Name == "FieldsToHide").Select(s => s.Id).FirstOrDefault();
            if (idParent > 0)
            {
                var result = General.GetChildrenSettings(DBAccess, idParent);
                var fieldToHide = result.Select(f => new { name = f.Value }).ToList();
                columnsToHide.AddRange(fieldToHide);
            }

            ViewBag.columnsSearch = JsonConvert.SerializeObject((_columnsSearch));
            ViewBag.columnsToHide = JsonConvert.SerializeObject((columnsToHide));
            ViewBag.version = new Random().Next();
            return View();
        }

        public IActionResult Search(DateTime startDate, DateTime endDate)
        {
            try
            {
                startDate = startDate.ToUniversalTime();
                endDate = endDate.ToUniversalTime();
                var result = SearchMessage(startDate, endDate);
                List<FilterViewModel> fields = new List<FilterViewModel>();

                foreach (var item in _columnsSearch)
                {
                    var data = result.AsEnumerable()
                        .Select(f => new { value = f.Field<string>(item.Name) }).ToList();

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

        public IActionResult GetLogs(string value)
        {
            try
            {
                var result = GetLogsMessageByIdHeader(value);

                var json = Json(
                    new
                    {
                        dataMessages = result
                    }

                );
                return json;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult BackupSearch()
        {
            _validFields = ValidFieldsDAL.GetListValidFields(DBAccess, "Messages");
            var columnsSearch = _validFields.Where(f => f.SearchType).ToList();
            var json = Json(
                    new
                    {
                        dataMessages = columnsSearch
                    }

                );
            return json;
        }
        public DataTable SearchMessage(DateTime startDate, DateTime endDate)
        {
            try
            {
                DBAccess = new General(_connectionString);
                DataTable dtResult = MessagesDAL.SearchMessages(DBAccess, startDate, endDate);


                //foreach (DataColumn item in dtResult.Columns)
                //{
                //    var customName = _validFields.Where(v => v.Name.Equals(item.ColumnName)).Select(v => v.CustomName).FirstOrDefault();
                //    if (!string.IsNullOrEmpty(customName))
                //    {

                //        item.ColumnName = customName.Replace(" ","");
                //    }
                //}

                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetLogsMessageByIdHeader(string value)
        {
            try
            {
                DBAccess = new General(_connectionString);
                DataTable dtResult = MessagesDAL.GetLogMessageByIdHeader(DBAccess, value);

                foreach (DataColumn item in dtResult.Columns)
                {
                    var customName = _validFields.Where(v => v.Name.Equals(item.ColumnName)).Select(v => v.CustomName).FirstOrDefault();
                    if (!string.IsNullOrEmpty(customName))
                    {
                        item.ColumnName = customName;
                    }
                }

                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}
