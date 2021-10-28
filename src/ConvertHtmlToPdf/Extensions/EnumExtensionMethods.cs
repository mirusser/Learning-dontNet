using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertHtmlToPdf.Extensions
{
    public static class EnumExtensionMethods
    {
        public static string GetDescription(this Enum enumValue)
        {
            var enumType = enumValue.GetType();
            var enumField = enumType.GetField(name: enumValue.ToString());

            object[]? attr = null;

            if (enumField is not null)
            {
                attr =
                   enumField.GetCustomAttributes(typeof(DescriptionAttribute), false);
            }

            return attr?.Length > 0 ?
                ((DescriptionAttribute)attr[0]).Description :
                enumValue.ToString();
        }

        /// <summary>
        /// Get int value of an enum and convert it to string 
        /// (for example: MyEnum.SomeEnum.ToIntString() == "1")
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToIntString(this Enum value)
        {
            return Convert.ToInt32(value).ToString();
        }
    }
}
