var dataHideColums, dataSearchColumns;

$(document).ready(function () {
    createElemntsTimes();
    multiSelect();
    drodownDataSearch(columnsSearch, 'CustomName', 'searchParam');
});

function exportToExcel() {
    // Obtén la tabla por su ID
    var table = document.getElementById("table");

    // Crea un objeto de trabajo de Excel
    var wb = XLSX.utils.table_to_book(table, { sheet: "Sheet 1" });

    // Genera el archivo de Excel
    var wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'array' });

    // Descarga el archivo
    saveAs(new Blob([wbout], { type: 'application/octet-stream' }), 'tickets.xlsx');
}

function ServiceGetTickets() {
    var startDate = $('#dtpStart').val();
    var endDate = $('#dtpEnd').val();
    var max = document.getElementById("maxSelect").value;
    var componente = $('#componente').val();
    console.log("idComponente: " + componente);


    // Display loading modal
    Swal.fire({
        title: 'Cargando...',
        allowOutsideClick: false,
        showConfirmButton: false, // Oculta el botón de confirmación
        onBeforeOpen: (modal) => {
            modal.showLoading();
            modal.disableCloseButton(); // Desactiva el botón de cancelación
        }
    });
    $.ajax({
        type: "GET",
        url: "/Tickets/GetTickets",
        data: { startDate: startDate, endDate: endDate, max:max, componente: componente },
        success: function (response) {

            // Hide loading modal
            Swal.close();
            // Clear the current table body
            $('#table tbody').empty();

            // Loop through the response and append new rows to the table
            $.each(response, function (index, ticket) {
                $('#table tbody').append(
                    '<tr>' +
                    '<td>' + ticket.id_ticket + '</td>' +
                    '<td>' + ticket.fecha_apertura + '</td>' +
                    '<td>' + ticket.id_componente + '</td>' +
                    '<td>' + ticket.estado_ticket + '</td>' +
                    '<td>' + ticket.nivel_falla + '</td>' +
                    '<td>' + ticket.codigo_falla + '</td>' +
                    '<td>' + ticket.diagnostico_falla + '</td>' +
                    '<td>' + ticket.fecha_arribo_locacion + '</td>' +
                    '<td>' + ticket.fecha_cierre + '</td>' +
                    '<td>' + ticket.componente_Parte + '</td>' +
                    '<td>' + ticket.descripcion_reparacion + '</td>' +
                    '<td>' + ticket.id_estacion + '</td>' +
                    '<td>' + ticket.id_vagon + '</td>' +
                    '<td>' + ticket.id_puerta + '</td>' +
                    '<td>' + ticket.identificacion + '</td>' +
                    '</tr>'
                );
            });
        },
        error: function (xhr, status, error) {
            // Hide loading modal
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: error
            });
        }
    });
}

function showMoreInformation(ticketId) {
    Swal.fire({
        title: 'Informacion del Ticket',
        html: `<p><strong>Ticket ID:</strong> ${ticketId}</p>
           <p><strong>Fecha apertura:</strong> @ticket.fecha_apertura</p>
           <p><strong>Id componente:</strong> @ticket.id_componente</p>
           <p><strong>Estado ticket:</strong> @ticket.estado_ticket</p>
           <!-- Include other ticket properties as needed -->
          `,
        confirmButtonText: 'Cerrar',
        width: 600,
        padding: '1.5rem',
        backdrop: true,
        allowOutsideClick: false,
        allowEscapeKey: false,
    });
}



