using DB.Data.ModelDB;
using DB.Data.ModelDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Services
{
    public class DataService
    {




        public MessageDTO ConvertToDTO(TbMessage message)
        {
            var messageDTO = new MessageDTO();
            messageDTO.Id = message.Id;
            messageDTO.IdHeaderMessage = message.IdHeaderMessage;
            messageDTO.VersionTrama = message.VersionTrama;
            messageDTO.IdRegistro = message.IdRegistro;
            messageDTO.IdOperador = message.IdOperador;
            messageDTO.IdEstacion = message.IdEstacion;
            messageDTO.IdVagon = message.IdVagon;
            messageDTO.IdPuerta = message.IdPuerta;
            messageDTO.CodigoPuerta = message.CodigoPuerta;
            messageDTO.FechaHoraLecturaDato = message.FechaHoraLecturaDato;
            messageDTO.FechaHoraEnvioDato = message.FechaHoraEnvioDato;
            messageDTO.TipoTrama = message.TipoTrama;
            messageDTO.TramaRetransmitida = message.TramaRetransmitida;
            messageDTO.CodigoEvento = message.CodigoEvento;
            messageDTO.EstadoAperturaCierrePuertas = message.EstadoAperturaCierrePuertas;
            messageDTO.UsoBotonManual = message.UsoBotonManual;
            messageDTO.EstadoBotonManual = message.EstadoBotonManual;
            messageDTO.EstadoErrorCritico = message.EstadoErrorCritico;
            messageDTO.PorcentajeCargaBaterias = message.PorcentajeCargaBaterias;
            messageDTO.CiclosApertura = message.CiclosApertura;
            messageDTO.HorasServicio = message.HorasServicio;
            messageDTO.TipoEnergizacion = message.TipoEnergizacion;
            messageDTO.VelocidaMotor = message.VelocidaMotor;
            messageDTO.FuerzaMotor = message.FuerzaMotor;
            messageDTO.ModoOperacion = message.ModoOperacion;
            messageDTO.NumeroEventoBusEstacion = message.NumeroEventoBusEstacion;
            messageDTO.IdVehiculo = message.IdVehiculo;
            messageDTO.PlacaVehiculo = message.PlacaVehiculo;
            messageDTO.TipologiaVehiculo = message.TipologiaVehiculo;
            messageDTO.NumeroParada = message.NumeroParada;
            messageDTO.NombreEstacion = message.NombreEstacion;
            messageDTO.NombreVagon = message.NombreVagon;
            messageDTO.TipoTramaBusEstacion = message.TipoTramaBusEstacion;
            messageDTO.CodigoAlarma = message.CodigoAlarma;
            messageDTO.CodigoNivelAlarma = message.CodigoNivelAlarma;
            messageDTO.TiempoApertura = message.TiempoApertura;

            return messageDTO;
        }

    }
}
