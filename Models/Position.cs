using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace NguyenThiQuynhTrangBTH2.Models;

    public class Position
    {
        //Khai bao cac thuoc tinh 
        [Key]
         public string? PositionID{ get; set; }
        public string? PositionName{ get; set; }
        
    }