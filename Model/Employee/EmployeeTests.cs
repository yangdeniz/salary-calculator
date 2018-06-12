using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace SalaryCalculator
{
    [TestFixture]
    public class EmployeeTests
    {
        [Test]
        public void IncorrectBaseSalaryTest()
        {
            Assert.Throws(typeof(ArgumentException), () => new Employee("John", DateTime.Today, EmployeeGroup.Employee, 0));
            Assert.Throws(typeof(ArgumentException), () => new Employee("John", DateTime.Today, EmployeeGroup.Employee, -100));
        }

        [Test]
        public void ExperienceTest()
        {
            var employee = new Employee("John", new DateTime(2010, 6, 1), EmployeeGroup.Employee, 1000);
            Assert.AreEqual(0, employee.GetExperience(new DateTime(2011, 5, 31)));
            Assert.AreEqual(1, employee.GetExperience(new DateTime(2011, 6, 1)));
            Assert.AreEqual(5, employee.GetExperience(new DateTime(2015, 7, 1)));
            Assert.AreEqual(0, employee.GetExperience(new DateTime(2010, 5, 31)));
        }
        
        [Test]
        public void EmployeeSalaryTest()
        {
            var employee = new Employee("John", new DateTime(2010, 1, 1), EmployeeGroup.Employee, 1000);
            Assert.AreEqual(0, employee.CalculateSalary(new DateTime(2009, 12, 31)));
            Assert.AreEqual(1000, employee.CalculateSalary(new DateTime(2010, 6, 1)));
            Assert.AreEqual(1030, employee.CalculateSalary(new DateTime(2011, 1, 1)));
            Assert.AreEqual(1150, employee.CalculateSalary(new DateTime(2015, 1, 1)));
            Assert.AreEqual(1300, employee.CalculateSalary(new DateTime(2025, 1, 1)));
        }

        [Test]
        public void ManagerExperienceBonusTest()
        {
            var manager = new Employee("John", new DateTime(2010, 1, 1), EmployeeGroup.Manager, 1000);
            manager.Subordinates = new List<Employee>();
            Assert.AreEqual(0, manager.CalculateSalary(new DateTime(2009, 12, 31)));
            Assert.AreEqual(1000, manager.CalculateSalary(new DateTime(2010, 6, 1)));
            Assert.AreEqual(1050, manager.CalculateSalary(new DateTime(2011, 1, 1)));
            Assert.AreEqual(1250, manager.CalculateSalary(new DateTime(2015, 1, 1)));
            Assert.AreEqual(1400, manager.CalculateSalary(new DateTime(2025, 1, 1)));
        }
        
        [Test]
        public void ManagerSalaryTest()
        {
            var manager = new Employee("John", new DateTime(2010, 1, 1), EmployeeGroup.Manager, 1000);

            var employee1 = new Employee("Peter", new DateTime(2010, 1, 1), EmployeeGroup.Employee, 600);
            var employee2 = new Employee("Mary", new DateTime(2010, 1, 1), EmployeeGroup.Manager, 800);
            manager.Subordinates = new List<Employee>() { employee1, employee2 };

            var employee3 = new Employee("David", new DateTime(2010, 1, 1), EmployeeGroup.Employee, 400);
            employee2.Subordinates = new List<Employee>() { employee3 };

            // В тот же год работы: 1000 + 600 * 0,5% + (800 + 400 * 0,5%) * 0,5%
            Assert.AreEqual(1007.01, manager.CalculateSalary(new DateTime(2010, 2, 1)));
            // Через год работы: 1000 * 105% + 600 * 103% * 0,5% + (800 * 105% + 400 * 103% * 0,5%) * 0,5%
            Assert.AreEqual(1057.3003, manager.CalculateSalary(new DateTime(2011, 2, 1)));
            // Максимальная зарплата: 1000 * 140% + 600 * 130% * 0,5% + (800 * 140% + 400 * 130% * 0,5%) * 0,5%
            Assert.AreEqual(1409.513, manager.CalculateSalary(new DateTime(2025, 2, 1)));
        }
        
        [Test]
        public void SalesmanExperienceBonusTest()
        {
            var salesman = new Employee("John", new DateTime(2010, 1, 1), EmployeeGroup.Salesman, 1000);
            salesman.Subordinates = new List<Employee>();
            Assert.AreEqual(0, salesman.CalculateSalary(new DateTime(2009, 12, 31)));
            Assert.AreEqual(1000, salesman.CalculateSalary(new DateTime(2010, 6, 1)));
            Assert.AreEqual(1010, salesman.CalculateSalary(new DateTime(2011, 1, 1)));
            Assert.AreEqual(1050, salesman.CalculateSalary(new DateTime(2015, 1, 1)));
            Assert.AreEqual(1350, salesman.CalculateSalary(new DateTime(2055, 1, 1)));
        }
        
        [Test]
        public void SalesmanSalaryTest()
        {
            var salesman = new Employee("John", new DateTime(2010, 1, 1), EmployeeGroup.Salesman, 1000);

            var employee1 = new Employee("Peter", new DateTime(2010, 1, 1), EmployeeGroup.Employee, 600);
            var manager = new Employee("Mary", new DateTime(2010, 1, 1), EmployeeGroup.Manager, 800);
            salesman.Subordinates = new List<Employee>() { employee1, manager };

            var employee2 = new Employee("David", new DateTime(2010, 1, 1), EmployeeGroup.Employee, 400);
            manager.Subordinates = new List<Employee>() { employee2 };

            // В тот же год работы: 1000 + 600 * 0,3% + (800 + 400 * 0,5%) * 0,3% + 400 * 0,3%
            var firstYearSalary = salesman.CalculateSalary(new DateTime(2010, 2, 1));
            Assert.AreEqual(1005.406, salesman.CalculateSalary(new DateTime(2010, 2, 1)));
            // Через год работы: 1000 * 101% + 600 * 103% * 0,3% + (800 * 105% + 400 * 103% * 0,5%) * 0,3% + 400 * 103% * 0,3%
            var secondYearSalary = salesman.CalculateSalary(new DateTime(2011, 2, 1));
            Assert.AreEqual(1015.61618, salesman.CalculateSalary(new DateTime(2011, 2, 1)));
            // Максимальная зарплата: 1000 * 135% + 600 * 130% * 0,3% + (800 * 140% + 400 * 130% * 0,5%) * 0,3% + 400 * 130% * 0,3%
            var maxSalary = salesman.CalculateSalary(new DateTime(2050, 2, 1));
            Assert.AreEqual(1357.2678, salesman.CalculateSalary(new DateTime(2050, 2, 1)));
        }
    }
}
