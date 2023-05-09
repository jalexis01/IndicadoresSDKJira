using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MQTT.Infrastructure.DAL;
using MQTT.Infrastructure.Models.DTO;
using Newtonsoft.Json;

namespace MQTT.Web.Controllers
{
    public class ElementsController : Controller
    {
        private readonly string _connectionString = AppSettings.Instance.Configuration["connectionString"].ToString();
        private General _objGeneral;
        private General DBAccess { get => _objGeneral; set => _objGeneral = value; }

        public ElementsController()
        {
            DBAccess = new General(_connectionString);
        }
        public IActionResult Index()
        {
            ViewBag.ElementTypes = JsonConvert.SerializeObject(ElementsDAL.GetElementTypes(DBAccess));
			return View();
        }
        public IActionResult GetElements()
        {
            try
            {
                var dataElements = ElementsDAL.GetElements(DBAccess);

                var json = Json(
                    new
                    {
                        data = dataElements
                    });

                return json;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult AddElement(ElementDTO elementDTO)
        {
            try
            {
                ElementsDAL.AddElement(DBAccess, elementDTO);
                return GetElements();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetDataElementViewModel(List<ElementDTO> dataElements, List<ElementTypeDTO> dataElementTypes) 
        {
            try
            {

				foreach (var item in dataElements)
				{

				}
			}
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult UpdateElement(ElementDTO newElementDTO, ElementDTO oldElementDTO)
        {
            try
            {
                LogElementDTO logElementDTO = new LogElementDTO
                {
                    IdElement = newElementDTO.Id,
                    OldName = oldElementDTO.Name,
                    NewName = newElementDTO.Name,
                    OldValue = oldElementDTO.Value,
                    NewValue = newElementDTO.Value,
                    OldFatherId = oldElementDTO.IdElementFather,
                    NewFatherId = newElementDTO.IdElementFather,
                    Enable = newElementDTO.Enable
                };
                ElementsDAL.AddLogElement(DBAccess, logElementDTO);
                ElementsDAL.UpdateElement(DBAccess, newElementDTO);
                return GetElements();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
