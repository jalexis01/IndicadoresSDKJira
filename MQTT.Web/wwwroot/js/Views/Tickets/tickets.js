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
function adjustColumnWidths(sheet) {
    var colWidths = [];

    // Obtener las dimensiones de las celdas
    for (var cell in sheet) {
        if (cell[0] === '!') continue;
        var col = cell.replace(/[^A-Z]/g, '');
        var row = parseInt(cell.replace(/[^0-9]/g, ''));
        var value = sheet[cell].v;

        // Calcular el ancho máximo de cada columna
        var width = value.toString().length * 1.3; // Ajusta el factor de ancho según tus necesidades

        if (colWidths[col] === undefined || width > colWidths[col]) {
            colWidths[col] = width;
        }
    }

    // Establecer el ancho de las columnas en la hoja de cálculo
    for (var col in colWidths) {
        sheet[col + '1'].s = { width: colWidths[col] };
    }
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



