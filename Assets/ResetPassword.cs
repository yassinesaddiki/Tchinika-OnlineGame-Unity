using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ResetPassword : MonoBehaviour
{
    public string apiUrl = "http://localhost:7079/api/Players/reset-password"; // Ajustez selon votre configuration

    public void PerformPasswordReset(string email, string token, string newPassword)
    {
        StartCoroutine(ResetPasswordCoroutine(email, token, newPassword));
    }

    IEnumerator ResetPasswordCoroutine(string email, string token, string newPassword)
    {
        WWWForm form = new WWWForm();
        form.AddField("Email", email);
        form.AddField("Token", token);
        form.AddField("NewPassword", newPassword);

        using (UnityWebRequest www = UnityWebRequest.Post(apiUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error: {www.error}");
            }
            else
            {
                Debug.Log("Password has been reset successfully.");
            }
        }
    }
}
