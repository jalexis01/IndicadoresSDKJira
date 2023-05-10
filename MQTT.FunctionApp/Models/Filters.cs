using MQTT.Infrastructure.DAL;
using MQTT.Infrastructure.Models.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MQTT.FunctionApp.Models
{
    public class Filters:IValidatableObject
    {
        public Filters()
        {
            issueType = "Solicitud de Mantenimiento";
        }

        public string fecha_inicial_rango { get; set; }
        public string fecha_final_rango { get; set; }
        public string tipo_fecha { get; set; }
        public string estado_ticket { get; set; }
        public string nivel_falla { get; set; }
        public string codigo_falla { get; set; }
        public string id_ticket { get; set; }
        public string issueType { get; set; }
        public string resultQuery { get; set; }
        public List<EquivalenceDTO> equivalenceServiceType { get; set; }

        public void GetAllEquivalences(General DBAccess)
        {
            try
            {
                equivalenceServiceType = EquivalencesDAL.GetAllEquivalences(DBAccess);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (this.id_ticket is null)
            {
                if (this.tipo_fecha is null || string.IsNullOrEmpty(this.tipo_fecha.ToString()))
                {
                    results.Add(new ValidationResult("El parámetro tipo_fecha es obligatorio"));
                }
                
                if (this.fecha_inicial_rango is null || string.IsNullOrEmpty(this.fecha_inicial_rango.ToString()))
                {
                    results.Add(new ValidationResult("El parámetro fecha_inicial_rango es obligatorio"));
                }
                
                if (this.fecha_final_rango is null || string.IsNullOrEmpty(this.fecha_final_rango.ToString()))
                {
                    results.Add(new ValidationResult("El parámetro fecha_inicial_rango es obligatorio"));
                }
            }
            else
            {
                this.fecha_inicial_rango = null;
                this.fecha_final_rango = null;
                this.tipo_fecha = null;
                this.estado_ticket = null;
                this.nivel_falla = null;
                this.codigo_falla = null;
            }

            if (!string.IsNullOrEmpty(this.issueType))
            {
                resultQuery = $"jql=issuetype in ('{this.issueType}')";
            }

            if (!string.IsNullOrEmpty(this.tipo_fecha))
            {
                switch (this.tipo_fecha)
                {
                    case "fecha_apertura":
                        resultQuery += $" AND created >= {this.fecha_inicial_rango} and created <= {this.fecha_final_rango}";
                        break;
                    case "fecha_cierre":
                        resultQuery += $" AND \"Fecha de solucion[Time stamp]\" >= {this.fecha_inicial_rango} and \"Fecha de solucion[Time stamp]\" <= {this.fecha_final_rango}";
                        break;
                    case "fecha_arribo_locacion":
                        resultQuery += $" AND \"Fecha y Hora de Llegada a Estacion[Time stamp]\" >= {this.fecha_inicial_rango} and \"Fecha y Hora de Llegada a Estacion[Time stamp]\" <= {this.fecha_final_rango}";
                        break;
                }
            }

            if (!string.IsNullOrEmpty(this.estado_ticket))
            {
                var equivalences = equivalenceServiceType.Where(e => e.Value.ToUpper() == estado_ticket.ToUpper()).Select(e => $"'{e.Name}'").ToList();
                resultQuery += $" AND status in ({string.Join(',',equivalences)})";

            }

            if (!string.IsNullOrEmpty(this.nivel_falla))
            {
                resultQuery += $" AND \"Clase de fallo[Dropdown]\" = '{this.nivel_falla}'";
            }

            if (!string.IsNullOrEmpty(this.codigo_falla))
            {
                resultQuery += $" AND \"Descripcion de fallo[Select List (multiple choices)]\" = '{this.codigo_falla}'";
            }

            if (!string.IsNullOrEmpty(this.id_ticket))
            {
                resultQuery += $" AND key = {this.id_ticket}";
            }
            

            return results;
        }
    }
}
