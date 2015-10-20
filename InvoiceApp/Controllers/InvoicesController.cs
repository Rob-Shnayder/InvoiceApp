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
        //Class Variables
        public static double WARNING_INVOICE_AMOUNT
        {
            get { return 500.00; }
        }
        public static double MAX_INVOICE_AMOUNT
        {
            get { return 600.00; }
        }
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
                    //Calculate the total amount of the entered invoice
                    double amount = (newInvoice.BaseInvoiceViewModel.Price * newInvoice.BaseInvoiceViewModel.Quantity)
                        + newInvoice.BaseInvoiceViewModel.Tax;                    

                    //Check if the entered amount will not exceed the limit.
                    if (!ValidateInvoiceEntry(amount))
                    {
                        //Calculate what the invoice balance would be if this order would be added.
                        double newAmount = amount + GetCurrentInvoiceSum();

                        //Throw a validation error that the new invoice exceeds the limit.
                        ModelState.AddModelError("CustomerError", "Invoice limit exceeded: Adding this order of $" 
                            + amount + " would create an exceeding invoice balance of $" + newAmount);
                        return View(newInvoice);
                    }

                    //Create a customer model object using our viewmodel.                 
                    Customer cust = SetupCustomerObject(newInvoice);
                    db.Customers.Add(cust);  

                    //Save the changes here so we can grab the "customerID" of the new entry
                    db.SaveChanges();

                    //Grab the "customerID" for the foreign key association
                    int customerID = cust.CustomerID;

                    //Create a invoice model object using our viewmodel.    
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

            Invoice_ViewModel invoice = RetrieveModelForDisplay(id.Value);

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
        public ActionResult ManageInvoiceAmount()
        {
            ManageInvoiceAmount_ViewModel invoiceAmount = new ManageInvoiceAmount_ViewModel();
            invoiceAmount.CurrentSum = db.Invoices.Sum(a => (a.Price * a.Quantity) + a.Tax);

            return PartialView(invoiceAmount);
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

        private bool ValidateInvoiceEntry(double amount)
        {
            double curr = GetCurrentInvoiceSum();
            if ((curr + amount) <= MAX_INVOICE_AMOUNT)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private double GetCurrentInvoiceSum()
        {
            return db.Invoices.Sum(a => (a.Price * a.Quantity) + a.Tax);
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
