using MQTT.Infrastructure.Models;
using MQTT.Infrastructure.Models.DTO;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace MQTT.Infrastructure.DAL
{
    public class LogRequestInDAL
    {
        public static void Add(General objContext,ref LogRequestInDTO logRequestIn)
        {
            try
            {
                using (var DBContext = objContext.DBConnection())
                {
                    TbLogRequestsIn tbLogRequestsIn = new TbLogRequestsIn
                    {
                        CreationDate = DateTime.Now,
                        DataBody = logRequestIn.DataBody,
                        DataQuery = logRequestIn.DataQuery,
                        IdEndPoint = logRequestIn.IdEndPoint,
                        Observations = logRequestIn.Observations,
                        Processed = logRequestIn.Processed
                    };

                    DBContext.Add(tbLogRequestsIn);
                    DBContext.SaveChanges();

                    logRequestIn.Id= tbLogRequestsIn.Id;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void Update(General objContext, LogRequestInDTO logRequestIn)
        {
            try
            {
                using (var DBContext = objContext.DBConnection())
                {
                    var result = DBContext.TbLogRequestsIn.Where(r => r.Id == logRequestIn.Id).First();

                    result.DataBody = logRequestIn.DataBody;
                    result.DataQuery = logRequestIn.DataQuery;
                    result.IdEndPoint = logRequestIn.IdEndPoint;
                    result.Observations = logRequestIn.Observations;
                    result.Processed = logRequestIn.Processed;

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
