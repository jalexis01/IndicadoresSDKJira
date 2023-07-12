using MQTT.Infrastructure.Models;
using System;
using System.Linq;

namespace MQTT.Infrastructure.DAL
{
    public class LogExecutionDAL
    {
        public static long Add(General objContext)
        {
            try
            {
                using (var dbContext = objContext.DBConnection())
                {
                    TbLogExecutions tbLogExecutions = new TbLogExecutions
                    {
                        InitDateTime = DateTime.UtcNow
                    };

                    dbContext.Add(tbLogExecutions);
                    dbContext.SaveChanges();

                    return tbLogExecutions.Id;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void Update(General objContext, long id, string observations)
        {
            try
            {
                using (var dbContext = objContext.DBConnection())
                {
                    var obLog = (from l in dbContext.TbLogExecutions
                                 where l.Id == id
                                 select l).FirstOrDefault();

                    obLog.EndDateTime = DateTime.UtcNow;
                    obLog.Observations = observations;

                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
