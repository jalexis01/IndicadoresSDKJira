var dataHideColums, dataSearchColumns;


$(document).ready(function(){
  createElemntsTimes();
  drodownDataSearch(null, 'CustomName', 'searchParam');
  GetElements();
  ActionBeginSet(UpdateElement, CreateElement);
});



function GetElements() {
    $(".container-loader").css({ 'display': 'flex' })
    $.ajax({
        type: "GET",
        dataType: "json",
        url: '/Elements/GetElements',
    }).then(response => JSON.parse(JSON.stringify(response)))
        .then(data => {
            $(".container-loader").css({ 'display': 'none' })
            if (data.data.length == 0) {
                noData();
            }
            let columnshide = [{'name':'idElementType'}, {'name':'id'}, {'name':'creationUser'}, {'name':'updateUser'}, {'name':'idElementFather'}];
            let columnsEdit= [ {'name':'name','validation':true}, {'name':'value','validation':false}, {'name':'enable','validation':true}];

            //let dataColumns = createColumnsEditing(data.data, columnshide, columnsEdit);
            var dataColumns = [
                {field: 'id', headerText: 'id', textAlign: 'Center', headerTextAlign: 'Center', visible: false, allowEditing: false, validationRules: {required: false}},
                {field: 'code', headerText: 'codigo', textAlign: 'Center', headerTextAlign: 'Center', visible: true, allowEditing: true, validationRules: {required: true}},
                {field: 'idElementType', headerText: 'Tipo Elemento', textAlign: 'Center', headerTextAlign: 'Center', visible: true, allowEditing: true, validationRules: {required: true}, editType: 'dropdownedit', 
                edit: {  
                    params: {  
                        query: new ej.data.Query(),   
                        dataSource: typeElements,
                        fields:{text:'Name',value:'Id'},
                        change: function (args) {
                       
                            if(args.itemData.Id == 4 || args.itemData.Id == 5 ){
                                treeGridObj.columns[4].allowEditing = true;
                                treeGridObj.columns[4].validationRules.required = true;
                            }else{
                                treeGridObj.columns[4].allowEditing = false
                                treeGridObj.columns[4].validationRules.required = false;
                            }
                        },
                    }
                }},
                {field: 'name', headerText: 'Nombre', textAlign: 'Center', headerTextAlign: 'Center', visible: true, allowEditing: true, validationRules: {required: true}},
                {field: 'value', headerText: 'Serial', textAlign: 'Center', headerTextAlign: 'Center', visible: true, allowEditing: true, validationRules: {required: true}},
                {field: 'enable', headerText: 'Activo', textAlign: 'Center', headerTextAlign: 'Center', visible: true, allowEditing: true, validationRules: {required: true}, editType: 'dropdownedit', 
                edit: {  
                    params: {  
                        query: new ej.data.Query(),   
                        dataSource: [{dataBoolCol: "Activo", value: true},{dataBoolCol: "Inactivo", value: false}], 
                        fields:{text:'dataBoolCol',value:'value'},
                    }
                }},
                {field: 'idElementFather', headerText: 'IdElementFather', textAlign: 'Center', headerTextAlign: 'Center', visible: false, allowEditing: false, validationRules: {required: false}},
                {field: 'creationDate', headerText: 'Creado el', textAlign: 'Center', headerTextAlign: 'Center', visible: true, allowEditing: false, validationRules: {required: false}},
                {field: 'creationUser', headerText: 'Creado por', textAlign: 'Center', headerTextAlign: 'Center', visible: false, allowEditing: false, validationRules: {required: false}},
                {field: 'lastUpdate', headerText: 'Modificado el', textAlign: 'Center', headerTextAlign: 'Center', visible: true, allowEditing: false, validationRules: {required: false}},
                {field: 'updateUser', headerText: 'Modificado por', textAlign: 'Center', headerTextAlign: 'Center', visible: false, allowEditing: false, validationRules: {required: false}},
            ];
            //dataColumns = commandEditingColumn(dataColumns)
            setTreeGrid(data.data, dataColumns, null, true, "subElements")
        })
        .catch(error => {
            $(".container-loader").css({ 'display': 'none' })
            errorsCase(error.name + ': ' + error.message)
        })
        .then(response => console.log('Success:', response));
};

var UpdateElement = (dataArgs, previousData = null) => {
    $(".container-loader").css({ 'display': 'flex' })
    console.log(dataArgs);
    var data = {
        id: dataArgs.id,
        idElementType: dataArgs.idElementType,
        idElementFather: dataArgs.idElementFather,
        creationUser: dataArgs.creationUser,
        name: dataArgs.name,
        value: dataArgs.value,
        enable: dataArgs.enable
    }
    var dataOld;
    dataOld = {
            id: previousData.id,
            idElementType: previousData.idElementType,
            idElementFather: previousData.idElementFather,
            creationUser: previousData.creationUser,
            name: previousData.name,
            value: previousData.value,
            enable: previousData.enable
    }
    
    $.ajax({
        type: "PUT",
        data: {
            newElementDTO: data,
            oldElementDTO: dataOld
        },
        dataType: "json",
        url: '/Elements/UpdateElement',
    }).then(response => JSON.parse(JSON.stringify(response)))
        .then(data => {
            $(".container-loader").css({ 'display': 'none' })
            columnsInEdit= [{'name':'id'}, {'name':'creationDate'}, {'name':'creationUser'}, {'name':'lastUpdate'}, {'name':'updateUser'}];
            let dataColumns = createColumnsEditing(data.data, null, columnsInEdit);
            setTreeGrid(data.data, dataColumns, null, true, "subElements")
        })
        .catch(error => {
            $(".container-loader").css({ 'display': 'none' })
            errorsCase(error.name + ': ' + error.message)
        })
        .then(response => console.log('Success:', response));
}

var CreateElement = (dataArgs) => {
    $(".container-loader").css({ 'display': 'flex' })
    console.log(dataArgs);
    var data = {
        idElementType: dataArgs.idElementType,
        idElementFather: dataArgs.idElementFather,
        creationUser: 1, //dataArgs.creationUser,
        name: dataArgs.name,
        value: dataArgs.value,
        enable: dataArgs.enable,
        code: dataArgs.code
    }
    $.ajax({
        type: "POST",
        data: data,
        dataType: "json",
        url: '/Elements/AddElement',
    }).then(response => JSON.parse(JSON.stringify(response)))
        .then(data => {
            $(".container-loader").css({ 'display': 'none' })
            columnsInEdit= [{'name':'id'}, {'name':'creationDate'}, {'name':'creationUser'}, {'name':'lastUpdate'}, {'name':'updateUser'}];

            let dataColumns = createColumnsEditing(data.data, null, columnsInEdit);
            setTreeGrid(data.data, dataColumns, null, true, "subElements")
        })
        .catch(error => {
            $(".container-loader").css({ 'display': 'none' })
            errorsCase(error.name + ': ' + error.message)
        })
        .then(response => console.log('Success:', response));
}