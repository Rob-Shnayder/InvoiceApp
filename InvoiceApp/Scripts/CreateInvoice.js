$(function () {
    //used to create a datepicker for the "InvoiceDueDate" field.
    //Using paramater "minDate:0" to prevent the option of picking a date earlier than today
    $(".datepicker").datepicker({ minDate: 0 });

    
});




$("#BaseInvoiceViewModel_Quantity").keyup(function () {
    CalculateTotals();
});

$("#BaseInvoiceViewModel_Price").keyup(function () {
    CalculateTotals();
});

$("#BaseInvoiceViewModel_Tax").keyup(function () {
    CalculateTotals();
});

function CalculateTotals() {
    //Get the values
    var price = parseFloat($('#BaseInvoiceViewModel_Price').val()) || 0.00;
    var tax = parseFloat($('#BaseInvoiceViewModel_Tax').val()) || 0.00;
    var quantity = parseFloat($('#BaseInvoiceViewModel_Quantity').val()) || 0.00;
    
    //Do calculations
    var display_subtotal = price * quantity;
    var display_tax = tax;
    var display_total = ((price + tax) * quantity);

    //Display them
    $('#subtotal').html(display_subtotal.toFixed(2));
    $('#tax').html(display_tax.toFixed(2));
    $('#total').html(display_total.toFixed(2));
}
