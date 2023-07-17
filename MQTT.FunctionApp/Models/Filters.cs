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

        public string fechaInicialRango { get; set; }
        public string fechaFinalRango { get; set; }
        public string tipoFecha { get; set; }
        public string estadoTicket { get; set; }
        public string nivelFalla { get; set; }
        public string codigoFalla { get; set; }
        public string idTicket { get; set; }
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

            if (this.idTicket is null)
            {
                if (this.tipoFecha is null || string.IsNullOrEmpty(this.tipoFecha.ToString()))
                {
                    results.Add(new ValidationResult("El parámetro tipoFecha es obligatorio"));
                }
                
                if (this.fechaInicialRango is null || string.IsNullOrEmpty(this.fechaInicialRango.ToString()))
                {
                    results.Add(new ValidationResult("El parámetro fechaInicialRango es obligatorio"));
                }
                
                if (this.fechaFinalRango is null || string.IsNullOrEmpty(this.fechaFinalRango.ToString()))
                {
                    results.Add(new ValidationResult("El parámetro fechaFinalRango es obligatorio"));
                }
            }
            else
            {
                this.fechaInicialRango = null;
                this.fechaFinalRango = null;
                this.tipoFecha = null;
                this.estadoTicket = null;
                this.nivelFalla = null;
                this.codigoFalla = null;
            }

            if (!string.IsNullOrEmpty(this.issueType))
            {
                resultQuery = $"jql=issuetype in ('{this.issueType}') AND status not in ('Descartado')";
            }

            if (!string.IsNullOrEmpty(this.tipoFecha))
            {
                switch (this.tipoFecha)
                {
                    case "fechaApertura":
                        resultQuery += $" AND created >= {this.fechaInicialRango} and created <= {this.fechaFinalRango}";
                        break;
                    case "fechaCierre":
                        resultQuery += $" AND \"Fecha de solucion[Time stamp]\" >= {this.fechaInicialRango} and \"Fecha de solucion[Time stamp]\" <= {this.fechaFinalRango}";
                        break;
                    case "fechaArriboLocacion":
                        resultQuery += $" AND \"Fecha y Hora de Llegada a Estacion[Time stamp]\" >= {this.fechaInicialRango} and \"Fecha y Hora de Llegada a Estacion[Time stamp]\" <= {this.fechaFinalRango}";
                        break;
                    default:
                        results.Add(new ValidationResult("El parámetro tipoFecha no es valido"));
                        break;
                }
            }

            if (!string.IsNullOrEmpty(this.estadoTicket))
            {
                switch (this.estadoTicket)
                {
                    case "Abierto":
                        resultQuery += $" AND status NOT IN(Cerrado, DESCARTADO)";
                        break;
                    case "Cerrado":
                        resultQuery += $" AND status IN (Cerrado)";
                        break;
                    
                    default:
                        results.Add(new ValidationResult("El valor estadoTicket no es valido"));
                        break;
                }
                

            }

            if (!string.IsNullOrEmpty(this.nivelFalla))
            {
                resultQuery += $" AND \"Clase de fallo[Dropdown]\" = '{this.nivelFalla}'";
            }

            if (!string.IsNullOrEmpty(this.codigoFalla))
            {
                resultQuery += $" AND \"Descripcion de fallo[Select List (multiple choices)]\" = '{this.codigoFalla}'";
            }

            if (!string.IsNullOrEmpty(this.idTicket))
            {
                resultQuery += $" AND key = {this.idTicket}";
            }
            resultQuery += " AND 'Descripcion de la reparacion' is not empty ";

            return results;
        }
    }
}
