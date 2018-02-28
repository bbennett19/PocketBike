using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner : MonoBehaviour {
    public GameObject spawnObject;
    public float spawnRate;
    private float m_elapsed = 0f;
    private GameObject m_lastClone = null;


	// Use this for initialization
	void Start () {
        m_elapsed = spawnRate;

    }

    // Update is called once per frame
    void Update () {
        m_elapsed += Time.deltaTime;

        if(m_elapsed >= spawnRate)
        {
            GameObject clone = Instantiate(spawnObject, this.transform.position, Quaternion.identity) as GameObject;
            if (m_lastClone != null) {
                clone.transform.localScale = new Vector3((this.transform.position.x - m_lastClone.transform.position.x)*1.15f, 1f, 1f);
            }
            m_lastClone = clone;
            m_elapsed = 0f;
        }
		
	}
}
