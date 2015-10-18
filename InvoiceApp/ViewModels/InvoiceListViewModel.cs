using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace InvoiceApp.ViewModels
{
    //Invoice view model used to list all invoices
    //Used in /Invoices/Index
    public class InvoiceListViewModel
    {
        [Key]
        public int InvoiceID { get; set; }

        [Display(Name = "Product")]
        public string ProductName { get; set; }

        [Display(Name = "Customer")]
        public string CustomerName { get; set; }

        [Display(Name = "Date")]
        public DateTime InvoiceCreationDate { get; set; }

        [Display(Name = "Due Date")]
        public DateTime InvoiceDueDate { get; set; }

        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Amount Due")]
        public double GrossTotal { get; set; }
        
        [Display(Name = "Sum of all invoices:")]
        public double AllInvoiceSum { get; set; }

    }
}