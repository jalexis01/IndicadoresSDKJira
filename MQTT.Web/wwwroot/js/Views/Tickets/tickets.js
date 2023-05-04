/*var dashboard;
var listComponents;
var rangeData;
var gauge;
var lineGauge;

var fechaActual = new Date(Date.now());
var datepickerMonth;
var treeGridSummary;


var dataTickets = [];

var dataTest = [
    {
        id: "TICKET-60",
        dateInitial: "2023-04-12T10:39:49.233",
        description: "data primary",
        component: "Componente RFID",
        classFailed: "AIO",
        diagnosisCauses: "Falla en puerta",
        status: "Abierto",
        dateSolution: "2023-04-13T12:51:23.421",      
    },
    {
        id: "TICKET-61",
        dateInitial: "2023-04-12T10:39:49.233",
        description: "data primary",
        component: "Componente RFID",
        classFailed: "AIO",
        diagnosisCauses: "Falla en puerta",
        status: "Cerrado",
        dateSolution: "2023-04-13T12:51:23.421",
    }
]  


$(document).ready(function () {
    treeGridTickets(dataTest);
    
});

function ExtractData(data) {
    var numberData = {
        header: "header" + data[0].id,
        head: "head" + data[0].id,
        diagramBar: "diagramBar" + data[0].id,
        diagram: "diagram" + data[0].id
    }
    var rows = 2;
    panels = [
        {
            'sizeX': ej.base.Browser.isDevice ? 1 : 3, 'sizeY': ej.base.Browser.isDevice ? 1 : 2, 'row': 0, 'col': 0,
            header: '<div class="title" id=' + numberData.header + '>' + data[0].title + '</div>', content: '<div style="height:100%; width:100%" id=' + numberData.head + '></div>'
        },
        {
            'sizeX': ej.base.Browser.isDevice ? 1 : 5, 'sizeY': ej.base.Browser.isDevice ? 1 : 2, 'row': 0, 'col': 3,
            header: '<div class="title" id=' + numberData.diagramBar + '>' + 'grafico detallado de ' + data[0].title + ' </div>', content: '<div  style="height:100%; width:100%" id=' + numberData.diagram + '></div>'
        },
    ]
}

function treeGridTickets(data) {
    var treeGridObj = new ej.treegrid.TreeGrid({
        dataSource: data,
        childMapping: 'childs',
        height: 350,
        treeColumnIndex: -1,
        allowPaging: true,
        columns: [
            { field: 'id', headerText: 'Id ticket', width: 100, textAlign: 'Left' },
            { field: 'dateInitial', headerText: 'Fecha creacion', width: 150, textAlign: 'Left' },
            { field: 'component', headerText: 'Id componente', width: 120, textAlign: 'Left' },
            { field: 'classFailed', headerText: 'Clase de fallo', width: 100, textAlign: 'Left' },
            { field: 'diagnosisCauses', headerText: 'Diagnostico causa', width: 120, textAlign: 'Left' },
            { field: 'status', headerText: 'Estado', width: 80, textAlign: 'Left' },
            { field: 'dateSolution', headerText: 'Fecha solucion', width: 150, textAlign: 'Left' },
        ]
    });
    treeGridObj.appendTo('#TreeGrid');
}

function showMore() {

    Swal.fire(
        'TICKET',
        'Información'
    )
}*/


var dataHideColums, dataSearchColumns;

$(document).ready(function () {
    createElemntsTimes();
    multiSelect();
    drodownDataSearch(columnsSearch, 'CustomName', 'searchParam');
});


function ServiceGetTickets() {
    var startDate = $('#dtpStart').val();
    var endDate = $('#dtpEnd').val();

    $.ajax({
        type: "GET",
        url: "/Tickets/GetTickets",
        data: { startDate: startDate, endDate: endDate },
        success: function (response) {
            // hacer algo con la respuesta del servidor
        },
        error: function (xhr, status, error) {
            // manejar el error
        }
    });
   
}