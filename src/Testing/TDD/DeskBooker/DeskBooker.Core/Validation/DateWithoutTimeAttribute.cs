using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeskBooker.Core.Validation
{
    public class DateWithoutTimeAttribute : ValidationAttribute
    {
        public DateWithoutTimeAttribute()
        {
            ErrorMessage = "Date must not contain time";
        }

        public override bool IsValid(object value)
        {
            bool isValid = false;

            if (value is DateTime dateTime)
            {
                isValid = dateTime.TimeOfDay.Ticks == 0;
            }

            return isValid;
        }
    }
}
