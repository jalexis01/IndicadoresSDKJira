using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MQTT.Infrastructure.DAL;
using MQTT.Infrastructure.Models;
using MQTT.Infrastructure.Models.DTO;
using Newtonsoft.Json;

namespace MQTT.Web.Controllers
{
    [Authorize]
    public class MessageTypesController : Controller
    {
        private readonly string _connectionString = AppSettings.Instance.Configuration["connectionString"].ToString();
        private General _objGeneral;
        private General DBAccess { get => _objGeneral; set => _objGeneral = value; }

        public MessageTypesController()
        {
            DBAccess = new General(_connectionString);
        }

        // GET: MessageTypes
        public async Task<IActionResult> Index()
        {
            var lstMessageTypes = MessagesDAL.GetAllMessageTypes(DBAccess);
            ViewBag.lstMessageTypes = JsonConvert.SerializeObject((lstMessageTypes));
            return View();
        }

        public IActionResult Creation()
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            var model =MessagesDAL.GetMessageType(DBAccess, id);
            var allFields = ValidFieldsDAL.GetAllValidFields(DBAccess);
            var selectedFields = MessagesDAL.GetValidFieldsByMessageTypeId(DBAccess, id);
            var unselectedFields = allFields.Where(f => !selectedFields.Contains(f)).ToList();
            Models.MessageTypeViewModel messageTypeFieldViewModel = new Models.MessageTypeViewModel
            {
                Id = model.Id,
                Code = model.Code,
                Description = model.Description,
                FieldCode = model.FieldCode,
                FieldIdentifierMessage = model.FieldIdentifierMessage,
                FieldWeft = model.FieldWeft,
                Name = model.Name,
                TableName = model.TableName
            };

            ViewBag.unselectedFields = unselectedFields;
            ViewBag.selectedFields = selectedFields;

            return View(messageTypeFieldViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Creation(Models.MessageTypeViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MessageTypeDTO messageTypeDTO = new MessageTypeDTO { 
                        Code = model.Code,
                        Description = model.Description,
                        FieldCode = model.FieldCode,
                        FieldIdentifierMessage = model.FieldIdentifierMessage,
                        FieldWeft = model.FieldWeft,
                        Name = model.Name,
                        TableName = model.TableName
                    };
                    MessagesDAL.AddMessageType(DBAccess, messageTypeDTO);
                    model.Id = messageTypeDTO.Id;
                }
                return RedirectToAction("Edit", new { id = model.Id});

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int Id, Models.MessageTypeViewModel model)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private void GetUnSelectedValidFields(int id)
        {
            try
            {
                var allFields = ValidFieldsDAL.GetAllValidFields(DBAccess);
                var selectedFields = MessagesDAL.GetValidFieldsByMessageTypeId(DBAccess, id);

                var unselectedFields = allFields.Except(selectedFields);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
