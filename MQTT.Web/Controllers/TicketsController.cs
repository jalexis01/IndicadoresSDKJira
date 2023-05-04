using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using MQTT.Infrastructure.DAL;
using MQTT.Infrastructure.Models.DTO;
using MQTT.Web.Models;
using System.Data;
using System.Linq;

using DashboarJira.Model;
using DashboarJira.Services;

namespace MQTT.Web.Controllers
{
    public class TicketsController : Controller
    {
        private readonly string _connectionString = AppSettings.Instance.Configuration["connectionString"].ToString();
        private General _objGeneral;
        private static List<MessageTypeFieldDTO> _validFields;
        private static List<MessageTypeFieldDTO> _columnsSearch;
        private General DBAccess { get => _objGeneral; set => _objGeneral = value; }

        public IActionResult Index()
        {
            //return View();

            List<Ticket> tickets = getTickets(startDate, endDate);
            return View(tickets);
        }



        int start = 0;
        int max = 10;
        string startDate = "2023-05-12";
        string endDate = "2023-05-25";
        string idComponente = null;

        private void mockMethod()
        {

        }
        public List<Ticket> getTickets(string startDate, string endDate)
        {
            //TODO Write some test
            try
            {
                JiraAccess jiraAccess = new JiraAccess();
                return jiraAccess.GetTikets(start, max, startDate, endDate, idComponente);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

   
    /*{
        [Authorize]
        public IActionResult Index()
        {
            //return View();
            List<Ticket> tickets = getTickets();
            return View(tickets);
        }

        int start = 0;
        int max = 100;
        string startDate = null;
        string endDate = null;
        string idComponente = null;
        public List<Ticket> getTickets()
        {
            try
            {
                JiraAccess jiraAccess = new JiraAccess();
                return jiraAccess.GetTikets(start, max, startDate, endDate, idComponente);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }*/
}