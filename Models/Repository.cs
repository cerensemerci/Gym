namespace Basics.Models
{
    public class Repository
    {
        private static List<Employee> employees = new List<Employee>();

        public void AddEmployee(Employee employee)
        {
            employees.Add(employee);
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return employees;
        }
    }
}