var dataHideColums, dataSearchColumns;

$(document).ready(function () {
    createElemntsTimes();
    multiSelect();
    drodownDataSearch(columnsSearch, 'CustomName', 'searchParam');
});

function ServiceGetTickets() {
    var startDate = $('#dtpStart').val();
    var endDate = $('#dtpEnd').val();
    var max = document.getElementById("maxSelect").value;
    var componente = $('#componente').val();
    console.log("idComponente: "+componente);

    $.ajax({
        type: "GET",
        url: "/Tickets/GetTickets",
        data: { startDate: startDate, endDate: endDate, max:max, componente: componente },
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
                    '<td>' + ticket.diagnostico_falla + '</td>' +
                    '<td>' + ticket.estado_ticket + '</td>' +
                    '<td>' + ticket.fecha_apertura + '</td>' +
                    '<td>' + ticket.fecha_arribo_locacion + '</td>' +
                    '<td>' + ticket.fecha_cierre + '</td>' +
                    '<td>' + ticket.id_componente + '</td>' +
                    '<td>' + ticket.id_estacion + '</td>' +
                    '<td>' + ticket.id_puerta + '</td>' +
                    '<td>' + ticket.id_ticket + '</td>' +
                    '<td>' + ticket.id_vagon + '</td>' +
                    '<td>' + ticket.identificacion + '</td>' +
                    '<td>' + ticket.nivel_falla + '</td>' +
                    '</tr>'
                );
            });
        },
        error: function (xhr, status, error) {
            // Handle the error
        }
    });
}

