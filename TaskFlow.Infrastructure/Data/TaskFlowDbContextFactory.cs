using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TaskFlow.Infrastructure.Data;

public sealed class TaskFlowDbContextFactory
    : IDesignTimeDbContextFactory<TaskFlowDbContext>
{
    public TaskFlowDbContext CreateDbContext(string[] args)
    {
        var currentDirectory = Directory.GetCurrentDirectory();

        var configurationPath = FindApiDirectory(currentDirectory);

        var configuration = new ConfigurationBuilder()
            .SetBasePath(configurationPath)
            .AddJsonFile(
                "appsettings.json",
                optional: false,
                reloadOnChange: false
            )
            .AddJsonFile(
                "appsettings.Development.json",
                optional: true,
                reloadOnChange: false
            )
            .AddEnvironmentVariables()
            .Build();

        var connectionString =
            configuration.GetConnectionString("PostgreSQL");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Connection string 'PostgreSQL' was not configured."
            );
        }

        var optionsBuilder =
            new DbContextOptionsBuilder<TaskFlowDbContext>();

        optionsBuilder.UseNpgsql(connectionString);

        return new TaskFlowDbContext(
            optionsBuilder.Options
        );
    }

    private static string FindApiDirectory(
        string currentDirectory)
    {
        // Caso o EF já esteja executando dentro de TaskFlow.Api
        var appSettingsInCurrentDirectory =
            Path.Combine(
                currentDirectory,
                "appsettings.json"
            );

        if (File.Exists(appSettingsInCurrentDirectory))
        {
            return currentDirectory;
        }

        // Caso esteja executando na raiz da solution
        var apiDirectory =
            Path.Combine(
                currentDirectory,
                "TaskFlow.Api"
            );

        var appSettingsInApiDirectory =
            Path.Combine(
                apiDirectory,
                "appsettings.json"
            );

        if (File.Exists(appSettingsInApiDirectory))
        {
            return apiDirectory;
        }

        // Procura subindo níveis da árvore
        var directory =
            new DirectoryInfo(currentDirectory);

        while (directory is not null)
        {
            var possibleApiDirectory =
                Path.Combine(
                    directory.FullName,
                    "TaskFlow.Api"
                );

            var possibleAppSettings =
                Path.Combine(
                    possibleApiDirectory,
                    "appsettings.json"
                );

            if (File.Exists(possibleAppSettings))
            {
                return possibleApiDirectory;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException(
            "Não foi possível localizar o diretório TaskFlow.Api contendo appsettings.json."
        );
    }
}