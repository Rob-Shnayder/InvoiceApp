using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InvoiceApp.Models
{
    public class Invoice
    {
        public int InvoiceID { get; set; }

        public DateTime InvoiceCreationDate { get; set; }

        public DateTime InvoiceDueDate { get; set; }

        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        public double Tax { get; set; }

        //Creating the foreign key association to a Customer
        public int CustomerID { get; set; }

        [ForeignKey("CustomerID")]
        public virtual Customer customer { get; set; }

    }
}