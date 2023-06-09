using System;
using System.Linq;

namespace MQTT.Infrastructure.DAL
{
    public class ParametersDAL
    {

        public static string GetValue(General objContext, string parameterName)
        {
            try
            {
                using (var dbContext = objContext.DBConnection())
                {
                    var val = (from param in dbContext.TbParameters
                               where param.Name.ToUpper().Equals(parameterName.ToUpper())
                               select param.Value).FirstOrDefault();

                    return val;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
