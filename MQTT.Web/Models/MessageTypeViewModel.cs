using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MQTT.Web.Models
{
    public class MessageTypeViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Debe indicar un valor, mínimo 2 caracteres.")]
        [DisplayName("Código")]
        public string Code { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "Debe indicar un valor, mínimo 2 caracteres.")]
        [DisplayName("Nombre")]
        public string Name { get; set; }
        [StringLength(1000, MinimumLength = 0)]
        [DisplayName("Descripción")]
        public string Description { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(116, MinimumLength = 10, ErrorMessage = "Debe indicar un valor, mínimo 10 caracteres.")]
        [DisplayName("Nombre Tabla")]
        public string TableName { get; set; }
        [Required(ErrorMessage = "*")]
        [DisplayName("Activo")]
        public bool Enable { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Debe indicar un valor, mínimo 2 caracteres.")]
        [DisplayName("Nombre Campo del Código del Mensaje")]
        public string FieldCode { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Debe indicar un valor, mínimo 2 caracteres.")]
        [DisplayName("Nombre Campo en que Llega la Trama")]
        public string FieldWeft { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "Debe indicar un valor, mínimo 2 caracteres.")]
        [DisplayName("Nombre Campo Identificador Único del Mensaje")]
        public string FieldIdentifierMessage { get; set; }
    }
}
