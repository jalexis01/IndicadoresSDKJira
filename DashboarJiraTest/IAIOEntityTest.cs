using DashboarJira.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboarJiraTest
{
    [TestFixture]
    public class IAIOEntityTests
    {
        [Test]
        public void CalcularIndicadorIAIO_NoAIO_ShouldReturn100()
        {
            // Arrange
            var AIO_POR_PUERTA = new List<List<Ticket>> { new List<Ticket>(), new List<Ticket>() };
            var total_puertas = 2;
            var iaioEntity = new IAIOEntity(AIO_POR_PUERTA, total_puertas);
            var expectedIAIO = 100;

            // Act
            var iaio = iaioEntity.CalcularIndicadorIAIO();

            // Assert
            Assert.That(iaio, Is.EqualTo(expectedIAIO));
        }

        [Test]
        public void CalcularIndicadorIAIO_OneAIO_ShouldReturn90()
        {
            // Arrange
            var AIO_POR_PUERTA = new List<List<Ticket>> { new List<Ticket> { new Ticket() }, new List<Ticket>() };
            var total_puertas = 2;
            var iaioEntity = new IAIOEntity(AIO_POR_PUERTA, total_puertas);
            var expectedIAIO = 90;

            // Act
            var iaio = iaioEntity.CalcularIndicadorIAIO();

            // Assert
            Assert.That(iaio, Is.EqualTo(expectedIAIO));
        }

        [Test]
        public void CalcularIndicadorIAIO_TwoAIO_ShouldReturn40()
        {
            // Arrange
            var AIO_POR_PUERTA = new List<List<Ticket>> { new List<Ticket> { new Ticket() }, new List<Ticket> { new Ticket() } };
            var total_puertas = 2;
            var iaioEntity = new IAIOEntity(AIO_POR_PUERTA, total_puertas);
            var expectedIAIO = 40;

            // Act
            var iaio = iaioEntity.CalcularIndicadorIAIO();

            // Assert
            Assert.That(iaio, Is.EqualTo(expectedIAIO));
        }

        [Test]
        public void CalcularIndicadorIAIO_ThreeOrMoreAIO_ShouldReturn0()
        {
            // Arrange
            var AIO_POR_PUERTA = new List<List<Ticket>> { new List<Ticket> { new Ticket() }, new List<Ticket> { new Ticket() }, new List<Ticket> { new Ticket() } };
            var total_puertas = 3;
            var iaioEntity = new IAIOEntity(AIO_POR_PUERTA, total_puertas);
            var expectedIAIO = 0;

            // Act
            var iaio = iaioEntity.CalcularIndicadorIAIO();

            // Assert
            Assert.That(iaio, Is.EqualTo(expectedIAIO));
        }
    }

}
