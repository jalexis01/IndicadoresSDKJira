using MQTT.Infrastructure.Models;
using MQTT.Infrastructure.Models.DTO;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace MQTT.Infrastructure.DAL
{
    public class ElementsDAL
    {
        public static void AddElement(General objContext, ElementDTO dataElement)
        {
            try
            {
                using (var DBContext = objContext.DBConnection())
                {
                    TbElements tbElement = new TbElements
                    {
                        IdElementType = dataElement.IdElementType,
                        Name = dataElement.Name,
                        Value = dataElement.Value,
                        IdElementFather = dataElement.IdElementFather,
                        CreationUser = dataElement.CreationUser,
                        Enable = dataElement.Enable,
                        CreationDate = DateTime.UtcNow
                    };

                    DBContext.Add(tbElement);
                    DBContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void UpdateElement(General objContext, ElementDTO dataElement)
        {
            try
            {
                using (var DBContext = objContext.DBConnection())
                {
                    var dbElement = DBContext.TbElements
                        .Where(e => e.Id == dataElement.Id)
                        .FirstOrDefault();

                    if (dbElement == null)
                    {
                        //TODO: return message.
                    }

                    dbElement.Name = dataElement.Name;
                    dbElement.Value = dataElement.Value;
                    dbElement.LastUpdate = DateTime.UtcNow;
                    dbElement.Enable = dataElement.Enable;
                    dbElement.IdElementFather = dataElement.IdElementFather;
                    dbElement.UpdateUser = dataElement.UpdateUser;

                    DBContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void AddLogElement(General objContext, LogElementDTO dataLogElement)
        {
            try
            {
                using (var DBContext = objContext.DBConnection())
                {
                    TbLogElements tbLogElements = new TbLogElements
                    {
                        IdElement = dataLogElement.IdElement,
                        OldName = dataLogElement.OldName,
                        NewName = dataLogElement.NewName,
                        OldFatherId = dataLogElement.OldFatherId,
                        NewFatherId = dataLogElement.NewFatherId,
                        OldValue = dataLogElement.OldValue,
                        NewValue = dataLogElement.NewValue,
                        Enable = dataLogElement.Enable,
                        CreationDate = DateTime.UtcNow,
                        CreationUser = dataLogElement.CreationUser
                    };

                    DBContext.Add(tbLogElements);
                    DBContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ElementDTO> GetElements(General objContext)
        {
            try
            {
                using (var DBContext = objContext.DBConnection())
                {
                    var result = DBContext.TbElements
                        .Select(e => new ElementDTO
                        {
                            Id = e.Id,
                            IdElementFather = e.IdElementFather,
                            Name = e.Name,
                            Value = e.Value,
                            SubElements = new List<ElementDTO>(),
                            CreationUser = e.CreationUser,
                            CreationDate = DateTime.UtcNow,
                            Enable = e.Enable.Value
                        }).ToList();

                    List<ElementDTO> lstElements = new List<ElementDTO>();
                    foreach (var item in result)
                    {
                        if (item.IdElementFather == null)
                        {
                            lstElements.Add(item);
                        }
                        else
                        {
                            if (result.Where(z => z.Id == item.IdElementFather).Any())
                            {
                                lstElements.Add(item);
                            }
                        }
                    }

                    result = lstElements
                    .Select(e => new ElementDTO()
                    {
                        Id = e.Id,
                        IdElementFather = e.IdElementFather,
                        Name = e.Name,
                        Value = e.Value,
                        SubElements = GetSubElements(lstElements, e.Id),
                        CreationUser = e.CreationUser,
                        CreationDate = e.CreationDate
                    }).Where(e => e.IdElementFather is null).OrderBy(e => e.CreationDate).ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ElementDTO> GetSubElements(List<ElementDTO> lstElements, long idElementFather)
        {
            try
            {
                List<ElementDTO> result = new List<ElementDTO>();
                result = lstElements
                    .Where(z => z.IdElementFather == idElementFather)
                    .Select(e => new ElementDTO()
                    {
                        Id = e.Id,
                        IdElementFather = e.IdElementFather,
                        Name = e.Name,
                        Value = e.Value,
                        SubElements = GetSubElements(lstElements, e.Id),
                        CreationUser = e.CreationUser,
                        CreationDate = e.CreationDate
                    }).OrderBy(e => e.CreationDate).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
