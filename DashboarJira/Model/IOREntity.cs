using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace DashboarJira.Model
{
    public class IOREntity
    {
        //CANTIDAD DE EVENTOS DE INICIO DE OPERACION
        private int CEI { get; set; }
        //CANTIDAD DE EVENTOS FIN DE OPERACION
        private int CEF { get; set; }

        private List<Evento> CEIList { get; set; }

        private List<Evento> CEFist { get; set; }


        public double calcularIOR() { 
            double resultado = (double) CEI/ (double) CEF;
            return resultado;
        }
        public JsonObject convertToJson() {
            JsonObject msg = new JsonObject();
            msg.Add("CEI", CEI);
            msg.Add("CEF", CEF );
            //JsonArray ceiJsonArray = new JsonArray(CEIList);
            //msg.Add("CEI LIST",  );

            return msg;
        }

    }
}
