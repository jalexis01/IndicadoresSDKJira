var dataHideColums, dataSearchColumns;

$(document).ready(function () {
    createElemntsTimes();
    multiSelect();
    drodownDataSearch(columnsSearch, 'CustomName', 'searchParam');
});

function ServiceGetTickets() {
    var startDate = $('#dtpStart').val();
    var endDate = $('#dtpEnd').val();

    $.ajax({
        type: "GET",
        url: "/Tickets/GetTickets",
        data: { startDate: startDate, endDate: endDate },
        success: function (response) {
            // Clear the current table body
            $('#table tbody').empty();

            // Loop through the response and append new rows to the table
            $.each(response, function (index, ticket) {
                $('#table tbody').append(
                    '<tr>' +
                    '<td>' + ticket.codigo_falla + '</td>' +
                    '<td>' + ticket.componente_Parte + '</td>' +
                    '<td>' + ticket.descripcion_reparacion + '</td>' +
                    '<td>' + ticket.fecha_apertura + '</td>' +
                    '<td>' + ticket.estado_ticket + '</td>' +
                    '</tr>'
                );
            });
        },
        error: function (xhr, status, error) {
            // Handle the error
        }
    });
}

