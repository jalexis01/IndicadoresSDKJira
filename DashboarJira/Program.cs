// See https://aka.ms/new-console-template for more information
using DashboarJira.Controller;
using DashboarJira.Services;

JiraAccess jira = new JiraAccess();
//DbConnector dbConnector = new DbConnector();

// Retrieve messages as JSON
//string messagesJson = dbConnector.GetMessagesAsJson();
//Console.WriteLine(messagesJson);

// Retrieve messages as string representation
//string messagesString = dbConnector.GetMessagesAsString();
//Console.WriteLine(messagesString);

IAIOController iaio = new IAIOController(jira);
IANOController iano = new IANOController(jira);
ICPMController icpm = new ICPMController(jira);
IEPMController iepm = new IEPMController(jira);
RAIOController raio = new RAIOController(jira);
RANOController rano = new RANOController(jira);
IRFController IRF = new IRFController(jira);

//Console.WriteLine("IAIO: " + iaio.IAIOGeneral("2023-05-01", "2023-06-01").CalcularIndicadorIAIO());
//Console.WriteLine("IAIO CONTRATISTA: " + iaio.IAIOContratista("2023-01-01", "2023-06-01").CalcularIndicadorIAIO());
//Console.WriteLine("IAIO NO CONTRATISTA: " + iaio.IAIONoContratista("2023-01-01", "2023-06-01").CalcularIndicadorIAIO());
//Console.WriteLine("IANO: " + iano.IANOGeneral("2023-05-01", "2023-06-01").CalcularIndicadorIANO());
//Console.WriteLine("IANO contratista: " + iano.IANOContratista("2023-01-01", "2023-06-01").CalcularIndicadorIANO());
//Console.WriteLine("IANO no contratista : " + iano.IANO_NO_Contratista("2023-01-01", "2023-06-01").CalcularIndicadorIANO());
/*
Console.WriteLine("ICPM MTTO: " + icpm.ICPM_MTTO("2023-01-01", "2023-06-01").CalcularIndicadorICPM());
Console.WriteLine("ICPM itts: " + icpm.ICPM_ITTS("2023-01-01", "2023-06-01").CalcularIndicadorICPM());
Console.WriteLine("ICPM PUERTAS: " + icpm.ICPM_PUERTAS("2023-01-01", "2023-06-01").CalcularIndicadorICPM());
Console.WriteLine("ICPM RFID: " + icpm.ICPM_RFID("2023-01-01", "2023-06-01").CalcularIndicadorICPM());

Console.WriteLine("IEPM " + iepm.IEPM_GENERAL("2023-05-01", "2023-06-01").CalcularIndicadorIEPM());
Console.WriteLine("IEPM contratista " + iepm.IEPM_CONTRATISTA("2023-01-01", "2023-06-01").CalcularIndicadorIEPM());
Console.WriteLine("IEPM no contratista " + iepm.IEPM_NO_CONTRATISTA("2023-01-01", "2023-06-01").CalcularIndicadorIEPM());

//Console.WriteLine("raio " + raio.RAIOGeneral("2023-05-01", "2023-06-01").CacularIndicadorRAIO());
//Console.WriteLine("raio contratista " + raio.RAIOContratista("2023-01-01", "2023-06-01").CacularIndicadorRAIO());
//Console.WriteLine("raio no contratista" + raio.RAIONoContratista("2023-01-01", "2023-06-01").CacularIndicadorRAIO());
*/
//Console.WriteLine("RANO " + rano.RANOGeneral("2023-05-01", "2023-06-01").CalcularIndicadorRANO());
//Console.WriteLine("RANO contratista " + rano.RANOContratista("2023-01-01", "2023-06-01").CalcularIndicadorRANO());
//Console.WriteLine("RANO no contratista" + rano.RANOContratista("2023-01-01", "2023-06-01").CalcularIndicadorRANO());

//Console.WriteLine("IRF " + IRF.IRFGeneral("2023-05-01", "2023-06-01").calculoIRF());
Indicadores indicadores = new Indicadores();


/*foreach (IndicadoresEntity indicador in indicadores.indicadores("2023-05-01", "2023-05-25"))
{
    Console.WriteLine($"Nombre: {indicador.nombre}");
    Console.WriteLine($"Cálculo: {indicador.calculo}");
    Console.WriteLine($"Descripción: {indicador.descripcion}");
    Console.WriteLine();
}
Console.WriteLine();*/


