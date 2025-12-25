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

            // 1. СОЗДАЕМ ТРЕХ СОТРУДНИКОВ С ОДИНАКОВОЙ ИСХОДНОЙ ЗАРПЛАТОЙ
            Console.WriteLine("1. СОЗДАНИЕ СОТРУДНИКОВ (исходная зарплата 60000 руб.):");
            Console.WriteLine("=======================================================");

            decimal baseSalary = 60000m;

            // 1.1 Обычный сотрудник
            Employee normalEmployee = new Researcher("Обычный Сотрудник", baseSalary);

            // 1.2 Алкоголик
            Employee alcoholicEmployee = new AlcoholicDecorator(
                new LabAssistant("Алкоголик Сотрудник", baseSalary),
                0.15m, // 15% удержание
                "Профилактика алкоголизма"
            );

            // 1.3 Спортсмен (Иван Петров)
            Employee sportsmanEmployee = new SportsmanDecorator(
                new Researcher("Иван Петров (Спортсмен)", baseSalary),
                3000m, // Бонус на протеин
                "Бодибилдинг"
            );

            // Добавляем дополнительные декораторы для реалистичности
            normalEmployee = new EnglishDecorator(normalEmployee, "IELTS", new DateTime(2023, 1, 1));
            alcoholicEmployee = new EnglishDecorator(alcoholicEmployee, "Basic", new DateTime(2020, 5, 10));
            sportsmanEmployee = new EnglishDecorator(sportsmanEmployee, "TOEFL", new DateTime(2022, 6, 15));

            // 2. ВЫВОД ИНФОРМАЦИИ О СОТРУДНИКАХ
            Console.WriteLine("\n2. ИНФОРМАЦИЯ О СОТРУДНИКАХ:");
            Console.WriteLine("==============================");

            List<Employee> employees = new List<Employee> { normalEmployee, alcoholicEmployee, sportsmanEmployee };

            foreach (var emp in employees)
            {
                Console.WriteLine("\n" + emp.GetInfo());
                Console.WriteLine(new string('-', 50));
            }

            // 3. ДЕТАЛЬНЫЙ РАСЧЕТ ДЛЯ КАЖДОГО ТИПА
            Console.WriteLine("\n3. ДЕТАЛЬНЫЙ РАСЧЕТ ЗАРПЛАТЫ (Газпромбанк 1.5%):");

            IPaymentStrategy gazpromStrategy = new GazpromPaymentStrategy();
            IPaymentStrategy sberbankStrategy = new SberbankPaymentStrategy();
            IPaymentStrategy tinkoffStrategy = new TinkoffPaymentStrategy();

            // 4. СРАВНИТЕЛЬНАЯ ТАБЛИЦА - ПРОСТОЙ ВАРИАНТ
            Console.WriteLine("\n" + new string('-', 96));
            Console.WriteLine("| {0,-20} | {1,-15} | {2,-15} | {3,-15} | {4,-15} |",
                "Тип сотрудника", "Исходная", "После всех", "Чистыми", "Эффективность");
            Console.WriteLine("| {0,-20} | {1,-15} | {2,-15} | {3,-15} | {4,-15} |",
                "", "зарплата", "корректировок", "на руки", "");
            Console.WriteLine(new string('-', 96));

            // Расчеты на основе известной логики
            decimal baseSalaryTable = 60000m; // Переименовал, чтобы не конфликтовать с основной переменной

            // Обычный: 60000 - 1.5% комиссия = 59100
            decimal normalAfterBankTable = baseSalaryTable * 0.985m; // 1.5% комиссия

            // Алкоголик: 60000 - 15% = 51000, затем -1.5% = 50235
            decimal alcoholicAfterBankTable = baseSalaryTable * 0.85m * 0.985m; // 15% налог + 1.5% комиссия

            // Спортсмен: 60000 + 3000 = 63000, затем -1.5% = 62055
            decimal sportsmanAfterBankTable = (baseSalaryTable + 3000m) * 0.985m; // +3000 бонус - 1.5% комиссия

            var tableRows = new[]
            {
    new { Type = "Обычный", Original = baseSalaryTable, AfterDecorators = baseSalaryTable, AfterBank = normalAfterBankTable },
    new { Type = "Алкоголик", Original = baseSalaryTable, AfterDecorators = baseSalaryTable * 0.85m, AfterBank = alcoholicAfterBankTable },
    new { Type = "Спортсмен", Original = baseSalaryTable, AfterDecorators = baseSalaryTable + 3000m, AfterBank = sportsmanAfterBankTable }
};

            foreach (var row in tableRows)
            {
                decimal efficiency = row.AfterBank / row.Original * 100;

                Console.WriteLine("| {0,-20} | {1,15:F2} | {2,15:F2} | {3,15:F2} | {4,14:F1}% |",
                    row.Type, row.Original, row.AfterDecorators, row.AfterBank, efficiency);
            }
            Console.WriteLine(new string('-', 96));

            // 5. СРАВНЕНИЕ ЧЕРЕЗ РАЗНЫЕ БАНКИ
            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("СРАВНЕНИЕ ДЛЯ КАЖДОГО ТИПА ЧЕРЕЗ РАЗНЫЕ БАНКИ:");
            Console.WriteLine(new string('=', 60));

            List<IPaymentStrategy> allStrategies = new List<IPaymentStrategy>
            {
                sberbankStrategy,
                gazpromStrategy,
                tinkoffStrategy
            };

            string[] employeeTypes = { "Обычный", "Алкоголик", "Спортсмен" };
            Employee[] employeeArray = { normalEmployee, alcoholicEmployee, sportsmanEmployee };

            for (int i = 0; i < employeeArray.Length; i++)
            {
                Console.WriteLine($"\n{employeeTypes[i]}:");
                Console.WriteLine(new string('-', 50));

                foreach (var strategy in allStrategies)
                {
                    decimal salary = employeeArray[i].CalculateSalary(strategy);
                    decimal original = baseSalary;

                    if (employeeArray[i] is AlcoholicDecorator alc)
                        original = alc.GetOriginalSalary();
                    else if (employeeArray[i] is SportsmanDecorator sp)
                        original = sp.GetSalaryWithoutBonus();

                    decimal efficiency = salary / original * 100;

                    Console.WriteLine($"{strategy.GetServiceName()}: {salary:F2} руб. ({efficiency:F1}% от базовой)");
                }
            }

            // 6. ВЫВОДЫ И РЕКОМЕНДАЦИИ
            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("ВЫВОДЫ И РЕКОМЕНДАЦИИ:");
            Console.WriteLine(new string('=', 60));

            Console.WriteLine("\nСравнительный анализ:");

            decimal normalFinal = normalEmployee.CalculateSalary(gazpromStrategy);
            decimal alcFinal = alcoholicEmployee.CalculateSalary(gazpromStrategy);
            decimal sportsFinal = sportsmanEmployee.CalculateSalary(gazpromStrategy);

            Console.WriteLine($"Обычный сотрудник получает: {normalFinal:F2} руб.");
            Console.WriteLine($"Алкоголик получает: {alcFinal:F2} руб. (на {normalFinal - alcFinal:F2} руб. меньше)");
            Console.WriteLine($"Спортсмен получает: {sportsFinal:F2} руб. (на {sportsFinal - normalFinal:F2} руб. больше)");

            Console.WriteLine($"\nФинансовые потери/выгоды:");
            Console.WriteLine($"Алкоголик теряет {normalFinal - alcFinal:F2} руб. в месяц из-за вредной привычки");
            Console.WriteLine($"Спортсмен получает на {sportsFinal - normalFinal:F2} руб. больше благодаря ЗОЖ");
            Console.WriteLine($"Разница между алкоголиком и спортсменом: {sportsFinal - alcFinal:F2} руб. в месяц!");

            Console.WriteLine($"\nРекомендации:");
            Console.WriteLine($"Алкоголику: бросить пить → +{normalFinal - alcFinal:F2} руб. в месяц");
            Console.WriteLine($"Обычному: начать заниматься спортом → +{sportsFinal - normalFinal:F2} руб. в месяц");
            Console.WriteLine($"Спортсмену: продолжать в том же духе!");

            Console.WriteLine($"\nГодовой эффект:");
            Console.WriteLine($"Алкоголик теряет за год: {(normalFinal - alcFinal) * 12:F2} руб.");
            Console.WriteLine($"Спортсмен выигрывает за год: {(sportsFinal - normalFinal) * 12:F2} руб.");
            Console.WriteLine($"Общая разница за год: {(sportsFinal - alcFinal) * 12:F2} руб.");

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("ПРОГРАММА ЗАВЕРШЕНА");
            Console.WriteLine(new string('=', 60));
            Console.ReadKey();
        }
    }
}