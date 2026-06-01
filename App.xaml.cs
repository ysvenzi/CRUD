using System.Configuration;
using System.Data;
using System.Windows;
using DotNetEnv;

namespace CRUD;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    internal static string? stringConexao;

    protected override void OnStartup(StartupEventArgs e)
    {
        Env.Load("C://Users//Aluno//RiderProjects//CRUD//.env");

        stringConexao = Environment.GetEnvironmentVariable("MYSQL_STRING");

        base.OnStartup(e);
    }
}