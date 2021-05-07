using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loans.Tests.Data
{
    public class MonthlyRepaymentCsvData
    {
        public static IEnumerable GetTestCases(string csvFilePath)
        {
            var csvLines = File.ReadAllLines(csvFilePath);

            var testCases = new List<TestCaseData>();

            foreach (var line in csvLines)
            {
                string[] values = line.Replace(" ", "").Split(',');

                decimal principal = decimal.Parse(values[0]);
                decimal interestRate = decimal.Parse(values[1]);
                int termInYears = int.Parse(values[2]);
                decimal expectedRepayment = decimal.Parse(values[3]);

                testCases.Add(new TestCaseData(principal, interestRate, termInYears, expectedRepayment));
            }

            return testCases;
        }
    }
}
