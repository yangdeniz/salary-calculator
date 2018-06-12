using System;
using NUnit.Framework;

namespace SalaryCalculator
{
    [TestFixture]
    public class CalculatorTests
    {
        [Test]
        public void IncorrectBonusPercentageTest()
        {
            Assert.Throws(typeof(ArgumentException), () => new Calculator(-0.01, 0.5, (employee, date) => 0));
            Assert.Throws(typeof(ArgumentException), () => new Calculator(0.01, -0.5, (employee, date) => 0));
        }

        [Test]
        public void SalaryWithoutBonusTest()
        {
            var calculator = new Calculator(0, 0, (employee, date) => 0);
            Test(1000, new DateTime(2010, 6, 1), calculator);
            Test(1000, new DateTime(2015, 1, 1), calculator);
        }

        [Test]
        public void CalculationDateBeforeHireDateTest()
        {
            var calculator = new Calculator(0, 0, (employee, date) => 0);
            Test(0, new DateTime(2000, 1, 1), calculator);
        }

        [Test]
        public void SalaryWithExperienceBonusTest()
        {
            var calculator = new Calculator(0.1, 0.5, (employee, date) => 0);
            Test(1000, new DateTime(2010, 6, 1), calculator);
            Test(1100, new DateTime(2011, 1, 1), calculator);
            Test(1500, new DateTime(2015, 1, 1), calculator);
            Test(1500, new DateTime(2020, 1, 1), calculator);
        }

        public void Test(double salary, DateTime date, Calculator calculator)
        {
            var employee = new Employee("John", new DateTime(2010, 1, 1), EmployeeGroup.Employee, 1000);
            Assert.AreEqual(salary, calculator.Calculate(employee, date));
        }
    }
}
