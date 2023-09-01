using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment
{
    public class QuanLy
    {
        public int Id { get; set; }
        public string EmpId { get; set; }
        public string EmpName { get; set; }
        public string Gender { get; set; }  
        public string Phone { get; set; }
        public DateTime WorkingDate { get; set; }
        public string EmpType { get; set; }
        public double Sales { get; set; }
        public double Allowance { get; set; }
        public bool WorkingYear
        {
            get
            {
                return (DateTime.Now - WorkingDate).TotalDays / 365 > 5;
            }
        }
    }
}
