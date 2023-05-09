using MQTT.Infrastructure.Models;
using MQTT.Infrastructure.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MQTT.Infrastructure.DAL
{
    public class LogMessagesDAL
    {
        public static LogMessageDTO Add(General objContext, string message)
        {
            try
            {
                using (var dbContext = objContext.DBConnection())
                {
                    TbLogMessageIn tbLogMessageIn = new TbLogMessageIn
                    {
                        CreationDate = DateTime.UtcNow,
                        Message = message
                    };

                    dbContext.Add(tbLogMessageIn);
                    dbContext.SaveChanges();

                    LogMessageDTO logMessageDTO = new LogMessageDTO()
                    {
                        Id = tbLogMessageIn.Id
                    };
                    return logMessageDTO;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public static void Update(General objContext, LogMessageDTO logMessageDTO)
        //{
        //    try
        //    {
        //        using (var dbContext = objContext.DBConnection())
        //        {
        //            var log = (from logMsg in dbContext.TbLogMessageIn
        //                       where logMsg.Id == logMessageDTO.Id
        //                       select logMsg).FirstOrDefault();

        //            log.Observations = logMessageDTO.Observations;
        //            log.Idprocessed = logMessageDTO.IdProcessed;
        //            log.Processed = logMessageDTO.Processed;
        //            log.Ideventrecord = logMessageDTO.IdEventRecord;

        //            dbContext.SaveChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public static long GetIdLogMessageInByDay(General objContext, DateTime date)
        {
            try
            {
                using (var dbContext = objContext.DBConnection())
                {
                    var result = (from l in dbContext.TbLogMessageInSummaryDay
                                  where l.DateDay == date
                                  select l.IdLogMessageIn).FirstOrDefault();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<LogMessageDTO> GetLogMessageGreaterThanId(General objContext, long idLog)
        {
            try
            {
                using (var dbContext = objContext.DBConnection())
                {
                    var result = (from l in dbContext.TbLogMessageIn
                                  where l.Id > idLog
                                  select new LogMessageDTO { 
                                      Id = l.Id,
                                      DateIn = l.CreationDate,
                                      IdHeaderMessage = l.IdHeaderMessage,
                                      IdProcessed = l.IdProcessed, 
                                      Message = l.Message,
                                      Observations = l.Observations,
                                      Processed = l.Processed
                                  }).ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
		}
		public static List<LogMessageDTO> GetLogMessagePending(General objContext)
		{
			try
			{
				using (var dbContext = objContext.DBConnection())
				{
					var result = (from msg in dbContext.TbLogMessageIn
								  where !msg.Processed
								  select new LogMessageDTO()
								  {
									  Id = msg.Id,
									  Message = msg.Message
								  }).ToList();

					return result;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public static LogMessageDTO GetLogMessageById(General objContext, long id)
        {
            try
            {
                using (var dbContext = objContext.DBConnection())
                {
                    var result = (from msg in dbContext.TbLogMessageIn
                                  where id == msg.Id
                                  select new LogMessageDTO()
                                  {
                                      Id = msg.Id,
                                      Message = msg.Message
                                  }).FirstOrDefault();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Update(General objContext, LogMessageDTO logMessageDTO)
        {
            try
            {
                using (var DBContext = objContext.DBConnection())
                {
                    var log = (from msg in DBContext.TbLogMessageIn
                               where msg.Id == logMessageDTO.Id
                               select msg).FirstOrDefault();

                    log.DateProcessed = DateTime.UtcNow;
                    log.Processed = logMessageDTO.Processed;
                    log.Observations = logMessageDTO.Observations;
                    log.IdProcessed = logMessageDTO.IdProcessed;
                    log.IdHeaderMessage = logMessageDTO.IdHeaderMessage;

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
