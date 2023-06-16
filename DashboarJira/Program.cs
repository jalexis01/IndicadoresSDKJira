﻿// See https://aka.ms/new-console-template for more information
using DashboarJira.Controller;
using DashboarJira.Model;
using DashboarJira.Services;
using System.Text.Json.Nodes;

JiraAccess jira = new JiraAccess();

Console.WriteLine(jira.GetTikets(0, 0, null, null, null).Count);
Console.WriteLine(jira.GetTikets(100, 0, null, null, null).Count);
Console.WriteLine(jira.GetTikets(200, 0, null, null, null).Count);

//DbConnector dbConnector = new DbConnector();
//string peticion = "WHERE fechaHoraEnvioDato >= '2023-01-01' AND fechaHoraEnvioDato <= '2023-02-01' AND codigoEvento = 'EVP8' ORDER BY fechaHoraEnvioDato ASC";
//dbConnector.GetEventos(peticion);
//-------------------------------------------------


//IDMController IDM = new IDMController(dbConnector);
//List<JsonObject> estaciones = new List<JsonObject>();
//JsonObject E9115 = new JsonObject();
//E9115.Add("idEstacion", "9116");
//E9115.Add("puertas", 26);
//estaciones.Add(E9115);
//foreach (EstacionEntity itt in IDM.calcularIDM(estaciones, "2023-05-01", "2023-05-31"))
//{
//    Console.WriteLine(itt.ConvertirAJsonIDM());
//}





//ITTSController itts = new ITTSController(jira,dbConnector);
//List<JsonObject> estaciones = new List<JsonObject>();
//JsonObject E9115 = new JsonObject();
//E9115.Add("idEstacion", "9115");
//E9115.Add("puertas", 26);
//estaciones.Add(E9115);
//foreach (EstacionEntity itt in itts.calcularTTOP(estaciones, "2023-05-01", "2023-05-31")) { 
//    Console.WriteLine(itt.ConvertirAJson());
//}




// Retrieve messages as JSON
//string messagesJson = dbConnector.GetMessagesAsJson();
//Console.WriteLine(messagesJson);

// Retrieve messages as string representation
//string messagesString = dbConnector.GetMessagesAsString();
//Console.WriteLine(messagesString);

//jira.getIssueJira("PRUEBAS-117");
//Console.WriteLine("");

//IAIOController iaio = new IAIOController(jira);
//Console.WriteLine("IAIO: " + iaio.IAIOGeneral("2023-05-01", "2023-06-01").CalcularIndicadorIAIO());
//Console.WriteLine("IAIO CONTRATISTA: " +iaio.IAIOContratista("2023-01-01","2023-06-01").CalcularIndicadorIAIO());
//Console.WriteLine("IAIO NO CONTRATISTA: " + iaio.IAIONoContratista("2023-01-01", "2023-06-01").CalcularIndicadorIAIO());
//IANOController iano = new IANOController(jira);
//Console.WriteLine("IANO: " + iano.IANOGeneral("2023-05-01", "2023-06-01").CalcularIndicadorIANO());
//Console.WriteLine("IANO contratista: " + iano.IANOContratista("2023-01-01", "2023-06-01").CalcularIndicadorIANO());
//Console.WriteLine("IANO no contratista : " + iano.IANO_NO_Contratista("2023-01-01", "2023-06-01").CalcularIndicadorIANO());
//ICPMController icpm = new ICPMController(jira);
//Console.WriteLine("ICPM MTTO: " + icpm.ICPM_MTTO("2023-01-01", "2023-06-01").CalcularIndicadorICPM());
//Console.WriteLine("ICPM itts: " + icpm.ICPM_ITTS("2023-01-01", "2023-06-01").CalcularIndicadorICPM());
//Console.WriteLine("ICPM PUERTAS: " + icpm.ICPM_PUERTAS("2023-01-01", "2023-06-01").CalcularIndicadorICPM());
//Console.WriteLine("ICPM RFID: " + icpm.ICPM_RFID("2023-01-01", "2023-06-01").CalcularIndicadorICPM());
//IEPMController iepm = new IEPMController(jira);
//Console.WriteLine("IEPM " + iepm.IEPM_GENERAL("2023-05-01", "2023-06-01").CalcularIndicadorIEPM());
//Console.WriteLine("IEPM contratista " + iepm.IEPM_CONTRATISTA("2023-01-01", "2023-06-01").CalcularIndicadorIEPM());
//Console.WriteLine("IEPM no contratista " + iepm.IEPM_NO_CONTRATISTA("2023-01-01", "2023-06-01").CalcularIndicadorIEPM());
//RAIOController raio = new RAIOController(jira);
//Console.WriteLine("raio " + raio.RAIOGeneral("2023-05-01", "2023-06-01").CacularIndicadorRAIO());
//Console.WriteLine("raio contratista " + raio.RAIOContratista("2023-01-01", "2023-06-01").CacularIndicadorRAIO());
//Console.WriteLine("raio no contratista" + raio.RAIONoContratista("2023-01-01", "2023-06-01").CacularIndicadorRAIO());
//RANOController rano = new RANOController(jira);
//Console.WriteLine("RANO " + rano.RANOGeneral("2023-05-01", "2023-06-01").CalcularIndicadorRANO());
//Console.WriteLine("RANO contratista " + rano.RAIOContratista("2023-01-01", "2023-06-01").CacularIndicadorRAIO());
//Console.WriteLine("RANO no contratista" + rano.RAIONoContratista("2023-01-01", "2023-06-01").CacularIndicadorRAIO());
//IRFController IRF = new IRFController(jira);
//Console.WriteLine("IRF " + IRF.IRFGeneral("2023-05-01", "2023-06-01").calculoIRF());
//Indicadores indicadores= new Indicadores();
//foreach (IndicadoresEntity indicador in indicadores.indicadores("2023-06-01", "2023-06-02"))
//{
//    Console.WriteLine($"Nombre: {indicador.nombre}");
//    Console.WriteLine($"Cálculo: {indicador.calculo}");
//    Console.WriteLine($"Descripción: {indicador.descripcion}");
//    Console.WriteLine();
//}
//Console.WriteLine();


