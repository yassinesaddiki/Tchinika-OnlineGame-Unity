using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro; // Pour l'utilisation de TextMeshPro pour l'UI.
using System.Text;

// Modèle pour les données d'inscription du joueur.
[System.Serializable]
public class PlayerRegister
{
    public string name;
    public string email;
    public string password;
}

public class RegisterUser : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private TMP_InputField emailField;
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private TextMeshProUGUI feedbackText;
    [SerializeField] private string nextSceneName = "LoginS";

    // Appelé quand l'utilisateur clique sur le bouton d'inscription.
    public void CallRegister()
    {
        StartCoroutine(Register());
    }

    private IEnumerator Register()
    {
        // Vérifie si tous les champs sont remplis.
        if (string.IsNullOrWhiteSpace(nameField.text) || string.IsNullOrWhiteSpace(emailField.text) || string.IsNullOrWhiteSpace(passwordField.text))
        {
            feedbackText.text = "Veuillez remplir tous les champs.";
            yield break;
        }

        // Préparation des données du joueur pour l'inscription.
        PlayerRegister player = new PlayerRegister
        {
            name = nameField.text,
            email = emailField.text,
            password = passwordField.text
        };

        string jsonData = JsonUtility.ToJson(player);

        using (UnityWebRequest www = UnityWebRequest.PostWwwForm("https://localhost:7079/api/Players/Register", ""))
        {
            www.method = UnityWebRequest.kHttpVerbPOST;
            www.SetRequestHeader("Content-Type", "application/json");
            www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonData));
            www.downloadHandler = new DownloadHandlerBuffer();

            // Envoie la requête au serveur et attend la réponse.
            yield return www.SendWebRequest();

            // Gestion des réponses du serveur.
            if (www.result != UnityWebRequest.Result.Success)
            {
                if (www.responseCode == 400)
                {
                    feedbackText.text = "Inscription échouée : Email déjà utilisé ou données non valides.";
                }
                else if (www.responseCode == 500)
                {
                    feedbackText.text = "Erreur serveur : Problème lors de la création de l'utilisateur.";
                }
                else
                {
                    feedbackText.text = $"Erreur d'inscription: {www.error}";
                }
            }
            else
            {
                feedbackText.text = "Inscription réussie!";
                SceneManager.LoadScene(nextSceneName); // Charge la scène suivante après une inscription réussie.
            }
        }
    }
}
