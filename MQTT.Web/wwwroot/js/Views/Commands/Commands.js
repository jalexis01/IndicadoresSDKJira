var dataHideColums, dataSearchColumns;

$(document).ready(function(){
  createElemntsTimes();
  drodownDataSearch(columnsSearch, 'CustomName', 'searchParam');
});



function ServiceGetCommands() {
    $(".container-loader").css({ 'display': 'flex' })
    let paramSearch = null;
    if ($("#searchParam").val() != undefined) {
        paramSearch = columnsSearch.find(x => x.CustomName == $("#searchParam").val());
    }
    var data = {
        startDate: $("#dtpStart").val(),
        endDate: $("#dtpEnd").val(),
        messageField: paramSearch,
        value: $("#searchValue").val() ? $("#searchValue").val() : null
    }
    $.ajax({
        data: data,
        type: "POST",
        dataType: "json",
        url: '/Commands/Search',
    }).then(response => JSON.parse(JSON.stringify(response)))
        .then(data => {
            $(".container-loader").css({ 'display': 'none' })
            if (data.dataMessages.length == 0) {
                noData();
                return;
            }
            dateDocuments = $("#dtpStart").val() + " " + $("#dtpEnd").val();
            let dataColumns = setColums(data.dataMessages, columnsToHide);
            let exportFunctions = addFnctionsGrid('Excel');
            setGrid(data.dataMessages, dataColumns, exportFunctions)
        })
        .catch(error => {
            $(".container-loader").css({ 'display': 'none' })
            errorsCase(error.name + ': ' + error.message)
        })
        .then(response => console.log('Success:', response));
}