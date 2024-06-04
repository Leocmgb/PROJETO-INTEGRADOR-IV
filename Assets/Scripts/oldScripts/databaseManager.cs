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
            Debug.Log("Conexão bem-sucedida!");

            // Verifique as credenciais do usuário aqui e, se forem corretas, navegue para outra tela
            // Exemplo de verificação:
            // if (VerificarCredenciais("email", "senha"))
            // {
            //     // Navegar para outra tela ou realizar ação desejada
            // }
            // else
            // {
            //     Debug.Log("Credenciais inválidas!");
            // }
        }
        catch (Exception e)
        {
            Debug.LogError("Erro ao conectar ao banco de dados: " + e.Message);
        }
    }

    private bool VerificarCredenciais(string email, string senha)
    {
        // Aqui você pode executar uma consulta SQL para verificar se o email e a senha fornecidos correspondem a um usuário na tabela "clientes"
        // Por razões de segurança, recomenda-se usar consultas parametrizadas ou Stored Procedures para evitar injeção de SQL
        // Aqui está um exemplo básico de como fazer isso:

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
            Debug.Log("Conexão fechada!");
        }
    }
}

