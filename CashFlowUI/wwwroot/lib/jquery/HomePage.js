function createChart() {
    var context = $('#transactionChart');

    var transactionValues;
    var transactionDays;

    $.get('/Home/GetLastThirtyDaysValues', function (receivedData) {
        console.log(receivedData);
        transactionValues = receivedData.amounts;
        transactionDays = receivedData.dates;
    }).done(function () {
        const myChart = new Chart(context, {
            type: 'line',
            data: {
                labels: transactionDays,
                datasets: [
                    {
                        data: transactionValues,
                        fill: true,
                        borderColor: 'rgb(255, 99, 132)',
                        backgroundColor: 'rgba(255, 99, 132, 0.2)',
                        lineTension: 0.1

                    }
                ]
            },
            options: {
                plugins: {
                    legend: {
                        display: false
                    }
                },
                scales: {
                    y: {
                        title: {
                            text: 'Amount (Units of Money)',
                            display: true,
                            font: {
                                size: 16
                            }
                        },
                        beginFromZero: true
                    },
                    x: {
                        title: {
                            text: 'Month/Day',
                            display: true,
                            font: {
                                size: 16
                            }
                        },
                    }
                }
            }
        });
    }).fail(function(){

    });    
}