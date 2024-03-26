using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ZombieSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject normalZombiePrefab;
    [SerializeField] private GameObject bigZombiePrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private int zombiesPerNormal;
    [SerializeField] private int bigZombiesPerWave;
    [SerializeField] private int wavesPerBigZombie;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private float fadeDuration = 1f; // Added fade duration

    private int currentWave = 1;
    private int normalZombiesToSpawn;
    private int bigZombiesToSpawn;
    private int zombiesSpawned = 0;

    private bool waveSystemStarted = false;

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
        waveText.gameObject.SetActive(false);

        // Check if the server has started
        if (IsServer)
        {
            // Start the wave system
            StartCoroutine(StartWaveSystem());
        }
    }

    private void OnDestroy()
    {
        NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
    }

    private void HandleClientConnected(ulong clientId)
    {
        // Start the wave system when the first client connects
        if (!waveSystemStarted && IsServer)
        {
            StartCoroutine(StartWaveSystem());
        }
    }

    private IEnumerator StartWaveSystem()
    {
        // Wait until at least one player has joined
        while (NetworkManager.Singleton.ConnectedClientsList.Count == 0)
        {
            yield return null;
        }

        // Start the wave system
        waveSystemStarted = true;
        StartCoroutine(SpawnZombiesRepeatedly());
    }

    private IEnumerator SpawnZombiesRepeatedly()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (zombiesSpawned >= normalZombiesToSpawn + bigZombiesToSpawn)
            {
                // If all zombies for the wave have been spawned, start next wave
                currentWave++;
                UpdateWaveText();
                CalculateZombiesToSpawn();

                // Wait for at least 10 seconds before starting the next wave
                yield return new WaitForSeconds(5f);
            }

            if (zombiesSpawned < normalZombiesToSpawn)
            {
                SpawnZombie(normalZombiePrefab);
            }
            else if (zombiesSpawned < normalZombiesToSpawn + bigZombiesToSpawn)
            {
                SpawnZombie(bigZombiePrefab);
            }
        }
    }

    private void SpawnZombie(GameObject zombiePrefab)
    {
        GameObject zombie = Instantiate(zombiePrefab, spawnPoint.position, Quaternion.identity);
        zombie.GetComponent<NetworkObject>().Spawn(true);
        zombiesSpawned++;
    }

    private void UpdateWaveText()
    {
        waveText.text = "Wave " + (currentWave - 1).ToString();
        StartCoroutine(FadeText()); // Start fade animation when updating wave text
    }

    private IEnumerator FadeText()
    {
        waveText.gameObject.SetActive(true);
        float timer = 0f;
        Color originalColor = waveText.color;

        // Fade in
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / fadeDuration);
            waveText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // Wait with fully visible text
        yield return new WaitForSeconds(2f);

        timer = 0f;

        // Fade out
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = 1f - Mathf.Clamp01(timer / fadeDuration);
            waveText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
        waveText.gameObject.SetActive(false);
    }

    private void CalculateZombiesToSpawn()
    {
        normalZombiesToSpawn = currentWave * zombiesPerNormal;

        // Spawn big zombies on odd-numbered waves starting from wave 3
        if (currentWave >= 3 && currentWave % 2 != 0)
        {
            bigZombiesToSpawn = 1;
        }
        else
        {
            bigZombiesToSpawn = 0;
        }
    }
}
