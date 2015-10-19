using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace InvoiceApp.ViewModels
{
    //Invoice view model used to list all invoices
    //Used in /Invoices/Index
    public class ListInvoices_ViewModel
    {
        [Key]
        public int InvoiceID { get; set; }

        [Display(Name = "Product")]
        public string ProductName { get; set; }

        [Display(Name = "Customer")]
        public string CustomerName { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime InvoiceCreationDate { get; set; }

        [Display(Name = "Due Date")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime InvoiceDueDate { get; set; }

        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,##0.00#}", ApplyFormatInEditMode = true)]
        [Display(Name = "Amount Due")]
        public double GrossTotal { get; set; }


    }
}