using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;
using System.Text;

[System.Serializable]
public class PlayerLogin
{
    public string email;
    public string password;
}

[System.Serializable]
public class LoginResponse
{
    public int id;
    public string token;
    public string name; // Assurez-vous que votre backend envoie bien cette donnée.
}

public class LoginUser : MonoBehaviour
{
    public TMP_InputField emailField;
    public TMP_InputField passwordField;
    public TextMeshProUGUI feedbackText;
    public string nextSceneName = "MainScene";

    public void CallLogin()
    {
        StartCoroutine(Login());
    }

    private IEnumerator Login()
    {
        if (string.IsNullOrWhiteSpace(emailField.text) || string.IsNullOrWhiteSpace(passwordField.text))
        {
            feedbackText.text = "Veuillez remplir tous les champs.";
            yield break;
        }

        PlayerLogin player = new PlayerLogin
        {
            email = emailField.text,
            password = passwordField.text
        };

        string jsonData = JsonUtility.ToJson(player);
        UnityWebRequest www = UnityWebRequest.Put("https://localhost:7079/api/Players/Login", jsonData);
        www.method = "POST";
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            // Analyse de la réponse pour des erreurs spécifiques
            if (www.responseCode == 400)
            {
                feedbackText.text = "Requête incorrecte. Vérifiez les données soumises.";
            }
            else if (www.responseCode == 401)
            {
                feedbackText.text = "Email ou mot de passe incorrect.";
            }
            else if (www.responseCode == 500)
            {
                feedbackText.text = "Erreur interne du serveur. Veuillez réessayer plus tard.";
            }
            else
            {
                feedbackText.text = $"Erreur de connexion: {www.error}";
            }
        }
        else
        {
            LoginResponse response = JsonUtility.FromJson<LoginResponse>(www.downloadHandler.text);
            if (!string.IsNullOrEmpty(response.name))
            {
                PlayerPrefs.SetString("PlayerName", response.name);
                PlayerPrefs.SetInt("PlayerID", response.id);
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                feedbackText.text = "Erreur: les informations de l'utilisateur n'ont pas été correctement reçues.";
            }
        }
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("Scenes/MainMenu");
    }
}