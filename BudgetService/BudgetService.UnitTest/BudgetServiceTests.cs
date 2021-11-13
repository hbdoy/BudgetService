using System;
using System.Collections.Generic;
using Moq;
using Xunit;

namespace BudgetService.UnitTest
{
    public class BudgetServiceTests
    {
        BudgetService _service;
        private readonly Mock<IBudgetRepository> _mockBudgetRepo = new Mock<IBudgetRepository>();

        public BudgetServiceTests()
        {
            _mockBudgetRepo.Setup(m => m.GetAll())
                .Returns(new List<Budget>()
                {
                    new Budget("201902", 2800),
                    new Budget("201903", 6200),
                    new Budget("201904", 6000),
                    new Budget("201906", 3000),
                    new Budget("202002", 2900),
                    new Budget("202003", 6200),
                    new Budget("202103", 6200),
                    new Budget("202111", 3000),
                });
        }

        [Fact]
        public void SameDay()
        {
            _service = new BudgetService(_mockBudgetRepo.Object);

            BudgetShouldBe(new DateTime(2021, 11, 1), new DateTime(2021, 11, 1), 100);
        }

        [Fact]
        public void InvalidDateInputWillReturnZero()
        {
            _service = new BudgetService(_mockBudgetRepo.Object);

            BudgetShouldBe(new DateTime(2021, 11, 14), new DateTime(2021, 11, 1), 0);
        }

        [Fact]
        public void CrossMonth()
        {
            _service = new BudgetService(_mockBudgetRepo.Object);

            BudgetShouldBe(new DateTime(2019, 2, 27), new DateTime(2019, 3, 2), 600);
        }

        [Fact]
        public void CrossMultiMonth()
        {
            _service = new BudgetService(_mockBudgetRepo.Object);

            BudgetShouldBe(new DateTime(2019, 2, 27), new DateTime(2019, 4, 2), 6800);
        }

        [Fact]
        public void CrossMonthWithEmpty()
        {
            _service = new BudgetService(_mockBudgetRepo.Object);

            BudgetShouldBe(new DateTime(2019, 4, 27), new DateTime(2019, 6, 2), 1000);
        }

        [Fact]
        public void EmptyMonth()
        {
            _service = new BudgetService(_mockBudgetRepo.Object);

            BudgetShouldBe(new DateTime(2019, 1, 27), new DateTime(2019, 1, 28), 0);
        }
        
        [Fact]
        public void LeapYear()
        {
            _service = new BudgetService(_mockBudgetRepo.Object);

            BudgetShouldBe(new DateTime(2020, 2, 27), new DateTime(2020, 3, 2), 700);
        }

        [Fact]
        public void CrossYear()
        {
            _service = new BudgetService(_mockBudgetRepo.Object);

            BudgetShouldBe(new DateTime(2020, 2, 27), new DateTime(2021, 3, 2), 700);
        }

        private void BudgetShouldBe(DateTime startDateTime, DateTime endDateTime, int expected)
        {
            var result = _service.Query(startDateTime, endDateTime);

            Assert.Equal(expected, result);
        }
    }
}
