using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvoiceApp.ViewModels
{
    //Invoice creation view model class
    //Used in /Invoices/Create
    public class InvoiceCreateViewModel
    {

        public BaseInvoiceViewModel BaseInvoiceViewModel { get; set; }
        public BaseCustomerViewModel BaseCustomerViewModel { get; set; }

        
    }
}