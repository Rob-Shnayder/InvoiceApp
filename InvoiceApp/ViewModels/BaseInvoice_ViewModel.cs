using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using InvoiceApp.Models;

namespace InvoiceApp.ViewModels
{
    public class BaseInvoice_ViewModel
    {

        public int InvoiceID { get; set; }
        public int CustomerID { get; set; }

        [Required]
        [Display(Name = "Due Date")]
        public DateTime InvoiceDueDate { get; set; }
        
        [Required]
        [Display(Name = "Product")]
        [StringLength(100)]
        public string ProductName { get; set; }

        
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        [StringLength(500)]
        public string ProductDescription { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter valid quantity amount")]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid price that is greater than 0")]
        [DisplayFormat(DataFormatString = "{0:#,##0.00#}", ApplyFormatInEditMode = true)]
        [Display(Name = "Price")]
        public double Price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid tax that is greater than 0")]
        [DisplayFormat(DataFormatString = "{0:#,##0.00#}", ApplyFormatInEditMode = true)]
        [Display(Name = "Tax")]
        public double Tax { get; set; }


        
    }
}