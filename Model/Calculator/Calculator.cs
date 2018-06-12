using System;

namespace SalaryCalculator
{
    public class Calculator
    {
        double experienceBonusPercentage;
        double maxExperienceBonusPercentage;

        Func<Employee, DateTime, double> governanceBonusCalculator;

        public Calculator(double experienceBonusPercentage, double maxExperienceBonusPercentage,
            Func<Employee, DateTime, double> governanceBonusCalculator)
        {
            if (experienceBonusPercentage < 0 || maxExperienceBonusPercentage < 0)
                throw new ArgumentException("Процент премии не может быть отрицательным");

            this.experienceBonusPercentage = experienceBonusPercentage;
            this.maxExperienceBonusPercentage = maxExperienceBonusPercentage;
            this.governanceBonusCalculator = governanceBonusCalculator;
        }

        public double Calculate(Employee employee, DateTime date)
        {
            var hireDate = new DateTime(employee.HireDate.Year, employee.HireDate.Month, employee.HireDate.Day);
            if (date < hireDate) return 0;

            return employee.BaseSalary 
                + CalculateExperienceBonus(employee, date) 
                + governanceBonusCalculator(employee, date);
        }

        double CalculateExperienceBonus(Employee employee, DateTime date)
        {
            var premium = employee.BaseSalary * experienceBonusPercentage * employee.GetExperience(date);
            var maxPremium = employee.BaseSalary * maxExperienceBonusPercentage;
            return Math.Min(premium, maxPremium);
        }
    }
}
