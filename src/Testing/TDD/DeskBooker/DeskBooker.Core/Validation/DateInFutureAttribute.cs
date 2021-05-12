using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace DeskBooker.Core.Validation
{
    public class DateInFutureAttribute : ValidationAttribute
    {
        private readonly Func<DateTime> _dateTimeNowProvider;

        public DateInFutureAttribute()
          : this(() => DateTime.Now)
        {
        }

        public DateInFutureAttribute(Func<DateTime> dateTimeNowProvider)
        {
            _dateTimeNowProvider = dateTimeNowProvider;
            ErrorMessage = "Date must be in the future";
        }

        public override bool IsValid(object value)
        {
            bool isValid = false;

            if (value is DateTime dateTime)
            {
                isValid = dateTime > _dateTimeNowProvider();
            }

            return isValid;
        }
    }
}
