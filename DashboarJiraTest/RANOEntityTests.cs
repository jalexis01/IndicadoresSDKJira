using System.Collections.Generic;
using NUnit.Framework;
using DashboarJira.Model;


namespace DashboarJiraTest
{
		[TestFixture]
		public class RANOEntityTests
		{
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
		public void AgregarTicket_DebeAgregarCorrectamente()
		{
			// Arrange
			var ticketTAN = new List<Ticket>();
			var ticketTCN = new List<Ticket>();
			var entity = new RANOEntity(ticketTAN, ticketTCN);
			var ticket = new Ticket { id_ticket = "T001", falla = "ANIO" };

			// Act
			entity.TicketTAN.Add(ticket);

			// Assert
			Assert.That(entity.TicketTAN, Has.Exactly(1).EqualTo(ticket));
			Assert.That(entity.TicketTCN, Has.Exactly(0).EqualTo(ticket));
		}
	}
	}
