using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InvoiceApp.ViewModels
{
    public class ViewInvoice_ViewModel
    {
        public BaseInvoice_ViewModel BaseInvoiceViewModel { get; set; }
        public BaseCustomer_ViewModel BaseCustomerViewModel { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime InvoiceCreationDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,##0.00#}", ApplyFormatInEditMode = true)]
        public double ItemAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,##0.00#}", ApplyFormatInEditMode = true)]
        public double SubTotal { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,##0.00#}", ApplyFormatInEditMode = true)]
        public double GrossTotal { get; set; }
    }
}