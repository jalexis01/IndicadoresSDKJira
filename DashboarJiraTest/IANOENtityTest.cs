using DashboarJira.Model;


namespace DashboarJiraTest
{
    [TestFixture]
    public class IANOEntityTests
    {
        [Test]
        public void CalcularIndicadorIANO_ShouldReturnExpectedValue()
        {
            // Arrange
            var anioPorPuerta = new List<List<Ticket>>()
        {
            new List<Ticket>(){ new Ticket(), new Ticket() },
            new List<Ticket>(){ new Ticket() },
            new List<Ticket>(){ new Ticket(), new Ticket(), new Ticket() },
            new List<Ticket>(){ new Ticket(), new Ticket() },
            new List<Ticket>()
        };
            var totalPuertas = 5.0;
            var ianoEntity = new IANOEntity(anioPorPuerta, totalPuertas);

            // Act
            var result = ianoEntity.CalcularIndicadorIANO();

            // Assert
            var expected = 1.0;
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void pano_ShouldReturnExpectedValue()
        {
            // Arrange
            var anioPorPuerta = new List<List<Ticket>>()
        {
            new List<Ticket>(){ new Ticket(), new Ticket() },
            new List<Ticket>(){ new Ticket() },
            new List<Ticket>(){ new Ticket(), new Ticket(), new Ticket() },
            new List<Ticket>(){ new Ticket(), new Ticket() },
            new List<Ticket>()
        };
            var totalPuertas = 5.0;
            var ianoEntity = new IANOEntity(anioPorPuerta, totalPuertas);

            // Act
            var result = ianoEntity.pano();

            // Assert
            var expected = 5.0;
            Assert.That(result, Is.EqualTo(expected));
        }
    }

}
