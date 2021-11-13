using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetService
{
    public class BudgetService
    {
        private readonly IBudgetRepository _repository;
        private List<Budget> _allBudgets;

        public BudgetService(IBudgetRepository repository)
        {
            _repository = repository;

            _allBudgets = _repository.GetAll();
        }

        public decimal Query(DateTime startDateTime, DateTime endDateTime)
        {

            if (endDateTime < startDateTime)
            {
                return 0;
            }
            
            if (startDateTime.ToString("yyyyMM") == endDateTime.ToString("yyyyMM"))
            {
                var diff = (endDateTime - startDateTime).Days + 1;
                return GetBudgetOfThatMonth(startDateTime, diff);
            }

            //處理頭尾
            var headDays = DateTime.DaysInMonth(startDateTime.Year, startDateTime.Month) - startDateTime.Day + 1;
            var tailDays = endDateTime.Day;

            //處理中間
            var currentMonth = startDateTime.AddMonths(1);
            decimal sumBudget = 0;
            while (currentMonth.Month < endDateTime.Month && currentMonth.Year <= endDateTime.Year)
            {
                sumBudget += (_allBudgets.SingleOrDefault(x => x.YearMonth == currentMonth.ToString("yyyyMM"))?.Amount ?? 0);
                currentMonth = currentMonth.AddMonths(1);
            }

            return GetBudgetOfThatMonth(startDateTime, headDays) + sumBudget + GetBudgetOfThatMonth(endDateTime, tailDays);
        }

        private decimal GetBudgetOfThatMonth(DateTime currentDateTime, int days)
        {
            var budget = _allBudgets.SingleOrDefault(x => x.YearMonth == currentDateTime.ToString("yyyyMM"));
            var daysInMonth = DateTime.DaysInMonth(currentDateTime.Year, currentDateTime.Month);
            return days * ((budget?.Amount ?? 0) / daysInMonth);
        }
    }
}
