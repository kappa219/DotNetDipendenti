using MySqlConnector;

namespace corsosharp.DB;

public class DatabaseConnection
{
    private readonly string _connectionString;

    public DatabaseConnection(string connectionString)
    {
        _connectionString = connectionString;
    }//njbjS

    public MySqlConnection GetConnection()
    {
        MySqlConnection connection = null;
        try
        {
            connection = new MySqlConnection(_connectionString);
            connection.Open();
            Console.WriteLine("Connessione al database riuscita!");
        }
        catch (MySqlException ex)
        {
            Console.WriteLine("Errore di connessione: " + ex.Message);
        }

        return connection;
    }

    public void CloseConnection(MySqlConnection connection)
    {
        try
        {
            if (connection != null && connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
                Console.WriteLine("Connessione chiusa.");
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine("Errore nella chiusura: " + ex.Message);
        }
    }
}