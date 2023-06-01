// See https://aka.ms/new-console-template for more information
using DashboarJira.Controller;
using DashboarJira.Model;
using DashboarJira.Services;
using MQTT.Infrastructure.DAL;

JiraAccess jira = new JiraAccess();
//jira.getIssueJira("PRUEBAS-117");
//Console.WriteLine("");

IAIOController iaio = new IAIOController();
Console.WriteLine("IAIO: " + iaio.IAIOGeneral("2023-01-01", "2023-06-01").CalcularIndicadorIAIO());
Console.WriteLine("IAIO CONTRATISTA: " +iaio.IAIOContratista("2023-01-01","2023-06-01").CalcularIndicadorIAIO());
Console.WriteLine("IAIO NO CONTRATISTA: " + iaio.IAIONoContratista("2023-01-01", "2023-06-01").CalcularIndicadorIAIO());
IANOController iano = new IANOController();
Console.WriteLine("IANO: " + iano.IANOGeneral("2023-01-01", "2023-06-01").CalcularIndicadorIANO());
Console.WriteLine("IANO contratista: " + iano.IANOContratista("2023-01-01", "2023-06-01").CalcularIndicadorIANO());
Console.WriteLine("IANO no contratista : " + iano.IANO_NO_Contratista("2023-01-01", "2023-06-01").CalcularIndicadorIANO());
ICPMController icpm = new ICPMController();
Console.WriteLine("ICPM itts: " + icpm.ICPM_ITTS("2023-01-01", "2023-06-01").CalcularIndicadorICPM());
Console.WriteLine("ICPM MTTO: " + icpm.ICPM_MTTO("2023-01-01", "2023-06-01").CalcularIndicadorICPM());
Console.WriteLine("ICPM PUERTAS: " + icpm.ICPM_PUERTAS("2023-01-01", "2023-06-01").CalcularIndicadorICPM());
Console.WriteLine("ICPM RFID: " + icpm.ICPM_RFID("2023-01-01", "2023-06-01").CalcularIndicadorICPM());
IEPMController iepm = new IEPMController();
Console.WriteLine("IEPM " + iepm.IEPM_GENERAL("2023-01-01", "2023-06-01").CalcularIndicadorIEPM());
Console.WriteLine("IEPM contratista " + iepm.IEPM_CONTRATISTA("2023-01-01", "2023-06-01").CalcularIndicadorIEPM());
Console.WriteLine("IEPM no contratista " + iepm.IEPM_NO_CONTRATISTA("2023-01-01", "2023-06-01").CalcularIndicadorIEPM());
RAIOController raio = new RAIOController();
Console.WriteLine("raio " + raio.RAIOGeneral("2023-01-01", "2023-06-01").CacularIndicadorRAIO());
Console.WriteLine("raio contratista " + raio.RAIOContratista("2023-01-01", "2023-06-01").CacularIndicadorRAIO());
Console.WriteLine("raio no contratista" + raio.RAIONoContratista("2023-01-01", "2023-06-01").CacularIndicadorRAIO());
RANOController rano = new RANOController();
Console.WriteLine("RANO " + rano.RAIOGeneral("2023-01-01", "2023-06-01").CacularIndicadorRAIO());
Console.WriteLine("RANO contratista " + rano.RAIOContratista("2023-01-01", "2023-06-01").CacularIndicadorRAIO());
Console.WriteLine("RANO no contratista" + rano.RAIONoContratista("2023-01-01", "2023-06-01").CacularIndicadorRAIO());
