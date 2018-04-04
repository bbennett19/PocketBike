using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
    public Text timerText;
    private float _elapsed;
    private bool _running = true;

    private void Start()
    {
        timerText.text = _elapsed.ToString("0.00");
    }

    // Update is called once per frame
    void Update () {
        if (_running)
        {
            _elapsed += Time.deltaTime;
            timerText.text = _elapsed.ToString("0.00");
        }

	}

    public void Stop()
    {
        _running = false;
    }
}
