

$('.currency').on('click', function () {
    var url = "https://localhost:7240/api/Currency/currencies/" + this.getAttribute("data-id")

    fetch(url)
        .then(response => response.json())
        .then(currency => {
            $('#currencyModal').modal('show');
            $('#charCode').text(currency.charCode);
            $('#value').text(currency.value);
            $('#previous').text(currency.previous);
            $('#name').text(currency.name);
            $('#nominal').text(currency.nominal)
        });
});

$('#modalClose').on('click', () => {
    $('#currencyModal').modal('hide');
});