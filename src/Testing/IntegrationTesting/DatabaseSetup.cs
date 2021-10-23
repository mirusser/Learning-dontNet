using System;
using System.Threading.Tasks;
using Npgsql;

namespace IntegrationTesting
{
    public class DatabaseSetup
    {
        public static async Task CreateDatabase(string connectionString, string db)
        {
            try
            {
                await using (NpgsqlConnection connection = new(connectionString))
                {
                    await connection.OpenAsync();
                    var command = @$"create database {db}
                               with owner = username
                               encoding = 'UTF8'
                               connection limit = -1;";
                    await using (var c = new NpgsqlCommand(command, connection))
                        await c.ExecuteNonQueryAsync();
                }

                if (connectionString.Contains("Database=postgres"))
                {
                    connectionString = connectionString.Replace("Database=postgres", $"Database={db}");
                }
                else
                {
                    connectionString += $"Database={db};";
                }

                await using (NpgsqlConnection connection = new(connectionString))
                {
                    await connection.OpenAsync();
                    var command = @"create table animals(
                                id serial PRIMARY KEY,
                                name text,
                                type text)";
                    await using (var c = new NpgsqlCommand(command, connection))
                        await c.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                var x = ex;
            }
        }

        public static async Task DeleteDatabase(string connectionString, string db)
        {
            NpgsqlConnection connection = new(connectionString);
            await connection.OpenAsync();

            string command = $@"SELECT pg_terminate_backend(pg_stat_activity.pid)
                               FROM pg_stat_activity
                               WHERE pg_stat_activity.datname = '{db}'";

            await using (var c = new NpgsqlCommand(command, connection))
                await c.ExecuteNonQueryAsync();

            await using (var c = new NpgsqlCommand($"drop database {db}", connection))
                await c.ExecuteNonQueryAsync();

            await connection.CloseAsync();
        }
    }
}