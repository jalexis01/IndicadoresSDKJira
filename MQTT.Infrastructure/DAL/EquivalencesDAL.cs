using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using MQTT.Infrastructure.Models.DTO;
using MQTT.Infrastructure.Models;

namespace MQTT.Infrastructure.DAL
{
    public class EquivalencesDAL
    {
        public static List<EquivalenceDTO> GetEquivalences(General objContext, int IdTypeEquivalence)
        {
            try
            {
                using (var DBContext = objContext.DBConnection())
                {
                    var result = DBContext.TbEquivalences
                        .Where(e => e.IdEquivalenceType == IdTypeEquivalence)
                        .Select(e => new EquivalenceDTO
                        {
                            Id = e.Id,
                            IdEquivalenceType = e.IdEquivalenceType,
                            Name = e.Name,
                            Value = e.Value,
                            CrerationDate = e.CrerationDate,
                            UserId = e.UserId,
                            LastUpdate = e.LastUpdate,
                            UserIdUpdate = e.UserIdUpdate
                        })
                        .ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<EquivalenceTypeDTO> GetEquivalenceTypes(General objContext)
        {
            try
            {
                using (var DBContext = objContext.DBConnection())
                {
                    var result = DBContext.TbEquivalenceTypes
                        .Where(e => e.Enable == true)
                        .Select(e => new EquivalenceTypeDTO
                        {
                            Id = e.Id,
                            Name = e.Name,
                            MenuController = e.MenuController, 
                            NameController = e.NameController,
                            TitleController = e.TitleController
                        })
                        .ToList();

                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void AddEquivalence(General objContext, EquivalenceDTO equivalenceData)
        {
            try
            {
                using (var DBContext = objContext.DBConnection())
                {
                    var result = DBContext.TbEquivalences
                        .Where(e => e.Name == equivalenceData.Name
                            && e.IdEquivalenceType == equivalenceData.IdEquivalenceType)
                        .FirstOrDefault();

                    if (result != null)
                    {
                        throw new Exception("Equivalencia existente con este mismo nombre.");
                    }

                    var tbEquivalence = new TbEquivalences
                    {
                        IdEquivalenceType = equivalenceData.IdEquivalenceType,
                        CrerationDate = DateTime.UtcNow,
                        UserId = 1, //TODO: Change when including user structure.
                        Name = equivalenceData.Name,
                        Value = equivalenceData.Value
                    };

                    DBContext.Add(tbEquivalence);
                    DBContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void UpdateEquivalence(General objContext, EquivalenceDTO equivalenceData)
        {
            try
            {
                using (var DBContext = objContext.DBConnection())
                {
                    var tbEquivalence = DBContext.TbEquivalences
                        .Where(e => e.Id == equivalenceData.Id)
                        .FirstOrDefault();

                    if (tbEquivalence == null)
                    {
                        throw new Exception("Equivalencia no existe.");
                    }

                    tbEquivalence.Name = equivalenceData.Name;
                    tbEquivalence.Value = equivalenceData.Value;
                    tbEquivalence.LastUpdate = DateTime.UtcNow;
                    tbEquivalence.UserIdUpdate = 1; //TODO: Change when including user structure.

                    DBContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
