using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Networking;

public class Autenticacao : MonoBehaviour
{
    public InputField campoEmail;
    public InputField campoSenha;
    public Text mensagemResultado;
    private bool Isauthenticated = false;

    public void Awake()
    {
        Isauthenticated = false;
    }

    public void Autenticar()
    {
        string email = campoEmail.text;
        string senha = campoSenha.text;

        StartCoroutine(EnviarAutenticacao(email, senha));
    }

    IEnumerator EnviarAutenticacao(string email, string senha)
    {
        string url = "URL_DA_SUA_API_DE_AUTENTICACAO"; // Substitua pelo URL real da sua API

        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("senha", senha);

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.downloadHandler.text == "autenticado")
                {
                    mensagemResultado.text = "Autenticado com sucesso!";
                    // Mudar de cena
                    SceneManager.LoadScene("NomeDaProximaCena");
                }
                else
                {
                    mensagemResultado.text = "Credenciais inválidas!";
                }
            }
        }
    }
}
