﻿var dataHideColums, dataSearchColumns;

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

function showMoreInformation(idTicket) {
    // Realizar una solicitud AJAX para obtener los detalles del ticket
    $.ajax({
        url: '/Tickets/consultarTicket', // Especifica la URL correcta para invocar el método consultarTicket en el servidor
        data: { idTicket: idTicket },
        success: function (ticket) {
            console.log(ticket);
            // Utilizar los datos del ticket para mostrar la información en el diálogo emergente
            Swal.fire({
                title: 'Información del Ticket',
                html: `<p style="text-align: left;"><strong>Id ticket:</strong> ${ticket.id}</p>
                       <p style="text-align: left;"><strong>Ajustes ITS:</strong> ${ticket.ajustesITS}</p>
                       <p style="text-align: left;"><strong>AjustesPuerta:</strong> ${ticket.ajustesPuerta}</p>
                       <p style="text-align: left;"><strong>AjusteRFID:</strong> ${ticket.ajustesRFID}</p>
                       <p style="text-align: left;"><strong>CanalComunicación:</strong> ${ticket.canalComunicacion}</p>
                       <p style="text-align: left;"><strong>CantidadRepuestosUtilizados:</strong> ${ticket.cantidadRepuestosUtilizados}</p>
                       <p style="text-align: left;"><strong>ClaseFallo:</strong> ${ticket.claseFallo}</p>
                       <p style="text-align: left;"><strong>Código plan mantenimiento:</strong> ${ticket.codigoPlanMantenimiento}</p>
                       <p style="text-align: left;"><strong>ConfiguracionITS:</strong> ${ticket.configuracionITS}</p>
                       <p style="text-align: left;"><strong>ConfiguracionPuerta:</strong> ${ticket.configuracionPuerta}</p>
                       <p style="text-align: left;"><strong>ConfiguracionRFID:</strong> ${ticket.configuracionRFID}</p>
                       <p style="text-align: left;"><strong>DescripionActividadMantenimiento:</strong> ${ticket.descripcionActividadMantenimiento}</p>
                       <p style="text-align: left;"><strong>DescripcionFallo:</strong> ${ticket.descripcionFallo}</p>
                       <p style="text-align: left;"><strong>DescripciónReparacion:</strong> ${ticket.descripcionReparacion}</p>
                       <p style="text-align: left;"><strong>DescripcionRepuesto:</strong> ${ticket.descripcionRepuesto}</p>
                       <p style="text-align: left;"><strong>DiagnosticoCausa:</strong> ${ticket.diagnosticoCausa}</p>
                       <!-- Include other ticket properties as needed -->`,
                confirmButtonText: 'Cerrar',
                width: 600,
                padding: '1.5rem',
                backdrop: true,
                allowOutsideClick: false,
                allowEscapeKey: false,
            });
        },
        error: function () {
            // Manejar el error si la solicitud AJAX falla
            Swal.fire('Error', 'No se pudo obtener la información del ticket', 'error');
        }
    });
}



