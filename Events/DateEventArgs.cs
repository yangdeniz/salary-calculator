using System;

namespace SalaryCalculator
{
    public class DateEventArgs : EventArgs
    {
        public DateTime Date { get; set; }
        public DateEventArgs(DateTime date)
        {
            Date = date;
        }
    }
}
