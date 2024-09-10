var dataHideColums, dataSearchColumns;
$(document).ready(function () {
    createElemntsTimes();
    multiSelect();
    drodownDataSearch(columnsSearch, 'CustomName', 'searchParam');
});
function fetchLogs() {
    var startDate = document.getElementById('dtpStartLog').value;
    var endDate = document.getElementById('dtpEndLog').value;
    var searchParam = document.getElementById('searchParam').value;

    $.ajax({
        url: '/LogActions/GetLogActions',
        type: 'GET',
        data: {
            startDate: startDate,
            endDate: endDate,
            searchUser: searchParam
        },
        success: function (data) {
            $('#logsTable tbody').empty(); // Limpiar tabla
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
        }
    });
}
