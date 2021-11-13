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

            

            // 處理 SDate 零碎、處理 EDate 零碎
            if (startDateTime.ToString("yyyyMM") == endDateTime.ToString("yyyyMM"))
            {
                int diff = (endDateTime - startDateTime).Days + 1;
                return GetBudgetOfThatMonth(startDateTime, diff);
            }
            else
            {
                //處理頭尾
                int headDays = DateTime.DaysInMonth(startDateTime.Year, startDateTime.Month) - startDateTime.Day + 1;
                int tailDays = endDateTime.Day;
                

                //處理中間
                DateTime currentMonth = startDateTime.AddMonths(1);
                decimal sumBudget = 0;
                while (currentMonth.Month < endDateTime.Month && currentMonth.Year <= endDateTime.Year)
                {
                    sumBudget += (_allBudgets.SingleOrDefault(x => x.YearMonth == currentMonth.ToString("yyyyMM"))?.Amount ?? 0);
                    currentMonth = currentMonth.AddMonths(1);
                }
                return GetBudgetOfThatMonth(startDateTime, headDays) + sumBudget + GetBudgetOfThatMonth(endDateTime, tailDays);
            }
        }

        private decimal GetBudgetOfThatMonth(DateTime yyyymm, int days)
        {
            var budget = _allBudgets.SingleOrDefault(x => x.YearMonth == yyyymm.ToString("yyyyMM"));
            var daysInMonth = DateTime.DaysInMonth(yyyymm.Year, yyyymm.Month);
            return (decimal)(days * ((budget?.Amount ?? 0) / daysInMonth));
        }
    }
}
