namespace DashboarJira.Model
{
    public class Ticket
    {
        public string? id_ticket { get; set; }
        public string? id_estacion { get; set; }
        public string? id_vagon { get; set; }
        public string? id_puerta { get; set; }
        public string? id_componente { get; set; }
        public string? tipoComponente { get; set; }
        public string? identificacion { get; set; }
        public string? estado_ticket { get; set; }
        public string? tipo_mantenimiento { get; set; }
        public string? nivel_falla { get; set; }
        public string? codigo_falla { get; set; }
        public DateTime? fecha_apertura { get; set; }
        public DateTime? fecha_cierre { get; set; }
        public DateTime? fecha_arribo_locacion { get; set; }
        public string? cantidad_repuesto_utilizado { get; set; }
        public string? componente_Parte { get; set; }
        public string? tipo_reparacion { get; set; }
        public string? tipo_ajuste_configuracion { get; set; }
        public string? descripcion_reparacion { get; set; }
        public string? diagnostico_causa { get; set; }
        public string? tipo_causa { get; set; }
        public string? descripcion { get; set; }
        public string? canal_comunicacion { get; set; }
        public string? quien_requiere_servicio { get; set; }
        //public string? operador_ma { get; set; }
        public string? codigo_plan_mantenimiento { get; set; }
        public string? descripcion_actividad_mantenimiento { get; set; }
        public string? tecnico_asignado { get; set; }
        public string? motivo_atraso { get; set; }
        public string? otro_motivo_atraso { get; set; }
        public override string ToString()
        {
            return $"IdTicket: {id_ticket}, IdEstacion: {id_estacion}, IdVagon: {id_vagon}, IdPuerta: {id_puerta}, IdComponente: {id_componente}, TipoComponente: {tipoComponente}, Identificacion: {identificacion}, TipoMantenimiento: {tipo_mantenimiento}, NivelFalla: {nivel_falla}, CodigoFalla: {codigo_falla}, FechaApertura: {fecha_apertura}, FechaCierre: {fecha_cierre}, FechaArriboLocacion: {fecha_arribo_locacion}, ComponenteParte: {componente_Parte}, TipoReparacion: {tipo_reparacion}, TipoAjusteConfiguracion: {tipo_ajuste_configuracion}, DescripcionReparacion: {descripcion_reparacion}, DiagnosticoCausa: {diagnostico_causa}, TipoCausa: {tipo_causa}, EstadoTicket: {estado_ticket}";
        }

    }
}