using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace NguyenThiQuynhTrangBTH2.Models;

    public class Person
    {
        //Khai bao cac thuoc tinh 
        [Key]
         public string? PersonID{ get; set; }
        public string? PersonName{ get; set; }
        
    }