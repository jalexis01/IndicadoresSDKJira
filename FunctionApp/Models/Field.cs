using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MQTT.FunctionApp.Models
{
    public class Field
    {
        [JsonPropertyName("customfield_10052")]
        public Category? Estacion { get; set; }

        [JsonPropertyName("customfield_10073")]
        public Category? Vagon { get; set; }

        [JsonPropertyName("customfield_10057")]
        public string? IdentificacionComponente { get; set; }

        [JsonPropertyName("customfield_10070")]
        public Category? TipoDeComponente { get; set; }

        [JsonPropertyName("customfield_10058")]
        public string? IdentificacionSerial { get; set; }

        [JsonPropertyName("customfield_10072")]
        public Category? TipoDeServicio { get; set; }

        [JsonPropertyName("customfield_10046")]
        public Category? ClaseDeFallo { get; set; }

        [JsonPropertyName("customfield_10048")]
        public List<Category>? DescripcionDeFallo { get; set; }

        [JsonPropertyName("created")]
        public DateTime? created { get; set; }

        [JsonPropertyName("statuscategorychangedate")]
        public DateTime? statuscategorychangedate { get; set; }

        [JsonPropertyName("customfield_10056")]
        public DateTime? FechayHoraDeLlegadaAEstacion { get; set; }

        [JsonPropertyName("customfield_10050")]
        public List<Category>? DescripcionRepuesto { get; set; }

        [JsonPropertyName("customfield_10071")]
        public List<Category>? TipoReparacion    { get; set; }

        [JsonPropertyName("customfield_10060")]
        public List<Category>? ListadoAjustesPuerta { get; set; }

        [JsonPropertyName("customfield_10063")]
        public List<Category>? ListadoConfiguracionPuerta { get; set; }

        [JsonPropertyName("customfield_10059")]
        public List<Category>? ListadoAjustesITS { get; set; }

        [JsonPropertyName("customfield_10062")]
        public List<Category>? ListadoConfiguracionITS { get; set; }

        [JsonPropertyName("customfield_10064")]
        public List<Category>? ListadoConfiguracionRFID { get; set; }

       /* [JsonPropertyName("customfield_10061")] //Se supone que funciona con Manatee
        public List<Category>? ListadoAjusteRFID { get; set; }

        */

        [JsonPropertyName("customfield_10061")] //FUNCIONA SOLO CON ASSAABLOY
        public List<Category>? ListadoAjusteRFID { get; set; }

        [JsonPropertyName("customfield_10087")]
        public string? DescripcionReparacion { get; set; }

        [JsonPropertyName("customfield_10088")]
        public string? DiagnosticoCausa { get; set; }

        [JsonPropertyName("customfield_10051")]
        public Category? TipoCausa { get; set; }

        [JsonPropertyName("customfield_10055")]
        public DateTime? FechaSolucion { get; set; }

        [JsonPropertyName("Status")]
        public Status? status { get; set; }
        public static Field ConverJsonInField(JObject fieldObject)
        {
            Field field = new Field();

            if (fieldObject.TryGetValue(Constantes.Estacion, out JToken estacion) && estacion.Type != JTokenType.Null)
            {
                Category Estacion = new Category();
                Estacion.Value = fieldObject[Constantes.Estacion]["value"].Value<string>();
                field.Estacion = Estacion;
            }
            else
            {
                field.Estacion = null;
            }

            if (fieldObject.TryGetValue(Constantes.Vagon, out JToken vagon) && vagon.Type != JTokenType.Null)
            {
                Category Vagon = new Category();
                Vagon.Value = fieldObject[Constantes.Vagon]["value"].Value<string>();
                field.Vagon = Vagon;
            }
            else
            {
                field.Vagon = null;
            }

            if (fieldObject.TryGetValue(Constantes.IdentificacionComponente, out JToken IdentificacionComponente) && IdentificacionComponente.Type != JTokenType.Null)
            {
                field.IdentificacionComponente = fieldObject[Constantes.IdentificacionComponente].Value<string>();
            }
            else
            {
                field.IdentificacionComponente = null;
            }


            //Category tipoDeComponente = fieldObject["customfield_10070"].Value<Category>();
            if (fieldObject.TryGetValue(Constantes.TipoDeComponente, out JToken TipoDeComponente) && TipoDeComponente.Type != JTokenType.Null)
            {
                Category tipoDeComponente = new Category();
                tipoDeComponente.Value = fieldObject[Constantes.TipoDeComponente]["value"].Value<string>();
                field.TipoDeComponente = tipoDeComponente;
            }
            else
            {
                field.TipoDeComponente = null;
            }

            if (fieldObject.TryGetValue(Constantes.IdentificacionSerial, out JToken IdentificacionSerial) && IdentificacionSerial.Type != JTokenType.Null)
            {
                field.IdentificacionSerial = fieldObject[Constantes.IdentificacionSerial].Value<string>();
            }
            else
            {
                field.IdentificacionSerial = null;
            }

            if (fieldObject.TryGetValue(Constantes.TipoDeServicio, out JToken TipoDeServicio) && TipoDeServicio.Type != JTokenType.Null)
            {
                Category tipoDeServicio = new Category();
                tipoDeServicio.Value = fieldObject[Constantes.TipoDeServicio]["value"].Value<string>();
                field.TipoDeServicio = tipoDeServicio;
            }
            else
            {
                field.TipoDeServicio = null;
            }

            if (fieldObject.TryGetValue(Constantes.ClaseDeFallo, out JToken ClaseDeFallo) && ClaseDeFallo.Type != JTokenType.Null)
            {
                Category claseDeFallo = new Category();
                claseDeFallo.Value = fieldObject[Constantes.ClaseDeFallo]["value"].Value<string>();
                field.ClaseDeFallo = claseDeFallo;
            }
            else
            {
                field.ClaseDeFallo = null;
            }

            if (fieldObject.TryGetValue(Constantes.DescripcionDeFallo, out JToken DescripcionDeFallo) && DescripcionDeFallo.Type != JTokenType.Null)
            {
                var descripcionDeFalloArray = fieldObject[Constantes.DescripcionDeFallo].Value<JArray>();
                List<Category> descripcionDeFallo = new List<Category>();
                foreach (JObject dataItem in descripcionDeFalloArray)
                {
                    Category temp = new Category();
                    temp.Value = dataItem["value"].Value<string>();
                    descripcionDeFallo.Add(temp);
                }
                field.DescripcionDeFallo = descripcionDeFallo;
            }
            else
            {
                field.DescripcionDeFallo = null;
            }

            if (fieldObject.TryGetValue(Constantes.Created, out JToken Created) && Created.Type != JTokenType.Null)
            {
                DateTime created = fieldObject[Constantes.Created].Value<DateTime>();
                field.created = created;
            }
            else
            {
                field.created = null;
            }

            if (fieldObject[Constantes.StatusCategoryChangeDate].Value<DateTime>() != null)
            {
                DateTime statuscategorychangedate = fieldObject[Constantes.StatusCategoryChangeDate].Value<DateTime>();
                field.statuscategorychangedate = statuscategorychangedate;
            }
            else
            {
                field.statuscategorychangedate = null;
            }
            if (fieldObject.TryGetValue(Constantes.FechayHoraDeLlegadaAEstacion, out JToken Fechay_Hora_De_Llegada_Estacion) && Fechay_Hora_De_Llegada_Estacion.Type != JTokenType.Null)
            {
                DateTime FechayHoraDeLlegadaAEstacion = fieldObject[Constantes.FechayHoraDeLlegadaAEstacion].Value<DateTime>();
                field.FechayHoraDeLlegadaAEstacion = FechayHoraDeLlegadaAEstacion;
            }
            else
            {
                field.FechayHoraDeLlegadaAEstacion = null;
            }

            if (fieldObject.TryGetValue(Constantes.DescripcionRepuesto, out JToken DescripcionRepuesto) && DescripcionRepuesto.Type != JTokenType.Null)
            {
                var descripcionRepuestoArray = fieldObject[Constantes.DescripcionRepuesto].Value<JArray>();
                List<Category> descripcionRepuesto = new List<Category>();
                foreach (JObject dataItem in descripcionRepuestoArray)
                {
                    Category temp = new Category();
                    temp.Value = dataItem["value"].Value<string>();
                    descripcionRepuesto.Add(temp);
                }
                field.DescripcionRepuesto = descripcionRepuesto;
            }
            else
            {
                field.DescripcionRepuesto = null;
            }

            if (fieldObject.TryGetValue(Constantes.TipoReparacion, out JToken tipoReparacion) && tipoReparacion.Type != JTokenType.Null)
            {
                var TipoReparacionArray = fieldObject[Constantes.TipoReparacion].Value<JArray>();
                List<Category> TipoReparacion = new List<Category>();
                foreach (JObject dataItem in TipoReparacionArray)
                {
                    Category temp = new Category();
                    temp.Value = dataItem["value"].Value<string>();
                    TipoReparacion.Add(temp);
                }
                field.TipoReparacion = TipoReparacion;
            }
            else
            {
                field.TipoReparacion = null;
            }

            if (fieldObject.TryGetValue(Constantes.ListadoAjustesPuerta, out JToken listadoAjustePuerta) && listadoAjustePuerta.Type != JTokenType.Null)
            {
                var ListadoAjustesPuertaArray = fieldObject[Constantes.ListadoAjustesPuerta].Value<JArray>();
                List<Category> ListadoAjustesPuerta = new List<Category>();
                foreach (JObject dataItem in ListadoAjustesPuertaArray)
                {
                    Category temp = new Category();
                    temp.Value = dataItem["value"].Value<string>();
                    ListadoAjustesPuerta.Add(temp);
                }
                field.ListadoAjustesPuerta = ListadoAjustesPuerta;
            }
            else
            {
                field.ListadoConfiguracionPuerta = null;
            }

            if (fieldObject.TryGetValue(Constantes.ListadoConfiguracionPuerta, out JToken listadoConfiguracionPuerta) && listadoConfiguracionPuerta.Type != JTokenType.Null)
            {
                var ListadoConfiguracionPuertaArray = fieldObject[Constantes.ListadoConfiguracionPuerta].Value<JArray>();
                List<Category> ListadoConfiguracionPuerta = new List<Category>();
                foreach (JObject dataItem in ListadoConfiguracionPuertaArray)
                {
                    Category temp = new Category();
                    temp.Value = dataItem["value"].Value<string>();
                    ListadoConfiguracionPuerta.Add(temp);
                }
                field.ListadoConfiguracionPuerta = ListadoConfiguracionPuerta;
            }
            else
            {
                field.ListadoConfiguracionPuerta = null;
            }

            if (fieldObject.TryGetValue(Constantes.ListadoAjustesITS, out JToken listadoAjusteIts) && listadoAjusteIts.Type != JTokenType.Null)
            {
                var ListadoAjustesITSArray = fieldObject[Constantes.ListadoAjustesITS].Value<JArray>();
                List<Category> ListadoAjustesITS = new List<Category>();
                foreach (JObject dataItem in ListadoAjustesITSArray)
                {
                    Category temp = new Category();
                    temp.Value = dataItem["value"].Value<string>();
                    ListadoAjustesITS.Add(temp);
                }
                field.ListadoAjustesITS = ListadoAjustesITS;
            }
            else
            {
                field.ListadoAjustesITS = null;
            }

            if (fieldObject.TryGetValue(Constantes.ListadoConfiguracionITS, out JToken listadoConfiguracionITS) && listadoConfiguracionITS.Type != JTokenType.Null)
            {
                var ListadoConfiguracionITSArray = fieldObject[Constantes.ListadoConfiguracionITS].Value<JArray>();
                List<Category> ListadoConfiguracionITS = new List<Category>();
                foreach (JObject dataItem in ListadoConfiguracionITSArray)
                {
                    Category temp = new Category();
                    temp.Value = dataItem["value"].Value<string>();
                    ListadoConfiguracionITS.Add(temp);
                }
                field.ListadoConfiguracionITS = ListadoConfiguracionITS;
            }
            else
            {
                field.ListadoConfiguracionITS = null;
            }
            if (fieldObject.TryGetValue(Constantes.ListadoAjusteRFID, out JToken listadoAjusteRfid) && listadoAjusteRfid.Type != JTokenType.Null)
            {
                var ListadoAjustesRFIDArray = fieldObject[Constantes.ListadoAjusteRFID].Value<JArray>();
                List<Category> ListadoAjustesRFID = new List<Category>();
                foreach (JObject dataItem in ListadoAjustesRFIDArray)
                {
                    Category temp = new Category();
                    temp.Value = dataItem["value"].Value<string>();
                    ListadoAjustesRFID.Add(temp);
                }
                field.ListadoAjusteRFID = ListadoAjustesRFID;
            }
            else
            {
                field.ListadoAjusteRFID = null;
            }
            if (fieldObject.TryGetValue(Constantes.ListadoConfiguracionRFID, out JToken listadoConfiguracionRFID) && listadoConfiguracionRFID.Type != JTokenType.Null)
            {
                var ListadoConfiguracionRFIDArray = fieldObject[Constantes.ListadoConfiguracionRFID].Value<JArray>();
                List<Category> ListadoConfiguracionRFID = new List<Category>();
                foreach (JObject dataItem in ListadoConfiguracionRFIDArray)
                {
                    Category temp = new Category();
                    temp.Value = dataItem["value"].Value<string>();
                    ListadoConfiguracionRFID.Add(temp);
                }
                field.ListadoConfiguracionRFID = ListadoConfiguracionRFID;
            }
            else
            {
                field.ListadoConfiguracionRFID = null;
            }

            if (fieldObject.TryGetValue(Constantes.DescripcionReparacion, out JToken DescripcionReparacion) && DescripcionReparacion.Type != JTokenType.Null)
            {
                field.DescripcionReparacion = fieldObject[Constantes.DescripcionReparacion].Value<string>();
            }
            else
            {
                field.DescripcionReparacion = null;
            }

            if (fieldObject.TryGetValue(Constantes.DiagnosticoCausa, out JToken DiagnosticoCausa) && DiagnosticoCausa.Type != JTokenType.Null)
            {

                field.DiagnosticoCausa = fieldObject[Constantes.DiagnosticoCausa].Value<string>();
            }
            else
            {
                field.DiagnosticoCausa = null;
            }
            if (fieldObject.TryGetValue(Constantes.TipoCausa, out JToken tipoCausa) && tipoCausa.Type != JTokenType.Null)
            {
                Category TipoCausa = new Category();
                TipoCausa.Value = fieldObject[Constantes.TipoCausa]["value"].Value<string>();
                field.TipoCausa = TipoCausa;
            }
            else
            {
                field.TipoCausa = null;
            }

            if (fieldObject.TryGetValue(Constantes.FechaSolucion, out JToken fechaSolucion) && fechaSolucion.Type != JTokenType.Null)
            {
                DateTime FechaSolucion = fieldObject[Constantes.FechaSolucion].Value<DateTime>();
                field.FechaSolucion = FechaSolucion;
            }
            else
            {
                field.FechaSolucion = null;
            }

            if (fieldObject.TryGetValue(Constantes.Status, out JToken Status) && Status.Type != JTokenType.Null)
            {
                Status status = new Status();
                status.name = fieldObject[Constantes.Status]["name"].Value<string>();
                field.status = status;
            }
            else
            {
                field.status = null;
            }
            return field;
        }
        public static Field ConverJsonInFieldMTO(JObject fieldObject)
        {
            Field field = new Field();

            if (fieldObject.TryGetValue(Constantes.Estacion, out JToken estacion) && estacion.Type != JTokenType.Null)
            {
                Category Estacion = new Category();
                Estacion.Value = fieldObject[Constantes.Estacion]["value"].Value<string>();
                field.Estacion = Estacion;
            }
            else
            {
                field.Estacion = null;
            }

            if (fieldObject.TryGetValue(Constantes.Vagon, out JToken vagon) && vagon.Type != JTokenType.Null)
            {
                Category Vagon = new Category();
                Vagon.Value = fieldObject[Constantes.Vagon]["value"].Value<string>();
                field.Vagon = Vagon;
            }
            else
            {
                field.Vagon = null;
            }

            if (fieldObject.TryGetValue(Constantes.IdentificacionComponente, out JToken IdentificacionComponente) && IdentificacionComponente.Type != JTokenType.Null)
            {
                field.IdentificacionComponente = fieldObject[Constantes.IdentificacionComponente].Value<string>();
            }
            else
            {
                field.IdentificacionComponente = null;
            }


            //Category tipoDeComponente = fieldObject["customfield_10070"].Value<Category>();
            if (fieldObject.TryGetValue(Constantes.TipoDeComponente, out JToken TipoDeComponente) && TipoDeComponente.Type != JTokenType.Null)
            {
                Category tipoDeComponente = new Category();
                tipoDeComponente.Value = fieldObject[Constantes.TipoDeComponente]["value"].Value<string>();
                field.TipoDeComponente = tipoDeComponente;
            }
            else
            {
                field.TipoDeComponente = null;
            }

            if (fieldObject.TryGetValue(Constantes.IdentificacionSerial, out JToken IdentificacionSerial) && IdentificacionSerial.Type != JTokenType.Null)
            {
                field.IdentificacionSerial = fieldObject[Constantes.IdentificacionSerial].Value<string>();
            }
            else
            {
                field.IdentificacionSerial = null;
            }

            if (fieldObject.TryGetValue(Constantes.TipoDeServicio, out JToken TipoDeServicio) && TipoDeServicio.Type != JTokenType.Null)
            {
                Category tipoDeServicio = new Category();
                tipoDeServicio.Value = fieldObject[Constantes.TipoDeServicio]["value"].Value<string>();
                field.TipoDeServicio = tipoDeServicio;
            }
            else
            {
                field.TipoDeServicio = null;
            }

            if (fieldObject.TryGetValue(Constantes.ClaseDeFallo, out JToken ClaseDeFallo) && ClaseDeFallo.Type != JTokenType.Null)
            {
                Category claseDeFallo = new Category();
                claseDeFallo.Value = fieldObject[Constantes.ClaseDeFallo]["value"].Value<string>();
                field.ClaseDeFallo = claseDeFallo;
            }
            else
            {
                field.ClaseDeFallo = null;
            }

            if (fieldObject.TryGetValue(Constantes.DescripcionDeFallo, out JToken DescripcionDeFallo) && DescripcionDeFallo.Type != JTokenType.Null)
            {
                var descripcionDeFalloArray = fieldObject[Constantes.DescripcionDeFallo].Value<JArray>();
                List<Category> descripcionDeFallo = new List<Category>();
                foreach (JObject dataItem in descripcionDeFalloArray)
                {
                    Category temp = new Category();
                    temp.Value = dataItem["value"].Value<string>();
                    descripcionDeFallo.Add(temp);
                }
                field.DescripcionDeFallo = descripcionDeFallo;
            }
            else
            {
                field.DescripcionDeFallo = null;
            }

            if (fieldObject.TryGetValue(Constantes.FechaCreacion, out JToken Created) && Created.Type != JTokenType.Null)
            {
                DateTime created = fieldObject[Constantes.FechaCreacion].Value<DateTime>();
                field.created = created;
            }
            else
            {
                field.created = null;
            }

            if (fieldObject[Constantes.StatusCategoryChangeDate].Value<DateTime>() != null)
            {
                DateTime statuscategorychangedate = fieldObject[Constantes.StatusCategoryChangeDate].Value<DateTime>();
                field.statuscategorychangedate = statuscategorychangedate;
            }
            else
            {
                field.statuscategorychangedate = null;
            }
            if (fieldObject.TryGetValue(Constantes.FechayHoraDeLlegadaAEstacion, out JToken Fechay_Hora_De_Llegada_Estacion) && Fechay_Hora_De_Llegada_Estacion.Type != JTokenType.Null)
            {
                DateTime FechayHoraDeLlegadaAEstacion = fieldObject[Constantes.FechayHoraDeLlegadaAEstacion].Value<DateTime>();
                field.FechayHoraDeLlegadaAEstacion = FechayHoraDeLlegadaAEstacion;
            }
            else
            {
                field.FechayHoraDeLlegadaAEstacion = null;
            }

            if (fieldObject.TryGetValue(Constantes.DescripcionRepuesto, out JToken DescripcionRepuesto) && DescripcionRepuesto.Type != JTokenType.Null)
            {
                var descripcionRepuestoArray = fieldObject[Constantes.DescripcionRepuesto].Value<JArray>();
                List<Category> descripcionRepuesto = new List<Category>();
                foreach (JObject dataItem in descripcionRepuestoArray)
                {
                    Category temp = new Category();
                    temp.Value = dataItem["value"].Value<string>();
                    descripcionRepuesto.Add(temp);
                }
                field.DescripcionRepuesto = descripcionRepuesto;
            }
            else
            {
                field.DescripcionRepuesto = null;
            }

            if (fieldObject.TryGetValue(Constantes.TipoReparacion, out JToken tipoReparacion) && tipoReparacion.Type != JTokenType.Null)
            {
                var TipoReparacionArray = fieldObject[Constantes.TipoReparacion].Value<JArray>();
                List<Category> TipoReparacion = new List<Category>();
                foreach (JObject dataItem in TipoReparacionArray)
                {
                    Category temp = new Category();
                    temp.Value = dataItem["value"].Value<string>();
                    TipoReparacion.Add(temp);
                }
                field.TipoReparacion = TipoReparacion;
            }
            else
            {
                field.TipoReparacion = null;
            }

            if (fieldObject.TryGetValue(Constantes.ListadoAjustesPuerta, out JToken listadoAjustePuerta) && listadoAjustePuerta.Type != JTokenType.Null)
            {
                var ListadoAjustesPuertaArray = fieldObject[Constantes.ListadoAjustesPuerta].Value<JArray>();
                List<Category> ListadoAjustesPuerta = new List<Category>();
                foreach (JObject dataItem in ListadoAjustesPuertaArray)
                {
                    Category temp = new Category();
                    temp.Value = dataItem["value"].Value<string>();
                    ListadoAjustesPuerta.Add(temp);
                }
                field.ListadoAjustesPuerta = ListadoAjustesPuerta;
            }
            else
            {
                field.ListadoConfiguracionPuerta = null;
            }

            if (fieldObject.TryGetValue(Constantes.ListadoConfiguracionPuerta, out JToken listadoConfiguracionPuerta) && listadoConfiguracionPuerta.Type != JTokenType.Null)
            {
                var ListadoConfiguracionPuertaArray = fieldObject[Constantes.ListadoConfiguracionPuerta].Value<JArray>();
                List<Category> ListadoConfiguracionPuerta = new List<Category>();
                foreach (JObject dataItem in ListadoConfiguracionPuertaArray)
                {
                    Category temp = new Category();
                    temp.Value = dataItem["value"].Value<string>();
                    ListadoConfiguracionPuerta.Add(temp);
                }
                field.ListadoConfiguracionPuerta = ListadoConfiguracionPuerta;
            }
            else
            {
                field.ListadoConfiguracionPuerta = null;
            }

            if (fieldObject.TryGetValue(Constantes.ListadoAjustesITS, out JToken listadoAjusteIts) && listadoAjusteIts.Type != JTokenType.Null)
            {
                var ListadoAjustesITSArray = fieldObject[Constantes.ListadoAjustesITS].Value<JArray>();
                List<Category> ListadoAjustesITS = new List<Category>();
                foreach (JObject dataItem in ListadoAjustesITSArray)
                {
                    Category temp = new Category();
                    temp.Value = dataItem["value"].Value<string>();
                    ListadoAjustesITS.Add(temp);
                }
                field.ListadoAjustesITS = ListadoAjustesITS;
            }
            else
            {
                field.ListadoAjustesITS = null;
            }

            if (fieldObject.TryGetValue(Constantes.ListadoConfiguracionITS, out JToken listadoConfiguracionITS) && listadoConfiguracionITS.Type != JTokenType.Null)
            {
                var ListadoConfiguracionITSArray = fieldObject[Constantes.ListadoConfiguracionITS].Value<JArray>();
                List<Category> ListadoConfiguracionITS = new List<Category>();
                foreach (JObject dataItem in ListadoConfiguracionITSArray)
                {
                    Category temp = new Category();
                    temp.Value = dataItem["value"].Value<string>();
                    ListadoConfiguracionITS.Add(temp);
                }
                field.ListadoConfiguracionITS = ListadoConfiguracionITS;
            }
            else
            {
                field.ListadoConfiguracionITS = null;
            }
            if (fieldObject.TryGetValue(Constantes.ListadoAjusteRFID, out JToken listadoAjusteRfid) && listadoAjusteRfid.Type != JTokenType.Null)
            {
                var ListadoAjustesRFIDArray = fieldObject[Constantes.ListadoAjusteRFID].Value<JArray>();
                List<Category> ListadoAjustesRFID = new List<Category>();
                foreach (JObject dataItem in ListadoAjustesRFIDArray)
                {
                    Category temp = new Category();
                    temp.Value = dataItem["value"].Value<string>();
                    ListadoAjustesRFID.Add(temp);
                }
                field.ListadoAjusteRFID = ListadoAjustesRFID;
            }
            else
            {
                field.ListadoAjusteRFID = null;
            }
            if (fieldObject.TryGetValue(Constantes.ListadoConfiguracionRFID, out JToken listadoConfiguracionRFID) && listadoConfiguracionRFID.Type != JTokenType.Null)
            {
                var ListadoConfiguracionRFIDArray = fieldObject[Constantes.ListadoConfiguracionRFID].Value<JArray>();
                List<Category> ListadoConfiguracionRFID = new List<Category>();
                foreach (JObject dataItem in ListadoConfiguracionRFIDArray)
                {
                    Category temp = new Category();
                    temp.Value = dataItem["value"].Value<string>();
                    ListadoConfiguracionRFID.Add(temp);
                }
                field.ListadoConfiguracionRFID = ListadoConfiguracionRFID;
            }
            else
            {
                field.ListadoConfiguracionRFID = null;
            }

            if (fieldObject.TryGetValue(Constantes.DescripcionReparacion, out JToken DescripcionReparacion) && DescripcionReparacion.Type != JTokenType.Null)
            {
                field.DescripcionReparacion = fieldObject[Constantes.DescripcionReparacion].Value<string>();
            }
            else
            {
                field.DescripcionReparacion = null;
            }

            if (fieldObject.TryGetValue(Constantes.DiagnosticoCausa, out JToken DiagnosticoCausa) && DiagnosticoCausa.Type != JTokenType.Null)
            {

                field.DiagnosticoCausa = fieldObject[Constantes.DiagnosticoCausa].Value<string>();
            }
            else
            {
                field.DiagnosticoCausa = null;
            }
            if (fieldObject.TryGetValue(Constantes.TipoCausa, out JToken tipoCausa) && tipoCausa.Type != JTokenType.Null)
            {
                Category TipoCausa = new Category();
                TipoCausa.Value = fieldObject[Constantes.TipoCausa]["value"].Value<string>();
                field.TipoCausa = TipoCausa;
            }
            else
            {
                field.TipoCausa = null;
            }

            if (fieldObject.TryGetValue(Constantes.FechaSolucion, out JToken fechaSolucion) && fechaSolucion.Type != JTokenType.Null)
            {
                DateTime FechaSolucion = fieldObject[Constantes.FechaSolucion].Value<DateTime>();
                field.FechaSolucion = FechaSolucion;
            }
            else
            {
                field.FechaSolucion = null;
            }

            if (fieldObject.TryGetValue(Constantes.Status, out JToken Status) && Status.Type != JTokenType.Null)
            {
                Status status = new Status();
                status.name = fieldObject[Constantes.Status]["name"].Value<string>();
                field.status = status;
            }
            else
            {
                field.status = null;
            }
            return field;
        }
    }

    public class Category
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class Status
    {
        [JsonPropertyName("name")]
        public string name { get; set; }
    }


}
