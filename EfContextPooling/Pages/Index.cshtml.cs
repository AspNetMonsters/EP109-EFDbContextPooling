using System.Collections.Generic;
using System.Linq;
using EfContextPooling.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EfContextPooling.Pages
{
    public class IndexModel : PageModel
    {
        private readonly EmployeeContext _context;

        public IndexModel(EmployeeContext context)
        {
            _context = context;
        }

        public IEnumerable<Employee> EmployeesLikeTibbs = new List<Employee>();

        public void OnGet()
        {            
            EmployeesLikeTibbs = _context.Employees.Where(e => EF.Functions.Like(e.LastName, "Tibbs%")).ToList();
        }
    }
}






































