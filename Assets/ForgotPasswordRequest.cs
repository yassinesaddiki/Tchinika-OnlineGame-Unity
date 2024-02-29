using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class ForgotPasswordRequest : MonoBehaviour
{
    public string apiUrl = "http://localhost:7079/api/Players/forgot-password"; 
    public TMP_InputField emailField; // Référence au champ d'entrée de l'email dans l'UI

    // Cette méthode est appelée lorsque l'utilisateur clique sur le bouton de réinitialisation du mot de passe
    public void OnResetPasswordButtonPressed()
    {
        // Vérifie si le champ d'email n'est pas vide
        if (!string.IsNullOrEmpty(emailField.text))
        {
            RequestPasswordReset(emailField.text);
        }
        else
        {
            Debug.LogError("Le champ d'email est vide.");
        }
    }

    // Appelé pour envoyer une demande de réinitialisation de mot de passe
    public void RequestPasswordReset(string email)
    {
        StartCoroutine(SendPasswordResetRequest(email));
    }

    IEnumerator SendPasswordResetRequest(string email)
    {
        WWWForm form = new WWWForm();
        form.AddField("Email", email);

        using (UnityWebRequest www = UnityWebRequest.Post(apiUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error: {www.error}");
            }
            else
            {
                Debug.Log("Password reset request sent. Check your email.");
            }
        }
    }
}
