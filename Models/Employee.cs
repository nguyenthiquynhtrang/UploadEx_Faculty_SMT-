using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace NguyenThiQuynhTrangBTH2.Models;

    public class Employee
    {
        //Khai bao cac thuoc tinh 
        [Key]
         public string? EmployeeID{ get; set; }
        public string? EmployeeName{ get; set; }

        public string? PositionID { get; set; }
        [ForeignKey("PositionID")]
        public Position? Position {get; set; }
    }