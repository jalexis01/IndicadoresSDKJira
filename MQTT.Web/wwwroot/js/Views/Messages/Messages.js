var dataHideColums, dataSearchColumns;

$(document).ready(function(){
  createElemntsTimes();
  drodownDataSearch(columnsSearch, 'CustomName', 'searchParam');
});





function ServiceGetMessages(){
    $(".container-loader").css({'display':'flex'})
    let paramSearch = null;
    if($("#searchParam").val() != undefined){
      paramSearch = columnsSearch.find(x => x.CustomName == $("#searchParam").val());
    }
    var data = {
      startDate: $("#dtpStart").val(),
      endDate: $("#dtpEnd").val(),
      messageField: paramSearch,
      value: $("#searchValue").val() ? $("#searchValue").val():null
      }
      $.ajax({
        data: data,
        type: "POST",
        dataType: "json",
        url: '/Messages/Search',
      }).then(response => JSON.parse(JSON.stringify(response)))
      .then(data => {
        $(".container-loader").css({'display':'none'})
        if(data.dataMessages.length == 0){
          noData();
          return;
        }
        dateDocuments = $("#dtpStart").val() + " " + $("#dtpEnd").val();
        let dataColumns = setColums(data.dataMessages,  columnsToHide);
        let exportFunctions = addFnctionsGrid(['Excel', 'Csv']);
        dataColumns = addCommandsGridDetails(dataColumns);
        setGrid(data.dataMessages, dataColumns, exportFunctions,detailsData)
      })
      .catch(error => {
        $(".container-loader").css({'display':'none'})
        errorsCase(error.name + ': ' + error.message)
      })
      .then(response => console.log('Success:', response));
}

const targetEl = document.getElementById('dropdownInformation');

// set the element that trigger the dropdown menu on click
const triggerEl = document.getElementById('dropdownInformationButton');

// options with default values
const options = {
  placement: 'bottom',
  onHide: () => {
      console.log('dropdown has been hidden');
  },
  onShow: () => {
      console.log('dropdown has been shown');
  }
};

var detailsData = function(args){
  //alert(JSON.stringify(args.rowData));
}