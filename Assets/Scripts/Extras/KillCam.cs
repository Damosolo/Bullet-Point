using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillCam : MonoBehaviour
{
    public Transform playerTransform; // The player's transform
    public Transform killCamTransform; // The kill cam's transform
    public float recordTime = 5f; // The length of time to record, in seconds

    private struct Snapshot
    {
        public Vector3 position;
        public Quaternion rotation;
    }

    private List<Snapshot> snapshots = new List<Snapshot>();
    private bool isKillCamActive = false;

    void Update()
    {
        if (isKillCamActive)
        {
            return;
        }

        // Record a new snapshot
        Snapshot snapshot = new Snapshot();
        snapshot.position = playerTransform.position;
        snapshot.rotation = playerTransform.rotation;
        snapshots.Add(snapshot);

        // Remove old snapshots if necessary
        while (snapshots.Count > recordTime * 60) // Assuming 60 frames per second
        {
            snapshots.RemoveAt(0);
        }
    }

    public void ActivateKillCam()
    {
        isKillCamActive = true;
        StartCoroutine(PlayKillCam());
    }

    private IEnumerator PlayKillCam()
    {
        foreach (Snapshot snapshot in snapshots)
        {
            killCamTransform.position = snapshot.position;
            killCamTransform.rotation = snapshot.rotation;
            yield return null;
        }

        // Reset the kill cam
        isKillCamActive = false;
        snapshots.Clear();
    }
}
