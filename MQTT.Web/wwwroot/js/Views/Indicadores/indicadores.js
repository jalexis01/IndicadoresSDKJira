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
        ServiceGetIndicadores();
    }
}

function serviceGetIndicadores() {
    var startDate = $('#dtpStart').val();
    var endDate = $('#dtpEnd').val();
    //var max = document.getElementById("maxSelect").value;
    //var componente = $('#componente').val();
    console.log("Fecha inicial: " + startDate);
    console.log("Fecha final: " + endDate);

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
        url: "/Indicadores/GetIndicadores",
        data: { startDate: startDate, endDate: endDate },
        success: function (response) {
            Swal.close();
            var tbody = $('#table tbody');
            tbody.empty();

            $.each(response, function (index, indicador) {
                var row = $('<tr>');
                console.log('Nombre indicador: ' + indicador.nombre)
                console.log('Valor: ' + indicador.calculo)
                row.append($('<td>').text(indicador.nombre));
                row.append($('<td>').text(indicador.calculo));
                //console.log(indicador.nombre)
                //row.append($('<td>').text(indicador.nombre));
                //row.append($('<td>').text(indicador.calculo));
                //row.append($('<td>').text(indicador.descripcion));

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


/********************************* */
function ServiceGetIndicadores() {
    var startDate = $('#dtpStart').val();
    var endDate = $('#dtpEnd').val();
    //var max = document.getElementById("maxSelect").value;
    //var componente = $('#componente').val();
    console.log("Fecha inicial: " + startDate);
    console.log("Fecha final: " + endDate);

    Swal.fire({
        title: 'Cargando indicadores...',
        allowOutsideClick: false,
        showConfirmButton: false,
        onBeforeOpen: (modal) => {
            modal.showLoading();
            modal.disableCloseButton();
        }
    });
    $.ajax({
        type: "GET",
        url: "/Indicadores/GetIndicadores",
        data: { startDate: startDate, endDate: endDate },

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