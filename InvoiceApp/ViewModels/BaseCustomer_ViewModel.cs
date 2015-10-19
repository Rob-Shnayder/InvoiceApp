using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using InvoiceApp.Models;

namespace InvoiceApp.ViewModels
{
    public class BaseCustomer_ViewModel
    {

        public int customerID;

        [Required]
        [Display(Name = "First Name:")]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name:")]
        [StringLength(150)]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email:")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
        ErrorMessage = "Email is not valid.")]
        public string Email { get; set; }


    }


}