using MQTT.Infrastructure.DAL;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQTT.BL
{
    public class SubscriberBL
    {
        readonly General _dbAccess;

        public SubscriberBL(General dbAccess)
        {
            _dbAccess = dbAccess;
        }

    }
}
