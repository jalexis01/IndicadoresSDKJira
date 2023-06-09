<<<<<<< Updated upstream
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
=======
﻿using DashboarJira.Model;
using System.Data.Common;
>>>>>>> Stashed changes

namespace DashboarJira.Controller
{
    internal class IORController
    {
        private DbConnection _connection;

        public IORController(DbConnection _connection) 
        { 
            this._connection = _connection;
        }
        public IOREntity Calcular(string startDate, string endDate)  
        {
            IOREntity result = new IOREntity();
            result.CEIList = new List<Evento>();
            result.CEFList = new List<Evento>();
            return result;
        }
    }
}
