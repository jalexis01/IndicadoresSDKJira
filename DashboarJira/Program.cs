// See https://aka.ms/new-console-template for more information
using DashboarJira.Services;

Console.WriteLine("Hello, World!");
JiraAccess jira = new JiraAccess();

jira.GetTikets(0,100,null,null,null);