// See https://aka.ms/new-console-template for more information
using DashboarJira.Controller;
using DashboarJira.Model;
using DashboarJira.Services;

JiraAccess jira = new JiraAccess();

IRFEntity RAIOController= new IRFEntity();
Console.WriteLine(RAIOController.calculoIRF("2023-02-01","2023-02-28"));

jira.getTicket();