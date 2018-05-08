using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishLineDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        RaceManager.EndRace(false);
    }
}
