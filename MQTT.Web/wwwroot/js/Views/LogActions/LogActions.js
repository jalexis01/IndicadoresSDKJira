var dataHideColums, dataSearchColumns;
$(document).ready(function () {
    createElemntsTimes();
    multiSelect();
    drodownDataSearch(columnsSearch, 'CustomName', 'searchParam');
});

function validateDates() {
    var startDate = $('#dtpStart').val();
    var endDate = $('#dtpEnd').val();

    if (startDate === "" || endDate === "") {
        Swal.fire({
            title: 'Debe seleccionar la fecha',
        });
    } else {
        fetchLogs();
    }
}
function fetchLogs() {
    var startDate = document.getElementById('dtpStart').value;
    var endDate = document.getElementById('dtpEnd').value;

    $.ajax({
        url: '/LogActions/GetLogActions',
        type: 'GET',
        data: {
            startDate: startDate,
            endDate: endDate
        },
        success: function (data) {
            $('#logsTable tbody').empty(); // Limpiar la tabla antes de añadir los nuevos resultados

            // Añadir las nuevas filas a la tabla
            $.each(data, function (index, log) {
                $('#logsTable tbody').append(
                    `<tr>
                        <td class="border border-gray-300 p-2">${log.usuario}</td>
                        <td class="border border-gray-300 p-2">${log.accion}</td>
                        <td class="border border-gray-300 p-2">${log.fechaAccion}</td>
                    </tr>`
                );
            });
        },
        error: function (xhr, status, error) {
            console.error('Error al obtener los logs:', error);
            Swal.fire('Error', 'Hubo un problema al obtener los logs.', 'error');
        }
    });
}
