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

        [Fact]
        public void InvalidDateInputWillReturnZero()
        {
            var mockBudgetRepo = new Mock<IBudgetRepository>();
            mockBudgetRepo.Setup(m => m.GetAll())
                .Returns(new List<Budget>()
                {
                    new Budget("202111", 3000)
                });

            var service = new BudgetService(mockBudgetRepo.Object);

            var result =
                service.Query(new DateTime(2021, 11, 14), new DateTime(2021, 11, 1));

            Assert.Equal(0, result);
        }
        [Fact]
        public void CrossMonth()
        {
            var mockBudgetRepo = new Mock<IBudgetRepository>();
            mockBudgetRepo.Setup(m => m.GetAll())
                .Returns(new List<Budget>()
                {
                    new Budget("201902", 2800),
                    new Budget("201903", 6200)
                });

            var service = new BudgetService(mockBudgetRepo.Object);

            var result =
                service.Query(new DateTime(2019, 2, 27), new DateTime(2019, 3, 2));

            Assert.Equal(600, result);
        }
        [Fact]
        public void CrossMultiMonth()
        {
            var mockBudgetRepo = new Mock<IBudgetRepository>();
            mockBudgetRepo.Setup(m => m.GetAll())
                .Returns(new List<Budget>()
                {
                    new Budget("201902", 2800),
                    new Budget("201903", 31),
                    new Budget("201904", 6000)
                });

            var service = new BudgetService(mockBudgetRepo.Object);

            var result =
                service.Query(new DateTime(2019, 2, 27), new DateTime(2019, 4, 2));

            Assert.Equal(631, result);
        }
        [Fact]
        public void CrossMonthWithEmpty()
        {
            var mockBudgetRepo = new Mock<IBudgetRepository>();
            mockBudgetRepo.Setup(m => m.GetAll())
                .Returns(new List<Budget>()
                {
                    new Budget("201902", 2800),
                    new Budget("201904", 6000)
                });

            var service = new BudgetService(mockBudgetRepo.Object);

            var result =
                service.Query(new DateTime(2019, 2, 27), new DateTime(2019, 4, 2));

            Assert.Equal(600, result);
        }

        [Fact]
        public void EmptyMonth()
        {
            var mockBudgetRepo = new Mock<IBudgetRepository>();
            mockBudgetRepo.Setup(m => m.GetAll())
                .Returns(new List<Budget>()
                {
                });

            var service = new BudgetService(mockBudgetRepo.Object);

            var result =
                service.Query(new DateTime(2019, 2, 27), new DateTime(2019, 2, 28));

            Assert.Equal(0, result);
        }

    }
}
