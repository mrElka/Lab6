using System;

namespace Lab6
{
    // Базовый абстрактный класс сотрудника
    public abstract class Employee
    {
        public string Name { get; set; }
        public decimal MonthlySalary { get; set; }

        public Employee(string name, decimal monthlySalary)
        {
            Name = name;
            MonthlySalary = monthlySalary;
        }

        // Метод для получения информации о сотруднике
        public abstract string GetInfo();

        // Метод для расчета зарплаты (будет использовать стратегию)
        public decimal CalculateSalary(IPaymentStrategy paymentStrategy)
        {
            return paymentStrategy.CalculateFinalSalary(MonthlySalary);
        }
    }

    // Конкретные классы сотрудников
    public class Researcher : Employee
    {
        public Researcher(string name, decimal monthlySalary)
            : base(name, monthlySalary) { }

        public override string GetInfo()
        {
            return $"Сотрудник: {Name}\nДолжность: Исследователь";
        }
    }

    public class LabAssistant : Employee
    {
        public LabAssistant(string name, decimal monthlySalary)
            : base(name, monthlySalary) { }

        public override string GetInfo()
        {
            return $"Сотрудник: {Name}\nДолжность: Лаборант";
        }
    }

    public class ProjectManager : Employee
    {
        public ProjectManager(string name, decimal monthlySalary)
            : base(name, monthlySalary) { }

        public override string GetInfo()
        {
            return $"Сотрудник: {Name}\nДолжность: Менеджер проекта";
        }
    }

    public class DataAnalyst : Employee
    {
        public DataAnalyst(string name, decimal monthlySalary)
            : base(name, monthlySalary) { }

        public override string GetInfo()
        {
            return $"Сотрудник: {Name}\nДолжность: Аналитик данных";
        }
    }
}