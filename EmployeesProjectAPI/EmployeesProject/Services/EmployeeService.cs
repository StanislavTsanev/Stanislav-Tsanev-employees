using CsvHelper;
using EmployeesProject.API.Models;
using System.Globalization;

namespace EmployeesProject.API.Services
{
    public class EmployeeService
    {
        private static readonly string[] dateFormats = { "yyyy-MM-dd", "MM/dd/yyyy", "dd-MM-yyyy", "dd/MM/yyyy" };

        public List<EmployeeProjectRecord> ParseCsv(Stream fileStream)
        {
            using var reader = new StreamReader(fileStream);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = new List<EmployeeProjectRecord>();

            while (csv.Read())
            {
                var empID = csv.GetField<int>(0);
                var projectID = csv.GetField<int>(1);
                var dateFrom = ParseDate(csv.GetField(2));
                var dateToStr = csv.GetField(3);
                var dateTo = string.IsNullOrWhiteSpace(dateToStr) || dateToStr.ToUpper() == "NULL"
                    ? DateTime.Today
                    : ParseDate(dateToStr);

                records.Add(new EmployeeProjectRecord
                {
                    EmpId = empID,
                    ProjectId = projectID,
                    DateFrom = dateFrom,
                    DateTo = dateTo
                });
            }

            return records;
        }

        public List<EmployeePairResult> CalculatePairs(List<EmployeeProjectRecord> records)
        {
            return records
              .GroupBy(r => r.ProjectId)
              .SelectMany(projectGroup =>
              {
                  var employees = projectGroup.ToList();
                  return GetEmployeePairs(employees, projectGroup.Key);
              })
              .Where(r => r.DaysWorked > 0)
              .OrderByDescending(r => r.DaysWorked)
              .ToList();
        }

        private IEnumerable<EmployeePairResult> GetEmployeePairs(List<EmployeeProjectRecord> employees, int projectId)
        {
            for (int i = 0; i < employees.Count; i++)
            {
                for (int j = i + 1; j < employees.Count; j++)
                {
                    var firstEmployee = employees[i];
                    var secondEmployee = employees[j];

                    var overlapStart = firstEmployee.DateFrom > secondEmployee.DateFrom ? firstEmployee.DateFrom : secondEmployee.DateFrom;
                    var overlapEnd = firstEmployee.DateTo < secondEmployee.DateTo ? firstEmployee.DateTo : secondEmployee.DateTo;

                    if (overlapStart < overlapEnd)
                    {
                        var daysWorked = (overlapEnd - overlapStart)?.Days ?? 0;
                        yield return new EmployeePairResult
                        {
                            EmpId1 = firstEmployee.EmpId,
                            EmpId2 = secondEmployee.EmpId,
                            ProjectId = projectId,
                            DaysWorked = daysWorked
                        };
                    }
                }
            }
        }

        private DateTime ParseDate(string dateStr)
        {
            if (DateTime.TryParseExact(dateStr, dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                return date;

            throw new FormatException($"Invalid date format: {dateStr}");
        }
    }
}
