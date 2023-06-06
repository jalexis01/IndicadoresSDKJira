using MQTT.Infrastructure.DAL;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MQTT.Infrastructure.Models;

namespace DashboarJira.Services
{
    public class IndicadoresDesdePI
    {
        private readonly DbContext dbContext;

        public IndicadoresDesdePI(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public TbMessages SearchMessages(DateTime dtInit, DateTime dtEnd)
        {
            try
            {
                string formattedDtInit = dtInit.ToString("yyyy-MM-dd HH:mm:ss");
                string formattedDtEnd = dtEnd.ToString("yyyy-MM-dd HH:mm:ss");

                var message = dbContext.TbMessages
                    .Include(m => m.IdHeaderMessageNavigation)
                    .FirstOrDefault(m => m.FechaHoraLecturaDato >= dtInit && m.FechaHoraLecturaDato <= dtEnd);

                return message;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
