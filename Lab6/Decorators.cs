using System;

namespace Lab6
{

    public abstract class EmployeeDecorator : Employee
    {
        protected Employee _employee;

        protected EmployeeDecorator(Employee employee)
            : base(employee.Name, employee.MonthlySalary)
        {
            _employee = employee;
        }

        public override string GetInfo()
        {
            return _employee.GetInfo();
        }

        public new decimal CalculateSalary(IPaymentStrategy paymentStrategy)
        {
            return paymentStrategy.CalculateFinalSalary(MonthlySalary);
        }
    }

    public class EnglishDecorator : EmployeeDecorator
    {
        private string _certificateName;
        private DateTime _certificateDate;
        private const string _level = "Intermediate";

        public EnglishDecorator(Employee employee, string certificateName, DateTime certificateDate)
            : base(employee)
        {
            _certificateName = certificateName;
            _certificateDate = certificateDate;
        }

        public override string GetInfo()
        {
            string baseInfo = _employee.GetInfo();
            string englishInfo = $"\nАнглийский язык: {_level}";
            string certificateInfo = $"\nСертификат: {_certificateName} (дата: {_certificateDate:dd.MM.yyyy})";

            return baseInfo + englishInfo + certificateInfo;
        }
    }

    public class DegreeDecorator : EmployeeDecorator
    {
        private string _fieldOfScience;
        private string _dissertationTopic;
        private int _defenseYear;

        public DegreeDecorator(Employee employee, string fieldOfScience,
                              string dissertationTopic, int defenseYear)
            : base(employee)
        {
            _fieldOfScience = fieldOfScience;
            _dissertationTopic = dissertationTopic;
            _defenseYear = defenseYear;
        }

        public override string GetInfo()
        {
            string baseInfo = _employee.GetInfo();
            string degreeInfo = $"\nУченая степень";
            string scienceInfo = $"\nОбласть наук: {_fieldOfScience}";
            string dissertationInfo = $"\nТема диссертации: {_dissertationTopic}";
            string yearInfo = $"\nГод защиты: {_defenseYear}";

            return baseInfo + degreeInfo + scienceInfo + dissertationInfo + yearInfo;
        }
    }

    public class ConferenceDecorator : EmployeeDecorator
    {
        private string _conferenceName;
        private string _presentationTopic;
        private int _year;

        public ConferenceDecorator(Employee employee, string conferenceName,
                                  string presentationTopic, int year)
            : base(employee)
        {
            _conferenceName = conferenceName;
            _presentationTopic = presentationTopic;
            _year = year;
        }

        public override string GetInfo()
        {
            string baseInfo = _employee.GetInfo();
            string conferenceInfo = $"\nУчастие в конференции: {_conferenceName}";
            string presentationInfo = $"\nДоклад: {_presentationTopic}";
            string yearInfo = $"\nГод: {_year}";

            return baseInfo + conferenceInfo + presentationInfo + yearInfo;
        }
    }
}