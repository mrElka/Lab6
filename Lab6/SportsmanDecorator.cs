using Lab6;

public class SportsmanDecorator : EmployeeDecorator
{
    private decimal _proteinBonus;
    private string _sportType;

    public SportsmanDecorator(Employee employee, decimal proteinBonus, string sportType)
        : base(employee)
    {
        _proteinBonus = proteinBonus;
        _sportType = sportType;

        MonthlySalary += _proteinBonus;
    }

    public override string GetInfo()
    {
        string baseInfo = _employee.GetInfo();
        string sportsInfo = $"\n🏆 Статус: Спортсмен ({_sportType})";
        string bonusInfo = $"\nБонус на протеин: +{_proteinBonus:F2} руб.";
        string salaryInfo = $"\nЗарплата с бонусом: {MonthlySalary:F2} руб.";

        return baseInfo + sportsInfo + bonusInfo + salaryInfo;
    }

    public decimal GetSalaryWithoutBonus()
    {
        return MonthlySalary - _proteinBonus;
    }
}