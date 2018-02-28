using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveGenerator : MonoBehaviour {
    [Range(0.1f, 20f)]
    public float heightScale = 5.0f;
    [Range(0.1f, 40f)]
    public float detailScale = 5.0f;

    private Mesh myMesh;
    private Vector3[] verticies;

    void GenerateWave()
    {
        myMesh = GetComponent<MeshFilter>().mesh;
        verticies = myMesh.vertices;

        int counter = 0;
        int yLevel = 0;

        for(int i = 0; i < 11; i++)
        {
            for(int j = 0; j < 11; j++)
            {
                CalculationMethod(counter, yLevel);
                counter++;
            }
            yLevel++;
        }

        myMesh.vertices = verticies;
        myMesh.RecalculateBounds();
        myMesh.RecalculateNormals();
        Destroy(gameObject.GetComponent<MeshCollider>());
        MeshCollider collider = gameObject.AddComponent<MeshCollider>();
        collider.sharedMesh = myMesh;
    }

    public bool waves = true;
    public float waveSpeed = 5f;
    private void CalculationMethod(int i, int j)
    {
        if(waves)
        {
            verticies[i].z = Mathf.PerlinNoise(Time.time/waveSpeed + (verticies[i].x+this.transform.position.x)/detailScale, Time.time / waveSpeed + (verticies[i].y+this.transform.position.y)/detailScale) * heightScale;
            verticies[i].z -= j;
        }
        else
        {
            verticies[i].z = Mathf.PerlinNoise((verticies[i].x + this.transform.position.x) / detailScale, (verticies[i].y + this.transform.position.y) / detailScale) * heightScale;
            verticies[i].z -= j;
        }
    }

    // Use this for initialization
    void Start () {
        GenerateWave();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
