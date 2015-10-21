$(function () {
    //used to create a datepicker for the "InvoiceDueDate" field.
    //Using paramater "minDate:0" to prevent the option of picking a date earlier than today
    $(".datepicker").datepicker({ minDate: 0 });
     
    //Do the initial calculations for the summary of an invoice
    CalculateTotals();
});



//Events to grab input values for the purpose of generating a calculate summary for the invoice
$("#BaseInvoiceViewModel_Quantity").keyup(function () {
    CalculateTotals();
});

$("#BaseInvoiceViewModel_Price").keyup(function () {
    CalculateTotals();
});

$("#BaseInvoiceViewModel_Tax").keyup(function () {
    CalculateTotals();
});


//Controller function for displaying calculated invoice summary on Create and Edit page
//This function gets called by a keyup event on the Tax, Price, and Quantity input field
function CalculateTotals() {
    //Get the values
    var price = parseFloat($('#BaseInvoiceViewModel_Price').val()) || 0.00;
    var tax = parseFloat($('#BaseInvoiceViewModel_Tax').val()) || 0.00;
    var quantity = parseFloat($('#BaseInvoiceViewModel_Quantity').val()) || 0.00;
    
    
    //Do calculations and display them
    DisplayTax(tax);
    DisplaySubTotal(price, quantity);
    DisplayTotal(price, quantity, tax);

}

//Functions to calculate and display invoice summary items
function DisplayTax(tax) {
    $('#tax').html(tax.toFixed(2));
}

function DisplaySubTotal(price, quantity) {
    var subtotal = price * quantity;
    $('#subtotal').html(subtotal.toFixed(2));
}

function DisplayTotal(price, quantity, tax) {
    var total = (price * quantity) + tax;
    $('#total').html(total.toFixed(2));
}



var display_total = display_subtotal + tax;