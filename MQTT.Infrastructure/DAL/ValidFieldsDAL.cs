using MQTT.Infrastructure.Models.DTO;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using MQTT.Infrastructure.Models;

namespace MQTT.Infrastructure.DAL
{
    public class ValidFieldsDAL
    {
        public static void AddValidField(General objContext, ValidFieldDTO validFieldDTO)
        {
            try
            {
                using (var DBContext = objContext.DBConnection())
                {
                    var result = (from field in DBContext.TbValidFields
                                  where field.Name.Equals(validFieldDTO.Name)
                                  select field).FirstOrDefault();

                    if (result == null)
                    {
                        TbValidFields tbValidFields = new TbValidFields()
                        {
                            Name = validFieldDTO.Name,
                            CreationDate = validFieldDTO.CreationDate,
                        };

                        DBContext.Add(tbValidFields);
                        DBContext.SaveChanges();

                        validFieldDTO.Id = tbValidFields.Id;
                    }
                    else
                    {
                        validFieldDTO.Id = result.Id;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void AddListValidField(General objContext, List<ValidFieldDTO> lstValidFields)
        {
            try
            {
                using (var DBContext = objContext.DBConnection())
                {
                    var result = (from valid in lstValidFields
                                  join field in DBContext.TbValidFields
                                    on valid.Name equals field.Name into validfield
                                  from vf in validfield.DefaultIfEmpty()
                                  select new { valid, vf }).ToList();

                    if (result != null)
                    {
                        foreach (var field in result)
                        {
                            if (field.vf == null)
                            {
                                var fieldAdd = field.valid;

                                TbValidFields tbValidFields = new TbValidFields()
                                {
                                    Name = fieldAdd.Name,
                                    CreationDate = fieldAdd.CreationDate,
                                };
                                DBContext.Add(tbValidFields);
                                DBContext.SaveChanges();
                                fieldAdd.Id = tbValidFields.Id;
                            }
                            else
                            {
                                field.valid.Id = field.vf.Id;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<MessageTypeFieldDTO> GetListValidFields(General objContext, string tableName = null)
        {
            try
            {
                using (var DBContext = objContext.DBConnection())
                {
                    var result = (from vf in DBContext.TbValidFields
                                  join mf in DBContext.TbMessageTypeFields
                                    on vf.Id equals mf.IdValidField
                                  join mt in DBContext.TbMessageTypes
                                    on mf.IdMessageType equals mt.Id
                                  where (string.IsNullOrEmpty(tableName) || mt.TableName == tableName)
                                  select new MessageTypeFieldDTO
                                  {
                                      IdValidField = vf.Id,
                                      CreationDate = vf.CreationDate,
                                      DataType = vf.DataType,
                                      Description = vf.Description,
                                      Name = vf.Name,
                                      UpdateDate = vf.UpdateDate,
                                      SearchType = vf.SearchType,
                                      CustomName = mf.CustomName,
                                      PrimaryType = vf.PrimaryData.Value
                                  }).Distinct().ToList();

                    var headers = (from vf in DBContext.TbHeaderFields
                                   select new MessageTypeFieldDTO
                                   {
                                       IdValidField = vf.Id,
                                       CreationDate = vf.CreationDate,
                                       DataType = vf.DataType,
                                       Description = vf.Description,
                                       Name = vf.Name,
                                       UpdateDate = vf.UpdateDate,
                                       SearchType = vf.SearchType,
                                       CustomName = vf.CustomName,
                                       PrimaryType = vf.PrimaryData
                                   }).Distinct().ToList();

                    result.AddRange(headers);

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<ValidFieldDTO> GetAllValidFields(General objContext)
        {
            try
            {
                using (var DBContext = objContext.DBConnection())
                {
                    var result = (from vf in DBContext.TbValidFields
                                  select new ValidFieldDTO
                                  {
                                      CreationDate = vf.CreationDate,
                                      DataType = vf.DataType,
                                      Description = vf.Description,
                                      Name = vf.Name,
                                      UpdateDate = vf.UpdateDate,
                                      SearchType = vf.SearchType,
                                      PrimaryType = vf.PrimaryData.Value
                                  }).Distinct().ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}