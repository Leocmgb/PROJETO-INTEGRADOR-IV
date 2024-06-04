using System;
using UnityEngine;
using MySql.Data.MySqlClient;


public class DatabaseManager : MonoBehaviour
{
    private string connectionString = "server=localhost;uid=seu_usuario_mysql;pwd=sua_senha_mysql;database=seu_banco_de_dados;";
    private MySqlConnection connection;

    void Start()
    {
        //ConnectToDatabase();
    }

    private void ConnectToDatabase()
    {
        try
        {
            connection = new MySqlConnection(connectionString);
            connection.Open();
            Debug.Log("Conex�o bem-sucedida!");

            // Verifique as credenciais do usu�rio aqui e, se forem corretas, navegue para outra tela
            // Exemplo de verifica��o:
            // if (VerificarCredenciais("email", "senha"))
            // {
            //     // Navegar para outra tela ou realizar a��o desejada
            // }
            // else
            // {
            //     Debug.Log("Credenciais inv�lidas!");
            // }
        }
        catch (Exception e)
        {
            Debug.LogError("Erro ao conectar ao banco de dados: " + e.Message);
        }
    }

    private bool VerificarCredenciais(string email, string senha)
    {
        // Aqui voc� pode executar uma consulta SQL para verificar se o email e a senha fornecidos correspondem a um usu�rio na tabela "clientes"
        // Por raz�es de seguran�a, recomenda-se usar consultas parametrizadas ou Stored Procedures para evitar inje��o de SQL
        // Aqui est� um exemplo b�sico de como fazer isso:

        string query = "SELECT COUNT(*) FROM clientes WHERE email = @email AND senha = @senha";
        MySqlCommand cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@email", email);
        cmd.Parameters.AddWithValue("@senha", senha);

        int count = Convert.ToInt32(cmd.ExecuteScalar());
        return count > 0;
    }

    void OnApplicationQuit()
    {
        if (connection != null && connection.State != System.Data.ConnectionState.Closed)
        {
            connection.Close();
            Debug.Log("Conex�o fechada!");
        }
    }
}

