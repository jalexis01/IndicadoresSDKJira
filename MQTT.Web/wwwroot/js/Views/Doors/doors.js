function renderTable(components) {
    // Limpia el contenido actual de la tabla
    var tableBody = $('#componentesTable tbody');
    tableBody.empty();

    // Recorre los componentes y añade una fila por cada uno
    components.forEach(component => {
        var row = `<tr>
            <td>${component.idComponente || ''}</td>
            <td>${component.serial || ''}</td>
            <td>${component.anioFabricacion || ''}</td>
            <td>${component.estado ? 'Activo' : 'Inactivo'}</td>
        </tr>`;
        tableBody.append(row);  // Añade la fila a la tabla
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
        url: '/Doors/GetEstacionesConComponentes',  // Cambia a tu nuevo método si es necesario
    })
        .then(response => {
            // Iterar sobre cada estación en la respuesta
            response.forEach(item => {
                const estacion = item.estacion; // Obtener la estación
                const componentes = item.componentes; // Obtener los componentes

                // Mostrar información de la estación
                console.log(`Estación ID: ${estacion.id}`);
                console.log(`ID Estación: ${estacion.idEstacion}`);
                console.log(`Nombre Estación: ${estacion.nombreEstacion}`);
                console.log('Componentes:');

                // Iterar sobre los componentes de la estación
                componentes.forEach(componente => {
                    console.log(`  Componente ID: ${componente.idComponente}`);
                    console.log(`  Serial: ${componente.serial}`);
                    console.log(`  Modelo: ${componente.modelo}`);
                    console.log(`  Año de Fabricación: ${componente.anioFabricacion}`);
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

            // Puedes añadir un listener para el cambio de selección
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
                // Aquí puedes procesar los datos recibidos para mostrarlos en una tabla o donde desees
                renderTable(response);  // Llama a la función que renderiza la tabla con los datos
            } else {
                console.log('No se encontraron componentes para la estación.');
            }
        })
        .catch(error => {
            $(".container-loader").css({ 'display': 'none' });
            console.error(error);  // Verifica si hay errores en la petición
        });
}