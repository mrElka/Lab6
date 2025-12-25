using System;
using System.Collections.Generic;

namespace Lab6
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

            // Создаем список для хранения сотрудников
            List<Employee> employees = new List<Employee>();

            // 1. СОЗДАНИЕ СОТРУДНИКОВ РАЗНЫХ ДОЛЖНОСТЕЙ
            Console.WriteLine("1. СОЗДАНИЕ СОТРУДНИКОВ:");
            Console.WriteLine("------------------------");

            Employee researcher = new Researcher("Иван Петров", 75000m);
            Employee labAssistant = new LabAssistant("Мария Сидорова", 45000m);
            Employee projectManager = new ProjectManager("Алексей Иванов", 90000m);
            Employee dataAnalyst = new DataAnalyst("Елена Кузнецова", 68000m);

            employees.Add(researcher);
            employees.Add(labAssistant);
            employees.Add(projectManager);
            employees.Add(dataAnalyst);

            foreach (var emp in employees)
            {
                Console.WriteLine(emp.GetInfo());
                Console.WriteLine($"Оклад: {emp.MonthlySalary} руб.");
                Console.WriteLine();
            }

            // 2. ПРИМЕНЕНИЕ ДЕКОРАТОРОВ
            Console.WriteLine("\n2. ДОБАВЛЕНИЕ ХАРАКТЕРИСТИК ЧЕРЕЗ ДЕКОРАТОРЫ:");
            Console.WriteLine("---------------------------------------------");

            // Создаем список декорированных сотрудников
            List<Employee> decoratedEmployees = new List<Employee>();

            // Исследователь: английский + ученая степень
            Employee decoratedResearcher = new EnglishDecorator(
                new DegreeDecorator(researcher, "Физические науки",
                    "Исследование квантовых эффектов в наноматериалах", 2020),
                "TOEFL iBT", new DateTime(2022, 6, 10)
            );
            decoratedEmployees.Add(decoratedResearcher);

            // Лаборант: только английский
            Employee decoratedLabAssistant = new EnglishDecorator(
                labAssistant, "IELTS General", new DateTime(2023, 3, 15)
            );
            decoratedEmployees.Add(decoratedLabAssistant);

            // Менеджер проекта: ученая степень + конференция
            Employee decoratedManager = new ConferenceDecorator(
                new DegreeDecorator(projectManager, "Экономические науки",
                    "Управление распределенными научными проектами", 2019),
                "Международная конференция по управлению проектами",
                "Agile в научных исследованиях", 2022
            );
            decoratedEmployees.Add(decoratedManager);

            // Аналитик данных: все три декоратора
            Employee decoratedAnalyst = new EnglishDecorator(
                new DegreeDecorator(
                    new ConferenceDecorator(dataAnalyst, "Data Science Conference",
                        "Машинное обучение для обработки экспериментальных данных", 2023),
                    "Информационные технологии",
                    "Алгоритмы анализа больших данных в реальном времени", 2021),
                "Cambridge English: Advanced", new DateTime(2021, 11, 5)
            );
            decoratedEmployees.Add(decoratedAnalyst);

            // Выводим информацию о декорированных сотрудниках
            Console.WriteLine("\nИнформация о сотрудниках после добавления характеристик:");
            Console.WriteLine("==========================================================");

            foreach (var emp in decoratedEmployees)
            {
                Console.WriteLine("\n" + emp.GetInfo());
                Console.WriteLine($"Оклад: {emp.MonthlySalary} руб.");
                Console.WriteLine("----------------------------------------------------------");
            }

            // 3. ИСПОЛЬЗОВАНИЕ СТРАТЕГИЙ ДЛЯ РАСЧЕТА ЗАРПЛАТЫ
            Console.WriteLine("\n3. РАСЧЕТ ЗАРПЛАТЫ С ИСПОЛЬЗОВАНИЕМ СТРАТЕГИЙ:");
            Console.WriteLine("==============================================");

            // Создаем стратегии
            IPaymentStrategy sberbankStrategy = new SberbankPaymentStrategy();
            IPaymentStrategy gazpromStrategy = new GazpromPaymentStrategy();
            IPaymentStrategy tinkoffStrategy = new TinkoffPaymentStrategy();
            IPaymentStrategy testStrategy = new NoCommissionPaymentStrategy();

            // Создаем словарь для сопоставления сотрудников и их стратегий оплаты
            Dictionary<Employee, IPaymentStrategy> employeePaymentStrategies = new Dictionary<Employee, IPaymentStrategy>
            {
                { decoratedResearcher, sberbankStrategy },
                { decoratedLabAssistant, gazpromStrategy },
                { decoratedManager, sberbankStrategy },
                { decoratedAnalyst, tinkoffStrategy }
            };

            // Расчет зарплаты для каждого сотрудника
            Console.WriteLine("\nРасчет заработной платы:");
            Console.WriteLine("-------------------------");

            decimal totalSalaryBefore = 0;
            decimal totalSalaryAfter = 0;

            foreach (var kvp in employeePaymentStrategies)
            {
                Employee emp = kvp.Key;
                IPaymentStrategy strategy = kvp.Value;

                decimal salaryBefore = emp.MonthlySalary;
                decimal salaryAfter = emp.CalculateSalary(strategy);
                decimal commission = salaryBefore - salaryAfter;

                totalSalaryBefore += salaryBefore;
                totalSalaryAfter += salaryAfter;

                Console.WriteLine($"\n{emp.Name}:");
                Console.WriteLine($"  Оклад: {salaryBefore} руб.");
                Console.WriteLine($"  Сервис: {strategy.GetServiceName()}");
                Console.WriteLine($"  Комиссия: {commission:F2} руб.");
                Console.WriteLine($"  К выплате: {salaryAfter:F2} руб.");
            }

            Console.WriteLine("\n----------------------------------------------------------");
            Console.WriteLine($"ИТОГО:");
            Console.WriteLine($"  Общая сумма окладов: {totalSalaryBefore} руб.");
            Console.WriteLine($"  Общая сумма к выплате: {totalSalaryAfter:F2} руб.");
            Console.WriteLine($"  Общая комиссия банков: {totalSalaryBefore - totalSalaryAfter:F2} руб.");

            // 4. ДЕМОНСТРАЦИЯ СМЕНЫ СТРАТЕГИИ "НА ЛЕТУ"
            Console.WriteLine("\n4. ДЕМОНСТРАЦИЯ СМЕНЫ СТРАТЕГИИ:");
            Console.WriteLine("================================");

            Employee testEmployee = decoratedResearcher;
            Console.WriteLine($"\nТестируем смену стратегии для: {testEmployee.Name}");
            Console.WriteLine($"Оклад: {testEmployee.MonthlySalary} руб.");

            Console.WriteLine("\nСравнение разных банковских сервисов:");
            Console.WriteLine("--------------------------------------");

            List<IPaymentStrategy> allStrategies = new List<IPaymentStrategy>
            {
                testStrategy,
                sberbankStrategy,
                gazpromStrategy,
                tinkoffStrategy
            };

            foreach (var strategy in allStrategies)
            {
                decimal finalSalary = testEmployee.CalculateSalary(strategy);
                decimal commission = testEmployee.MonthlySalary - finalSalary;

                Console.WriteLine($"  {strategy.GetServiceName()}:");
                Console.WriteLine($"    Комиссия: {commission:F2} руб.");
                Console.WriteLine($"    К выплате: {finalSalary:F2} руб.");
            }

            // 5. ДОПОЛНИТЕЛЬНЫЕ ПРИМЕРЫ И ТЕСТЫ
            Console.WriteLine("\n5. ДОПОЛНИТЕЛЬНЫЕ ПРИМЕРЫ:");
            Console.WriteLine("==========================");

            // Создаем нового сотрудника без декораторов
            Console.WriteLine("\nПример сотрудника без дополнительных характеристик:");
            Employee simpleEmployee = new LabAssistant("Петр Сергеев", 40000m);
            Console.WriteLine(simpleEmployee.GetInfo());
            Console.WriteLine($"Зарплата через Газпромбанк: {simpleEmployee.CalculateSalary(gazpromStrategy):F2} руб.");

            // Добавляем декораторы динамически
            Console.WriteLine("\nДинамическое добавление характеристик:");
            Employee dynamicEmployee = new DataAnalyst("Ольга Новикова", 55000m);
            Console.WriteLine("Исходная информация:");
            Console.WriteLine(dynamicEmployee.GetInfo());

            dynamicEmployee = new EnglishDecorator(dynamicEmployee, "Test of English", DateTime.Now);
            Console.WriteLine("\nПосле добавления английского:");
            Console.WriteLine(dynamicEmployee.GetInfo());

            dynamicEmployee = new DegreeDecorator(dynamicEmployee, "Математика", "Статистические методы", 2023);
            Console.WriteLine("\nПосле добавления ученой степени:");
            Console.WriteLine(dynamicEmployee.GetInfo());

            // 6. ЗАКЛЮЧЕНИЕ
            Console.WriteLine("\n6. ВЫВОДЫ:");
            Console.WriteLine("==========");
            Console.WriteLine("\n✓ Паттерн 'Декоратор' позволяет динамически добавлять");
            Console.WriteLine("  новые характеристики сотрудникам без изменения их классов.");
            Console.WriteLine("\n✓ Паттерн 'Стратегия' позволяет легко менять алгоритм");
            Console.WriteLine("  расчета зарплаты и добавлять новые банковские сервисы.");
            Console.WriteLine("\n✓ Оба паттерна соответствуют принципу Open/Closed:");
            Console.WriteLine("  система открыта для расширения, но закрыта для модификации.");

            Console.WriteLine("\n======================================================");
            Console.WriteLine("ПРОГРАММА ЗАВЕРШЕНА. НАЖМИТЕ ЛЮБУЮ КЛАВИШУ ДЛЯ ВЫХОДА.");
            Console.WriteLine("======================================================");
            Console.ReadKey();
        }
    }
}