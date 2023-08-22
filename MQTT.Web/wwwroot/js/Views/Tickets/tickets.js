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

    if (startDate === "" || endDate === "") {
        Swal.fire({
            title: 'Debe seleccionar la fecha',
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
                var footerHtml =  '<button id="verVideoBtn" style="background: linear-gradient(to bottom right, #ff4d4d, #ff9999); color: white; border: none; border-radius: 4px; padding: 8px 16px; cursor: pointer; font-weight: bold; margin-left: 5px; margin-right: 5px;" onclick="openVideoModal(\'' + idTicket + '\')">Ver video</button>' +
                    '<button id="cerrarBtn" style="background: linear-gradient(to bottom right, #888888, #555555); color: white; border: none; border-radius: 4px; padding: 8px 16px; cursor: pointer; font-weight: bold; margin-left: 5px; margin-right: 5px;" onclick="closeSwal()">Cerrar</button>';
                // Iterate through the list of base64 images and create image elements
                for (var i = 0; i < base64Images.length; i++) {
                    var base64Image = base64Images[i];

                    // Create the image element
                    var imageElement = document.createElement('img');
                    imageElement.src = 'data:image/jpeg;base64,' + base64Image;
                    imageElement.style.width = '90%'; // Adjust the width as needed
                    imageElement.style.margin = '10px';
                    imageContainer.appendChild(imageElement);

                    // Create the download button for the image
                    var downloadButton = document.createElement('a');
                    downloadButton.textContent = 'Descargar imagen';
                    downloadButton.href = 'data:image/jpeg;base64,' + base64Image;
                    downloadButton.download = 'imagen_' + (i + 1) + '.jpg';
                    downloadButton.style.display = 'block';
                    downloadButton.style.marginTop = '5px';
                    downloadButton.style.background = 'linear-gradient(to bottom right, #4CAF50, #66BB6A)';
                    downloadButton.style.color = 'white';
                    downloadButton.style.border = '5px solid #63c267';
                    downloadButton.style.borderRadius = '4px';
                    downloadButton.style.padding = '8px 16px';
                    downloadButton.style.cursor = 'pointer';
                    downloadButton.style.textDecoration = 'none';
                    imageContainer.appendChild(downloadButton);
                }

                // Show the image container in the Swal dialog
                Swal.fire({
                    title: 'Imágenes del ' + idTicket,
                    html: imageContainer,
                    //confirmButtonText: 'Cerrar',
                    showCloseButton: true,
                    showConfirmButton: false,
                    footer: footerHtml,
                    customClass: {
                        container: 'swal-wide',
                    },
                    width: '50%', // Adjust the width as needed
                    padding: '2rem',
                    backdrop: true,
                    allowOutsideClick: true,
                    allowEscapeKey: false,

                });


                document.getElementById('verVideoBtn').addEventListener('click', function () {
                    Swal.fire({
                        title: 'Cargando videos...',
                        html: '<div class="spinner-border text-primary" role="status"><span class="sr-only">Cargando imágenes...</span></div>',
                        showCancelButton: false,
                        showConfirmButton: false,
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: {
                            popup: 'swal2-no-close',
                            container: 'swal2-no-close',
                        },
                        didOpen: () => {
                            Swal.showLoading();
                            // Llama a openVideoModal aquí, después de mostrar el modal de carga
                        }
                    });
                });



            } else {
                Swal.fire({
                    showConfirmButton: false,
                    title: 'Advertencia',
                    text: 'El ticket no tiene imagenes',
                    icon: 'info',
                    footer: '<button id="cerrarBtn1" style="background: linear-gradient(to bottom right, #888888, #555555); color: white; border: none; border-radius: 4px; padding: 8px 16px; cursor: pointer; font-weight: bold; margin-left: 5px;" onclick="closeSwal();">Cerrar</button>'
                });
            }
        },
        error: function () {
            Swal.fire({
                showConfirmButton: false,
                title: 'Advertencia',
                text: 'El ticket no tiene imagenes',
                icon: 'info',
                footer: '<button id="cerrarBtn1" style="background: linear-gradient(to bottom right, #888888, #555555); color: white; border: none; border-radius: 4px; padding: 8px 16px; cursor: pointer; font-weight: bold; margin-left: 5px;" onclick="closeSwal();">Cerrar</button>'
            });
        }
    });
}

function getContImageTicket(idTicket) {
    $.ajax({
        url: '/Tickets/getContadorImagenes?idTicket=' + idTicket,
        
        success: function (response) {
            var cantImagenes = response;

            // Call the function to update the UI with the new value
            updateButtonLabelImagen(idTicket, cantImagenes);
        },
        error: function () {
            // If there's an error, disable the button
            updateButtonLabelImagen(idTicket, 0);
        }
    });
}

function getContVideoTicket(idTicket) {
    $.ajax({
        url: '/Tickets/getContadorVideos?idTicket=' + idTicket,
        success: function (response) {
            cantVideos = response; // Update the variable value

            // Call the function to update the UI with the new value
            updateButtonLabelVideo(idTicket, cantVideos);
        },
        error: function () {
            //cantVideos = 0; // Update the variable value on error
            // Call the function to update the UI with the new value
            updateButtonLabelVideo(idTicket, 0);
        }
    });
}


function updateButtonLabelImagen(idTicket, cantImagenes) {
    var verImagenButton = document.getElementById('verMasBtn');    
    if (verImagenButton) {
        if (cantImagenes > 0) {
            verImagenButton.innerHTML = 'Ver imagen (' + cantImagenes + ')';
            verImagenButton.disabled = false; // Enable the button
        } else {
            verImagenButton.innerHTML = 'Ver imagen (0)';
            verImagenButton.disabled = true; // Disable the button
            verImagenButton.setAttribute('title', 'No hay imágenes adjuntas');
        }
    }
    
}

function updateButtonLabelVideo(idTicket, cantVideos) {
    var verVideoButton = document.getElementById('verVideoBtn');
    if (cantVideos) {
        if (cantVideos > 0) {
            verVideoButton.innerHTML = 'Ver video (' + cantVideos + ')';
            verVideoButton.disabled = false; // Enable the button
        } else {
            verVideoButton.innerHTML = 'Ver video (0)';
            verVideoButton.disabled = true; // Disable the button
            verVideoButton.setAttribute('title', 'No hay videos adjuntos');
        }
    }
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
                videoContainer.style.flexDirection = 'column';
                videoContainer.style.alignItems = 'center';
                videoContainer.style.flexWrap = 'wrap';
                videoContainer.style.justifyContent = 'center';
                var footerHtml = '<button id="verMasBtn" style="background: linear-gradient(to bottom, #0071A1, #00BFFF); color: white; border: none; border-radius: 4px; padding: 8px 16px; cursor: pointer; font-weight: bold; margin-right: 5px;" onclick="getImageTicket(\'' + idTicket + '\')">Ver imagen</button>' +
                    '<button id="cerrarBtn" style="background: linear-gradient(to bottom right, #888888, #555555); color: white; border: none; border-radius: 4px; padding: 8px 16px; cursor: pointer; font-weight: bold; margin-left: 5px; margin-right: 5px;" onclick="closeSwal()">Cerrar</button>';
                   

                for (var i = 0; i < base64Videos.length; i++) {
                    var base64Video = base64Videos[i];
                    var videoElement = document.createElement('video');
                    videoElement.controls = true;
                    videoElement.src = 'data:video/mp4;base64,' + base64Video;
                    videoElement.style.width = '70%'; // Adjust the width as needed
                    videoElement.style.margin = '10px';
                    videoContainer.appendChild(videoElement);

                    // Create a div for the download button container
                    var downloadContainer = document.createElement('div');
                    downloadContainer.style.textAlign = 'center';
                    downloadContainer.style.marginTop = '5px';

                    // Create the download button for the video
                    var downloadButton = document.createElement('a');
                    downloadButton.textContent = 'Descargar video';
                    downloadButton.href = 'data:video/mp4;base64,' + base64Video;
                    downloadButton.download = 'video_' + (i + 1) + '.mp4';
                    downloadButton.style.display = 'block';
                    downloadButton.style.marginTop = '5px';
                    downloadButton.style.background = 'linear-gradient(to bottom right, #4CAF50, #66BB6A)';
                    downloadButton.style.color = 'white';
                    downloadButton.style.border = '5px solid #63c267';
                    downloadButton.style.borderRadius = '4px';
                    downloadButton.style.padding = '8px 16px';
                    downloadButton.style.cursor = 'pointer';
                    downloadButton.style.textDecoration = 'none';
                    downloadContainer.appendChild(downloadButton);


                    // Append the download container to the video container
                    videoContainer.appendChild(downloadContainer);
                }

                // Show the video container in the Swal dialog
                Swal.fire({
                    title: 'Videos del ' + idTicket,
                    html: videoContainer,
                    //confirmButtonText: 'Cerrar',
                    showCloseButton: true,
                    showConfirmButton: false,
                    footer: footerHtml,
                    customClass: {
                        container: 'swal-wide',
                    },
                    width: '50%', // Adjust the width as needed
                    padding: '2rem',
                    backdrop: true,
                    allowOutsideClick: true,
                    allowEscapeKey: false,
                });

                document.getElementById('verMasBtn').addEventListener('click', async function () {
                    // var imageContent = await getImageTicket(idTicket);
                    Swal.fire({
                        title: 'Cargando imágenes...',
                        html: '<div class="spinner-border text-primary" role="status"><span class="sr-only">Cargando imágenes...</span></div>',
                        showCancelButton: false,
                        showConfirmButton: false,
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: {
                            popup: 'swal2-no-close',
                            container: 'swal2-no-close',
                        },
                        didOpen: () => {
                            Swal.showLoading();
                        }
                    });
                });



            } else {
                Swal.fire({
                    showConfirmButton: false,
                    title: 'Advertencia',
                    text: 'El ticket no tiene videos',
                    icon: 'info',
                    footer: '<button id="cerrarBtn1" style="background: linear-gradient(to bottom right, #888888, #555555); color: white; border: none; border-radius: 4px; padding: 8px 16px; cursor: pointer; font-weight: bold; margin-left: 5px;" onclick="closeSwal();">Cerrar</button>'
                });
            }
        })
        .catch(function (error) {
            Swal.fire({
                showConfirmButton: false,
                title: 'Advertencia',
                text: 'El ticket no tiene videos',
                icon: 'info',
                footer: '<button id="cerrarBtn1" style="background: linear-gradient(to bottom right, #888888, #555555); color: white; border: none; border-radius: 4px; padding: 8px 16px; cursor: pointer; font-weight: bold; margin-left: 5px;" onclick="closeSwal();">Cerrar</button>'
            });
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
            console.log("Tickets")
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