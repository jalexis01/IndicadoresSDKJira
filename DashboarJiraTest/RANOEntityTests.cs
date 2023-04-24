using System.Collections.Generic;
using NUnit.Framework;
using System.Text.Json;
using DashboarJira.Model;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DashboarJiraTest
{
	[TestFixture]
	public class RANOEntityTests
	{
		private List<Ticket> tickets;


		[SetUp]
		public void setup1()
		{
			string json = DataTest._dataTicketsANIO;
			tickets = JsonSerializer.Deserialize<List<Ticket>>(json);
		}
		[SetUp]
		public void setUpScenario1()
		{
			List<Ticket> ticketsList = new List<Ticket>();

			for (int i = 1; i <= 10; i++)
			{
				var ticket = new Ticket
				{
					id_ticket = $"TICKET-{i}",
					id_estacion = "9117",
					id_vagon = "WA",
					id_puerta = "9117-WA-OR-5",
					tipoComponente = "Puerta",
					identificacion = "N1T-0017",
					tipo_mantenimiento = "Mantenimiento Correctivo",
					nivel_falla = "AIO",
					codigo_falla = "No reproducción de mensajes de audio FLL-N-0001",
					fecha_apertura = DateTime.Parse("2023-03-01T14:35:17.505"),
					fecha_cierre = DateTime.Parse("2023-03-03T13:27:05.61"),
					fecha_arribo_locacion = DateTime.Parse("2023-03-01T14:35:26.59"),
					componente_Parte = null,
					tipo_reparacion = "Ajuste",
					tipo_ajuste_configuracion = "Ajuste cables de datos AJS-P-0001",
					descripcion_reparacion = null,
					diagnostico_causa = "A cargo del contratista",
					estado_ticket = "Cerrado"
				};
				ticketsList.Add(ticket);
			}


		}
		[Test]
		public void InitializationTest()
		{
			// Arrange
			var ticketTAN = new List<Ticket>();
			var ticketTCN = new List<Ticket>();

			// Act
			var entity = new RANOEntity(ticketTAN, ticketTCN);

			// Assert
			Assert.IsNotNull(entity);
			Assert.IsNotNull(entity.TicketTAN);
			Assert.IsNotNull(entity.TicketTCN);
			Assert.AreEqual(ticketTAN, entity.TicketTAN);
			Assert.AreEqual(ticketTCN, entity.TicketTCN);
		}

		[Test]
		public void TestAgregarTicket()
		{
			// Arrange
			var ticket = new Ticket
			{
				id_ticket = "TICKET-86",
				id_estacion = "9117",
				id_vagon = "WA",
				id_puerta = "9117-WA-OR-5",
				tipoComponente = "Puerta",
				identificacion = "N1T-0017",
				tipo_mantenimiento = "Mantenimiento Correctivo",
				nivel_falla = "AIO",
				codigo_falla = "No reproducción de mensajes de audio FLL-N-0001",
				fecha_apertura = DateTime.Parse("2023-03-01T14:35:17.505"),
				fecha_cierre = DateTime.Parse("2023-03-03T13:27:05.61"),
				fecha_arribo_locacion = DateTime.Parse("2023-03-01T14:35:26.59"),
				componente_Parte = null,
				tipo_reparacion = "Ajuste",
				tipo_ajuste_configuracion = "Ajuste cables de datos AJS-P-0001",
				descripcion_reparacion = null,
				diagnostico_causa = "A cargo del contratista",
				estado_ticket = "Cerrado"
			};
			var listaTicketTAN = new List<Ticket>();
			var listaTicketTCN = new List<Ticket>();
			var entity = new RANOEntity(listaTicketTAN, listaTicketTCN);

			// Act
			entity.TicketTAN.Add(ticket);

			// Assert
			Assert.AreEqual(1, entity.TicketTAN.Count);
			Assert.AreEqual(0, entity.TicketTCN.Count);
			Assert.AreEqual(ticket, entity.TicketTAN[0]);
		}
		[Test]
		public void CalcularIndicadorSameDataAllRANOTest()
		{
			var listaTicketTAN = new List<Ticket>();
			var listaTicketTCN = new List<Ticket>();
			var rano = new RANOEntity(listaTicketTAN, listaTicketTCN);
			// Agregar los tickets a la lista de tickets
			for (int i = 1; i <= 10; i++)
			{
				var ticket = new Ticket
				{
					id_ticket = $"TICKET-{i}",
					id_estacion = "9117",
					id_vagon = "WA",
					id_puerta = "9117-WA-OR-5",
					tipoComponente = "Puerta",
					identificacion = "N1T-0017",
					tipo_mantenimiento = "Mantenimiento Correctivo",
					nivel_falla = "AIO",
					codigo_falla = "No reproducción de mensajes de audio FLL-N-0001",
					fecha_apertura = DateTime.Parse("2023-03-01T14:35:17.505"),
					fecha_cierre = DateTime.Parse("2023-03-03T13:27:05.61"),
					fecha_arribo_locacion = DateTime.Parse("2023-03-01T14:35:26.59"),
					componente_Parte = null,
					tipo_reparacion = "Ajuste",
					tipo_ajuste_configuracion = "Ajuste cables de datos AJS-P-0001",
					descripcion_reparacion = null,
					diagnostico_causa = "A cargo del contratista",
					estado_ticket = "Cerrado"
				};
				rano.TicketTAN.Add(ticket);
				rano.TicketTCN.Add(ticket);
			}

			// Calcular el indicador RANO
			var indicadorRANO = rano.CalcularIndicadorRANO();

			// Verificar que el resultado sea el esperado
			Assert.AreEqual(100.0, indicadorRANO);
		}
		[Test]
		public void CalcularIndicadorRANOTest()
		{
			var listaTicketTAN = new List<Ticket>();
			var listaTicketTCN = new List<Ticket>();
			var rano = new RANOEntity(listaTicketTAN, listaTicketTCN);
			// Agregar los tickets a la lista de tickets
			for (int i = 1; i <= 10; i++)
			{
				var ticket = new Ticket
				{
					id_ticket = $"TICKET-{i}",
					id_estacion = "9117",
					id_vagon = "WA",
					id_puerta = "9117-WA-OR-5",
					tipoComponente = "Puerta",
					identificacion = "N1T-0017",
					tipo_mantenimiento = "Mantenimiento Correctivo",
					nivel_falla = "AIO",
					codigo_falla = "No reproducción de mensajes de audio FLL-N-0001",
					fecha_apertura = DateTime.Parse("2023-03-01T14:35:17.505"),
					fecha_cierre = DateTime.Parse("2023-03-03T13:27:05.61"),
					fecha_arribo_locacion = DateTime.Parse("2023-03-01T14:35:26.59"),
					componente_Parte = null,
					tipo_reparacion = "Ajuste",
					tipo_ajuste_configuracion = "Ajuste cables de datos AJS-P-0001",
					descripcion_reparacion = null,
					diagnostico_causa = "A cargo del contratista",
					estado_ticket = "Cerrado"
				};
				rano.TicketTAN.Add(ticket);
				rano.TicketTCN.Add(ticket);
			}

			// Calcular el indicador RANO
			var indicadorRANO = rano.CalcularIndicadorRANO();

			// Verificar que el resultado sea el esperado
			Assert.AreEqual(100.0, indicadorRANO);
		}

	}
}