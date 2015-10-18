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
        private double MAX_INVOICE_LIMIT = 600.00;
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: Invoices
        public ActionResult Index()
        {
            //Get all the invoices from the DB and display it in "InvoiceListViewModel"
            var invoices = from a in db.Invoices
                           select new InvoiceListViewModel
                           {
                               InvoiceID = a.InvoiceID,
                               ProductName = a.ProductName,                               
                               CustomerName = a.customer.FirstName + " " + a.customer.LastName,
                               InvoiceCreationDate = a.InvoiceCreationDate,
                               InvoiceDueDate = a.InvoiceDueDate,
                               Quantity = a.Quantity,
                               GrossTotal = (a.Price * a.Quantity) + a.Tax,
                               AllInvoiceSum = db.Invoices.Sum(b => (b.Price * b.Quantity) + b.Tax) 
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
            Invoice invoice = db.Invoices.Find(id);
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
        public ActionResult Create(InvoiceCreateViewModel newInvoice)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Create the customer object and save it to the DB.
                    Customer cust = (new Customer
                    {
                        FirstName = newInvoice.BaseCustomerViewModel.FirstName,
                        LastName = newInvoice.BaseCustomerViewModel.LastName,
                        Email = newInvoice.BaseCustomerViewModel.Email
                    });
                    db.Customers.Add(cust);
                    db.SaveChanges();

                    //Get the id from customer for the foreign key association
                    int customerID = cust.CustomerID;


                    //Setup the Invoice object
                    db.Invoices.Add(new Invoice
                    {
                        CustomerID = customerID,
                        InvoiceCreationDate = DateTime.Today,
                        InvoiceDueDate = newInvoice.BaseInvoiceViewModel.InvoiceDueDate,
                        ProductName = newInvoice.BaseInvoiceViewModel.ProductName,
                        ProductDescription = newInvoice.BaseInvoiceViewModel.ProductDescription,
                        Price = newInvoice.BaseInvoiceViewModel.Price,
                        Quantity = newInvoice.BaseInvoiceViewModel.Quantity,
                        Tax = newInvoice.BaseInvoiceViewModel.Tax

                    });

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
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "FirstName", invoice.CustomerID);
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InvoiceID,InvoiceCreationDate,InvoiceDueDate,Quantity,Tax,GrossTotal,ProductID,CustomerID")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invoice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "FirstName", invoice.CustomerID);
            return View(invoice);
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
