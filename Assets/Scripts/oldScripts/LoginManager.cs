using UnityEngine;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
using System;

public class LoginManager : MonoBehaviour
{
    // Referências aos campos de entrada de email e senha no Unity
    public InputField emailInput;
    public InputField senhaInput;

    // Mensagem de texto para exibir o resultado do login
    public Text loginResultText;

    // String de conexão com o banco de dados
    private string connectionString = "server=localhost;uid=root;pwd=;database=ptiserver;";

    public void FazerLogin()
    {
        // Obter os valores inseridos nos campos de entrada de email e senha
        string email = emailInput.text;
        string senha = senhaInput.text;

        // Verificar se os campos de entrada não estão vazios
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
        {
            loginResultText.text = "Por favor, preencha todos os campos!";
            return;
        }

        // Criar uma conexão com o banco de dados
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                // Abrir a conexão com o banco de dados
                connection.Open();

                // Consultar o banco de dados para verificar se o email e a senha correspondem a um registro na tabela clientes
                string query = "SELECT COUNT(*) FROM clientes WHERE email=@Email AND senha=@Senha";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Senha", senha);
                int count = Convert.ToInt32(command.ExecuteScalar());

                // Verificar se o login foi bem-sucedido
                if (count > 0)
                {
                    loginResultText.text = "Login bem-sucedido!";
                    // Aqui você pode redirecionar o usuário para a próxima tela ou realizar outras ações necessárias
                }
                else
                {
                    loginResultText.text = "Email ou senha incorretos!";
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Erro ao fazer login: " + ex.Message);
                loginResultText.text = "Erro ao fazer login. Por favor, tente novamente mais tarde.";
            }
        }
    }
}

