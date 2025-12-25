using Lab6;
using System;
using System.Collections.Generic;

namespace Lab6Patterns
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("======================================================");
            Console.WriteLine("ПРАКТИЧЕСКАЯ РАБОТА №6");
            Console.WriteLine("Паттерны проектирования: Декоратор и Стратегия");
            Console.WriteLine("======================================================\n");

            List<Employee> employees = new List<Employee>();

            Console.WriteLine("1. СОЗДАНИЕ СОТРУДНИКОВ:");
            Console.WriteLine("------------------------");

            Employee researcher = new Researcher("Иван Петров", 75000m);
            Employee labAssistant = new LabAssistant("Мария Сидорова", 45000m);
            Employee projectManager = new ProjectManager("Алексей Иванов", 90000m);
            Employee dataAnalyst = new DataAnalyst("Елена Кузнецова", 68000m);

            Employee alcoholicEmployee = new LabAssistant("Петр Водкин", 50000m);

            employees.Add(researcher);
            employees.Add(labAssistant);
            employees.Add(projectManager);
            employees.Add(dataAnalyst);
            employees.Add(alcoholicEmployee);

            foreach (var emp in employees)
            {
                Console.WriteLine(emp.GetInfo());
                Console.WriteLine($"Оклад: {emp.MonthlySalary} руб.");
                Console.WriteLine();
            }

            Console.WriteLine("\n2. ДОБАВЛЕНИЕ ХАРАКТЕРИСТИК ЧЕРЕЗ ДЕКОРАТОРЫ:");
            Console.WriteLine("---------------------------------------------");

            List<Employee> decoratedEmployees = new List<Employee>();

            Employee decoratedResearcher = new EnglishDecorator(
                new DegreeDecorator(researcher, "Физические науки",
                    "Исследование квантовых эффектов в наноматериалах", 2020),
                "TOEFL iBT", new DateTime(2022, 6, 10)
            );
            decoratedEmployees.Add(decoratedResearcher);

            Employee decoratedLabAssistant = new EnglishDecorator(
                labAssistant, "IELTS General", new DateTime(2023, 3, 15)
            );
            decoratedEmployees.Add(decoratedLabAssistant);

            Employee decoratedManager = new ConferenceDecorator(
                new DegreeDecorator(projectManager, "Экономические науки",
                    "Управление распределенными научными проектами", 2019),
                "Международная конференция по управлению проектами",
                "Agile в научных исследованиях", 2022
            );
            decoratedEmployees.Add(decoratedManager);

            Employee decoratedAnalyst = new EnglishDecorator(
                new DegreeDecorator(
                    new ConferenceDecorator(dataAnalyst, "Data Science Conference",
                        "Машинное обучение для обработки экспериментальных данных", 2023),
                    "Информационные технологии",
                    "Алгоритмы анализа больших данных в реальном времени", 2021),
                "Cambridge English: Advanced", new DateTime(2021, 11, 5)
            );
            decoratedEmployees.Add(decoratedAnalyst);

            Employee decoratedAlcoholic = new AlcoholicDecorator(
                alcoholicEmployee,
                0.15m,
                "Удержание части зарплаты для ограничения расходов на алкогольные напитки"
            );


            decoratedAlcoholic = new EnglishDecorator(
                decoratedAlcoholic,
                "Basic English Certificate",
                new DateTime(2020, 5, 10)
            );

            decoratedEmployees.Add(decoratedAlcoholic);

            Console.WriteLine("\nИнформация о сотрудниках после добавления характеристик:");
            Console.WriteLine("==========================================================");

            foreach (var emp in decoratedEmployees)
            {
                Console.WriteLine("\n" + emp.GetInfo());

                if (emp is AlcoholicDecorator alcoholic)
                {
                    Console.WriteLine($"Оригинальный оклад (до вычета): {alcoholic.GetOriginalSalary():F2} руб.");
                }
                else
                {
                    Console.WriteLine($"Оклад: {emp.MonthlySalary} руб.");
                }

                Console.WriteLine("----------------------------------------------------------");
            }

            Console.WriteLine("\n3. РАСЧЕТ ЗАРПЛАТЫ С ИСПОЛЬЗОВАНИЕМ СТРАТЕГИЙ:");
            Console.WriteLine("==============================================");

            IPaymentStrategy sberbankStrategy = new SberbankPaymentStrategy();
            IPaymentStrategy gazpromStrategy = new GazpromPaymentStrategy();
            IPaymentStrategy tinkoffStrategy = new TinkoffPaymentStrategy();
            IPaymentStrategy testStrategy = new NoCommissionPaymentStrategy();

            Dictionary<Employee, IPaymentStrategy> employeePaymentStrategies = new Dictionary<Employee, IPaymentStrategy>
            {
                { decoratedResearcher, sberbankStrategy },
                { decoratedLabAssistant, gazpromStrategy },
                { decoratedManager, sberbankStrategy },
                { decoratedAnalyst, tinkoffStrategy },
                { decoratedAlcoholic, gazpromStrategy }
            };

            Console.WriteLine("\nРасчет заработной платы:");
            Console.WriteLine("-------------------------");

            decimal totalAlcoholTax = 0;

            foreach (var kvp in employeePaymentStrategies)
            {
                Employee emp = kvp.Key;
                IPaymentStrategy strategy = kvp.Value;

                decimal salaryBefore = emp.MonthlySalary;
                decimal salaryAfter = emp.CalculateSalary(strategy);
                decimal commission = salaryBefore - salaryAfter;
                decimal alcoholTax = 0;
                if (emp is AlcoholicDecorator alcoholic)
                {
                    decimal originalSalary = alcoholic.GetOriginalSalary();
                    alcoholTax = originalSalary - salaryBefore;
                    totalAlcoholTax += alcoholTax;

                    Console.WriteLine($"\n⚠️ {emp.Name} (АЛКОГОЛИК):");
                    Console.WriteLine($"  Оригинальный оклад: {originalSalary:F2} руб.");
                    Console.WriteLine($"  Налог на алкоголь ({alcoholTax / originalSalary * 100:F1}%): -{alcoholTax:F2} руб.");
                    Console.WriteLine($"  Оклад после налога: {salaryBefore:F2} руб.");
                }
                else
                {
                    Console.WriteLine($"\n{emp.Name}:");
                }

                Console.WriteLine($"  Сервис: {strategy.GetServiceName()}");
                Console.WriteLine($"  Комиссия банка: {commission:F2} руб.");
                Console.WriteLine($"  К выплате: {salaryAfter:F2} руб.");
            }

            if (decoratedAlcoholic is AlcoholicDecorator detailedAlcoholic)
            {
                decimal original = detailedAlcoholic.GetOriginalSalary();
                decimal afterAlcoholTax = detailedAlcoholic.MonthlySalary;
                decimal alcoholTaxAmount = original - afterAlcoholTax;

                Console.WriteLine($"1. Оригинальный оклад: {original:F2} руб.");
                Console.WriteLine($"2. Налог на алкоголь (15%): -{alcoholTaxAmount:F2} руб.");
                Console.WriteLine($"3. Остаток после налога: {afterAlcoholTax:F2} руб.");
                Console.WriteLine("\nСравнение банковских сервисов для алкоголика:");
                Console.WriteLine("---------------------------------------------");

                List<IPaymentStrategy> strategiesForAlcoholic = new List<IPaymentStrategy>
                {
                    testStrategy,
                    sberbankStrategy,
                    gazpromStrategy,
                    tinkoffStrategy
                };

                foreach (var strategy in strategiesForAlcoholic)
                {
                    decimal final = detailedAlcoholic.CalculateSalary(strategy);
                    decimal bankCommission = afterAlcoholTax - final;
                    decimal totalCommission = alcoholTaxAmount + bankCommission;
                    decimal totalPercentage = (totalCommission / original) * 100;

                    Console.WriteLine($"\n  {strategy.GetServiceName()}:");
                    Console.WriteLine($"    Налог на алкоголь: -{alcoholTaxAmount:F2} руб.");
                    Console.WriteLine($"    Комиссия банка: -{bankCommission:F2} руб.");
                    Console.WriteLine($"    Всего удержано: -{totalCommission:F2} руб. ({totalPercentage:F1}%)");
                    Console.WriteLine($"    К выплате: {final:F2} руб.");
                }
            }
        }
    }
}