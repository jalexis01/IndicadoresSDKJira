using MQTT.Infrastructure.DAL;
using System;
using System.Linq;

namespace MQTT.AdditionalProcess.BL
{
    public class AdditionalProcessBL
    {
        private readonly string _connectionString = AppSettings.Instance.Configuration["connectionString"].ToString();
        private General _objGeneral;
        private General DBAccess { get => _objGeneral; set => _objGeneral = value; }

        public AdditionalProcessBL()
        {
            DBAccess = new General(_connectionString);
        }
        public void Process()
        {
            try
            {
                DateTime lastDate = DateTime.UtcNow.Date.AddDays(-1);
                var idLogMessageIn = GetIdLogMessageInSummary(lastDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private long? GetIdLogMessageInSummary(DateTime date)
        {
            try
            {
                return LogMessagesDAL.GetIdLogMessageInByDay(DBAccess, date);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetLastIdLogMessageIn(long id)
        {
            try
            {
                var lstLogs = LogMessagesDAL.GetLogMessageGreaterThanId(DBAccess, id);

                var x = lstLogs
                    .GroupBy(l => l.DateIn.Date)
                    .Select(l => new
                    {
                        idLog = l.Max(d => d.Id),
                        date = l.Key
                    });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
