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
    public class IEPMEntityTests
    {
        [Test]
        public void CalcularIndicadorIEPM_TicketsAMEEmpty_Returns100()
        {
            // Arrange
            var ticketsANP = new List<Ticket>();
            var ticketsAME = new List<Ticket>();
            var entity = new IEPMEntity(ticketsANP, ticketsAME);

            // Act
            var result = entity.CalcularIndicadorIEPM();

            // Assert
            Assert.That(result, Is.EqualTo(100));
        }

        [Test]
        public void CalcularIndicadorIEPM_TicketsAMENotEmpty_ReturnsCorrectValue()
        {
            // Arrange
            var ticketsANP = new List<Ticket>();
            var ticketsAME = new List<Ticket> { new Ticket(), new Ticket(), new Ticket() };
            var entity = new IEPMEntity(ticketsANP, ticketsAME);

            // Act
            var result = entity.CalcularIndicadorIEPM();

            // Assert
            Assert.That(result, Is.EqualTo(1.0 - (double)ticketsANP.Count / (double)ticketsAME.Count));
        }
    }

}
