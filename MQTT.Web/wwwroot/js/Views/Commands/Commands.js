var dataHideColums, dataSearchColumns;

$(document).ready(function(){
  createElemntsTimes();
  multiSelect();
  drodownDataSearch(columnsSearch, 'CustomName', 'searchParam');
});



function ServiceGetCommands() {
    $(".container-loader").css({ 'display': 'flex' })
    var data = {
        startDate: $("#dtpStart").val(),
        endDate: $("#dtpEnd").val(),
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
            filtersData = data.filters;
            dropdowns.value = null;
            multiSelectInput.value = null;
            dropdowns.enabled = false;
            multiSelectInput.enabled = false;
            const btn = document.getElementById('button-filter');
            btn.disabled = false;
            dateDocuments = $("#dtpStart").val() + " " + $("#dtpEnd").val();
            let dataColumns = setColums(data.dataMessages, columnsToHide);
            let exportFunctions = addFnctionsGrid('Excel');
            dropdowns.enabled = true;
            dataGridSave = data.dataMessages;
            setGrid(data.dataMessages, dataColumns, exportFunctions)
        })
        .catch(error => {
            $(".container-loader").css({ 'display': 'none' })
            errorsCase(error.name + ': ' + error.message)
        })
        .then(response => console.log('Success:', response));
}