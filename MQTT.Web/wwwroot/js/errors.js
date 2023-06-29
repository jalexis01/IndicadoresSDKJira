function errorCustom(status, message) {
    Swal.fire({
        title: '<strong><u>'+ status+'</u></strong>',
        icon: 'error',
        text: message,
        confirmButtonText: 'Ok',
     })
}

function errorMessage(message) {
    Swal.fire({
        title: '<strong><u>Oops...</u></strong>',
        icon: 'error',
        text: message,
        confirmButtonText: 'Ok',
     })
}

function noData() {
    Swal.fire({
        title: '<strong><u>No se encontraron datos</u></strong>',
        icon: 'question',
        text: 'reintente usando parametros diferentes',
        confirmButtonText: 'Ok',
     })
}

function errorsCase(status) {
    switch (status) {
        case 500:
            errorCustom(status, "Error del servidor");
            break;
        case 400:
            errorCustom(status, "Campo/s incorrecto/s");
            break;
        case 401:
            errorCustom(status, "No esta autorizado para acceder a este elemento");
            break;
        case 404:
            errorCustom(status, "No se encontraron datos");
            break;
        case 408:
            errorCustom(status, "El tiempo de espera culmino y el servidor no dio respuesta");
            break;
        default:
            errorCustom('Oops...', "Error desconocido, verifique su conexion o comuniquese con el area de ayuda");
            break;
    }
}