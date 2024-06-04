using System;
using UnityEngine;
using MySql.Data.MySqlClient;

public class showTables : MonoBehaviour
{
    private string connectionString = "server=localhost;uid=root;pwd=;database=ptiserver;";
    private MySqlConnection connection;

    void Start()
    {
        ConnectToDatabase();
        DisplayTableContents();
    }

    private void ConnectToDatabase()
    {
        try
        {
            connection = new MySqlConnection(connectionString);
            connection.Open();
            Debug.Log("Conexão bem-sucedida!");
        }
        catch (Exception e)
        {
            Debug.LogError("Erro ao conectar ao banco de dados: " + e.Message);
        }
    }

    private void DisplayTableContents()
    {
        string query = "SELECT * FROM clientes";

        MySqlCommand cmd = new MySqlCommand(query, connection);
        try
        {
            MySqlDataReader dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                int id = dataReader.GetInt32("ID");
                string nome = dataReader.GetString("nome");
                string email = dataReader.GetString("email");
                string senha = dataReader.GetString("senha");
                string cpf = dataReader.GetString("cpf");
                string telefone = dataReader.GetString("telefone");

                Debug.Log($"ID: {id}, Nome: {nome}, Email: {email}, Senha: {senha}, CPF: {cpf}, Telefone: {telefone}");
            }
            dataReader.Close();
        }
        catch (Exception e)
        {
            Debug.LogError("Erro ao executar o comando SQL: " + e.Message);
        }
    }

    void OnApplicationQuit()
    {
        if (connection != null && connection.State == System.Data.ConnectionState.Open)
        {
            connection.Close();
            Debug.Log("Conexão fechada!");
        }
    }
}
