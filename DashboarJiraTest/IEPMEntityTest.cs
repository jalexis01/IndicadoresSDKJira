using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DashboarJira.Model;
using NUnit.Framework;
using System.Collections.Generic;

namespace DashboarJiraTest
{


    [TestFixture]
    public class ICPMEntityTests
    {
        [Test]
        public void CalcularIndicadorIEPM_TicketTAPEmpty_Returns0()
        {
            // Arrange
            var ticketTAP = new List<Ticket>();
            var ticketAPE = new List<Ticket> { new Ticket(), new Ticket() };
            var entity = new IEPMEntity(ticketTAP, ticketAPE);

            // Act
            var result = entity.CalcularIndicadorIEPM();

            // Assert
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void CalcularIndicadorIEPM_TicketTAPNotEmpty_ReturnsCorrectValue()
        {
            // Arrange
            var ticketTAP = new List<Ticket> { new Ticket(), new Ticket() };
            var ticketAPE = new List<Ticket> { new Ticket(), new Ticket() };
            var entity = new IEPMEntity(ticketTAP, ticketAPE);

            // Act
            var result = entity.CalcularIndicadorIEPM();

            // Assert
            Assert.That(result, Is.EqualTo((double)ticketAPE.Count / (double)ticketTAP.Count * 100));
        }
    }

}
