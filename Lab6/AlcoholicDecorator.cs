using Lab6;

public class AlcoholicDecorator : EmployeeDecorator
{
    private decimal _alcoholTaxRate;
    private string _reason;

    public AlcoholicDecorator(Employee employee, decimal alcoholTaxRate, string reason)
        : base(employee)
    {
        _alcoholTaxRate = alcoholTaxRate;
        _reason = reason;

        MonthlySalary = CalculateAfterTaxSalary(employee.MonthlySalary);
    }

    private decimal CalculateAfterTaxSalary(decimal originalSalary)
    {
        decimal taxAmount = originalSalary * _alcoholTaxRate;
        return originalSalary - taxAmount;
    }

    public override string GetInfo()
    {
        string baseInfo = _employee.GetInfo();
        string alcoholInfo = $"\n🚫 Статус: Алкоголик (удержание {_alcoholTaxRate * 100}% зарплаты)";
        string reasonInfo = $"\nПричина удержания: {_reason}";
        string salaryInfo = $"\nЗарплата после удержания: {MonthlySalary:F2} руб.";

        return baseInfo + alcoholInfo + reasonInfo + salaryInfo;
    }

    public new decimal CalculateSalary(IPaymentStrategy paymentStrategy)
    {
        return paymentStrategy.CalculateFinalSalary(MonthlySalary);
    }

    public decimal GetOriginalSalary()
    {
        return MonthlySalary / (1 - _alcoholTaxRate);
    }
}