﻿var dataHideColums, dataSearchColumns;

function validateEstacionValue(value) {
    // Verificar que el valor contiene solo números y comas
    var regex = /^[0-9, ]*$/;
    return regex.test(value);
}

function handleEstacionInput(event) {
    var value = event.target.value;

    if (!validateEstacionValue(value)) {
        // Muestra un mensaje de error y limpia el campo si el valor es inválido
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'El campo ID Estación solo debe contener números, comas y espacios.'
        }).then(() => {
            event.target.value = value.replace(/[^0-9, ]/g, ''); // Elimina caracteres no permitidos
        });
    }
}

$(document).ready(function () {
    createElemntsTimes();
    multiSelect();
    drodownDataSearch(columnsSearch, 'CustomName', 'searchParam');
    GetStations();

    // Listener para el cambio en el select de estaciones
    $('#station-select').on('change', function () {
        const selectedStation = $(this).val(); // Valor seleccionado
        let currentSelectedStations = $('#multiSelect2').val(); // Valor actual en multiSelect2

        // Si se selecciona "Seleccione...", limpiar todo
        if (selectedStation === "") {
            $('#multiSelect2').val(""); // Limpiar el campo multiSelect2
            $('#station-select option').removeClass('selected-option'); // Limpiar la clase seleccionada
            return;
        }

        // Verificar si la estación ya está seleccionada
        if ($('#station-select option[value="' + selectedStation + '"]').hasClass('selected-option')) {
            // Si la estación ya está seleccionada, eliminarla
            currentSelectedStations = currentSelectedStations.split(', ').filter(station => station !== selectedStation);
            $('#multiSelect2').val(currentSelectedStations.join(', ')); // Actualizar el campo sin la estación deseleccionada
            $('#station-select option[value="' + selectedStation + '"]').removeClass('selected-option'); // Quitar el color
        } else {
            // Si la estación no está seleccionada, agregarla
            if (currentSelectedStations.length > 0) {
                // Si ya hay valores en el campo, agregar la nueva estación separada por una coma
                currentSelectedStations += ', ' + selectedStation;
            } else {
                // Si no hay valores, simplemente agregar la nueva estación
                currentSelectedStations = selectedStation;
            }
            $('#multiSelect2').val(currentSelectedStations); // Actualizar el campo con las estaciones seleccionadas
            $('#station-select option[value="' + selectedStation + '"]').addClass('selected-option'); // Aplicar el color de selección
        }
    });

    // Evento al hacer clic en el botón de buscar
    $('#button-primary').on('click', function () {
        validarDates(); // Validar las fechas antes de buscar
    });
});

function validarDates() {
    var startDate = $('#dtpStartMessage').val();
    var endDate = $('#dtpEndMessage').val();

    if (startDate === "" || endDate === "") {
        Swal.fire({
            title: 'Debe seleccionar la fecha',
        });
    } else {
        ServiceGetMessages();
    }
}

function GetStations() {
    // Muestra un loader mientras se obtienen los datos
    $(".container-loader").css({ 'display': 'flex' });

    $.ajax({
        type: "GET",
        dataType: "json",
        url: '/Elements/GetElements',
    }).then(response => JSON.parse(JSON.stringify(response)))
        .then(data => {
            $(".container-loader").css({ 'display': 'none' });

            if (data.data.length === 0) {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: 'No se encontraron estaciones.'
                });
                return;
            }

            // Poblar el select con los elementos obtenidos
            const stationSelect = $('#station-select'); // Asegúrate de tener un select con este ID en tu vista
            stationSelect.empty(); // Limpia el select antes de llenarlo

            // Agrega la opción predeterminada
            stationSelect.append('<option value="">Seleccione...</option>');

            data.data.forEach(element => {
                // Agrega una opción por cada estación con el code como valor y el name como texto
                stationSelect.append(new Option(element.name, element.code));
            });
        })
        .catch(error => {
            $(".container-loader").css({ 'display': 'none' });
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: error.name + ': ' + error.message
            });
        });
}

function ServiceGetMessages() {
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

    //var idEstacion = ['8001'];
    //var estacionValue = $("#estacion").val().trim();
    //var estacionValue = $("#station-select").val();

    var estacionesSeleccionadas = $('#multiSelect2').val();

    var idEstacion = [];
    if ($('#multiSelect2').val().length) {
        idEstacion = estacionesSeleccionadas.split(',').map(function (estacion) {
            return estacion.trim(); // Eliminar espacios en blanco alrededor de cada estación
        });
    }



    var data = {
        startDate: $("#dtpStartMessage").val(),
        endDate: $("#dtpEndMessage").val(),
        idEstacion: idEstacion
    };

    $.ajax({
        data: data,
        type: "POST",
        dataType: "json",
        url: '/Messages/Search',
    }).then(response => JSON.parse(JSON.stringify(response)))
        .then(data => {
            // Hide loading modal
            Swal.close();

            if (data.dataMessages.length == 0) {
                noData();
                return;
            }
            filtersData = data.filters;
            dropdowns.value = null;
            multiSelectInput.value = null;
            dropdowns.enabled = false;
            multiSelectInput.enabled = false;
            const btn = document.getElementById('button-filter');
            btn.disabled = false;
            dateDocuments = $("#dtpStartMessage").val() + " " + $("#dtpEndMessage").val();
            let dataColumns = setColums(data.dataMessages, columnsToHide);
            let exportFunctions = addFnctionsGrid(['Excel']);
            dataColumns = addCommandsGridDetails(dataColumns);
            dropdowns.enabled = true;
            dataGridSave = data.dataMessages;
            setGrid(data.dataMessages, dataColumns, exportFunctions);
        })
        .catch(error => {
            // Hide loading modal
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