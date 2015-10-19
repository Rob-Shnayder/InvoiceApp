$(function () {
    //Function used to create a datepicker for the "InvoiceDueDate" field.
    //Using paramater "minDate:0" to prevent the option of picking a date earlier than today

    $(".datepicker").datepicker({ minDate: 0 });
});
