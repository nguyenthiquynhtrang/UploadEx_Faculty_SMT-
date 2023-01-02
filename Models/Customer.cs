using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace NguyenThiQuynhTrangBTH2.Models;

    public class Customer
    {
        //Khai bao cac thuoc tinh 
        [Key]
         public string? CustomerID{ get; set; }
        public string? CustomerName{ get; set; }
        
    }