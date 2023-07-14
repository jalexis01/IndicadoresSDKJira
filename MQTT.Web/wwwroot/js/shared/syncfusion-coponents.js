var grid = null, multiSelectInput,columns, dataArgs, treeGridObj = null, dropdowns = null, idDrag, dropdownsModal = null, isDrag =false, datepicker=null, datepickerEnd=null, datepickerModal=null, 
datepickerEndModal=null, dateDocuments =  new Date(Date.now()).toUTCString(), valueFild,beginUpdate, beginCreate;
var listFunctionalitiesExportGrid = ['ExcelExport','PdfExport', 'CsvExport'];






$(document).ready(function(){
    var indicatortypes = [
        { id: 'Shimmer', type: 'Shimmer' },
        { id: 'Spinner', type: 'Spinner' },
    ];
  });


ej.base.enableRipple(true);

var L10n = ej.base.L10n;

L10n.load({
    'es': {
        'datepicker': {
            placeholder: 'Wählen Sie ein Datum aus',
            today: 'Hoy'
        }
    },
});

ej.base.L10n.load({
    'es-CO': {
        'grid': {
            'EmptyRecord': 'No hay registros',
            'SaveButton': 'Aplicar Cambios',
            'CancelEdit': 'Cancelar',
            'Add': 'Crear',
            'Edit': 'Modificar',
            'Update': 'Guardar',
            'Cancel': 'Cancelar',
            'Search': 'Buscar',
            'No records selected for edit operation': 'Seleccione un dato para modificarlo'
        },
        'pager': {
            'currentPageInfo': '{0} de {1} Paginas',
            'totalItemsInfo': '({0} Registros)',
            'firstPageTooltip': 'Primera Pagina',
            'lastPageTooltip': 'Ultima Pagina',
            'nextPageTooltip': 'Siguiente Pagina',
            'previousPageTooltip': 'Pagina Anterior',
            'nextPagerTooltip': 'Zum nächsten Pager',
            'previousPagerTooltip': 'Zum vorherigen Pager',
            'pagerDropDown': 'Items por Pagina',
            'All': 'Todo'
        },
        'formValidator': {
            "required": "Campo requerido.",
            'number': 'Ingrese un valor de numero.'
        }
    }
});

function ActionBeginSet(varUpdate, varCreate){
    beginUpdate = varUpdate;
    beginCreate = varCreate;
}

function setGrid(data, dataColumns, exportFunctions = null, nameGrid = "Grid"){
    if (grid != null) {
        grid.dataSource = data;
        return;
    }
    if(exportFunctions == null ){
        exportFunctions = ['Search']
    }else{
        exportFunctions.push('Search')
    }
    grid = new ej.grids.Grid({
        allowPaging: true,
        allowSorting: true,
        allowExcelExport: true,
        allowPdfExport: true,
        allowCsvExport: true,
        editSettings: { allowEditing: true, allowEditOnDblClick: false  },
        loadingIndicator: { indicatorType: 'Shimmer' },
        toolbar: exportFunctions,
        locale: 'es-CO',
        pageSettings: { pageCount: 5, pageSizes: true, currentPage: 1 },
        allowFiltering: true,
        allowTextWrap: true,
        filterSettings: { type: 'Menu' },
        enableHover: false,
        width: '100%',
        height: '100%',
        queryCellInfo: customiseCell,
        dataSource: data,
        columns: dataColumns,
    });

    grid.dataBound = () => {
        grid.autoFitColumns()
    };
    grid.appendTo("#"+nameGrid);
    grid.toolbarClick = function (args) {
        if (args.item.id === nameGrid+'_pdfexport') {
            grid.pdfExport(getPdfExportProperties());
        }
        if (args.item.id === nameGrid+'_excelexport') {
            grid.excelExport(getExcelExportProperties("exceldocument.xlsx"));
        }
        if (args.item.id === nameGrid+'_csvexport') {
            grid.csvExport();
        }
    };


    grid.commandClick = detailsData;
    
}

function createColumnsEditing(dataColumns, columnsHide = null, columnInEdit = null){
    var arr = [];
    for (var key in dataColumns[0]) {
        let keyFormat = key.replace(/([A-Z])/g, ' $1').trim();
        let title = keyFormat.replace(/  +/g, ' ');

        let typeKey =  dataColumns[0][key] == null ? null : typeof dataColumns[0][key];

        let dataVisible = true;

        let dataEditinBool = true;

        let validationRuleRequerid = true;

        if (columnsHide != null) {
            dataVisible = columnsHide.filter(x => x.name == key).length == 0 ? true : false; 
        }
        
        if (columnInEdit != null) {
            let filterData = columnInEdit.filter(x => x.name == key);
            dataEditinBool = filterData.length == 0 ? false : true; 
            validationRuleRequerid = filterData.length == 0 ? false : filterData[0].validation;
        }

        if (typeKey == 'object') {
            arr.push({
                field: key, headerText: title, textAlign: 'Center', headerTextAlign: 'Center', visible: false
            })
        }else if(typeKey == 'boolean'){
            arr.push({
                field: key, headerText: title, textAlign: 'Center', headerTextAlign: 'Center', visible: dataVisible, allowEditing: dataEditinBool, validationRules: {required: validationRuleRequerid}, editType: 'dropdownedit', 
                edit: {  
                    params: {  
                        query: new ej.data.Query(),   
                        dataSource: [{dataBoolCol: "Activo", value: true},{dataBoolCol: "Inactivo", value: false}], 
                        fields:{text:'dataBoolCol',value:'value'},
                    }
                }
            })
        }else{
            arr.push({
                field: key, headerText: title, textAlign: 'Center', headerTextAlign: 'Center', visible: dataVisible, allowEditing: dataEditinBool, validationRules: {required: validationRuleRequerid}
            })
        }
        
    }
    return arr;
}

function setColums(dataColumns, columnsHide = null){
    var arr = [];
    for (var key in dataColumns[0]) {
        let keyFormat = key.replace(/([A-Z])/g, ' $1').trim();
        let title = keyFormat.replace(/  +/g, ' ');
        let formattedKey = key.replace(/_(\w)/g, function (_, letter) {
            return letter.toUpperCase();
        });
        let typeKey =  dataColumns[0][key] == null ? null : typeof dataColumns[0][key];

        let dataVisible = true;

        if (columnsHide != null) {
            dataVisible = columnsHide.filter(x => x.name == key).length == 0 ? true : false; 
        }
        

        if (typeKey == 'object') {

            arr.push({
                field: key, headerText: formattedKey, textAlign: 'Center', headerTextAlign: 'Center', visible: false
            })
        } else {

            arr.push({
                field: key,
                headerText: formattedKey,
                textAlign: 'Center',
                headerTextAlign: 'Center',
                visible: dataVisible
            });
        }
    }
    return arr;
}

function customiseCell(args) {
    for (var key in args.data) {
        typeKey = typeof  args.data[key];
        if (typeKey == 'boolean') {
            if (args.column.field == key) {
                if(args.data[key]){
                    args.cell.classList.add('active');
                    args.cell.textContent = "Activo";
                }else{
                    args.cell.classList.add('inactive');
                    args.cell.textContent = "Inactivo";
                }
            }
        }
    }
    if (args.column.field === "idElementType") { 
        let value = typeElements.find(x => x.Id == args.data["idElementType"]); 
        args.cell.textContent = value.Name; 
    } 
}
/*
function createElemntsTimes() {
    if (datepicker != null) {
        return;
    }
    document.getElementById("dtpStart")
    datepicker = new ej.calendars.DatePicker({
        placeholder: 'Ingrese fecha de inicio',
        format: 'yyyy-MM-dd',
        close: selectDateStar,
        cleared: clean
    });
    datepicker.appendTo('#dtpStart');

    datepickerEnd = new ej.calendars.DatePicker({
        placeholder: 'Ingrese fecha fin',
        enabled: false,
        format: 'yyyy-MM-dd',
    });
    datepickerEnd.appendTo('#dtpEnd');
   
    datepickerMessage = new ej.calendars.DateTimePicker({
        placeholder: 'Ingrese fecha de inicio',
        format: 'yyyy-MM-dd HH:mm',
        close: selectDateStarMessage,
        cleared: clean
    });
    datepickerMessage.appendTo('#dtpStartMessage');

    datepickerEnd = new ej.calendars.DateTimePicker({
        placeholder: 'Ingrese fecha fin',
        enabled: false,
        format: 'yyyy-MM-dd HH:mm',
    });
    datepickerEnd.appendTo('#dtpEndMessage');
}
*/
function createElemntsTimes() {
    if (datepicker != null) {
        return;
    }
    if (document.getElementById("dtpStartMessage") || document.getElementById("dtpEndMessage")) {
        createElemntsTimesMessage();
    } else {
        datepicker = new ej.calendars.DatePicker({
            placeholder: 'Ingrese fecha de inicio',
            format: 'yyyy-MM-dd',
            close: selectDateStar,
            cleared: clean
        });
        datepicker.appendTo('#dtpStart');

        datepickerEnd = new ej.calendars.DatePicker({
            placeholder: 'Ingrese fecha fin',
            enabled: false,
            format: 'yyyy-MM-dd',
        });
        datepickerEnd.appendTo('#dtpEnd');

    }
}
function createElemntsTimesBackup() {
    if (datepickerModal != null) {
        return;
    }
    datepickerModal = new ej.calendars.DatePicker({
        placeholder: 'Ingrese fecha de inicio',
        format: 'yyyy-MM-dd',
        close: selectDateStarB,
        cleared: cleanB
    });
    datepickerModal.appendTo('#dtpStartModal');

    datepickerEndModal = new ej.calendars.DatePicker({
        placeholder: 'Ingrese fecha fin',
        enabled:false,
        format: 'yyyy-MM-dd',

    });
    datepickerEndModal.appendTo('#dtpEndModal');
    /*Messages*/
    datepickerModalMessage = new ej.calendars.DateTimePicker({
        placeholder: 'Ingrese fecha de inicio',
        format: 'yyyy-MM-dd',
        close: selectDateStarBMessage,
        cleared: cleanB
    });
    datepickerModal.appendTo('#dtpStartModal');

    datepickerEndModalMessage = new ej.calendars.DateTimePicker({
        placeholder: 'Ingrese fecha fin',
        enabled: false,
        format: 'yyyy-MM-dd',

    });
    datepickerEndModal.appendTo('#dtpEndModal');
}

function selectDateStar() {
    var stDateStart = $("#dtpStart").val();
    let arr = stDateStart.split('-');
    if(arr[1] != undefined){
        datepickerEnd.value = new Date(Date.now())
        let arrDay = arr[2].split(' ');
        datepickerEnd.enabled = true;
        datepickerEnd.min = new Date(parseInt(arr[0]), parseInt(arr[1]-1), parseInt(arrDay[0]))
        datepickerEnd.max = new Date(parseInt(arr[0]), parseInt(arr[1]+3), parseInt(arrDay[0]))
    }else{
        datepickerEnd.value = undefined;
        datepickerEnd.enabled = false;
        
    }
}

function selectDateStarB() {
    var stDateStart = $("#dtpStartModal").val();
    let arr = stDateStart.split('-');
    if(arr[1] != undefined){
        datepickerEndModal.value = new Date(Date.now())
        let arrDay = arr[2].split(' ');
        datepickerEndModal.enabled = true;
        datepickerEndModal.min = new Date(parseInt(arr[0]), parseInt(arr[1]-1), parseInt(arrDay[0]))
        datepickerEndModal.max = new Date(parseInt(arr[0]), parseInt(arr[1]+3), parseInt(arrDay[0]))
    }else{
        datepickerEndModal.value = undefined;
        datepickerEndModal.enabled = false;
        
    }
}

function clean(){
    datepickerEnd.value = undefined;
    datepickerEnd.enabled = false;
}

function cleanB(){
    datepickerEndModal.value = undefined;
    datepickerEndModal.enabled = false;
}

function drodownDataSearch(searchData, searchTitlesText,nameDiv){
    if(dropdowns != null){
        dropdowns.dataSource = searchData;
        return;
    }
     dropdowns = new ej.dropdowns.DropDownList({
        dataSource: searchData,
        fields: { text: searchTitlesText },
        placeholder: 'Seleccione un campo',
        enabled:false,
        popupHeight: '200px',
        change: valueChange
    });
    dropdowns.appendTo('#' +nameDiv);
}

function valueChange(args){
    multiSelectInput.value = null
    grid.dataSource =dataGridSave;
    if(filtersData.filter(x => x.nameField == args.itemData.Name)[0]['value'] == null){
        multiSelectInput.dataSource = [];
    }else{
        multiSelectInput.dataSource = filtersData.filter(x => x.nameField == args.itemData.Name);
    }
    multiSelectInput.enabled = true;
    valueFild = args.itemData.Name;
}



function drodownDataSearchBackup(searchData, searchTitlesText){
    if(dropdownsModal != null){
        dropdowns.dataSource = searchData.dataMessages;
        return;
    }
     dropdownsModal = new ej.dropdowns.DropDownList({
        width: '100%',
        dataSource: searchData.dataMessages,
        fields: { text: searchTitlesText},
        placeholder: 'Seleccione un campo',
        popupHeight: '200px',
    });
    dropdownsModal.appendTo('#searchParamModal');
}

function addFnctionsGrid(dataExport){
    if(typeof dataExport == 'string'){
        return listFunctionalitiesExportGrid.filter(x => x == dataExport + "Export");
    }else{
        arrDataExport = []
        for(var key in dataExport){
            arrDataExport.push(listFunctionalitiesExportGrid.filter(x => x == dataExport[key] + "Export")[0]);
        }
        return arrDataExport;
    }
}

function addCommandsGrid(columsList){
    columsList.unshift({
        headerText: 'opciones', width: 120, commands: [{ type: 'Edit', buttonOption: { iconCss: ' e-icons e-edit', cssClass: 'e-flat' , text:"ver mas"}}]
    })
    return columsList;
}

function addCommandsGridDetails(columsList){
    columsList.unshift({
        headerText: 'Ver', title:"Ver mas", width: 250, commands: [{ buttonOption: { title: "Ver más" , iconCss: 'e-icons e-eye', cssClass: 'e-flat' , text:"ver mas"} }]
    })
    return columsList;
}

function unabledCommandsGrid(){
    grid.commandClick = function(args) {
        location.href = location.origin + '/MessageTypes/Edit/' + args.rowData.Id;
    };
}

function getExcelExportProperties(fileName) {
    return {
        header: {
            headerRows: 0,
            rows: [
                { index: 1, cells: [{ index: 1, colSpan: 5, value: dataExcel.titulo, style: { fontColor: dataExcel.fontColor2, fontSize: 25, hAlign: 'Center', bold: true } }] },
                {
                    index: 3,
                    cells: [
                        { index: 1, colSpan: 2, value: "Contacto", style: { fontColor: dataExcel.fontColor, fontSize: 15, bold: true } },
                        { index: 5, value: "DATE", style: { fontColor: dataExcel.fontColor, bold: true }, width: 150 }
                    ]
                },
                {
                    index: 4,
                    cells: [{ index: 1, colSpan: 2, value: dataExcel.location },
                    { index: 5, value: dateDocuments, width: 150 }

                    ]
                },

                {
                    index: 5,
                    cells: [
                        { index: 1, colSpan: 2, value: dataExcel.info },
                        { index: 5, value: "TERMS", width: 150, style: { fontColor: dataExcel.fontColor, bold: true } }

                    ]
                },
                {
                    index: 6,
                    cells: [
                        
                    ]
                }
            ]
        },

        footer: {
            footerRows: 8,
            rows: [


            ]
        },
        
        fileName: fileName
    };
}

function getPdfExportProperties() {
    return {
        header: {
            fromTop: 0,
            height: 120,
            contents: [
                {
                    type: 'Text',
                    value: dataExcel.titulo,
                    position: { x: 280, y: 0 },
                    style: { textBrushColor: dataExcel.fontColor2, fontSize: 25 },
                },
                {
                    type: 'Text',
                    value: 'Date',
                    position: { x: 600, y: 30 },
                    style: { textBrushColor: dataExcel.fontColor, fontSize: 10 },
                },
                {
                    type: 'Text',
                    value: dateDocuments,
                    position: { x: 600, y: 50 },
                    style: { textBrushColor: '#000000', fontSize: 10 },
                },
                {
                    type: 'Text',
                    value: 'TERMS',
                    position: { x: 600, y: 70 },
                    style: { textBrushColor: dataExcel.fontColor, fontSize: 10 },
                },
                {
                    type: 'Text',
                    value: 'Net 30 days',
                    position: { x: 600, y: 90 },
                    style: { textBrushColor: '#000000', fontSize: 10 },
                },
                {
                    type: 'Text',
                    value: 'Contacto',
                    position: { x: 20, y: 30 },
                    style: { textBrushColor: dataExcel.fontColor, fontSize: 20 }
                },
                {
                    type: 'Text',
                    value: dataExcel.location,
                    position: { x: 20, y: 65 },
                    style: { textBrushColor: '#000000', fontSize: 11 }
                },
                {
                    type: 'Text',
                    value: dataExcel.info,
                    position: { x: 20, y: 80 },
                    style: { textBrushColor: '#000000', fontSize: 11 }
                },
            ]
        },
        footer: {
            fromBottom: 160,
            height: 100,
            contents: [
                {
                    type: 'Text',
                    value: 'Thank you for your business !',
                    position: { x: 250, y: 20 },
                    style: { textBrushColor: dataExcel.fontColor, fontSize: 14 }
                },
                {
                    type: 'Text',
                    value: '! Visit Again !',
                    position: { x: 300, y: 45 },
                    style: { textBrushColor: dataExcel.fontColor, fontSize: 14 }
                }
            ]
        },
        
        fileName: "pdfdocument.pdf"
    };
}

function setTreeGrid(data, dataColumns, exportFunctions = null, add = false, nameKeyChildGrid, nameGrid = "Grid") {

    if (treeGridObj != null) {
        treeGridObj.dataSource = data;
        return;
    }
    if(exportFunctions == null ){
        exportFunctions = ['Search']
    }else{
        exportFunctions.push('Search')
    }

    if(add == true){
        var dataEdit =  ['Add', 'Edit', 'Update', 'Cancel'];
        exportFunctions = exportFunctions.concat(dataEdit)

    }
    
    treeGridObj = new ej.treegrid.TreeGrid({
        editSettings: {
            allowAdding: true,
            allowEditing: true,
            mode: 'Row'
        },
        allowRowDragAndDrop: true,
        childMapping: nameKeyChildGrid,
        treeColumnIndex: 1,
        allowPaging: true,
        toolbar: exportFunctions,
        allowSorting: true,
        allowFiltering: true,
        filterSettings: { type: 'Menu' },
        pageSettings: { pageCount: 5, pageSizes: true, currentPage: 1 },
        width: '100%',
        height: '100%',
        locale: 'es-CO',
        queryCellInfo: customiseCell,
        rowDragStart: rowDragStart,
        rowDataBound: function (args) {
            if (args.data.childRecords.length == 0) {
                args.row.style.backgroundColor = "#FFFFFF";
            }else if (args.data.childRecords.length > 0 && args.data.idElementFather != null) {
                args.row.style.backgroundColor = "#A7E1FD";
            } else{
                args.row.style.backgroundColor = "#ACCBDA";
            }
        },
        actionBegin: ActionBegin,
        actionComplete: ActionComplete,
        columns: dataColumns,
        dataSource: data
    });
    treeGridObj.appendTo('#'+ nameGrid);
    treeGridObj.dataBound = () => {
        treeGridObj.autoFitColumns()
    };
};

function rowDragStart(args) {
    idDrag = args.data[0].id;
    isDrag = true;
}

function ActionBegin(args){
    if (args.requestType == 'save') {
        if (args.action == "edit" ) {
            beginUpdate(args.data, args.previousData);
        }
        if(args.action == "add" ) {
            beginCreate(args.data);
        }
    }
}



function ActionComplete(args){
    if(args.requestType == 'refresh'){
        if(isDrag){
            var dataRow= args.rows.find(x => x.data.id == idDrag)
            var dataRowAdd = dataRow;
            if(dataRow.data.parentItem){
                dataRowAdd.data.idElementFather = dataRow.data.parentItem.id;
            }else{
                dataRowAdd.data.idElementFather = null
            }
            beginUpdate(dataRowAdd.data, dataRowAdd.data);
            isDrag = false
        }
    }
}

function multiSelect(){
    multiSelectInput = new ej.dropdowns.MultiSelect({
        placeholder: 'Seleccionar',
        query: new ej.data.Query().take(10),
        enabled:false,
        fields: { text: 'value', value: 'value' },
        change: valueChangeMulti,
    });
    multiSelectInput.appendTo('#multiSelect');
}

function valueChangeMulti(args){
    dataArgs = args.value;
}

function aplicFilter(){
    var dataSet = [];
    if(dataArgs.length == 0){
        grid.dataSource = dataGridSave;
        return;
    }
    dataArgs.forEach(element => {
        var dataFil = dataGridSave.filter(datares => datares[valueFild] == element);
        dataSet = dataSet.concat(dataFil);
    })
    grid.dataSource = dataSet;
}

// dataHtmlList += "<li style='padding: 1% 0%;'><div class='flex items-center space-x-4'><div class='flex-1 min-w-0' style='text-align: initial;'><p class='text-sm font-small text-gray-900 truncate dark:text-white' style=''>" + key + "</p></div><div class='inline-flex items-center text-sm font-small text-gray-900 truncate dark:text-white' style='''>" + args.rowData[key] + "</div></div></li>"

var detailsData = function (args) {
    var dataHtmlList = "";
    for (var key in args.rowData) {
        let formattedKey = key.replace(/_(\w)/g, function (_, letter) {
            return letter.toUpperCase();
        });
        formattedKey = formattedKey.charAt(0).toLowerCase() + formattedKey.slice(1);
        let value = args.rowData[key];
        dataHtmlList += "<ul><li style='padding: 1% 0%;'><div class='flex items-start space-x-4'><div class='flex-1 min-w-0' style='text-align: initial;'><p class='text-sm font-medium text-gray-900 truncate dark:text-white'>" + formattedKey + "</p></div></li><li><div class='flex items-start space-x-4'><div class='flex-1 min-w-0' style='text-align: initial'><p class='text-sm font-sm text-gray-900 truncate dark:text-white'>" + value + "</p></div></li></ul>"
    }
    console.log("Entro al swal")
    Swal.fire({
        title: '<strong><u>Información</u></strong>',
        html: '<div style="max-height: 100vh; overflow-y: auto; overflow-x: scroll;"><div style="width: 60vw;"><ul class="max-w-full divide-y divide-gray-200 dark:divide-gray-700">' + dataHtmlList + '</ul></div></div>',
        showConfirmButton: false,
        showCloseButton: true,
        customClass: {
            container: 'swal2-container',
            content: 'max-h-full',
            popup: 'swal2-popup',
        },
        width: 'auto',
        didOpen: function () {
            Swal.getContent().style.setProperty('flex-direction', 'column');
            Swal.getHtmlContainer().style.setProperty('max-width', 'none');
        },
    });
}
/*
 var detailsData = function(args) {
    var dataHtmlList = "";
    for (var key in args.rowData) {
        let formattedKey = key.replace(/_(\w)/g, function(_, letter) {
            return letter.toUpperCase();
        });
        formattedKey = formattedKey.charAt(0).toLowerCase() + formattedKey.slice(1);
        let value = args.rowData[key];
        dataHtmlList += "<li style='padding: 1% 0%;'><div class='flex items-start space-x-4'><div class='flex-1 min-w-0' style='text-align: initial;'><p class='text-sm font-medium text-gray-900 truncate dark:text-white'>" + formattedKey + "</p></div><div class='inline-flex items-start text-base font-normal text-gray-900 dark:text-white'>" + value + "</div></div></li>"
    }

    Swal.fire({
        title: '<strong><u>Informacion</u></strong>',
        html: '<div style="max-height: 100vh; overflow-y: auto; overflow-x: scroll;"><div style="width: 50vw;"><ul class="max-w-full divide-y divide-gray-200 dark:divide-gray-700">' + dataHtmlList + '</ul></div></div>',
        showConfirmButton: false,
        showCloseButton: true,
        customClass: {
            container: 'swal2-container',
            content: 'max-h-full',
            popup: 'swal2-popup',
        },
        width: '50%',
    });
}

 */


/* idTicket eg
var detailsData = function (args) {
    var dataHtmlList = "";
    for (var key in args.rowData) {
        let formattedKey = key.replace(/_(\w)/g, function (_, letter) {
            return letter.toUpperCase();
        });
        formattedKey = formattedKey.charAt(0).toLowerCase() + formattedKey.slice(1);
        dataHtmlList += "<li style='padding: 1% 0%;'><div class='flex items-center space-x-4'><div class='flex-1 min-w-0' style='text-align: initial;'><p class='text-sm font-small text-gray-900 truncate dark:text-white' style=''>" + formattedKey + "</p></div><div class='inline-flex items-center text-sm font-small text-gray-900 truncate dark:text-white' style='''>" + args.rowData[key] + "</div></div></li>"
    }

    Swal.fire({
        title: '<strong><u>Informacion</u></strong>',
        html: '<ul class="max-w-md divide-y divide-gray-200 dark:divide-gray-700">' + dataHtmlList + '</ul>',
        showConfirmButton: false,
        showCloseButton: true,
    });
}
*/
/*
var detailsData = function(args){
    var dataHtmlList = "";
    for (var key in args.rowData) {
        let formattedKey = key.replace(/_(\w)/g, function (_, letter) {
            return letter.toUpperCase();
        });
       
    }
    //alert(JSON.stringify(args.rowData));
    Swal.fire({
        title: '<strong><u>Informacion</u></strong>',
        html: '<ul class="max-w-md divide-y divide-gray-200 dark:divide-gray-700">' + dataHtmlList + '</ul>',
        showConfirmButton: false,
        showCloseButton: true,
      })
  }*/








function createElemntsTimesMessage() {
    if (datepicker != null) {
        return;
    }
    datepicker = new ej.calendars.DateTimePicker({
        placeholder: 'Ingrese fecha de inicio',
        format: 'yyyy-MM-dd HH:mm',
        close: selectDateStarMessage,
        cleared: clean
    });
    datepicker.appendTo('#dtpStartMessage');

    datepickerEnd = new ej.calendars.DateTimePicker({
        placeholder: 'Ingrese fecha fin',
        enabled: false,
        format: 'yyyy-MM-dd HH:mm',
    });
    datepickerEnd.appendTo('#dtpEndMessage');
}

function createElemntsTimesBackupMessage() {
    if (datepickerModal != null) {
        return;
    }
    datepickerModal = new ej.calendars.DatePicker({
        placeholder: 'Ingrese fecha de inicio',
        format: 'yyyy-MM-dd',
        close: selectDateStarBMessage,
        cleared: cleanB
    });
    datepickerModal.appendTo('#dtpStartModal');

    datepickerEndModal = new ej.calendars.DatePicker({
        placeholder: 'Ingrese fecha fin',
        enabled: false,
        format: 'yyyy-MM-dd',

    });
    datepickerEndModal.appendTo('#dtpEndModalMessage');
}

function selectDateStarMessage() {
    console.log("Start Message")
    var stDateStart = $("#dtpStartMessage").val();
    let arr = stDateStart.split('-');
    if (arr[1] != undefined) {
        datepickerEnd.value = new Date(Date.now())
        let arrDay = arr[2].split(' ');
        datepickerEnd.enabled = true;
        datepickerEnd.min = new Date(parseInt(arr[0]), parseInt(arr[1] - 1), parseInt(arrDay[0]))
        datepickerEnd.max = new Date(parseInt(arr[0]), parseInt(arr[1] + 3), parseInt(arrDay[0]))
    } else {
        datepickerEnd.value = undefined;
        datepickerEnd.enabled = false;

    }
}

function selectDateStarBMessage() {
    var stDateStart = $("#dtpStartModalMessage").val();
    let arr = stDateStart.split('-');
    if (arr[1] != undefined) {
        datepickerEndModal.value = new Date(Date.now())
        let arrDay = arr[2].split(' ');
        datepickerEndModal.enabled = true;
        datepickerEndModal.min = new Date(parseInt(arr[0]), parseInt(arr[1] - 1), parseInt(arrDay[0]))
        datepickerEndModal.max = new Date(parseInt(arr[0]), parseInt(arr[1] + 3), parseInt(arrDay[0]))
    } else {
        datepickerEndModal.value = undefined;
        datepickerEndModal.enabled = false;

    }
}