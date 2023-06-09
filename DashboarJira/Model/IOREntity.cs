using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboarJira.Model
{
    internal class IOREntity
    {
<<<<<<< Updated upstream
=======
        

        //CANTIDAD DE EVENTOS DE INICIO DE OPERACION
        public List<Evento> CEIList { get; set; }
        //CANTIDAD DE EVENTOS FIN DE OPERACION
        public List<Evento> CEFList { get; set; }


        public double calcularIOR() { 
            double resultado = (double)CEIList.Count / (double) CEFList.Count;
            return resultado;
        }
        public JsonObject convertToJson() {
            JsonObject msg = new JsonObject();
            msg.Add("CEI", CEIList.Count);
            msg.Add("CEF", CEFList.Count);
            //JsonArray ceiJsonArray = new JsonArray(CEIList);
            //msg.Add("CEI LIST",  );

            return msg;
        }

>>>>>>> Stashed changes
    }
}
