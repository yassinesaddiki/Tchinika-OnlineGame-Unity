using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.Netcode;

public class ZombieV1Input : NetworkBehaviour
{
    public float speed = 200f;
    public float nexWaypointDistance = 3f;
    public Transform enemyGFX;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    Seeker seeker;
    Rigidbody2D rb;

    List<Transform> players = new List<Transform>(); // List to store player transforms

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, .5f);

        if (IsServer)
        {
            StartCoroutine(UpdatePlayerList());
        }
    }

    IEnumerator UpdatePlayerList()
    {
        while (true)
        {
            players.Clear();

            // Find all players in the scene
            foreach (KeyValuePair<ulong, NetworkObject> kvp in NetworkManager.Singleton.SpawnManager.SpawnedObjects)
            {
                if (kvp.Value.gameObject.CompareTag("Player"))
                {
                    players.Add(kvp.Value.transform);
                }
            }

            yield return new WaitForSeconds(1f); // Adjust the interval based on your needs
        }
    }

    void UpdatePath()
    {
        if (!rb || players.Count == 0) return;

        float shortestDistance = Mathf.Infinity;
        Transform closestPlayer = null;

        // Find the closest player
        foreach (Transform player in players)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer < shortestDistance)
            {
                shortestDistance = distanceToPlayer;
                closestPlayer = player;
            }
        }

        if (closestPlayer != null && seeker.IsDone())
            seeker.StartPath(rb.position, closestPlayer.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void Update()
    {
        if (path == null) return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nexWaypointDistance)
        {
            currentWaypoint++;
        }

        // Flip the sprite based on movement direction
        if (rb.velocity.x >= 0.01f)
        {
            enemyGFX.localScale = new Vector3(Mathf.Abs(enemyGFX.localScale.x), enemyGFX.localScale.y, enemyGFX.localScale.z);
        }
        else if (rb.velocity.x <= -0.01f)
        {
            enemyGFX.localScale = new Vector3(-Mathf.Abs(enemyGFX.localScale.x), enemyGFX.localScale.y, enemyGFX.localScale.z);
        }
    }
}
