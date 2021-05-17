using AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutoFixtureDemo.Tests.Generators
{
    public class AirportCodeStringPropertyGenerator : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            var propertyInfo = request as PropertyInfo;

            if (propertyInfo is null)
            {
                //this specimen builder does not apply to current request
                return new NoSpecimen(); //null is a valid specimen so return NoSpecimen
            }

            //now we know we're dealing with a property
            var isAirportCodeProperty = propertyInfo.Name.Contains("AirportCode");
            var isStringProperty = propertyInfo.PropertyType == typeof(string);

            if (isAirportCodeProperty && isStringProperty)
            {
                return RandomAirportCode();
            }

            return new NoSpecimen();
        }

        private string RandomAirportCode()
        {
            return DateTime.Now.Ticks % 2 == 0 ? "LHR" : "PER";
        }
    }
}
