using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InvoiceApp.Models;
using InvoiceApp.ViewModels;

namespace InvoiceApp.Controllers
{
    public class InvoicesController : Controller
    {

        private double WARNING_INVOICE_AMOUNT = 500.00;
        public static double MAX_INVOICE_AMOUNT = 600.00;
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: Invoices
        public ActionResult Index()
        {
            //Get all the invoices from the DB and display it in "InvoiceListViewModel"
            var invoices = from a in db.Invoices
                           select new ListInvoices_ViewModel
                           {
                               InvoiceID = a.InvoiceID,
                               ProductName = a.ProductName,                               
                               CustomerName = a.customer.FirstName + " " + a.customer.LastName,
                               InvoiceCreationDate = a.InvoiceCreationDate,
                               InvoiceDueDate = a.InvoiceDueDate,
                               Quantity = a.Quantity,
                               GrossTotal = (a.Price * a.Quantity) + a.Tax                               
                           };

             return View(invoices.ToList());
        }

        // GET: Invoices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Invoice_ViewModel invoice = RetrieveModelForDisplay(id.Value);  
            
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }


        // GET: Invoices/Create
        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "FirstName");
            //ViewBag.ProductID = new SelectList(db.Products, "ProductID", "Name");
            return View();
        }

        // POST: Invoices/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Invoice_ViewModel newInvoice)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Create the customer object and save it to the DB.
                    //The current implementation only allows for every new invoice to create a new customer                    
                    Customer cust = SetupCustomerObject(newInvoice);
                    db.Customers.Add(cust);  
                    //Save the changes here so we can grab the "customerID" of the new entry
                    db.SaveChanges();


                    //Grab the "customerID" for the foreign key association
                    int customerID = cust.CustomerID;


                    //Setup the Invoice object and add it to the db
                    Invoice invoice = SetupInvoiceObject(newInvoice, customerID);
                    db.Invoices.Add(invoice);
                    db.SaveChanges();


                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Create", "Invoice"));
            }


            return View(newInvoice);
        }


        // GET: Invoices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Invoice_ViewModel invoice = RetrieveModelForDisplay(id.Value);           
            
            if (invoice == null)
            {
                return HttpNotFound();
            }

            return View(invoice);
        }


        // POST: Invoices/Edit/5       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Invoice_ViewModel data)
        {
            if (ModelState.IsValid)
            {
                //Get both ID's
                int customerID = data.BaseInvoiceViewModel.CustomerID;
                int invoiceID = data.BaseInvoiceViewModel.InvoiceID;

                //Setup the model object to save it in the DB
                var invoice = SetupInvoiceObject(data, customerID, invoiceID);
                var customer = SetupCustomerObject(data, customerID);

                db.Entry(invoice).State = EntityState.Modified;
                db.Entry(customer).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "FirstName", invoice.CustomerID);
            return View(data);
        }

        // GET: Invoices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invoice invoice = db.Invoices.Find(id);
            db.Invoices.Remove(invoice);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [ChildActionOnly]
        public ActionResult GetSumOfInvoice()
        {
            ViewBag.invoiceSum = db.Invoices.Sum(a => (a.Price * a.Quantity) + a.Tax);
            return PartialView();
        }

        private Customer SetupCustomerObject(Invoice_ViewModel data)
        {
            Customer cust = (new Customer
            {
                FirstName = data.BaseCustomerViewModel.FirstName,
                LastName = data.BaseCustomerViewModel.LastName,
                Email = data.BaseCustomerViewModel.Email
            });

            return cust;
        }
        private Customer SetupCustomerObject(Invoice_ViewModel data, int customerID)
        {
            Customer cust = (new Customer
            {
                CustomerID = customerID,
                FirstName = data.BaseCustomerViewModel.FirstName,
                LastName = data.BaseCustomerViewModel.LastName,
                Email = data.BaseCustomerViewModel.Email
            });

            return cust;
        }

        private Invoice SetupInvoiceObject(Invoice_ViewModel data, int customerID)
        {
            Invoice invoice = (new Invoice
            {
                CustomerID = customerID,
                InvoiceCreationDate = DateTime.Today,
                InvoiceDueDate = data.BaseInvoiceViewModel.InvoiceDueDate,
                ProductName = data.BaseInvoiceViewModel.ProductName,
                ProductDescription = data.BaseInvoiceViewModel.ProductDescription,
                Price = data.BaseInvoiceViewModel.Price,
                Quantity = data.BaseInvoiceViewModel.Quantity,
                Tax = data.BaseInvoiceViewModel.Tax

            });

            return invoice;
        }
        private Invoice SetupInvoiceObject(Invoice_ViewModel data, int customerID, int invoiceID)
        {
            Invoice invoice = (new Invoice
            {
                CustomerID = customerID,
                InvoiceID = invoiceID,
                InvoiceCreationDate = DateTime.Today,
                InvoiceDueDate = data.BaseInvoiceViewModel.InvoiceDueDate,
                ProductName = data.BaseInvoiceViewModel.ProductName,
                ProductDescription = data.BaseInvoiceViewModel.ProductDescription,
                Price = data.BaseInvoiceViewModel.Price,
                Quantity = data.BaseInvoiceViewModel.Quantity,
                Tax = data.BaseInvoiceViewModel.Tax

            });

            return invoice;
        }

        private Invoice_ViewModel RetrieveModelForDisplay(int id)
        {
            var invoice = (from a in db.Invoices
                           join b in db.Customers on a.CustomerID equals b.CustomerID
                           where a.InvoiceID == id
                           select new Invoice_ViewModel
                           {
                               BaseCustomerViewModel = new BaseCustomer_ViewModel
                               {
                                   customerID = a.CustomerID,
                                   FirstName = a.customer.FirstName,
                                   LastName = a.customer.LastName,
                                   Email = a.customer.Email
                               },
                               BaseInvoiceViewModel = new BaseInvoice_ViewModel
                               {
                                   InvoiceID = a.InvoiceID,
                                   CustomerID = a.customer.CustomerID,
                                   InvoiceDueDate = a.InvoiceDueDate,
                                   Price = a.Price,
                                   ProductName = a.ProductName,
                                   ProductDescription = a.ProductDescription,
                                   Quantity = a.Quantity,
                                   Tax = a.Tax
                               }
                           }).FirstOrDefault();
            return invoice;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
