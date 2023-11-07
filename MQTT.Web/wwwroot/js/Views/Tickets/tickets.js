var dataHideColums, dataSearchColumns;
var cantImagenes;
var cantVideos;
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

function getContImageTicket(idTicket, callback) {
    $.ajax({
        url: '/Tickets/getContadorImagenes?idTicket=' + idTicket,
        
        success: function (response) {            
            cantImagenes = response;
            callback(cantImagenes);
        },
        error: function () {
            callback(0);
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
function ServiceGetMessagesOriginal() {
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

//Patricia
async function ServiceGetMessages() {
    var startDate = $('#dtpStart').val();
    console.log("Hora inicial seleccionada: " + startDate);
    var endDate = $('#dtpEnd').val();
    console.log("Hora final seleccionada: " + endDate);
    var max = 0;
    var fechaInicial = new Date(startDate);
    console.log("Fecha inicial: " + fechaInicial);
    var fechaFinal = new Date(endDate);
    console.log("Fecha final: " + fechaFinal);
    var totalDatos = [];
    var componente = $('#componente').val();
    console.log("idComponente: " + componente);
    console.log("Max: " + max);

    const dynamicText = "%";

    Swal.fire({
        title: 'Cargando...',
        allowOutsideClick: false,
        showConfirmButton: false,
        onBeforeOpen: (modal) => {
            modal.showLoading();
            modal.disableCloseButton();
        }, text: dynamicText
    });
    var fechaInicio = new Date(fechaInicial);
    var fechaFin = new Date(fechaFinal);

    var totalDays = Math.ceil((fechaFinal - fechaInicial) / (1000 * 60 * 60 * 24));
    var currentDay = -1;

    //$("#cargando").html("Cargando...");
    while (fechaInicio <= fechaFin) {       

        // Imprime la fecha actual
        console.log(fechaInicio.toUTCString());
        const año = fechaInicio.getUTCFullYear();
        const mes = String(fechaInicio.getUTCMonth() + 1).padStart(2, '0'); // El mes es 0-indexado, por lo que sumamos 1
        const dia = String(fechaInicio.getUTCDate()).padStart(2, '0');
        const fechaFormateada = `${año}-${mes}-${dia}`;
        console.log(fechaFormateada);

        currentDay++;
        
        var progressPercentage = (currentDay / totalDays) * 100;
                
        Swal.update({
            text: `${progressPercentage.toFixed(0)}%`
        });

        //$("#cargando").html("Cargando... " + progressPercentage.toFixed(0) + "%");

        //$("#cargando").html("Cargando..." + fechaFormateada);
        try {
            const respuesta = await realizarSolicitudAjax(fechaFormateada);
            totalDatos.push(respuesta);
        } catch (error) {
            console.error("Error en la solicitud:", error);
        }
        // Incrementa la fecha en 1 día
        fechaInicio.setDate(fechaInicio.getDate() + 1);
    }
    //$("#cargando").html("");

    if (totalDatos.length == 0) {
        noData();
        return;
    } else {
        const arregloSimple = totalDatos.reduce((acumulador, arreglo) => {
            return acumulador.concat(arreglo);
        }, []);
        console.log(arregloSimple);
        let dataColumns = setColums(arregloSimple, null);
        let exportFunctions = addFnctionsGrid(['Excel']);
        dataColumns = addCommandsGridDetails(dataColumns);
        dataGridSave = arregloSimple;
        setGrid(arregloSimple, dataColumns, exportFunctions);
    }
    Swal.close();
}
function realizarSolicitudAjax(fecha, max, componente) {
    return new Promise((resolve, reject) => {
        $.ajax({
            type: "GET",
            url: "/Tickets/GetTickets",
            data: { startDate: fecha, endDate: fecha, max: max, componente: componente },
            success: function (data) {
                resolve(JSON.parse(JSON.stringify(data))); // Resuelve la promesa con la respuesta de la solicitud
            },
            error: function (xhr, status, error) {
                reject(error); // Rechaza la promesa en caso de error
            }
        });
    });
}

//Alexis
async function ServiceGetMessagesAlexis() {
    var startDate = $('#dtpStart').val();
    console.log("Hora inicial seleccionada: " + startDate);
    var endDate = $('#dtpEnd').val();
    console.log("Hora final seleccionada: " + endDate);
    var max = 0;

    var fechaInicial = new Date(startDate);
    fechaInicial.setHours(fechaInicial.getHours() + 5); // Sumar 5 horas
    fechaInicial.toISOString().slice(0, 10); // Formatear como 'YYYY-MM-DD'
    console.log("Fecha inicial P: " + fechaInicial);

    var fechaFinal = new Date(endDate);
    fechaFinal.setDate(fechaFinal.getDate() + 1); // Sumar 1 día
    //fechaInicial.setHours(fechaFinal.getHours() + 29); // Sumar 5 horas
    fechaFinal = fechaFinal.toISOString().slice(0, 10); // Formatear como 'YYYY-MM-DD'
    console.log("Fecha final P: " + fechaFinal);
    var totalDatos = [];

    var componente = $('#componente').val();
    console.log("idComponente: " + componente);
    console.log("Max: " + max);
    //Swal.fire({
    //    title: 'Cargando...',
    //    allowOutsideClick: false,
    //    showConfirmButton: false,
    //    onBeforeOpen: (modal) => {
    //        modal.showLoading();
    //        modal.disableCloseButton();
    //    }
    //});

    //Restar fechas y diferencia nos da el ciclo final de peticiones
    var fechaInicio = new Date(fechaInicial);

    var fechaFin = new Date(fechaFinal);

    // Calcular el número total de días entre fechaInicio y fechaFin
    var totalDays = Math.ceil((fechaFin - fechaInicio) / (1000 * 60 * 60 * 24));
    var dayCount = 0;

    $("#cargando").html("Cargando...");
    while (fechaInicio <= fechaFin) {

        // Calcular el progreso en términos de porcentaje
        dayCount++;
        var progressPercentage = (dayCount / totalDays) * 100;

        // Actualizar el texto en #cargando con el progreso
        $("#cargando").html("Cargando... " + progressPercentage.toFixed(0) + "%");


        // Imprime la fecha actual
        const año = fechaInicio.getFullYear();
        const mes = String(fechaInicio.getMonth() + 1).padStart(2, '0'); // El mes es 0-indexado, por lo que sumamos 1
        const dia = String(fechaInicio.getDate()).padStart(2, '0');
        const fechaFormateada = `${año}-${mes}-${dia}`;

        console.log(fechaFormateada);
        //$("#cargando").html("Cargando..." + fechaFormateada);
        try {
            const respuesta = await realizarSolicitudAjax(fechaFormateada);
            totalDatos.push(respuesta);
        } catch (error) {
            console.error("Error en la solicitud:", error);
        }
        // Incrementa la fecha en 1 día
        fechaInicio.setDate(fechaInicio.getDate() + 1);
    }
    $("#cargando").html("");
    if (totalDatos.length == 0) {
        noData();
        return;
    } else {
        const arregloSimple = totalDatos.reduce((acumulador, arreglo) => {
            return acumulador.concat(arreglo);
        }, []);

        // Ordenar el arreglo por la propiedad de fecha
        arregloSimple.sort((a, b) => new Date(a.fecha) - new Date(b.fecha));

        console.log(arregloSimple);

        let dataColumns = setColums(arregloSimple, null);
        let exportFunctions = addFnctionsGrid(['Excel']);

        dataColumns = addCommandsGridDetails(dataColumns);
        dataGridSave = arregloSimple;
        setGrid(arregloSimple, dataColumns, exportFunctions);
    }
}
function realizarSolicitudAjax(fecha, max, componente) {
    return new Promise((resolve, reject) => {
        $.ajax({
            type: "GET",
            url: "/Tickets/GetTickets",
            data: { startDate: fecha, endDate: fecha, max: max, componente: componente },
            success: function (data) {
                resolve(JSON.parse(JSON.stringify(data))); // Resuelve la promesa con la respuesta de la solicitud
            },
            error: function (xhr, status, error) {
                reject(error); // Rechaza la promesa en caso de error
            }
        });
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