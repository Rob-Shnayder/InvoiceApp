using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InvoiceApp.ViewModels
{
    public class BaseInvoiceViewModel
    {
        public int InvoiceID { get; set; }        

        [Required]
        [Display(Name = "Due Date:")]
        public DateTime InvoiceDueDate { get; set; }
        
        [Required]
        [Display(Name = "Product")]
        [StringLength(100)]
        public string ProductName { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        [StringLength(100)]
        public string ProductDescription { get; set; }

        [Required]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Required]
        [Display(Name = "Price")]
        public double Price { get; set; }

        [Required]
        [Display(Name = "Tax")]
        public double Tax { get; set; }

    }
}