var dataHideColums, dataSearchColumns;

$(document).ready(function(){
  createElemntsTimes();
  multiSelect();
  drodownDataSearch(columnsSearch, 'CustomName', 'searchParam');
});




function ServiceGetMessages() {
    // Display loading modal
    Swal.fire({
        title: 'Loading...',
        allowOutsideClick: false,
        showConfirmButton: false, // Oculta el botón de confirmación
        onBeforeOpen: (modal) => {
            modal.showLoading();
            modal.disableCloseButton(); // Desactiva el botón de cancelación
        }
    });

    var data = {
        startDate: $("#dtpStart").val(),
        endDate: $("#dtpEnd").val(),
    };

    $.ajax({
        data: data,
        type: "POST",
        dataType: "json",
        url: '/Messages/Search',
    }).then(response => JSON.parse(JSON.stringify(response)))
        .then(data => {
            // Hide loading modal
            Swal.close();

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
            let exportFunctions = addFnctionsGrid(['Excel']);
            dataColumns = addCommandsGridDetails(dataColumns);
            dropdowns.enabled = true;
            dataGridSave = data.dataMessages;
            setGrid(data.dataMessages, dataColumns, exportFunctions);
        })
        .catch(error => {
            // Hide loading modal
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: error.name + ': ' + error.message
            });
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