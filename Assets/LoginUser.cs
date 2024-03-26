using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro; // Utilisé pour l'interaction avec les éléments UI TextMeshPro.
using System.Text;

// Définit la structure des données de connexion d'un joueur.
[System.Serializable]
public class PlayerLogin
{
    public string email;
    public string password;
}

// Ce composant gère la connexion d'un utilisateur.
public class LoginUser : MonoBehaviour
{
    // Références aux champs d'entrée de l'interface utilisateur pour l'email et le mot de passe.
    public TMP_InputField emailField;
    public TMP_InputField passwordField;

    // Affiche les retours à l'utilisateur.
    public TextMeshProUGUI feedbackText;

    // Nom de la scène à charger après une connexion réussie.
    string nextSceneName = "Scenes/Demo";

    // Déclenche la tentative de connexion.
    [System.Obsolete]
    public void CallLogin()
    {
        StartCoroutine(Login());
    }

    // Coroutine qui effectue la demande de connexion.
    [System.Obsolete]
    private IEnumerator Login()
    {
        if (string.IsNullOrWhiteSpace(emailField.text) || string.IsNullOrWhiteSpace(passwordField.text))
        {
            feedbackText.text = "Veuillez remplir tous les champs.";
            yield break;
        }

        PlayerLogin player = new()
        {
            email = emailField.text,
            password = passwordField.text
        };

        string jsonData = JsonUtility.ToJson(player);

        using UnityWebRequest www = UnityWebRequest.Post("https://localhost:7079/api/Players/Login", jsonData);
        {
            www.SetRequestHeader("Content-Type", "application/json");
            www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonData));
            www.downloadHandler = new DownloadHandlerBuffer();

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                // Amélioration de la gestion des erreurs avec des messages spécifiques selon le code de réponse HTTP
                switch (www.responseCode)
                {
                    case 400: // Bad Request
                        feedbackText.text = "Demande incorrecte. Vérifiez les données saisies.";
                        break;
                    case 401: // Unauthorized
                        feedbackText.text = "Authentification échouée. Vérifiez votre email et mot de passe.";
                        break;
                    case 403: // Forbidden
                        feedbackText.text = "Accès refusé. Vous n'avez pas la permission d'accéder à cette ressource.";
                        break;
                    case 404: // Not Found
                        feedbackText.text = "Service non trouvé. Vérifiez l'URL de l'API.";
                        break;
                    case 500: // Internal Server Error
                        feedbackText.text = "Erreur interne du serveur. Veuillez réessayer plus tard.";
                        break;
                    default: // Autres erreurs non spécifiquement gérées
                        feedbackText.text = $"Erreur de connexion: {www.error}";
                        break;
                }
            }
            else
            {
                feedbackText.text = "Connexion réussie!";
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("Scenes/MainMenu");
    }
}

