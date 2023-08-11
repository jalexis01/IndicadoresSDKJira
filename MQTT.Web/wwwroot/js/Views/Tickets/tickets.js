var dataHideColums, dataSearchColumns;
$(document).ready(function () {
    createElemntsTimes();
    multiSelect();
    drodownDataSearch(columnsSearch, 'CustomName', 'searchParam');
});

//function exportToExcel() {
//    var table = document.getElementById("table");
//    var wb = XLSX.utils.table_to_book(table, { sheet: "Sheet 1" });
//    var wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'array' });
//    saveAs(new Blob([wbout], { type: 'application/octet-stream' }), 'tickets.xlsx');
//}


function validateDates() {
    var startDate = $('#dtpStart').val();
    var endDate = $('#dtpEnd').val();
    console.log("Fecha inicial: " + startDate);
    console.log("Fecha final: " + endDate);

    if (startDate === "" || endDate === "") {
        Swal.fire({
            title: 'Debe seleccionar la fecha',
        });
    } else {
        ServiceGetMessages();
    }
}

function ServiceGetTickets() {
    var startDate = $('#dtpStart').val();
    var endDate = $('#dtpEnd').val();
    var max = 0;
    var componente = $('#componente').val();
    console.log("idComponente: " + componente);
    console.log("Max: " + max);

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
                row.append($('<td onclick="showMoreInformation(\'' + ticket.id_ticket + '\')" style="cursor: pointer; background: none;">').text('Ver más'));
                row.append($('<td>').text(ticket.id_ticket));
                row.append($('<td>').text(ticket.fecha_apertura));
                row.append($('<td>').text(ticket.id_componente));
                row.append($('<td>').text(ticket.tipoComponente));
                row.append($('<td>').text(ticket.estado_ticket));
                row.append($('<td>').text(ticket.nivel_falla));
                row.append($('<td>').text(ticket.codigo_falla));
                row.append($('<td>').text(ticket.diagnostico_causa));
                row.append($('<td>').text(ticket.fecha_arribo_locacion));
                row.append($('<td>').text(ticket.fecha_cierre));
                row.append($('<td>').text(ticket.componente_Parte));
                row.append($('<td>').text(ticket.tipo_reparacion));
                row.append($('<td>').text(ticket.descripcion_reparacion));
                row.append($('<td>').text(ticket.id_estacion));
                row.append($('<td>').text(ticket.id_vagon));
                row.append($('<td>').text(ticket.id_puerta));
                row.append($('<td>').text(ticket.identificacion));
                row.append($('<td>').text(ticket.tipo_mantenimiento));
                row.append($('<td>').text(ticket.tipo_causa));
                row.append($('<td>').text(ticket.tipo_ajuste_configuracion));
                row.append($('<td>').text(ticket.descripcion));

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

function getImageTicket(idTicket) {
    $.ajax({
        url: '/Tickets/getImageTicket?idTicket=' + idTicket,
        data: { idTicket: idTicket },
        success: function (base64Images) {
            if (base64Images && base64Images.length > 0) {
                var imageContainer = document.createElement('div');
                imageContainer.style.display = 'flex';
                imageContainer.style.flexWrap = 'wrap';
                imageContainer.style.justifyContent = 'center';
                // Iterate through the list of base64 images and create image elements
                for (var i = 0; i < base64Images.length; i++) {
                    var base64Image = base64Images[i];
                    var imageElement = document.createElement('img');
                    imageElement.src = 'data:image/jpeg;base64,' + base64Image;
                    imageElement.style.width = '90%'; // Adjust the width as needed
                    imageElement.style.margin = '10px';
                    imageContainer.appendChild(imageElement);
                }

                // Show the image container in the Swal dialog
                Swal.fire({
                    title: 'Imágenes del ' + idTicket,
                    html: imageContainer,
                    confirmButtonText: 'Cerrar',
                    showCloseButton: true,
                    showConfirmButton: true,
                    customClass: {
                        container: 'swal-wide',
                    },
                    width: '80%', // Adjust the width as needed
                    padding: '2rem',
                    backdrop: true,
                    allowOutsideClick: true,
                    allowEscapeKey: false,
                   
                });
            } else {
                Swal.fire('Información', 'El ticket no tiene imágenes adjuntas', 'info');
            }
        },
        error: function () {
            Swal.fire('Error', 'El ticket no tiene imágenes', 'error');
        }
    });
}

function getVideoTicket(idTicket) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: '/Tickets/getVideoTicket?idTicket=' + idTicket,
            data: { idTicket: idTicket },
            success: function (base64Videos) {
                if (base64Videos && base64Videos.length > 0) {
                    resolve(base64Videos);
                } else {
                    reject('No se encontraron videos adjuntos');
                }
            },
            error: function () {
                reject('Error al obtener los videos');
            }
        });
    });
}
function openVideoModal(idTicket) {
    getVideoTicket(idTicket)
        .then(function (base64Videos) {
            if (base64Videos.length > 0) {
                var videoContainer = document.createElement('div');
                videoContainer.style.display = 'flex';
                videoContainer.style.flexWrap = 'wrap';
                videoContainer.style.justifyContent = 'center';

                // Iterate through the list of base64 videos and create video elements
                for (var i = 0; i < base64Videos.length; i++) {
                    var base64Video = base64Videos[i];
                    var videoElement = document.createElement('video');
                    videoElement.controls = true;
                    videoElement.src = 'data:video/mp4;base64,' + base64Video;
                    videoElement.style.width = '70%'; // Adjust the width as needed
                    videoElement.style.margin = '10px';
                    videoContainer.appendChild(videoElement);
                }

                // Show the video container in the Swal dialog
                Swal.fire({
                    title: 'Videos del ' + idTicket,
                    html: videoContainer,
                    confirmButtonText: 'Cerrar',
                    showCloseButton: true,
                    showConfirmButton: true,
                    customClass: {
                        container: 'swal-wide',
                    },
                    width: '50%', // Adjust the width as needed
                    padding: '2rem',
                    backdrop: true,
                    allowOutsideClick: true,
                    allowEscapeKey: false,
                });
            } else {
                Swal.fire('Información', 'El ticket no tiene videos adjuntos', 'info');
            }
        })
        .catch(function (error) {
            Swal.fire('Error', 'El ticket no tiene videos', 'error');
        });
}



function showMoreInformation(idTicket) {
    $.ajax({
        url: '/Tickets/consultarTicket?idTicket=' + idTicket,
        data: { idTicket: idTicket },
        success: function (ticket) {

            var html = '<table style="font-family: Arial, sans-serif; width: 100%; border-collapse: collapse;">';

            $.each(ticket, function (key, value) {

                html += '<tr>';
                html += '<td style="text-align: left; padding: 8px; border: 2px solid #7f69a5; white-space: nowrap; font-weight: bold; font-size: 14px; background-color: rgba(127, 105, 165, 0.3);">' + key + ':</td>';
                html += '<td style="text-align: left; padding: 8px; border: 2px solid #7f69a5; font-size: 13px;">' + value + '</td>';
                html += '</tr>';
            });

            html += '</table>';

            Swal.fire({
                title: 'Información del Ticket',
                html: html,
                confirmButtonText: 'Cerrar',
                showCloseButton: true,
                showConfirmButton: false,
                customClass: {
                    container: 'swal-wide',
                },
                width: '50%',
                padding: '2rem',
                backdrop: true,
                allowOutsideClick: true,
                allowEscapeKey: false,
            });
        },
        error: function () {
           
        }
    });
}

function exportToExcel() {
    var table = document.getElementById("table");
    var headers = table.getElementsByTagName("th");
    var rows = table.getElementsByTagName("tr");

    // Clonar la tabla para preservar la vista HTML original
    var clonedTable = table.cloneNode(true);

    // Eliminar el título de la primera columna en las cabeceras clonadas
    var clonedHeaders = clonedTable.getElementsByTagName("th");
    if (clonedHeaders.length > 0) {
        clonedHeaders[0].parentNode.removeChild(clonedHeaders[0]);
    }

    // Eliminar la primera columna de cada fila clonada (excepto las cabeceras)
    var clonedRows = clonedTable.getElementsByTagName("tr");
    for (var i = 0; i < clonedRows.length; i++) {
        var clonedCells = clonedRows[i].getElementsByTagName("td");
        if (clonedCells.length > 0) {
            clonedRows[i].removeChild(clonedCells[0]);
        }
    }

    // Crear el libro de Excel sin la primera columna y el título
    var wb = XLSX.utils.table_to_book(clonedTable, { sheet: "Tickets" });
    var wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'array' });
    saveAs(new Blob([wbout], { type: 'application/octet-stream' }), 'tickets.xlsx');
}

function showMoreInformationTickets(idTicket) {
    let ticketToReturn
    $.ajax({
        url: '/Tickets/consultarTicket?idTicket=' + idTicket,
        data: { idTicket: idTicket },
    }).then(response => JSON.parse(JSON.stringify(response)))
        .then(data => {
            console.log("Ticket data: "+data)
            if (data.length == 0) {
                noData();
                return;
            } else {
                ticketToReturn = setColums(data, null);
                return Ok(ticketToReturn);
            }
        })
        .catch(error => {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: error.name + ': ' + error.message
            });
        })
        .then(response => console.log('Success:', response));
}



/********************************* */
function ServiceGetMessages() {
    var startDate = $('#dtpStart').val();
    var endDate = $('#dtpEnd').val();
    var max = 0;
    var componente = $('#componente').val();
    console.log("idComponente: " + componente);
    console.log("Max: " + max);
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

    }).then(response => JSON.parse(JSON.stringify(response)))
        .then(data => {

            Swal.close();
            console.log(data)
            if (data.length == 0) {
                noData();
                return;
            } else {
                let dataColumns = setColums(data, null);
                let exportFunctions = addFnctionsGrid(['Excel']);
              
                dataColumns = addCommandsGridDetails(dataColumns);
                dataGridSave = data;
                setGrid(data, dataColumns, exportFunctions);
            }
        })
        .catch(error => {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: error.name + ': ' + error.message
            });
        })
        .then(response => console.log('Success:', response));
}



const targetEl = document.getElementById('dropdownInformation');

// set the element that trigger the dropdown menu on click
const triggerEl = document.getElementById('dropdownInformationButton');

// options with default values
const options = {
    placement: 'bottom',
    onHide: () => {
        console.log('dropdown has been hidden');
    },
    onShow: () => {
        console.log('dropdown has been shown');
    }
};

var detailsData = function (args) {
    //alert(JSON.stringify(args.rowData));
}