using System.Collections.Generic;

namespace MQTT.Web.Models
{
	public class ElementViewModel:MQTT.Infrastructure.Models.DTO.ElementDTO
	{
		public List<Infrastructure.Models.DTO.ElementTypeDTO> ElementTypes { get; set; }
	}
}
