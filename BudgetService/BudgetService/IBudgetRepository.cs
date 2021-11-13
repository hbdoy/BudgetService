using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetService
{
    public interface IBudgetRepository
    {
        public List<Budget> GetAll();
    }
}
