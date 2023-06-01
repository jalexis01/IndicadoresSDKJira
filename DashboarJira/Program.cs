// See https://aka.ms/new-console-template for more information
using DashboarJira.Controller;
using DashboarJira.Model;
using DashboarJira.Services;
using MQTT.Infrastructure.DAL;

JiraAccess jira = new JiraAccess();
jira.getIssueJira("PRUEBAS-117");
Console.WriteLine("");

IAIOController iaio = new IAIOController();
Console.WriteLine(iaio.IAIOContratista("2023-01-01","2023-06-01").CalcularIndicadorIAIO());

