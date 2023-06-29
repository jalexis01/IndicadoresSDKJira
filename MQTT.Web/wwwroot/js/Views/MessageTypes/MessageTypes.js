$(document).ready(function () {
    let dataColumns = setColums(lstMessageTypes);
    let exportFunctions = addFnctionsGrid('Excel','Add','Edit');
    dataColumns = addCommandsGrid(dataColumns)
    setGrid(lstMessageTypes, dataColumns, exportFunctions, "dgMessageType")
    unabledCommandsGrid()
})
