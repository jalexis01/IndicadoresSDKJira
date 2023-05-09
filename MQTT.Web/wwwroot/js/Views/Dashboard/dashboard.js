var dataTemp = [
    {
        "id": "1",
        "name": "PRINCIPAL",
        "value": "100",
        "expanded": true,
        "subChild": [
            {
            "id": "2",
            "name": "Promedio 2.0",
            "value": "80",
            "expanded": true,
            "subChild": [
                    {
                        "id": "5",
                        "name": "secundario 1",
                        "value": "90",
                    },
                    {
                        "id": "6",
                        "name": "secundario 2",
                        "value": "75",
                    },
                    {
                        "id": "7",
                        "name": "secundario 3",
                        "value": "60",
                    }
                ]
            },
            {
                "id": "3",
                "name": "Promedio 2.1",
                "value": "50",
                "expanded": false,
                "subChild": [
                    {
                        "id": "8",
                        "name": "secundario 4",
                        "value": "45",
                    },
                    {
                        "id": "9",
                        "name": "secundario 5",
                        "value": "32",
                    },
                    {
                        "id": "10",
                        "name": "secundario 6",
                        "value": "28",
                    }
                ]
            },
            {
                "id": "4",
                "name": "Promedio 2.2",
                "value": "30",
                "expanded": false,
                "subChild": [
                    {
                        "id": "11",
                        "name": "secundario 7",
                        "value": "20",
                    },
                    {
                        "id": "12",
                        "name": "secundario 8",
                        "value": "16",
                    },
                    {
                        "id": "13",
                        "name": "secundario 9",
                        "value": "10",
                    }
                ]
            }
        ]
    }
]


var dashboard;
var listComponents;
var rangeData;
var gauge;
var lineGauge;
var fechaActual= new Date(Date.now());
var datepickerMonth
var treeGridSummary

$(document).ready(function(){
    SelectorMonth();

    ListTreeComponent();
    RangeDataPrimary();
    GaugeCreate();
    GaugeLine();
    ResumenGrid();
    document.getElementById("textGeneral").innerHTML = dataTemp[0]['name'];
    document.getElementById("valueGeneral").innerHTML = dataTemp[0]['value'];
    document.getElementById("textPrimary").innerHTML = dataTemp[0]['subChild'][0]['name'];
    document.getElementById("valuePrimary").innerHTML = dataTemp[0]['subChild'][0]['value'];
    document.getElementById("textSecondary").innerHTML = dataTemp[0]['subChild'][0]['subChild'][0]['name'];
    document.getElementById("valueSecondary").innerHTML = dataTemp[0]['subChild'][0]['subChild'][0]['value'];
  });

function ResumenGrid(){
    treeGridSummary = new ej.treegrid.TreeGrid({
        dataSource: dataTemp,
        childMapping: 'subChild',
        height: '100%',
        width: '100%',
        treeColumnIndex: 1,
        allowPaging: true,
        columns: [
            { field: 'id', headerText: 'ID',  textAlign: 'Left' },
            { field: 'name', headerText: 'Nombre',  textAlign: 'Left' },
            { field: 'value', headerText: 'Valor',  textAlign: 'Left'},
        ]
    });
    treeGridSummary.appendTo('#TreeGrid');
}

function ListTreeComponent(){
    listComponents = new ej.navigations.TreeView({
        fields: { dataSource: dataTemp, id: 'id', text: 'name', child: 'subChild' },
        nodeSelected: selectFunction
    });
    listComponents.appendTo('#tree');
}

function selectFunction(args){
    console.log(args);
    var dataSelect = args.nodeData;
    var titleGauge ;
    var titleLine;
    var valueGauge ;
    var valueLine;
    let primaryText = document.getElementById("textPrimary");
    let primaryValue = document.getElementById("valuePrimary");
    let secondaryText = document.getElementById("textSecondary");
    let secondaryValue = document.getElementById("valueSecondary");
    if(dataSelect.hasChildren){
        titleGauge = dataSelect.text; 
        titleLine = dataTemp[0]['subChild'].find(x => x.id == dataSelect.id)['subChild'][0]['name'];
        valueGauge = dataTemp[0]['subChild'].find(x => x.id == dataSelect.id)['value'];
        valueLine = dataTemp[0]['subChild'].find(x => x.id == dataSelect.id)['subChild'][0]['value'];
    }else{
        titleGauge =  dataTemp[0]['subChild'].find(x => x.id == dataSelect.parentID)['name'];
        titleLine = dataSelect.text; 
        valueGauge = dataTemp[0]['subChild'].find(x => x.id == dataSelect.parentID)['value'];
        valueLine = dataTemp[0]['subChild'].find(x => x.id == dataSelect.parentID)['subChild'].find(x=> x.id == dataSelect.id)['value'];
    }
    gauge.title = titleGauge;
    lineGauge.title = titleLine;
    gauge.axes[0].pointers[0].value = valueGauge;
    lineGauge.axes[0].pointers[0].value = valueLine;
    primaryText.innerHTML = titleGauge;
    primaryValue.innerHTML = valueGauge;
    secondaryText.innerHTML = titleLine;
    secondaryValue.innerHTML = valueLine;
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

function GaugeLine(){
    lineGauge = new ej.lineargauge.LinearGauge({
        rangePalettes: ['#30b32d', '#ffdd00', '#f03e3e'],
        title: dataTemp[0]['subChild'][0]['subChild'][0]['name'],
        titleStyle: {
            size: '18px',
            fontFamily: 'Segoe UI'
        },
        orientation: 'Horizontal',
        axes: [{
            maximum: 100,
            labelStyle: {
                font: {
                    size: '0px',
                    fontFamily: 'Segoe UI'
                }
            },
            line: {
                width: 0
            },
            pointers: [
                {
                    value: dataTemp[0]['subChild'][0]['subChild'][0]['value'],
                    height: 15,
                    width: 15,
                    placement: 'Near',
                    markerType: 'Triangle',
                    offset: -50
                }
            ],
            ranges: [{
                start: 0,
                end: 30,
                startWidth: 50,
                endWidth: 50
            },
            {
                start: 30,
                end: 70,
                startWidth: 50,
                endWidth: 50
            },
            {
                start: 70,
                end: 100,
                startWidth: 50,
                endWidth: 50
            }],
            majorTicks: {
                height: 0
            },
            minorTicks: {
                height: 0
            },
        }],
        annotations: [
            {
                content: '<div id="low"><img style="height:25px;width:25px;" src="../img/Low.png"/></div>',
                axisIndex: 0,
                axisValue: 15,
                y: -25,
                zIndex: '1'
            },
            {
                content: '<div id="moderate"><img style="height:25px;width:25px;" src="../img/Moderate.png"/></div>',
                axisIndex: 0,
                axisValue: 45,
                y: -25,
                zIndex: '1'
            },
            {
                content: '<div id="high"><img style="height:25px;width:25px;" src="../img/High.png"/></div>',
                axisIndex: 0,
                axisValue: 75,
                y: -25,
                zIndex: '1'
            },
            {
                content: '<div id="lowText"><p style="font-size:15px;color:#248622;font-family: Segoe UI;">Bajo</p></div>',
                axisIndex: 0,
                axisValue: 15,
                y: 20, zIndex: '1'
            },
            {
                content: '<div id="moderateText"><p style="font-size:15px;color:#ba9e2a;font-family: Segoe UI;">Medio</p></div>',
                axisIndex: 0,
                axisValue: 45,
                y: 20, zIndex: '1'
            },
            {
                content: '<div id="highText"><p style="font-size:15px;color:#b42f2f;font-family: Segoe UI">Alto</p></div>',
                axisIndex: 0,
                axisValue: 75,
                y: 20, zIndex: '1'
            },
        ],

    });
    lineGauge.appendTo('#annotationContainer');
}

function GaugeCreate(){
    gauge = new ej.lineargauge.LinearGauge({
        orientation: 'Horizontal',
        allowPdfExport : true,
        allowImageExport: true,
        allowPrint : true,
        title: dataTemp[0]['subChild'][0]['name'],
        titleStyle: {
            size: '18px',
            fontFamily: 'Segoe UI'
        },
        axes: [{
            minimum: 0,
            maximum: 100,
            line:
            {
            width:0
            },
            majorTicks: {
            height: 0,
            width: 0,
            interval: 20
            },
            minorTicks: {
            height: 7,
            width: 0,
            interval: 4
            },
            labelStyle: {
            position: "Outside",
            font: {
            fontFamily: 'Segoe UI'
            },
            offset: 4
            },
            ranges: [{
            start: 0,
            end: 20,
            startWidth: 15,
            endWidth: 25,
            color: '#82b944'
            },
            {
            start: 20,
            end: 40,
            startWidth: 25,
            endWidth: 35,
            color: '#a1cb43'
            },
            {
            start: 40,
            end: 60,
            startWidth: 35,
            endWidth: 45,
            color: '#ddec12'
            },
            {
            start: 60,
            end: 80,
            startWidth: 45,
            endWidth: 55,
            color: '#ffbc00'
            },
            {
            start: 80,
            end: 100,
            startWidth: 55,
            endWidth: 65,
            color: 'red'
            },
            ],
            pointers: [{
            value: dataTemp[0]['subChild'][0]['value'],
            height: 23,
            width: 35,
            offset: -52,
            markerType: 'Triangle',
            border:
            {
            width: 2,
            color:'white'
            }
            }],
            }],

    });
    gauge.appendTo('#gauge');
}

function RangeDataPrimary(){
    rangeData = new ej.circulargauge.CircularGauge({
        title: dataTemp[0]['name'],
        titleStyle: {
            size: '18px',
            fontFamily: 'Segoe UI'
        },
        axes: [{
            lineStyle: { width: 10, color: 'transparent' },
            labelStyle: {
                position: 'Inside', useRangeColor: false,
                font: { size: '12px', fontFamily: 'Segoe UI', fontStyle: 'Regular' }
            }, majorTicks: { height: 10, offset: 5 }, minorTicks: { height: 0 },
            annotations: [{
                content: '<div><span style="font-size:14px; font-family:Segoe UI">Speedometer</span></div>',
                radius: '30%', angle: 0, zIndex: 1
            }, {
                content: '<div><span style="font-size:20px; font-family:Segoe UI">'+dataTemp[0]['value']+'%</span></div>',
                radius: '40%', angle: 180, zIndex: 1
            }],
            startAngle: 210, endAngle: 150, minimum: 0, maximum: 100, radius: '80%',
            ranges: [{ start: 0, end: 30, color: '#30B32D' }, { start: 30, end: 70, color: '#FFDD00' },
            { start: 70, end: 100, color: '#F03E3E' }],
            pointers: [{
                value: dataTemp[0]['value'], radius: dataTemp[0]['value'] +'%', pointerWidth: 8,
                cap: { radius: 7 }, needleTail: { length: '18%' }
            }]
        }],

    });
    rangeData.appendTo('#range-container');
}