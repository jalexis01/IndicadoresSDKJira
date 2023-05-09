var grid = null, columns, treeGridObj = null, dropdowns = null, idDrag, dropdownsModal = null, isDrag =false, datepicker=null, datepickerEnd=null, datepickerModal=null, 
datepickerEndModal=null, dateDocuments =  new Date(Date.now()).toUTCString(), beginUpdate, beginCreate;
var listFunctionalitiesExportGrid = ['ExcelExport','PdfExport', 'CsvExport'];





$(document).ready(function(){
    var indicatortypes = [
        { id: 'Shimmer', type: 'Shimmer' },
        { id: 'Spinner', type: 'Spinner' },
    ];
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

function setGrid(data, dataColumns, exportFunctions = null, commandClickF = null,nameGrid = "Grid"){
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
        editSettings: { allowEditing: true },
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
        locale: 'es-CO',
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

    if(commandClickF != null){
        grid.commandClick = commandClickF;
    }
}

function commandEditingColumn(columsList){
    columsList.unshift(
        {
            headerText: 'Editar elementos',
            commands: [{ type: 'Edit', buttonOption: { iconCss: ' e-icons e-edit', cssClass: 'e-flat' } },
                { type: 'Save', buttonOption: { iconCss: 'e-icons e-update', cssClass: 'e-flat' } },
                { type: 'Cancel', buttonOption: { iconCss: 'e-icons e-cancel-icon', cssClass: 'e-flat' } }]
        }
    )
    return columsList;
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

        let typeKey =  dataColumns[0][key] == null ? null : typeof dataColumns[0][key];

        let dataVisible = true;

        if (columnsHide != null) {
            dataVisible = columnsHide.filter(x => x.name == key).length == 0 ? true : false; 
        }
        

        if (typeKey == 'object') {
            arr.push({
                field: key, headerText: title, textAlign: 'Center', headerTextAlign: 'Center', visible: false
            })
        }else{
            arr.push({
                field: key, headerText: title, textAlign: 'Center', headerTextAlign: 'Center', visible: dataVisible
            })
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
                }else{
                    args.cell.classList.add('inactive');
                }
            }
        }
    }
}

function createElemntsTimes() {
    if (datepicker != null) {
        return;
    }
    datepicker = new ej.calendars.DateTimePicker({
        placeholder: 'Ingrese fecha de inicio',
        format: 'yyyy-MM-dd HH:mm',
        close: selectDateStar,
        cleared: clean
    });
    datepicker.appendTo('#dtpStart');

    datepickerEnd = new ej.calendars.DateTimePicker({
        placeholder: 'Ingrese fecha fin',
        enabled:false,
        format: 'yyyy-MM-dd HH:mm',
        close: simulatedClick,
    });
    datepickerEnd.appendTo('#dtpEnd');
}

function createElemntsTimesBackup() {
    if (datepickerModal != null) {
        return;
    }
    datepickerModal = new ej.calendars.DateTimePicker({
        placeholder: 'Ingrese fecha de inicio',
        format: 'yyyy-MM-dd HH:mm',
        close: selectDateStarB,
        cleared: cleanB
    });
    datepickerModal.appendTo('#dtpStartModal');

    datepickerEndModal = new ej.calendars.DateTimePicker({
        placeholder: 'Ingrese fecha fin',
        enabled:false,
        format: 'yyyy-MM-dd HH:mm',
        close: simulatedClick,
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
    simulatedClick();
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
        width: '100%',
        dataSource: searchData,
        fields: { text: searchTitlesText },
        placeholder: 'Seleccione un campo',
        popupHeight: '200px',
        change: simulatedClick
    });
    dropdowns.appendTo('#' +nameDiv);
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
        headerText: 'opciones', width: 120, commands: [{ type: 'Edit', buttonOption: { iconCss: ' e-icons e-edit', cssClass: 'e-flat' }}]
    })
    return columsList;
}

function addCommandsGridDetails(columsList){
    columsList.unshift({
        headerText: 'opciones', width: 120, commands: [{ buttonOption: { iconCss: 'e-icons e-eye', cssClass: 'e-flat' } }]
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
            headerRows: 7,
            rows: [
                { index: 1, cells: [{ index: 1, colSpan: 5, value: 'EYS', style: { fontColor: '#0066D1', fontSize: 25, hAlign: 'Center', bold: true } }] },
                {
                    index: 3,
                    cells: [
                        { index: 1, colSpan: 2, value: "Contacto", style: { fontColor: '#00A0EF', fontSize: 15, bold: true } },
                        { index: 5, value: "DATE", style: { fontColor: '#00A0EF', bold: true }, width: 150 }
                    ]
                },
                {
                    index: 4,
                    cells: [{ index: 1, colSpan: 2, value: "Av. 7Nte. # 26N - 35, Sta. Mónica Residencial. Cali, Colombia" },
                    { index: 5, value: dateDocuments, width: 150 }

                    ]
                },

                {
                    index: 5,
                    cells: [
                        { index: 1, colSpan: 2, value: "Tel +57 (602) 435 0777 Email servicioalcliente@eysingenieria.com" },
                        { index: 5, value: "TERMS", width: 150, style: { fontColor: '#00A0EF', bold: true } }

                    ]
                },
                {
                    index: 6,
                    cells: [
                        { index: 5, value: "Net 30 days", width: 150 }
                    ]
                }
            ]
        },

        footer: {
            footerRows: 8,
            rows: [
                { cells: [{ colSpan: 6, value: "Thank you for your business!", style: { fontColor: '#00A0EF', hAlign: 'Center', bold: true } }] },
                { cells: [{ colSpan: 6, value: "!Visit Again!", style: { fontColor: '#00A0EF', hAlign: 'Center', bold: true } }] }
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
                    value: 'EYS',
                    position: { x: 280, y: 0 },
                    style: { textBrushColor: '#0066D1', fontSize: 25 },
                },
                {
                    type: 'Text',
                    value: 'Date',
                    position: { x: 600, y: 30 },
                    style: { textBrushColor: '#00A0EF', fontSize: 10 },
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
                    style: { textBrushColor: '#00A0EF', fontSize: 10 },
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
                    style: { textBrushColor: '#00A0EF', fontSize: 20 }
                },
                {
                    type: 'Text',
                    value: 'Av. 7Nte. # 26N - 35, Sta. Mónica Residencial. Cali, Colombia',
                    position: { x: 20, y: 65 },
                    style: { textBrushColor: '#000000', fontSize: 11 }
                },
                {
                    type: 'Text',
                    value: 'Tel +57 (602) 435 0777 Email servicioalcliente@eysingenieria.com',
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
                    style: { textBrushColor: '#00A0EF', fontSize: 14 }
                },
                {
                    type: 'Text',
                    value: '! Visit Again !',
                    position: { x: 300, y: 45 },
                    style: { textBrushColor: '#00A0EF', fontSize: 14 }
                }
            ]
        },
        
        fileName: "pdfdocument.pdf"
    };
}

function simulatedClick(){
    document.getElementById('dropdownInformationButton').click();
}


function setListBox(listUnSelected, listSelected, value, text) {


    listUnSelected = new ej.dropdowns.ListBox({
        fields: { value: value, text: text },
        height: '330px',
        scope: listSelected,
        toolbarSettings: { items: ['moveTo', 'moveFrom', 'moveAllTo', 'moveAllFrom'] },
        noRecordsTemplate: '<div class= "e-list-nrt"><span>No hay datos</span></div>'
    });
    listObjUserAsignation1.appendTo(listUnSelected);


    listSelected = new ej.dropdowns.ListBox({
        fields: { value: value, text: text },
        height: '330px',
        noRecordsTemplate: '<div class= "e-list-nrt"><span>No hay datos</span></div>'
    });
    listObjUserAsignation2.appendTo(listSelected);
}

function setTreeGrid(data, dataColumns, exportFunctions = null, add = false, nameKeyChildGrid, nameGrid = "Grid"){
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
        exportFunctions.push('Add')
    }
    
    treeGridObj = new ej.treegrid.TreeGrid({
        dataSource: data,
        allowRowDragAndDrop: true,
        childMapping: nameKeyChildGrid,
        treeColumnIndex: 3,
        allowPaging: true,
        toolbar: exportFunctions,
        allowSorting: true,
        allowFiltering: true,
        filterSettings: { type: 'Menu' },
        loadingIndicator: { indicatorType: 'Shimmer' },
        columns: dataColumns,
        editSettings: {
            allowAdding: true,
            allowEditing: true,
            allowDeleting: true,
            mode: 'Row'
        },
        pageSettings: { pageCount: 3 },
        width: '100%',
        height: '100%',
        locale: 'es-CO',
        queryCellInfo: customiseCell,
        rowDragStart: rowDragStart,
        rowDataBound: function (args) {
            if (args.data.childRecords.length == 0) {
                args.row.style.backgroundColor = "#ECECEC";
            }else if (args.data.childRecords.length > 0 && args.data.idElementFather != null) {
                args.row.style.backgroundColor = "#A7E1FD";
            } else{
                args.row.style.backgroundColor = "#ACCBDA";
            }
        },
        actionBegin: ActionBegin,
        actionComplete: ActionComplete,
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

