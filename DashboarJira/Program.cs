// See https://aka.ms/new-console-template for more information
using DashboarJira.Controller;
using DashboarJira.Services;

Console.WriteLine("Hello, World!");
JiraAccess jira = new JiraAccess();

//jira.GetTikets(0,100,null,null,null);
RAIOController RAIOController= new RAIOController();
Console.WriteLine(RAIOController.RAIOContratista("2023-01-01","2023-03-02"));