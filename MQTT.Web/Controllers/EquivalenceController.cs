using System;
using Microsoft.AspNetCore.Mvc;
using MQTT.Infrastructure.DAL;
using MQTT.Infrastructure.Models.DTO;

namespace MQTT.Web.Controllers
{
    public class EquivalenceController : Controller
    {
        private readonly string _connectionString = AppSettings.Instance.Configuration["connectionString"].ToString();
        private General _objGeneral;
        private General DBAccess { get => _objGeneral; set => _objGeneral = value; }

        public EquivalenceController()
        {
            DBAccess = new General(_connectionString);
        }

        public IActionResult Index(int id, int title)
        {
            ViewBag.Title = title;
            ViewBag.IdEquivalenteType = id;
            return View();
        }

        public IActionResult GetEquivalences(int idEquivalenceType)
        {
            try
            {
                var json = Json(
                    new
                    {
                        data = EquivalencesDAL.GetEquivalences(DBAccess, idEquivalenceType)
                    });

                return json;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult AddEquivalence(EquivalenceDTO equivalenceData)
        {
            try
            {
                EquivalencesDAL.AddEquivalence(DBAccess, equivalenceData);
                var json = Json(
                    new
                    {
                        data = EquivalencesDAL.GetEquivalences(DBAccess, equivalenceData.IdEquivalenceType)
                    });

                return json;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateEquivalence(EquivalenceDTO equivalenceData)
        {
            try
            {
                EquivalencesDAL.UpdateEquivalence(DBAccess, equivalenceData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
