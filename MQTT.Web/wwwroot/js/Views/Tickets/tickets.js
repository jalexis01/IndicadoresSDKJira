$(document).ready(function () {
    createElemntsTimes();
    multiSelect();
    drodownDataSearch(columnsSearch, 'CustomName', 'searchParam');
});

function exportToExcel() {
    var table = document.getElementById("table");
    var wb = XLSX.utils.table_to_book(table, { sheet: "Sheet 1" });
    var wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'array' });
    saveAs(new Blob([wbout], { type: 'application/octet-stream' }), 'tickets.xlsx');
}

function ServiceGetTickets() {
    var startDate = $('#dtpStart').val();
    var endDate = $('#dtpEnd').val();
    var max = document.getElementById("maxSelect").value;
    var componente = $('#componente').val();
    console.log("idComponente: " + componente);

    Swal.fire({
        title: 'Cargando...',
        allowOutsideClick: false,
        showConfirmButton: false,
        onBeforeOpen: (modal) => {
            modal.showLoading();
            modal.disableCloseButton();
        }
    });
    $.ajax({
        type: "GET",
        url: "/Tickets/GetTickets",
        data: { startDate: startDate, endDate: endDate, max: max, componente: componente },
        success: function (response) {
            Swal.close();
            var tbody = $('#table tbody');
            tbody.empty();

            $.each(response, function (index, ticket) {
                var row = $('<tr>');
                console.log(ticket.id_ticket)
                row.append($('<td onclick="showMoreInformation(\'' + ticket.id_ticket + '\')" style="cursor: pointer; background: none;">').text('Ver Mas'));
                row.append($('<td>').text(ticket.id_ticket));
                row.append($('<td>').text(ticket.fecha_apertura));
                row.append($('<td>').text(ticket.id_componente));
                row.append($('<td>').text(ticket.estado_ticket));
                row.append($('<td>').text(ticket.nivel_falla));
                row.append($('<td>').text(ticket.codigo_falla));
                row.append($('<td>').text(ticket.diagnostico_causa));
                row.append($('<td>').text(ticket.fecha_arribo_locacion));
                row.append($('<td>').text(ticket.fecha_cierre));
                row.append($('<td>').text(ticket.componente_Parte));
                row.append($('<td>').text(ticket.descripcion_reparacion));
                row.append($('<td>').text(ticket.id_estacion));
                row.append($('<td>').text(ticket.id_vagon));
                row.append($('<td>').text(ticket.id_puerta));
                row.append($('<td>').text(ticket.identificacion));

                tbody.append(row);
            });
        },
        error: function (xhr, status, error) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: error
            });
        }
    });
}

function showMoreInformation(idTicket) {
    $.ajax({
        url: '/Tickets/consultarTicket?idTicket=' + idTicket,
        data: { idTicket: idTicket },
        success: function (ticket) {
            var html = '';
            $.each(ticket, function (key, value) {
                html += `<p style="text-align: left;"><strong>${key}:</strong> ${value}</p>`;
            });

            Swal.fire({
                title: 'Información del Ticket',
                html: html,
                confirmButtonText: 'Cerrar',
                width: 600,
                padding: '1.5rem',
                backdrop: true,
                allowOutsideClick: false,
                allowEscapeKey: false,
            });
        },
        error: function () {
            Swal.fire('Error', 'No se pudo obtener la información del ticket', 'error');
        }
    });
}
