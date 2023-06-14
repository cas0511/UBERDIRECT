using PH.UberConnect.Core.Connection;
using PH.UberConnect.Core.Repositories;

namespace PH.UberConnect.UnitTesting
{
    [TestClass]
    public class LocalesTest
    {
        private LocalesRepository localesRepository = new(new DapperConnection(""));

        [TestMethod]
        public void GetTiempoTotal_ShouldCalculateTiempoTotalCorrectly()
        {
            // Arrange
            var dropoffReadyHour = new DateTime(2023, 5, 7, 21, 44, 8);
            var waitingHour = new DateTime(2023, 5, 7, 21, 20, 0);
            var preparationTime = 6;
            var waitingTime = 5;
            var expectedTiempoTotal = 35; // Assuming the expected value is in minutes

            // Act
            var tiempoTotal = localesRepository.GetTiempoTotal(dropoffReadyHour, waitingHour, preparationTime, waitingTime);

            // Assert
            Assert.AreEqual(expectedTiempoTotal, tiempoTotal);
        }
    }
}