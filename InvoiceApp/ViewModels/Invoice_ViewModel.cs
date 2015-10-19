using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvoiceApp.ViewModels
{
    //Invoice creation view model class
    public class Invoice_ViewModel
    {
        public BaseInvoice_ViewModel BaseInvoiceViewModel { get; set; }
        public BaseCustomer_ViewModel BaseCustomerViewModel { get; set; }
        
    }
}