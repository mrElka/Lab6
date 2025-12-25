namespace Lab6
{
    public interface IPaymentStrategy
    {
        decimal CalculateFinalSalary(decimal monthlySalary);
        string GetServiceName();
    }

    public class SberbankPaymentStrategy : IPaymentStrategy
    {
        private const decimal _commissionRate = 0.01m;
        private const string _serviceName = "Сбербанк";

        public decimal CalculateFinalSalary(decimal monthlySalary)
        {
            decimal commission = monthlySalary * _commissionRate;
            decimal finalSalary = monthlySalary - commission;

            return Math.Round(finalSalary, 2);
        }

        public string GetServiceName()
        {
            return _serviceName;
        }
    }

    public class GazpromPaymentStrategy : IPaymentStrategy
    {
        private const decimal _commissionRate = 0.015m;
        private const string _serviceName = "Газпромбанк";

        public decimal CalculateFinalSalary(decimal monthlySalary)
        {
            decimal commission = monthlySalary * _commissionRate;
            decimal finalSalary = monthlySalary - commission;

            return Math.Round(finalSalary, 2);
        }

        public string GetServiceName()
        {
            return _serviceName;
        }
    }

    public class TinkoffPaymentStrategy : IPaymentStrategy
    {
        private const decimal _commissionRate = 0.008m;
        private const decimal _fixedFee = 50m;
        private const string _serviceName = "Тинькофф Банк";

        public decimal CalculateFinalSalary(decimal monthlySalary)
        {
            decimal commission = monthlySalary * _commissionRate + _fixedFee;
            decimal finalSalary = monthlySalary - commission;

            return Math.Round(finalSalary, 2);
        }

        public string GetServiceName()
        {
            return _serviceName;
        }
    }

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