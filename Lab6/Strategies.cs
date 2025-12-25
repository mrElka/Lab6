namespace Lab6
{
    // Интерфейс стратегии
    public interface IPaymentStrategy
    {
        decimal CalculateFinalSalary(decimal monthlySalary);
        string GetServiceName();
    }

    // Конкретные стратегии
    public class SberbankPaymentStrategy : IPaymentStrategy
    {
        private const decimal _commissionRate = 0.01m; // 1%
        private const string _serviceName = "Сбербанк";

        public decimal CalculateFinalSalary(decimal monthlySalary)
        {
            decimal commission = monthlySalary * _commissionRate;
            decimal finalSalary = monthlySalary - commission;

            // Округляем до 2 знаков после запятой
            return Math.Round(finalSalary, 2);
        }

        public string GetServiceName()
        {
            return _serviceName;
        }
    }

    public class GazpromPaymentStrategy : IPaymentStrategy
    {
        private const decimal _commissionRate = 0.015m; // 1.5%
        private const string _serviceName = "Газпромбанк";

        public decimal CalculateFinalSalary(decimal monthlySalary)
        {
            decimal commission = monthlySalary * _commissionRate;
            decimal finalSalary = monthlySalary - commission;

            // Округляем до 2 знаков после запятой
            return Math.Round(finalSalary, 2);
        }

        public string GetServiceName()
        {
            return _serviceName;
        }
    }

    // Дополнительная стратегия для демонстрации расширяемости
    public class TinkoffPaymentStrategy : IPaymentStrategy
    {
        private const decimal _commissionRate = 0.008m; // 0.8%
        private const decimal _fixedFee = 50m; // Фиксированная комиссия
        private const string _serviceName = "Тинькофф Банк";

        public decimal CalculateFinalSalary(decimal monthlySalary)
        {
            decimal commission = monthlySalary * _commissionRate + _fixedFee;
            decimal finalSalary = monthlySalary - commission;

            // Округляем до 2 знаков после запятой
            return Math.Round(finalSalary, 2);
        }

        public string GetServiceName()
        {
            return _serviceName;
        }
    }

    // Стратегия без комиссии (для тестирования)
    public class NoCommissionPaymentStrategy : IPaymentStrategy
    {
        private const string _serviceName = "Тестовый сервис (без комиссии)";

        public decimal CalculateFinalSalary(decimal monthlySalary)
        {
            return monthlySalary;
        }

        public string GetServiceName()
        {
            return _serviceName;
        }
    }
}