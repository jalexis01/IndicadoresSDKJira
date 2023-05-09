﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboarJira.Model
{
    class Ticket
    {
        public string? id_ticket { get; set; }
        public string? id_estacion { get; set; }
        public string? id_vagon { get; set; }
        public string? id_puerta { get; set; }
        public string? id_componente { get; set; }
        public string? tipoComponente { get; set; }
        public string? identificacion { get; set; }
        public string? tipo_mantenimiento { get; set; }
        public string? nivel_falla { get; set; }
        public string? codigo_falla { get; set; }
        public DateTime? fecha_apertura { get; set; }
        public DateTime? fecha_cierre { get; set; }
        public DateTime? fecha_arribo_locacion { get; set; }
        public string? componente_Parte { get; set; }
        public string? tipo_reparacion { get; set; }
        public string? tipo_ajuste_configuracion { get; set; }
        public string? descripcion_reparacion { get; set; }
        public string? diagnostico_causa { get; set; }
        public string? tipo_causa { get; set; }
        public string? estado_ticket { get; set; }
    }
}
 