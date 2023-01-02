using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace NguyenThiQuynhTrangBTH2.Models;

    public class Student
    {
        //Khai bao cac thuoc tinh 
        [Key]
         public string? StudentID{ get; set; }
         public string? StudentName{ get; set; }
         
         public string? FacultyID { get; set; }
         [ForeignKey("FacultyID")]
         public Faculty? Faculty {get; set; }
        
    }
