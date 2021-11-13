using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetService
{
    public class BudgetService
    {
        private readonly IBudgetRepository _repository;

        public BudgetService(IBudgetRepository repository)
        {
            _repository = repository;
        }

        public decimal Query(DateTime startDateTime, DateTime endDateTime)
        {
            return 0;
        }
    }
}
