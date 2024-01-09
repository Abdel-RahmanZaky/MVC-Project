using Project.DAL.Models;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace Project.PL.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is Required!")]
        [MaxLength(50, ErrorMessage = "Max Length of Name is 50 Char")]
        [MinLength(5, ErrorMessage = "Min Length of Name is 5 Char")]
        public string Name { get; set; }

        [Range(22, 30)]
        public int? Age { get; set; }

        [RegularExpression(@"^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$",
            ErrorMessage = "Address Must Be Like 123-Street-City-Country")]
        public string Address { get; set; }

        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        [Display(Name = "Is ACtive")]
        public bool IsActive { get; set; }

        public IFormFile Image { get; set; }

        public string ImageName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]

        public string PhoneNumber { get; set; }

        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }

        public int? DepartmentId { get; set; } // foreign key column

        //Navigational Property => One
        public Department Department { get; set; }
    }
}
