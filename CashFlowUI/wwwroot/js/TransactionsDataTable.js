function startDataTable() {
    $(document).ready(function () {
        var transactionId = '0';
        var hasAccessToDeleteCommand = $('#userHasAccessToDeleteCommand').text() == 'true';
        var hasAccessToEditCommand = $('#userHasAccessToEditCommand').text() == 'true';
        var rowData;
        var rowIndex;

        var table = $('#TransactionTable')
            .DataTable({
                dom: 'Qlfrtip',
                searchBuilder: {
                    depthLimit: 1,
                    columns: [2, 4],
                    conditions: {
                        date: {
                            '!null': null,
                            'null': null
                        },
                        string: {
                            '!null': null, 'null': null, 'starts': null, '!starts': null, 'contains': null,
                            '!contains': null, 'ends': null, '!ends': null, 'empty': null, '!empty': null,
                            '!=': {
                                conditionName: 'Different than'
                            }
                        }
                    },
                },
                ajax: {
                    url: '/Transaction/GetTransactionList',
                    type: 'POST',
                    complete: function () {
                        setupDeleteButton();
                        setupEditButton();
                    }
                },
                columns: [
                    { data: 'id' },
                    { data: 'amount' },
                    { data: 'paymentType' },
                    { data: 'description' },
                    { data: 'transactionTime' },
                    {
                        defaultContent: createDeleteAndEditButtons()
                    }
                ],
                columnDefs: [
                    { 'type': 'num', 'targets': 0 },
                    { 'type': 'string', 'targets': 1 },
                    { 'type': 'string', 'targets': 2 },
                    { 'type': 'string', 'targets': 3 },
                    {
                        type: 'date',
                        targets: 4,
                        render: function (data) {
                            return moment(data).format('DD/MM/YYYY HH:MM:SS');
                        }
                    }
                ],
                processing: true,
                serverSide: true,
                ordering: true,
                paging: true,
                pagingType: 'full_numbers',
                pageLength: 10
            });

        function createDeleteAndEditButtons() {
            return ('<button type="button" class="btn btn-danger deleteTransactionButton" value="Delete" disabled><i class="bi bi-trash"></i></button>/'
                + '<button type="button" class="btn btn-success editTransactionButton" value="Edit" disabled><i class="bi bi-pencil-square"></i></button>');
        }

        function setupDeleteButton() {
            if (hasAccessToDeleteCommand) {
                $('.deleteTransactionButton').attr("disabled", false);
                $('#TransactionTable tbody').on('click', '.deleteTransactionButton', function () {
                    $('#deleteTransactionModal').modal('show');
                    rowData = table.row($(this).parents('tr')).data();
                    rowIndex = table.row($(this).parents('tr')).index();
                    transactionId = rowData['id'];
                    $('#transactionIdLi').text('Id: ' + rowData['id']);
                    $('#transactionDescriptionLi').text('Description: ' + rowData['description']);
                    $('#transactionPaymentTypeLi').text('Payment Type: ' + rowData['paymentType']);
                    $('#transactionAmountLi').text('Amount: ' + rowData['amount']);
                    $('#transactionTimeLi').text('Date: ' + $.format.date(rowData['transactionTime'], "dd/MM/yyyy HH:mm:ss"));
                });
            }
        };

        function setupEditButton() {
            if (hasAccessToEditCommand) {
                $('.editTransactionButton').attr("disabled", false);
                $('#TransactionTable tbody').on('click', '.editTransactionButton', function () {
                    $('#editTransactionModal').modal('show');
                    rowData = table.row($(this).parents('tr')).data();
                    rowIndex = table.row($(this).parents('tr')).index();
                    $('#transactionIdInput').val(rowData['id']);
                    $('#transactionDescriptionInput').val(rowData['description']);
                    $('#transactionPaymentTypeInput').val(rowData['paymentType']);
                    $('#transactionAmountInput').val(rowData['amount']);
                    $('#transactionTimeInput').val($.format.date(rowData['transactionTime'], "dd/MM/yyyy HH:mm:ss"));
                });
            }
        };

        deleteModalConfirmDeletionEvent = $('#confirmDeleteButton').click(function () {
            if (transactionId != '0') {
                $.ajax({
                    url: '/Transaction/DeleteTransactionById',
                    type: 'DELETE',
                    data: { 'id': transactionId },
                    success: function () {
                        $('#TransactionTable').DataTable().row('.selected').remove().draw(false);
                        $('#deleteTransactionModal').modal('hide');
                    }
                });
                transactionId = '0';
            }
        });

        deleteModalCancelDeletionEvent = $('#cancelDeleteButton').click(function () {
            $('#deleteTransactionModal').modal('hide');
            transactionId = '0';
            $('#transactionIdLi').text('');
            $('#transactionDescriptionLi').text('');
            $('#transactionPaymentTypeLi').text('');
            $('#transactionAmountLi').text('');
            $('#transactionTimeLi').text('');
        });

        setupEditModalCancelEditionButton = $('#closeModalButton').click(function () {
            $('#editTransactionModal').modal('hide');
        });

        setupEditModalConfirmEditionButton = $('#saveModalButton').click(function () {
            var id = $('#transactionIdInput').val();
            var description = $('#transactionDescriptionInput').val();
            var paymentType = $('#transactionPaymentTypeInput').val();
            var amountAsString = $('#transactionAmountInput').val();
            var amount = parseFloat($('#transactionAmountInput').val());
            var transactionTime = rowData['transactionTime'];

            var isAmountInvalid = validateAmount(amountAsString, '#validationForAmount');
            var isPaymentTypeInvalid = validatePaymentType(paymentType, '#validationForPaymentType');
            var isDescriptionInvalid = validateDescription(description, '#validationForDescription');

            if (isAmountInvalid | isPaymentTypeInvalid | isDescriptionInvalid) {
                return;
            }

            $.ajax({
                url: '/Transaction/UpdateTransactionById',
                type: 'PUT',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ id, description, paymentType, amount, transactionTime }),
                success: function () {
                    rowData['id'] = id;
                    rowData['description'] = description;
                    rowData['paymentType'] = paymentType;
                    rowData['amount'] = amount;
                    rowData['transactionTime'] = transactionTime;

                    table.row(rowIndex).data(rowData).draw(false);
                    $('#editTransactionModal').modal('hide');
                },
                error: function (data) {

                }
            });
        });
    });
}