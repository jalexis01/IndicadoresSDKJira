using MQTT.Infrastructure.Models;
using MQTT.Infrastructure.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MQTT.Infrastructure.DAL
{
    public class General
    {
        private string connectionString;

        public General(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public EYSIntegrationContext DBConnection()
        {
            return new EYSIntegrationContext(connectionString);
        }

        public static List<SettingDTO> GetSettings(General objContext, string name = null)
        {
            try
            {
                using (var DBContext = objContext.DBConnection())
                {
                    var result = (from data in DBContext.TbSettings
                                  where (name == null || data.Name == name)
                                  select new SettingDTO
                                  {
                                      Id = data.Id,
                                      IdParent = data.IdParent,
                                      Name = data.Name,
                                      Value = data.Value
                                  }).ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<SettingDTO> GetChildrenSettings(General objContext, int idParent)
        {
            try
            {
                using (var DBContext = objContext.DBConnection())
                {
                    var result = (from data in DBContext.TbSettings
                                  where data.IdParent == idParent
                                  select new SettingDTO
                                  {
                                      Id = data.Id,
                                      IdParent = data.IdParent,
                                      Name = data.Name,
                                      Value = data.Value
                                  }).ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetValueFromFields(string dataType, string value, string formatDate, bool simple = false)
        {
            try
            {
                string result = string.Empty;
                if (dataType.ToUpper().Contains("INT") || dataType.Contains("BIGINT") || dataType.Contains("FLOAT"))
                {
                    value = value.Replace(',', '.');
                    result += $"{value},";
                }
                else if (dataType.ToUpper().Contains("VARCHAR"))
                    result += $"'{value}',";
                else if (dataType.ToUpper().Contains("BIT"))
                    result += $"{(value.ToUpper().ToString() == "TRUE" ? 1 : 0)},";
                else if (dataType.ToUpper().Contains("DATETIME"))
                {
                    DateTime dt;
                    try
                    {
                        List<string> parts = GetPartsFromDate(value);
                        List<string> formatParts = GetPartsFromDate(formatDate);
                        int year = 0; int month = 0; int day = 0; int hour = 0; int minute = 0; int second = 0; int millisecond = 0;

                        for (int i = 0; i < formatParts.Count; i++)
                        {
                            string item = formatParts[i];
                            if (item.Contains("yyyy"))
                            {
                                year = Convert.ToInt16(parts[i]);
                            }
                            else if (item.Contains("MM"))
                            {
                                month = Convert.ToInt16(parts[i]);
                            }
                            else if (item.Contains("dd"))
                            {
                                day = Convert.ToInt16(parts[i]);
                            }
                            else if (item.Contains("HH"))
                            {
                                hour = Convert.ToInt16(parts[i]);
                            }
                            else if (item.Contains("mm"))
                            {
                                minute = Convert.ToInt16(parts[i]);
                            }
                            else if (item.Contains("ss"))
                            {
                                second = Convert.ToInt16(parts[i]);
                            }
                            else if (item.Contains("fff"))
                            {
                                millisecond = Convert.ToInt16(parts[i]);
                            }
                        }
                        dt = new DateTime(year, month, day, hour, minute, second, millisecond);
                    }
                    catch (Exception ex)
                    {
                        dt = new DateTime(1753, 1, 1, 0, 0, 0, 0);
                    }
                    if (simple)
                    {

                        result += $"{dt.ToString("yyyy/MM/dd HH:mm:ss.fff")}";
                    }
                    else
                    {

                    result += $"'{dt.ToString("yyyy/MM/dd HH:mm:ss.fff")}',";
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static List<string> GetPartsFromDate(string Value)
        {
            try
            {
                List<string> formatParts = Value.Split('/').ToList();
                List<string> yearPart = formatParts[2].Split(' ').ToList();
                formatParts.RemoveAt(2);
                List<string> timeParts = yearPart[1].Split(':').ToList();
                yearPart.RemoveAt(1);
                var ffParts = timeParts[2].Split('.');
                timeParts.RemoveAt(2);

                formatParts.AddRange(yearPart);
                formatParts.AddRange(timeParts);
                formatParts.AddRange(ffParts);

                return formatParts;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
