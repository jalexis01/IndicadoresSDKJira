var dashboard;
var listComponents;
var rangeData;
var gauge;
var lineGauge;
var fechaActual = new Date(Date.now());
var datepickerMonth
var treeGridSummary
var indicadoresJson;
var transformedData


$(document).ready(function () {
    //ExtractData(dataTest);
    //treeGridDashBoard(dataTest)
    SelectorMonth();
});



function ServiceGetIndicadoresDashboard() {
    var startDate = '2023-05-01';
    var endDate = '2023-06-01';
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
        success: function (response) {
            Swal.close();
            var indicadores = [];
            $.each(response, function (index, indicador) {
                console.log('Nombre indicador: ' + indicador.nombre)
                console.log('Valor: ' + indicador.calculo)
                indicadorJson = {
                    nombre: indicador.nombre,
                    calculo: indicador.calculo,
                };
                indicadores.push(indicadorJson);
                transformedData = transformData(indicadores);
            });
            indicadoresJson = JSON.stringify(indicadores);

            ExtractData(transformedData)
            treeGridDashBoard(transformedData)
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


function transformData(data) {

    var generalChilds = [];
    var IAIOChilds = [];
    var IANOChilds = [];
    var RAIOChilds = [];
    var RANOChilds = [];
    var RANOChilds = [];
    var RANOChilds = [];

    for (var i = 0; i < data.length; i++) {
        var indicador = data[i];

        if (indicador.nombre.includes("IAIO GENERAL")) {
            generalChilds.push({
                id: i + 2,
                idFather: 1,
                title: indicador.nombre,
                description: "child data primary",
                value: indicador.calculo,
                childs: []
            });
        } else if (indicador.nombre.includes("IANO GENERAL")) {
            generalChilds.push({
                id: i + 2,
                idFather: 1,
                title: indicador.nombre,
                description: "child data primary",
                value: indicador.calculo,
                childs: []
            });
        } else if (indicador.nombre.includes("RAIO GENERAL")) {
            generalChilds.push({
                id: i + 2,
                idFather: 1,
                title: indicador.nombre,
                description: "child data primary",
                value: indicador.calculo,
                childs: []
            });
        } else if (indicador.nombre.includes("RAIO GENERAL")) {
            generalChilds.push({
                id: i + 2,
                idFather: 1,
                title: indicador.nombre,
                description: "child data primary",
                value: indicador.calculo,
                childs: []
            });
        } else if (indicador.nombre.includes("IEPM GENERAL")) {
            generalChilds.push({
                id: i + 2,
                idFather: 1,
                title: indicador.nombre,
                description: "child data primary",
                value: indicador.calculo,
                childs: []
            });
        }else if (indicador.nombre.includes("IRF GENERAL")) {
            generalChilds.push({
                id: i + 2,
                idFather: 1,
                title: indicador.nombre,
                description: "child data primary",
                value: indicador.calculo,
                childs: []
            });
        }
        else if (indicador.nombre.includes("ICPM")) {
            generalChilds.push({
                id: i + 2,
                idFather: 1,
                title: indicador.nombre,
                description: "child data primary",
                value: indicador.calculo,
                childs: []
            });
        }
       
    }
    var result = [
        {
            id: 1,
            title: "Indicadores",
            description: "Aquí están todos los indicadores",
            childs: generalChilds
        }
    ];

   

    return result;
}






var dataTest = [
    {
        id: 1,
        title: "Indicadores",
        description: "Aqui estan todos los indicadores",
        childs: [
            {
                id: 2,
                idFather: 1,
                title: "Indicador",
                description: "child data primary",
                value: "40",
                childs: [
                    {
                        id: 21,
                        idFather: 2,
                        title: "Test-second-0101",
                        description: "child data secondary",
                        value: "90",
                    },
                    {
                        id: 22,
                        idFather: 2,
                        title: "Test-second-0102",
                        description: "child data secondary",
                        value: "5",
                    },
                    {
                        id: 23,
                        idFather: 2,
                        title: "Test-second-0103",
                        description: "child data secondary",
                        value: "12",
                    }
                ]
            },
            {
                id: 3,
                idFather: 1,
                title: "Indicador B",
                description: "child data primary",
                value: "80",
                childs: [
                    {
                        id: 31,
                        idFather: 3,
                        title: "Test-second-0201",
                        description: "child data secondary",
                        value: "25",
                    },
                    {
                        id: 32,
                        idFather: 3,
                        title: "Test-second-0202",
                        description: "child data secondary",
                        value: "70",
                    },
                    {
                        id: 33,
                        idFather: 3,
                        title: "Test-second-0203",
                        description: "child data secondary",
                        value: "100",
                    }
                ]
            },
            {
                id: 6,
                idFather: 1,
                title: "Indicador C",
                description: "child data primary",
                value: "25",
                childs: [
                    {
                        id: 41,
                        idFather: 4,
                        title: "Test-second-0301",
                        description: "child data secondary",
                        value: "16",
                    },
                    {
                        id: 42,
                        idFather: 4,
                        title: "Test-second-0302",
                        description: "child data secondary",
                        value: "89",
                    },
                    {
                        id: 43,
                        idFather: 4,
                        title: "Test-second-0303",
                        description: "child data secondary",
                        value: "41",
                    }
                ]
            },
        ]
    },
]



function ExtractData(data) {
    var numberData = {
        header: "header" + data[0].id,
        head: "head" + data[0].id,
        diagramBar: "diagramBar" + data[0].id,
        diagram: "diagram" + data[0].id
    }
    var rows = 2;
    panels = [
       
    ]

    data[0]['childs'].forEach(x => {
        panels = setPanels(panels, x, rows)
        rows += 2;
    });
    setDashBoard(panels);
    GaugeChartDiagram(data[0]);
    setDiagramPrimary(data[0]);
    data[0]['childs'].forEach(x => {
        GaugeChartDiagram(x);
        setDiagramPrimary(x);
    })
}

function setPanels(panels, data, row) {
    var numberData = {
        header: "header" + data.id,
        head: "head" + data.id,
        diagramBar: "diagramBar" + data.id,
        diagram: "diagram" + data.id
    }
    arrayPanel = [
        {
            'sizeX': ej.base.Browser.isDevice ? 1 : 3, 'sizeY': ej.base.Browser.isDevice ? 1 : 2, 'row': row, 'col': 0,
            header: '<div class="title" id=' + numberData.header + '>' + data.title + '</div>', content: '<div style="height:100%; width:100%" id=' + numberData.head + '></div>'
        },
        {
            'sizeX': ej.base.Browser.isDevice ? 1 : 5, 'sizeY': ej.base.Browser.isDevice ? 1 : 2, 'row': row, 'col': 3,
            header: '<div class="title" id=' + numberData.diagramBar + '>' + 'Grafico detallado de ' + data.title + ' </div>', content: '<div  style="height:100%; width:100%" id=' + numberData.diagram + '></div>'
        },
    ];

    return panels.concat(arrayPanel);
}


function setDashBoard(data) {
    var dashboard = new ej.layouts.DashboardLayout({
        cellSpacing: [15, 15],
        cellAspectRatio: ej.base.Browser.isDevice ? 1 : 0.8,
        columns: ej.base.Browser.isDevice ? 2 : 8,

        panels: data

    });
    dashboard.appendTo('#defaultLayout');
};

function GaugeChartDiagram(data) {
    var mainChart = new ej.circulargauge.CircularGauge({
        axes: [{
            lineStyle: { width: 10, color: 'transparent' },
            labelStyle: {
                position: 'Inside', useRangeColor: false,
                font: { size: '12px', fontFamily: 'Segoe UI', fontStyle: 'Regular' }
            }, majorTicks: { height: 10, offset: 5 }, minorTicks: { height: 0 },
            annotations: [{
                content: '<div><span style="font-size:14px; font-family:Segoe UI">Porcentaje</span></div>',
                radius: '30%', angle: 0, zIndex: 1
            }, {
                content: '<div><span style="font-size:20px; font-family:Segoe UI">' + data.value + '%</span></div>',
                radius: '40%', angle: 180, zIndex: 1
            }],
            startAngle: 210, endAngle: 150, minimum: 0, maximum: 100, radius: '80%',
            ranges: [{ start: 0, end: 59, color: '#d11010' }, { start: 60, end: 69, color: '#FFDD00' },
                { start: 70, end: 79, color: '#ea7e12' }, { start: 80, end: 89, color: '#00b300' }, { start: 90, end: 100, color: '#00ff00' }],
            pointers: [{
                value: data.value, radius: 100 + '%', pointerWidth: 8,
                cap: { radius: 7 }, needleTail: { length: '18%' }
            }]
        }],
        tooltip: {
            type: ['Pointer', 'Range'],
            enable: true,
            enableAnimation: false,
            textStyle: {
                fontFamily: 'Segoe UI'
            }
        },

    });
    mainChart.appendTo("#head" + data.id);
}

function setDiagramPrimary(data) {
    var subChart1 = new ej.charts.Chart({
        primaryXAxis: {
            valueType: 'Category',
            majorGridLines: { width: 0 }
        },
        primaryYAxis: {
            labelFormat: '{value}%',
            edgeLabelPlacement: 'Shift',
            majorTickLines: { width: 0 },
            lineStyle: { width: 0 },
        },
        chartArea: {
            border: {
                width: 0
            }
        },
        tooltip: {
            enable: true
        },
        legendSettings: { enableHighlight: true },
        series: [
            {
                type: 'Column',
                dataSource: data.childs,
                xName: "title",
                yName: "value",
                height: "150px",
                width: "200px"
            }
        ]
    });
    subChart1.appendTo("#diagram" + data.id);
}


function SelectorMonth() {
    datepickerMonth = new ej.calendars.DatePicker({
        start: 'Year',
        depth: 'Year',
        format: 'MMMM y',
        renderDayCell: disableDate,
        value: fechaActual
    });
    datepickerMonth.appendTo('#datepicker');
}

function disableDate(args) {
    if (args.date.getMonth() > fechaActual.getMonth()) {
        args.isDisabled = true;
    }
}

function treeGridDashBoard(data) {
    var treeGridObj = new ej.treegrid.TreeGrid({
        dataSource: data,
        childMapping: 'childs',
        height: 350,
        treeColumnIndex: 1,
        allowPaging: true,
        columns: [
            { field: 'id', headerText: 'ID', width: 70, textAlign: 'Right' },
            { field: 'title', headerText: 'Nombre', width: 200, textAlign: 'Left' },
            { field: 'value', headerText: 'Valor', width: 90, textAlign: 'Left' },
        ]
    });
    treeGridObj.appendTo('#TreeGrid');
}