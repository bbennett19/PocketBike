using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishLineDetector : MonoBehaviour
{
    public Timer timer;
    public PlayerController playerController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger");
        timer.Stop();
        playerController.Freeze();
    }
}
