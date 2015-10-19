using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InvoiceApp.Controllers;
using System.ComponentModel.DataAnnotations;

namespace InvoiceApp.ViewModels
{
    public class ManageInvoiceAmount_ViewModel
    {                
        //Creating an instance of our constant limit amounts here to
        //keep to the persistant MVC architecture. Do not want to access
        //class variables from our view
        public double warningAmount_VM
        {
            get { return InvoicesController.WARNING_INVOICE_AMOUNT; }
        }
        public double maxAmount_VM
        {
            get { return InvoicesController.MAX_INVOICE_AMOUNT; }
        }

       
        [Display(Name = "Sum of all invoices: ")]
        [DisplayFormat(DataFormatString = "{0:#,##0.00#}", ApplyFormatInEditMode = true)]
        public double CurrentSum { get; set; }
    }
}