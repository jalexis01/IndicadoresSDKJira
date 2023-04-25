using DashboarJira.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Collections.Generic;

namespace DashboarJiraTest
{
    [TestFixture]
    public class RAIOEntityTests
    {
        [Test]
        public void TestRAIOWithEmptyLists()
        {
            // Arrange
            var ticketTCI = new List<Ticket>();
            var ticketTAI = new List<Ticket>();
            var raioEntity = new RAIOEntity(ticketTCI, ticketTAI);

            // Act
            var actualRAIO = raioEntity.CacularIndicadorRAIO();

            // Assert
            Assert.That(actualRAIO, Is.EqualTo(100));
        }

        [Test]
        public void TestRAIOWithEmptyTAIList()
        {
            // Arrange
            var ticketTCI = new List<Ticket>
        {
            new Ticket(),
            new Ticket(),
            new Ticket(),
        };
            var ticketTAI = new List<Ticket>();
            var raioEntity = new RAIOEntity(ticketTCI, ticketTAI);

            // Act
            var actualRAIO = raioEntity.CacularIndicadorRAIO();

            // Assert
            Assert.That(actualRAIO, Is.EqualTo(100));
        }
        [Test]
        public void TestRAIOWithNonEmptyLists()
        {
            // Arrange
            var ticketTCI = new List<Ticket>
    {
        new Ticket(),
        new Ticket(),
        new Ticket(),
    };
            var ticketTAI = new List<Ticket>
    {
        new Ticket(),
        new Ticket(),
    };
            var raioEntity = new RAIOEntity(ticketTCI, ticketTAI);

            // Act
            var actualRAIO = raioEntity.CacularIndicadorRAIO();

            // Assert
            Assert.That(actualRAIO, Is.EqualTo(150));
        }

        [Test]
        public void TestToString()
        {
            // Arrange
            var ticketTCI = new List<Ticket>
    {
        new Ticket(),
        new Ticket(),
        new Ticket(),
    };
            var ticketTAI = new List<Ticket>
    {
        new Ticket(),
        new Ticket(),
    };
            var raioEntity = new RAIOEntity(ticketTCI, ticketTAI);

            // Act
            var actualString = raioEntity.ToString();

            // Assert
            Assert.IsTrue(actualString.Contains("\"RAIO\":150"));
            Assert.IsTrue(actualString.Contains("\"TotalTai\":2"));
            Assert.IsTrue(actualString.Contains("\"TotalTCI\":3"));
            Assert.IsTrue(actualString.Contains("\"TicketTCI\":["));
            Assert.IsTrue(actualString.Contains("\"TicketTAI\":["));
        }

        [Test]
        public void TestConstructor()
        {
            // Arrange
            var ticketTCI = new List<Ticket>
    {
        new Ticket(),
        new Ticket(),
        new Ticket(),
    };
            var ticketTAI = new List<Ticket>
    {
        new Ticket(),
        new Ticket(),
    };

            // Act
            var raioEntity = new RAIOEntity(ticketTCI, ticketTAI);

            // Assert
            Assert.That(raioEntity.TicketTCI, Is.EqualTo(ticketTCI));
            Assert.That(raioEntity.TicketTAI, Is.EqualTo(ticketTAI));
        }

    }
}