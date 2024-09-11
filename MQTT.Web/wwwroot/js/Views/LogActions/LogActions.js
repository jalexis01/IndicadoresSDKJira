var dataHideColums, dataSearchColumns;

$(document).ready(function () {
    createElemntsTimes();
    multiSelect();
    drodownDataSearch(columnsSearch, 'CustomName', 'searchParam');
});

function validarDatos() {
    var startDate = $('#dtpStart').val();
    var endDate = $('#dtpEnd').val();

    if (startDate === "" || endDate === "") {
        Swal.fire({
            title: 'Debe seleccionar la fecha',
        });
    } else {
        fetchLogs();
    }
}

function fetchLogs() {
    var startDate = document.getElementById('dtpStart').value;
    var endDate = document.getElementById('dtpEnd').value;

    $.ajax({
        url: '/LogActions/GetLogActions',
        type: 'GET',
        data: {
            startDate: startDate,
            endDate: endDate
        },
        success: function (data) {
            // Destruir el Grid anterior si ya existe
            if (grid) {
                grid.destroy();
                $('#Grid').empty(); // Limpiar el contenedor del Grid
            }
            let exportFunctions = addFnctionsGrid(['Excel']);
            // Inicializar el Syncfusion Grid con los datos recibidos
            grid = new ej.grids.Grid({
                toolbar: exportFunctions,
                dataSource: data,
                allowPaging: true,
                allowSorting: true,
                allowExcelExport: true,
                allowFiltering: true,
                allowTextWrap: true,
                filterSettings: { type: 'Menu' },
                pageSettings: { pageSize: 10 },
                columns: [
                    { field: 'usuario', headerText: 'Usuario', width: 180, textAlign: 'Center' },
                    { field: 'accion', headerText: 'Accion', width: 180, textAlign: 'Center' },
                    { field: 'fechaAccion', headerText: 'Fecha Accion', width: 180, textAlign: 'Center' }
                ]
            });
            grid.toolbarClick = function (args) {
                grid.excelExport(getExcelExportProperties("LogsCentroControl.xlsx"));
               
            };
            grid.appendTo('#Grid'); // Renderiza la tabla dentro del div con id 'Grid'
        },
        error: function (xhr, status, error) {
            console.error('Error al obtener los logs:', error);
            Swal.fire('Error', 'Hubo un problema al obtener los logs.', 'error');
        }
    });
}

var detailsData = function (args) {
    //alert(JSON.stringify(args.rowData));
}