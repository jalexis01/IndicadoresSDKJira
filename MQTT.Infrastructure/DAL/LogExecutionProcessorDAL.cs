using MQTT.Infrastructure.Models;
using MQTT.Infrastructure.Models.DTO;
using System;

namespace MQTT.Infrastructure.DAL
{
    public class LogExecutionProcessorDAL
    {
        public static void Add(General objContext, LogExecutionProcessorDTO logExecutionProcessor)
        {
            try
            {
                using (var DBContext = objContext.DBConnection())
                {
                    TbLogExecutionProcessor tbLogExecutionProcessor = new TbLogExecutionProcessor
                    {
                        CreationDate = DateTime.UtcNow,
                        IdLogMessageInInit = logExecutionProcessor.IdLogMessageInInit,
                        InitDate = logExecutionProcessor.Init,
                        IdLogMessageInEnd = logExecutionProcessor.IdLogMessageInEnd,
                        EndDate = logExecutionProcessor.End,
                        Observations = logExecutionProcessor.Observation
                    };

                    DBContext.Add(tbLogExecutionProcessor);
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
