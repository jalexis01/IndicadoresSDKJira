function renderTable(components) {
    // Limpia el contenido actual de la tabla
    var tableBody = $('#componentesTable tbody');
    tableBody.empty();

    // Recorre los componentes y a�ade una fila por cada uno
    components.forEach(component => {
        var row = `<tr>
            <td>${component.idComponente || ''}</td>
            <td>${component.serial || ''}</td>
            <td>${component.anioFabricacion || ''}</td>
            <td>${component.estado ? 'Activo' : 'Inactivo'}</td>
        </tr>`;
        tableBody.append(row);  // A�ade la fila a la tabla
    });
}

$(document).ready(function () {
    //GetComponents(9121);
    //GetEstacionesConComponentes();
    //GetEstaciones();
});


function GetEstacionesConComponentes() {
    $.ajax({
        type: "GET",
        dataType: "json",
        url: '/Doors/GetEstacionesConComponentes',  // Cambia a tu nuevo m�todo si es necesario
    })
        .then(response => {
            // Iterar sobre cada estaci�n en la respuesta
            response.forEach(item => {
                const estacion = item.estacion; // Obtener la estaci�n
                const componentes = item.componentes; // Obtener los componentes

                // Mostrar informaci�n de la estaci�n
                console.log(`Estaci�n ID: ${estacion.id}`);
                console.log(`ID Estaci�n: ${estacion.idEstacion}`);
                console.log(`Nombre Estaci�n: ${estacion.nombreEstacion}`);
                console.log('Componentes:');

                // Iterar sobre los componentes de la estaci�n
                componentes.forEach(componente => {
                    console.log(`  Componente ID: ${componente.idComponente}`);
                    console.log(`  Serial: ${componente.serial}`);
                    console.log(`  Modelo: ${componente.modelo}`);
                    console.log(`  A�o de Fabricaci�n: ${componente.anioFabricacion}`);
                    console.log(`  Fecha de Inicio: ${componente.fechaInicio}`);
                    console.log(`  Estado: ${componente.estado !== null ? componente.estado : 'N/A'}`);
                    console.log('----------------------------------');
                });
            });
        })
        .catch(error => {
            console.error("Error al obtener las estaciones:", error);
        });
}

function GetEstaciones() {
    $.ajax({
        type: "GET",
        dataType: "json",
        url: '/Doors/GetEstaciones',
    })
        .then(response => {
            response.forEach(estacion => {
                $('#estacionSelect').append(new Option(estacion.nombreEstacion, estacion.idEstacion));
            });

            // Puedes a�adir un listener para el cambio de selecci�n
            $('#estacionSelect').change(function () {
                const estacionId = $(this).val();
                GetComponents(estacionId);
            });
        })
        .catch(error => {
            console.error("Error al obtener las estaciones:", error);
        });
}

function GetComponents(estacionId) {
    console.log('Estacion ID:', estacionId);
    $(".container-loader").css({ 'display': 'flex' });

    $.ajax({
        type: "GET",
        dataType: "json",
        url: '/Doors/GetPuertasByEstacionId?estacionId=' + estacionId,
    })
        .then(response => {
            $(".container-loader").css({ 'display': 'none' });

            // Verifica si la respuesta tiene datos
            if (response && response.length > 0) {
                console.log('Datos recibidos:', response);  // Imprime los datos para verificar
                // Aqu� puedes procesar los datos recibidos para mostrarlos en una tabla o donde desees
                renderTable(response);  // Llama a la funci�n que renderiza la tabla con los datos
            } else {
                console.log('No se encontraron componentes para la estaci�n.');
            }
        })
        .catch(error => {
            $(".container-loader").css({ 'display': 'none' });
            console.error(error);  // Verifica si hay errores en la petici�n
        });
}