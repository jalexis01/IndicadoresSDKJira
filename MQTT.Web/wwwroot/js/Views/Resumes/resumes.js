var dataHideColums, dataSearchColumns;
$(document).ready(function () {
    createElemntsTimes();
    multiSelect();
    drodownDataSearch(columnsSearch, 'CustomName', 'searchParam');
});

function validateDates() {
    var componente = $('#componente').val();

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

                var verVideoButtonHtml = '';
                if (cantVideos > 0) {
                    verVideoButtonHtml = '<button id="verVideoBtn" style="background: linear-gradient(to bottom right, #ff4d4d, #ff9999); color: white; border: none; border-radius: 4px; padding: 8px 16px; cursor: pointer; font-weight: bold; margin-left: 5px; margin-right: 5px;" onclick="openVideoModal(\'' + idTicket + '\')">Ver video (' + cantVideos + ')</button>';
                } else {
                    verVideoButtonHtml = '<button id="verVideoBtn" style="background: linear-gradient(to bottom right, #ff4d4d, #ff9999); color: white; border: none; border-radius: 4px; padding: 8px 16px; cursor: not-allowed; font-weight: bold; margin-left: 5px; margin-right: 5px;" disabled title="No tiene videos adjuntos">Ver video (0)</button>';
                }

                var footerHtml = verVideoButtonHtml + '<button id="cerrarBtn" style="background: linear-gradient(to bottom right, #888888, #555555); color: white; border: none; border-radius: 4px; padding: 8px 16px; cursor: pointer; font-weight: bold; margin-left: 5px; margin-right: 5px;" onclick="closeSwal()">Cerrar</button>';

                for (var i = 0; i < base64Images.length; i++) {
                    var base64Image = base64Images[i];

                    var imageElement = document.createElement('img');
                    imageElement.src = 'data:image/jpeg;base64,' + base64Image;
                    imageElement.style.width = '90%';
                    imageElement.style.margin = '10px';
                    imageContainer.appendChild(imageElement);

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

                Swal.fire({
                    title: 'Imágenes del ' + idTicket,
                    html: imageContainer,
                    showCloseButton: true,
                    showConfirmButton: false,
                    footer: footerHtml,
                    customClass: {
                        container: 'swal-wide',
                    },
                    width: '50%',
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
function getContAdjuntosTicket(idTicket, callback) {
    var adjuntos = null;

    $.ajax({
        url: '/Tickets/getContadorAdjuntos?idTicket=' + idTicket,
        success: function (response) {
            cantImagenes = response.cantidadImagenes;
            cantVideos = response.cantidadVideos;
            adjuntos = {
                cantImagenes,
                cantVideos
            }
            callback(adjuntos);
        },
        error: function () {
            callback(0);

        }
    });
}

function getContVideoTicket(idTicket) {
    $.ajax({
        url: '/Tickets/getContadorVideos?idTicket=' + idTicket,
        success: function (response) {
            cantVideos = response;
            updateButtonLabelVideo(idTicket, cantVideos);
        },
        error: function () {
            var cantVideos = 0;
            updateButtonLabelVideo(idTicket, cantVideos);
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
            verVideoButton.disabled = false;
        } else {
            verVideoButton.innerHTML = 'Ver video (0)';
            verVideoButton.disabled = true;
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

function getAdjuntoTicket(idTicket) {
    if (cantImagenes > 0) {
        getImageTicket(idTicket);
    }
    else if (cantVideos > 0) {
        openVideoModal(idTicket);
    }
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

                var verImagenButtonHtml = '';
                if (cantImagenes > 0) {
                    verImagenButtonHtml = '<button id="verMasBtn" style="background: linear-gradient(to bottom right, #ff4d4d, #ff9999); color: white; border: none; border-radius: 4px; padding: 8px 16px; cursor: pointer; font-weight: bold; margin-left: 5px; margin-right: 5px;" onclick="getImageTicket(\'' + idTicket + '\')">Ver imagen (' + cantImagenes + ')</button>';
                } else {
                    verImagenButtonHtml = '<button id="verMasBtn" style="background: linear-gradient(to bottom right, #ff4d4d, #ff9999); color: white; border: none; border-radius: 4px; padding: 8px 16px; cursor: not-allowed; font-weight: bold; margin-left: 5px; margin-right: 5px;" disabled title="No tiene imagenes adjuntos">Ver imagen (0)</button>';
                }

                var footerHtml = verImagenButtonHtml + '<button id="cerrarBtn" style="background: linear-gradient(to bottom right, #888888, #555555); color: white; border: none; border-radius: 4px; padding: 8px 16px; cursor: pointer; font-weight: bold; margin-left: 5px; margin-right: 5px;" onclick="closeSwal()">Cerrar</button>';

                for (var i = 0; i < base64Videos.length; i++) {
                    var base64Video = base64Videos[i];
                    var videoElement = document.createElement('video');
                    videoElement.controls = true;
                    videoElement.src = 'data:video/mp4;base64,' + base64Video;
                    videoElement.style.width = '70%';
                    videoElement.style.margin = '10px';
                    videoContainer.appendChild(videoElement);

                    var downloadContainer = document.createElement('div');
                    downloadContainer.style.textAlign = 'center';
                    downloadContainer.style.marginTop = '5px';

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

                    videoContainer.appendChild(downloadContainer);
                }

                Swal.fire({
                    title: 'Videos del ' + idTicket,
                    html: videoContainer,
                    showCloseButton: true,
                    showConfirmButton: false,
                    footer: footerHtml,
                    customClass: {
                        container: 'swal-wide',
                    },
                    width: '50%',
                    padding: '2rem',
                    backdrop: true,
                    allowOutsideClick: true,
                    allowEscapeKey: false,
                });

                document.getElementById('verMasBtn').addEventListener('click', async function () {

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

    $.ajax({
        type: "GET",
        url: "/Resumes/GetComponenteHV",
        data: { idComponente: componente },
    })
        .then(componenteData => {

            Swal.fire({
                title: 'Cargando...',
                allowOutsideClick: false,
                showConfirmButton: false,
                onBeforeOpen: (modal) => {
                    modal.showLoading();
                    modal.disableCloseButton();
                }
            });
            // Verifica si se obtuvieron datos del componente
            if (componenteData) {
                // Crea la nueva lista de componentes con los datos obtenidos
                var serial = componenteData.estado ? componenteData.serial : componenteData.idComponente;

                var estado = componenteData.estado;

                listaComponentes = [
                    {
                        Marca: 'Manatee',
                        Modelo: componenteData.modelo,
                        Fabricante: 'Manatee',
                        IDPuerta: componenteData.idComponente,
                        NumeroInterno: componenteData.serial,
                        AnioFabricacion: componenteData.anioFabricacion,
                        InicioOperacion: componenteData.fechaInicio,
                        HorasOperacion: componenteData.horasDeOperacion
                    }
                ];

                return $.ajax({
                    type: "GET",
                    url: "/Resumes/GetTickets",
                    data: { startDate: startDate, endDate: endDate, max: max, serial: serial, cerrados: true, estado: estado },
                });
            } else {
                throw new Error('No se encontraron datos del componente.');
            }
        })
        .then(data => {
            Swal.close();
            console.log(data);
            //if (data.length == 0) {
            //    noData();
            //    return;

            //} else {
            var columnContent = '<table class="result-box-table">';

            columnContent += '<tr>';
            columnContent += '<th class="header-cell1" colspan="4" style="text-align: center;">INFORMACIÓN COMPONENTE</th>';

            columnContent += '</tr>';

            columnContent += '<tr>';
            columnContent += '<th class="header-cell">MARCA</th><td>' + listaComponentes[0].Marca + '</td>';
            columnContent += '<th class="header-cell">IDPUERTA</th><td>' + listaComponentes[0].IDPuerta + '</td>';
            columnContent += '</tr>';

            columnContent += '<tr>';
            columnContent += '<th class="header-cell">MODELO</th><td>' + listaComponentes[0].Modelo + '</td>';
            columnContent += '<th class="header-cell">N° INTERNO</th><td>' + listaComponentes[0].NumeroInterno + '</td>';
            columnContent += '</tr>';

            columnContent += '<tr>';
            columnContent += '<th class="header-cell">FABRICANTE</th><td>' + listaComponentes[0].Fabricante + '</td>';

            var anio = '';
            anio = listaComponentes[0].AnioFabricacion == 1900 ? '' : listaComponentes[0].AnioFabricacion;
            columnContent += '<th class="header-cell">AÑO DE FABRICACIÓN</th><td>' + anio + '</td>';
            columnContent += '</tr>';

            //var fechaInicioOperacion = new Date(listaComponentes[0].InicioOperacion);
            //var fechaInicioOperacionFormateada = fechaInicioOperacion.toISOString().substring(0, 10);

            //columnContent += '<tr>';
            //columnContent += '<th class="header-cell">FECHA INICIO OPERACIÓN</th><td>' + fechaInicioOperacionFormateada + '</td>';

            var fechaInicioOperacionFormateada = '';
            var fechaInicioOperacion = new Date(listaComponentes[0].InicioOperacion);
            if (fechaInicioOperacion.toISOString().substring(0, 10) !== '1900-01-01') {
                fechaInicioOperacionFormateada = fechaInicioOperacion.toISOString().substring(0, 10);
            }

            columnContent += '<tr>';
            columnContent += '<th class="header-cell">FECHA INICIO OPERACIÓN</th><td>' + fechaInicioOperacionFormateada + '</td>';

            //Se cambia en la vista las horas de operacion
            var horas = '';
            horas = listaComponentes[0].HorasOperacion == -1 ? '' : listaComponentes[0].HorasOperacion;
            columnContent += '<th class="header-cell">HORAS DE OPERACIÓN</th><td>' + horas + '</td>';

            columnContent += '</table>';

            // Mostrar la tabla en el cuadro de texto
            $('#resultBox').html(columnContent);
            $('#resultBoxContainer').show();

            let dataColumns = setColums(data, null);
            let exportFunctions = addFnctionsGrid(['Excel']);

            dataColumns = addCommandsGridDetails(dataColumns);
            dataGridSave = data;
            setGrid(data, dataColumns, exportFunctions);

            $('#button-primary-descargar').show();
            //}
        })
        .catch(error => {
            if (error.status && error.status === 404) {
                Swal.fire({
                    title: 'ID Componente no encontrado',
                });
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Error...',
                    text: error.message
                });
            }
            //Swal.close();
        });
}

const targetEl = document.getElementById('dropdownInformation');

const triggerEl = document.getElementById('dropdownInformationButton');

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

    var modelo = listaComponentes[0].Modelo;

    //var environmentType = '@ViewBag.EnvironmentType';

    var imageUrl = "";

    if (environmentType === 'Manatee') {
        imageUrl = "../../img/plantillasNautilus/" + modelo + ".jpg";
    } else {
        imageUrl = "../../img/plantillasAssa/" + modelo + ".jpg";
    }

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

function descargarHv() {
    Swal.fire({
        icon: 'info',
        title: 'Descarga hoja de vida',
        text: 'Descarga en proceso...',
    });
    var componente = $('#componente').val();
    $.ajax({
        url: '/Resumes/DownloadExcelAjax',
        type: 'POST',
        data: { idComponente: componente },
        success: function (result) {
            if (result.success) {
                Swal.close();
                console.log(result.message);
            } else {
                console.error(result.message);
            }
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
}