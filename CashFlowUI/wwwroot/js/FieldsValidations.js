function validateAmount(amountAsString, validationTextAreaId) {
    var amount = parseFloat(amountAsString);
    var isThereInvalidInput = false;
    var regex = /(^\d+(?:(\.|\,)\d{0,2})$)|(^\d+$)/;


    if (isNaN(amount) || !regex.test(amountAsString)) {
        $(validationTextAreaId).text('Invalid input!');
        isThereInvalidInput = true;
    } else if (amountAsString.includes(',')) {
        $(validationTextAreaId).text('Please use "." instead of ","');
        isThereInvalidInput = true;
    } else {
        $(validationTextAreaId).text('');
    }

    return isThereInvalidInput;
}

function validateDescription(description, validationTextAreaId) {
    var isThereInvalidInput = false;

    if (description == '') {
        $(validationTextAreaId).text('Description cannot be empty!');
        isThereInvalidInput = true;
    } else {
        $(validationTextAreaId).text('');
    }

    return isThereInvalidInput;
}

function validatePaymentType(paymentType, validationTextAreaId) {
    var isThereInvalidInput = false;

    if (paymentType != 'Credit Card' && paymentType != 'Money') {
        $(validationTextAreaId).text('Invalid payment type!');
        isThereInvalidInput = true;
    } else {
        $(validationTextAreaId).text('');
    }

    return isThereInvalidInput;
}