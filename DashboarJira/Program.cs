// See https://aka.ms/new-console-template for more information
using DashboarJira.Controller;
using DashboarJira.Model;
using DashboarJira.Services;
using MQTT.Infrastructure.DAL;

JiraAccess jira = new JiraAccess();
jira.getIssueJira("PRUEBAS-117");
Console.WriteLine("");

