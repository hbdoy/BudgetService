using System;
using System.Collections.Generic;
using Moq;
using Xunit;

namespace BudgetService.UnitTest
{
    public class BudgetServiceTests
    {
        [Fact]
        public void SameDay()
        {
            var mockBudgetRepo = new Mock<IBudgetRepository>();
            mockBudgetRepo.Setup(m => m.GetAll())
                .Returns(new List<Budget>()
                {
                    new Budget("202111", 3000)
                });

            var service = new BudgetService(mockBudgetRepo.Object);

            var result =
                service.Query(new DateTime(2021, 11, 1), new DateTime(2021, 11, 1));

            Assert.Equal(100, result);
        }
    }
}
