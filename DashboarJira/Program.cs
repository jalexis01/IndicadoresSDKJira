// See https://aka.ms/new-console-template for more information
using DashboarJira.Controller;
using DashboarJira.Model;
using DashboarJira.Services;
using MQTT.Infrastructure.DAL;

JiraAccess jira = new JiraAccess();
jira.GetTikets(0,500,"2023-05-01","2023-05-04",null);
Console.WriteLine("");

