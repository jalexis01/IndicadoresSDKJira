// See https://aka.ms/new-console-template for more information
using DashboarJira.Controller;
using DashboarJira.Services;

JiraAccess jira = new JiraAccess();

RANOController RAIOController= new RANOController();
Console.WriteLine(RAIOController.RAIOGeneral("2023-02-01","2023-02-28"));

jira.getTicket();