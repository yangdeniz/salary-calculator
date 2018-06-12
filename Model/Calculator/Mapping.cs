using System.Collections.Generic;
using System.Linq;

namespace SalaryCalculator
{
    public static class Mapping
    {
        public static Dictionary<EmployeeGroup, Calculator> Calculators = new Dictionary<EmployeeGroup, Calculator>()
        {
            {
                EmployeeGroup.Employee,
                new Calculator(0.03, 0.3, (employee, date) => 0)
            },
            {
                EmployeeGroup.Manager,
                new Calculator(0.05, 0.4, (employee, date) => 
                    employee.Subordinates
                    .Select(z => z.CalculateSalary(date) * 0.005)
                    .Sum())
            },
            {
                EmployeeGroup.Salesman,
                new Calculator(0.01, 0.35, (employee, date) => 
                {
                    double bonus = 0;
                    var queue = new Queue<Employee>();
                    foreach (var subordinate in employee.Subordinates)
                        queue.Enqueue(subordinate);
                    while (queue.Count > 0)
                    {
                        var worker = queue.Dequeue();
                        bonus += worker.CalculateSalary(date) * 0.003;
                        if (worker.CanBeChief)
                            foreach (var subordinate in worker.Subordinates)
                                queue.Enqueue(subordinate);
                    }
                    return bonus;
                })
            }
        };
    }
}
