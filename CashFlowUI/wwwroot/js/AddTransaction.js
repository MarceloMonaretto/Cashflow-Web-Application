﻿function setupTransactionCreationEvent() {
    $(document).ready(function () {
        $('#createTransactionButton').click(function () {
            var description = $('#transactionDescription').val();
            var amountAsString = $('#transactionAmount').val().toString();;
            var paymentType = $('#transactionPaymentType').val();


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
                data: JSON.stringify({ description, paymentType, amount })
            });
        });
    });
}