using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MySql.Data.MySqlClient;
using System;

public class InputManager : MonoBehaviour
{
    private string inputEmail;
    private string inputPassword;
    private string inputNome;
    private string inputTelefone;
    private string inputCPF;

    public GameObject telainicial, telacliente, telaErro, telaRegistrar;

    public InputField emailInputField;
    public InputField senhaInputField;
    public InputField nomeInputField;
    public InputField cpfInputField;
    public InputField telefoneInputField;

    public Text nomeText;
    public Text emailText;
    public Text senhaText;
    public Text cpfText;
    public Text telefoneText;

    // String de conexão com o banco de dados
    private string connectionString = "server=localhost;uid=root;pwd=;database=ptiserver;";

    void Start()
    {
        telainicial.SetActive(true);
        telacliente.SetActive(false);
        telaErro.SetActive(false);
        telaRegistrar.SetActive(false);
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("APPScene");
    }

    public void btnVoltar()
    {
        LoadScene();
    }

    public void btnTelaRegistrar()
    {
        telainicial.SetActive(false);
        telaRegistrar.SetActive(true);
    }

    public void GrabFromInputFieldEmail(string inputEmailtxt)
    {
        inputEmail = inputEmailtxt;
    }

    public void GrabFromInputFieldSenha(string inputSenhatxt)
    {
        inputPassword = inputSenhatxt;
    }

    public void GrabFromInputFieldNome(string inputNometxt)
    {
        inputNome = inputNometxt;
    }

    public void GrabFromInputFieldCPF(string inputCPFtxt)
    {
        inputCPF = inputCPFtxt;
    }

    public void GrabFromInputFieldTelefone(string inputTelefonetxt)
    {
        inputTelefone = inputTelefonetxt;
    }

    public void startbutton()
    {
        Debug.Log("Iniciando tentativa de login...");
        Debug.Log("Email: " + inputEmail);
        Debug.Log("Senha: " + inputPassword);
        FazerLogin(inputEmail, inputPassword);
    }

    public void registerbutton()
    {
        Debug.Log("Iniciando tentativa de registro...");
        FazerRegistro(inputNome, inputEmail, inputPassword, inputCPF, inputTelefone);
    }

    public void updatebutton()
    {
        Debug.Log("Iniciando atualização de cliente...");
        AtualizarCliente();
    }

    private void FazerLogin(string email, string senha)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
        {
            Debug.LogError("Campos de email ou senha estão vazios.");
            telainicial.SetActive(false);
            telaErro.SetActive(true);
            return;
        }

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                Debug.Log("Conexão com o banco de dados aberta.");

                string query = "SELECT nome, email, senha, cpf, telefone FROM clientes WHERE email=@Email AND senha=@Senha";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Senha", senha);
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    string nome = reader.GetString("nome");
                    string emailCliente = reader.GetString("email");
                    string senhaCliente = reader.GetString("senha");
                    string cpf = reader.GetString("cpf");
                    string telefone = reader.GetString("telefone");

                    nomeText.text = nome;
                    emailText.text = emailCliente;
                    senhaText.text = senhaCliente;
                    cpfText.text = cpf;
                    telefoneText.text = telefone;

                    inputNome = nome;
                    inputEmail = emailCliente;
                    inputPassword = senhaCliente;
                    inputCPF = cpf;
                    inputTelefone = telefone;

                    Debug.Log("Login bem-sucedido.");
                    telainicial.SetActive(false);
                    telacliente.SetActive(true);
                }
                else
                {
                    Debug.LogError("Email ou senha incorretos.");
                    telainicial.SetActive(false);
                    telaErro.SetActive(true);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Erro ao fazer login: " + ex.Message);
                telainicial.SetActive(false);
                telaErro.SetActive(true);
            }
        }
    }

    private void FazerRegistro(string nome, string email, string senha, string cpf, string telefone)
    {
        if (string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha) || string.IsNullOrEmpty(cpf) || string.IsNullOrEmpty(telefone))
        {
            Debug.LogError("Todos os campos devem ser preenchidos.");
            telaRegistrar.SetActive(false);
            telaErro.SetActive(true);
            return;
        }

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                Debug.Log("Conexão com o banco de dados aberta.");

                string query = "INSERT INTO clientes (nome, email, senha, cpf, telefone) VALUES (@Nome, @Email, @Senha, @CPF, @Telefone)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Nome", nome);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Senha", senha);
                command.Parameters.AddWithValue("@CPF", cpf);
                command.Parameters.AddWithValue("@Telefone", telefone);
                command.ExecuteNonQuery();

                Debug.Log("Registro bem-sucedido.");

                // Após registrar com sucesso, fazer login automaticamente
                telaRegistrar.SetActive(false);
                FazerLogin(email, senha);
            }
            catch (Exception ex)
            {
                Debug.LogError("Erro ao registrar: " + ex.Message);
                telaRegistrar.SetActive(false);
                telaErro.SetActive(true);
            }
        }
    }

    public void ExcluirUsuario()
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                Debug.Log("Conexão com o banco de dados aberta.");

                string query = "DELETE FROM clientes WHERE email=@Email";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", inputEmail);
                command.ExecuteNonQuery();

                Debug.Log("Usuário excluído com sucesso.");

                // Desconectar e voltar para a tela inicial
                telacliente.SetActive(false);
                LoadScene();
            }
            catch (Exception ex)
            {
                Debug.LogError("Erro ao excluir usuário: " + ex.Message);
                telaErro.SetActive(true);
            }
        }
    }

    private void AtualizarCliente()
    {
        if (nomeInputField == null || emailInputField == null || senhaInputField == null || cpfInputField == null || telefoneInputField == null)
        {
            Debug.LogError("Um ou mais campos de entrada não estão atribuídos.");
            telaErro.SetActive(true);
            return;
        }

        inputNome = nomeInputField.text;
        inputEmail = emailInputField.text;
        inputPassword = senhaInputField.text;
        inputCPF = cpfInputField.text;
        inputTelefone = telefoneInputField.text;

        if (string.IsNullOrEmpty(inputNome) || string.IsNullOrEmpty(inputEmail) || string.IsNullOrEmpty(inputPassword) || string.IsNullOrEmpty(inputCPF) || string.IsNullOrEmpty(inputTelefone))
        {
            Debug.LogError("Todos os campos devem ser preenchidos.");
            telaErro.SetActive(true);
            return;
        }

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                Debug.Log("Conexão com o banco de dados aberta.");

                string query = "UPDATE clientes SET nome=@Nome, senha=@Senha, cpf=@CPF, telefone=@Telefone WHERE email=@Email";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Nome", inputNome);
                command.Parameters.AddWithValue("@Email", inputEmail);
                command.Parameters.AddWithValue("@Senha", inputPassword);
                command.Parameters.AddWithValue("@CPF", inputCPF);
                command.Parameters.AddWithValue("@Telefone", inputTelefone);
                command.ExecuteNonQuery();

                Debug.Log("Cliente atualizado com sucesso.");

                // Atualizar os textos na tela do cliente
                nomeText.text = inputNome;
                emailText.text = inputEmail;
                senhaText.text = inputPassword;
                cpfText.text = inputCPF;
                telefoneText.text = inputTelefone;

                Debug.Log("Dados na tela do cliente atualizados.");
            }
            catch (Exception ex)
            {
                Debug.LogError("Erro ao atualizar cliente: " + ex.Message);
                telaErro.SetActive(true);
            }
        }
    }
}
