using UnityEngine;
using System.Collections.Generic;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance;

    public List<Transform> respawnPoints;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one instance of RespawnManager in the scene!");
            return;
        }

        Instance = this;
    }

    public Vector3 GetRandomRespawnPoint()
    {
        if (respawnPoints.Count == 0)
        {
            Debug.LogError("No respawn points defined!");
            return Vector3.zero;
        }

        int index = Random.Range(0, respawnPoints.Count);
        Debug.Log($"Respawn point selected: {index}");
        return respawnPoints[index].position;
    }
}
