var dataHideColums, dataSearchColumns;
$(document).ready(function () {
    createElemntsTimes();
    multiSelect();
    drodownDataSearch(columnsSearch, 'CustomName', 'searchParam');
});

function validateDates() {
    var startDate = $('#dtpStart').val();
    var endDate = $('#dtpEnd').val();
    console.log("Fecha inicial: " + startDate);
    console.log("Fecha final: " + endDate);

    var componente = $('#componente').val();
    console.log("idComponente: " + componente);

    if (componente.trim() === "") {
        Swal.fire({
            title: 'Debe ingresar el idComponente',
        });
    } else {
        ServiceGetMessages();
    }
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



function showMoreInformationTickets(idTicket) {
    let ticketToReturn
    $.ajax({
        url: '/Tickets/consultarTicket?idTicket=' + idTicket,
        data: { idTicket: idTicket },
    }).then(response => JSON.parse(JSON.stringify(response)))
        .then(data => {
            console.log("Ticket data: " + data)
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

                // Datos quemados para cada celda
                var datosQuemados = [
                    { Marca: 'xxxxxxxxxxxxxxxxxx', Modelo: 'xxxx', Fabricante: 'xxxx', IDComponente: 'xxxx', NumeroInterno: 'xxxx', AnioFabricacion: 'xxxx', InicioOperacion: 'xxxx', HorasOperacion: 'xxxx' },
                ];

                // Construir el contenido de la tabla HTML
                var columnContent = '<table class="result-box-table">';

                columnContent += '<tr>';
                columnContent += '<th>MARCA</th><td>' + datosQuemados[0].Marca + '</td>';
                columnContent += '<th>IDPUERTA</th><td>' + datosQuemados[0].IDComponente + '</td>';
                columnContent += '</tr>';

                columnContent += '<tr>';
                columnContent += '<th>MODELO</th><td>' + datosQuemados[0].Modelo + '</td>';
                columnContent += '<th>N° INTERNO</th><td>' + datosQuemados[0].NumeroInterno + '</td>';
                columnContent += '</tr>';

                columnContent += '<tr>';
                columnContent += '<th>FABRICANTE</th><td>' + datosQuemados[0].Fabricante + '</td>';
                columnContent += '<th>AÑO DE FABRICACION</th><td>' + datosQuemados[0].AnioFabricacion + '</td>';
                columnContent += '</tr>';

                columnContent += '<tr>';
                columnContent += '<th>FECHA INICIO OPERACIÓN</th><td>' + datosQuemados[0].InicioOperacion + '</td>';
                columnContent += '<th>HORAS DE OPERACIÓN</th><td>' + datosQuemados[0].HorasOperacion + '</td>';
                columnContent += '</tr>';

                columnContent += '</table>';

                // Mostrar la tabla en el cuadro de texto
                $('#resultBox').html(columnContent);
                $('#resultBoxContainer').show();

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

function openImage() {
    var datosQuemados = [
        { Marca: 'xxxx', Modelo: 'ss-nautilus', Fabricante: 'xxxx', IDComponente: 'xxxx', NumeroInterno: 'xxxx', AnioFabricacion: 'xxxx', InicioOperacion: 'xxxx', HorasOperacion: 'xxxx' },
    ];
    var modelo = datosQuemados[0].Modelo;
    // Puedes cambiar la URL de la imagen según tus necesidades
    var imageUrl = "../../img/" + modelo + ".jpg";

    // Muestra la imagen en un modal usando SweetAlert2
    Swal.fire({
        imageUrl: imageUrl,
        imageAlt: 'Imagen de ' + modelo,
        confirmButtonText: 'Cerrar',
        showCloseButton: true,
        customClass: {
            popup: 'custom-modal-class',
            closeButton: 'custom-close-button-class',
            image: 'custom-image-class'
        }
    });
}