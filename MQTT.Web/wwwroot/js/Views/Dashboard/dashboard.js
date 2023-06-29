
var dashboard;
var listComponents;
var rangeData;
var gauge;
var lineGauge;
var fechaActual= new Date(Date.now());
var datepickerMonth
var treeGridSummary

var dataTest = [
    {
        id: 1,
        title:"Principal-data",
        description:"data primary",
        value:"80",
        childs: [
            {
                id: 2,
                idFather: 1,
                title:"Test-data-01",
                description:"child data primary",
                value:"40",
                childs: [
                    {
                        id: 21,
                        idFather: 2,
                        title:"Test-second-0101",
                        description:"child data secondary",
                        value:"90",
                    },
                    {
                        id: 22,
                        idFather: 2,
                        title:"Test-second-0102",
                        description:"child data secondary",
                        value:"5",
                    },
                    {
                        id: 23,
                        idFather: 2,
                        title:"Test-second-0103",
                        description:"child data secondary",
                        value:"12",
                    }
                ]
            },
            {
                id: 3,
                idFather: 1,
                title:"Test-data-02",
                description:"child data primary",
                value:"80",
                childs: [
                    {
                        id: 31,
                        idFather: 3,
                        title:"Test-second-0201",
                        description:"child data secondary",
                        value:"25",
                    },
                    {
                        id: 32,
                        idFather: 3,
                        title:"Test-second-0202",
                        description:"child data secondary",
                        value:"70",
                    },
                    {
                        id: 33,
                        idFather: 3,
                        title:"Test-second-0203",
                        description:"child data secondary",
                        value:"100",
                    }
                ]
            },
            {
                id: 4,
                idFather: 1,
                title:"Test-data-03",
                description:"child data primary",
                value:"10",
                childs: [
                    {
                        id: 41,
                        idFather: 4,
                        title:"Test-second-0301",
                        description:"child data secondary",
                        value:"16",
                    },
                    {
                        id: 42,
                        idFather: 4,
                        title:"Test-second-0302",
                        description:"child data secondary",
                        value:"89",
                    },
                    {
                        id: 43,
                        idFather: 4,
                        title:"Test-second-0303",
                        description:"child data secondary",
                        value:"41",
                    }
                ]
            },
        ]
    },
]


$(document).ready(function(){
    ExtractData(dataTest);
    treeGridDashBoard(dataTest)
    SelectorMonth();
});

function ExtractData(data){
    var numberData = {
        header: "header"+data[0].id,
        head: "head"+data[0].id,
        diagramBar: "diagramBar"+data[0].id,
        diagram: "diagram"+data[0].id
    }
    var rows = 2;
    panels = [
        {
            'sizeX': ej.base.Browser.isDevice ? 1 : 3, 'sizeY': ej.base.Browser.isDevice ? 1 : 2, 'row': 0, 'col': 0,
            header: '<div class="title" id='+ numberData.header + '>' + data[0].title +'</div>', content: '<div style="height:100%; width:100%" id=' + numberData.head + '></div>'
        },
        {
            'sizeX': ej.base.Browser.isDevice ? 1 : 5, 'sizeY': ej.base.Browser.isDevice ? 1 : 2, 'row': 0, 'col': 3,
            header: '<div class="title" id='+ numberData.diagramBar + '>'+ 'grafico detallado de ' + data[0].title +' </div>', content: '<div  style="height:100%; width:100%" id=' + numberData.diagram + '></div>'
        },
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

function setPanels(panels, data, row){
    var numberData = {
        header: "header"+data.id,
        head: "head"+data.id,
        diagramBar: "diagramBar"+data.id,
        diagram: "diagram"+data.id
    }
    arrayPanel = [
        {
            'sizeX': ej.base.Browser.isDevice ? 1 : 3, 'sizeY': ej.base.Browser.isDevice ? 1 : 2, 'row': row, 'col': 0,
            header: '<div class="title" id='+ numberData.header + '>' + data.title +'</div>', content: '<div style="height:100%; width:100%" id=' + numberData.head + '></div>'
        },
        {
            'sizeX': ej.base.Browser.isDevice ? 1 : 5, 'sizeY': ej.base.Browser.isDevice ? 1 : 2, 'row': row, 'col': 3,
            header: '<div class="title" id='+ numberData.diagramBar + '>'+ 'grafico detallado de ' + data.title +' </div>', content: '<div  style="height:100%; width:100%" id=' + numberData.diagram + '></div>'
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

function GaugeChartDiagram(data){
    var mainChart = new  ej.circulargauge.CircularGauge({
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
                content: '<div><span style="font-size:20px; font-family:Segoe UI">'+ data.value +'%</span></div>',
                radius: '40%', angle: 180, zIndex: 1
            }],
            startAngle: 210, endAngle: 150, minimum: 0, maximum: 100, radius: '80%',
            ranges: [{ start: 0, end: 35, color: '#30B32D' }, { start: 35, end: 70, color: '#FFDD00' },
            { start: 70, end: 100, color: '#F03E3E' }],
            pointers: [{
                value: data.value, radius: data.value + '%', pointerWidth: 8,
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

function setDiagramPrimary(data){
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
        legendSettings: { enableHighlight :true },
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


function SelectorMonth(){
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
    if (args.date.getMonth() > fechaActual.getMonth()){
        args.isDisabled = true;
    }
}

function treeGridDashBoard(data){
    var treeGridObj = new ej.treegrid.TreeGrid({
        dataSource: data,
        childMapping: 'childs',
        height: 350,
        treeColumnIndex: 1,
        allowPaging: true,
        columns: [
            { field: 'id', headerText: 'ID', width: 70, textAlign: 'Right' },
            { field: 'title', headerText: 'Nombre', width: 200, textAlign: 'Left' },
            { field: 'description', headerText: 'Descricion', width: 200, textAlign: 'Left'},
            { field: 'value', headerText: 'Valor', width: 90, textAlign: 'Left' },
        ]
    });
    treeGridObj.appendTo('#TreeGrid');
}
