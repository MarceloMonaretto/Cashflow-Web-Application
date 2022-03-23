function setupTransactionCreationEvent() {
    $('#createTransactionButton').click(function () {
        var description = $('#transactionDescription').val();
        var amountAsString = $('#transactionAmount').val().toString();
        var paymentType = $('#transactionPaymentType').val();
        var amount = parseFloat(amountAsString);


        var isAmountInvalid = validateAmount(amountAsString, '#validationForAmount');;
        var isDescriptionInvalid = validateDescription(description, '#validationForDescription');;
        var isPaymentTypeInvalid = validatePaymentType(paymentType, '#validationForPaymentType');;

        if (isAmountInvalid || isDescriptionInvalid || isPaymentTypeInvalid) {
            return;
        }

        $.ajax({
            url: '/Transaction/CreateTransaction',
            type: 'POST',
            dataType: 'json',
            contentType: 'application/json',
            data: JSON.stringify({ description, paymentType, amount }),
            statusCode: {
                200: function () {
                    $('#transactionDescription').val('');
                    $('#transactionAmount').val('');
                    $('#transactionPaymentType').val('Select Type');
                    $('#transactionStatus').text('Transaction added!');
                    setTimeout(function () {
                        $('#transactionStatus').text('');
                    }, 2000);
                }
            },
        });
    });
}

$(document).ready(setupTransactionCreationEvent());