$(document).ready(function () {
    createElemntsTimes();
    multiSelect();
    drodownDataSearch(columnsSearch, 'CustomName', 'searchParam');
});


function ServiceGetIndicadores() {
    var startDate = $('#dtpStart').val();
    var endDate = $('#dtpEnd').val();
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
        data: { startDate: startDate, endDate: endDate},
        success: function (response) {
            Swal.close();
            var tbody = $('#table tbody');
            tbody.empty();

            $.each(response, function (index, indicador) {
                console.log(data)
                var row = $('<tr>');
                console.log('Nombre indicador: '+ indicador.nombre)
                console.log('Valor: ' + indicador.calculo)
                row.append($('<td>').text(indicador.nombre));
                row.append($('<td>').text(indicador.calculo));
                //row.append($('<td>').text(indicador.descripcion));                
                console.log(indicador.nombre)
                row.append($('<td>').text(indicador.nombre));
                row.append($('<td>').text(indicador.calculo));
                row.append($('<td>').text(indicador.descripcion));                
                tbody.append(row);
            });
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
