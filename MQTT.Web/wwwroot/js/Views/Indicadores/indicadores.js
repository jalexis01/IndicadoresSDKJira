$(document).ready(function () {
    createElemntsTimes();
    multiSelect();
    drodownDataSearch(columnsSearch, 'CustomName', 'searchParam');
});


function ServiceGetIndicadores() {
    var startDate = $('#dtpStart').val();
    var endDate = $('#dtpEnd').val();
    var max = document.getElementById("maxSelect").value;
    var componente = $('#componente').val();
    console.log("idComponente: " + componente);

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
        url: "/Tickets/GetTickets",
        data: { startDate: startDate, endDate: endDate, max: max, componente: componente },
        success: function (response) {
            Swal.close();
            var tbody = $('#table tbody');
            tbody.empty();

            $.each(response, function (index, ticket) {
                var row = $('<tr>');
                console.log(ticket.id_ticket)
                row.append($('<td>').text(ticket.id_ticket));
                row.append($('<td>').text(ticket.fecha_apertura));
                

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
