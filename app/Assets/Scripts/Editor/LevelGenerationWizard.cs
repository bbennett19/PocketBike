using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelGenerationWizard : ScriptableWizard
{
    public Material material;
    public float levelLength = 120f;
    public float perlinInitialXValue = 0f;
    public float perlinXStep = 0.1f;
    public float perlinInitialYValue = 0f;
    public float perlinYStep = 0f;
    public float worldspaceInitialXValue = 0f;
    public float worldspaceXStep = 0.5f;
    public float worldspaceInitialYValue = -5f;
    public float worldspaceYStep = 0f;
    public float heightMultiplier = 5f;

    [MenuItem("Level Tools/Create Level Wizard...")]
    static void CreateLevelWizard()
    {
        ScriptableWizard.DisplayWizard<LevelGenerationWizard>("Create Level", "Create");
    }

    private void OnWizardCreate()
    {
        GameObject level = new GameObject();
        // Add components 
        level.AddComponent<MeshRenderer>();
        level.AddComponent<MeshFilter>();
        level.AddComponent<PolygonCollider2D>();
        level.GetComponent<Renderer>().material = material;
        // Create the level

        // Collider path, used to create a PolygonCollider2D
        List<Vector2> colliderPath = new List<Vector2>();

        MeshFilter mf = level.GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        mf.mesh = mesh;

        int numBlocks = (int)(levelLength / worldspaceXStep);

        Vector3[] verts = new Vector3[(numBlocks + 1) * 2];
        int[] tris = new int[numBlocks * 6];
        Vector3[] normals = new Vector3[(numBlocks + 1) * 2];
        Vector2[] uvs = new Vector2[(numBlocks + 1) * 2];
        float height = heightMultiplier * Mathf.PerlinNoise(perlinInitialXValue, perlinInitialYValue);
        // Create inital first two points
        verts[0] = new Vector3(worldspaceInitialXValue, worldspaceInitialYValue, 0);
        verts[1] = new Vector3(worldspaceInitialXValue, worldspaceInitialYValue+height, 0);

        // Add both vertices to the collider path
        colliderPath.Add(verts[0]);
        colliderPath.Add(verts[1]);

        normals[0] = -Vector3.forward;
        normals[1] = -Vector3.forward;

        uvs[0] = new Vector2(0f, 0f);
        uvs[1] = new Vector2(0f, 1f);
        int triCount = 0;

        for (int i = 0; i < numBlocks; i++)
        {
            // Create two new verties for this block (two verts from previous block will be reused)
			float xVal = (i+1)*worldspaceXStep;
			float yVal = worldspaceInitialYValue + ((i + 1) * worldspaceYStep);
            height = heightMultiplier * Mathf.PerlinNoise(perlinInitialXValue + ((i + 1) * perlinXStep), perlinInitialYValue + ((i + 1) * perlinYStep));
            verts[(i * 2) + 2] = new Vector3(xVal, yVal, 0);
            verts[(i * 2) + 3] = new Vector3(xVal, yVal+height, 0);

            // Add the top vertex to the collider path
            colliderPath.Add(verts[(i * 2) + 3]);

            // Create normals and uvs
            normals[(i * 2) + 2] = -Vector3.forward;
            normals[(i * 2) + 3] = -Vector3.forward;
            uvs[(i * 2) + 2] = new Vector2((i + 1) / (float)numBlocks, 0f);
            uvs[(i * 2) + 3] = new Vector2((i + 1) / (float)numBlocks, 1f);

            // Create the two triangles for this block
            tris[(triCount * 3) + 0] = (i * 2) + 0;
            tris[(triCount * 3) + 1] = (i * 2) + 1;
            tris[(triCount * 3) + 2] = (i * 2) + 2;
            triCount++;
            tris[(triCount * 3) + 0] = (i * 2) + 1;
            tris[(triCount * 3) + 1] = (i * 2) + 3;
            tris[(triCount * 3) + 2] = (i * 2) + 2;
            triCount++;
        }

        // Close off the polygon collider path
        colliderPath.Add(verts[verts.Length - 2]);
        colliderPath.Add(verts[0]);

        // Assign to mesh
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.normals = normals;
        mesh.uv = uvs;

        // Add the polygon collider
        PolygonCollider2D col = level.GetComponent<PolygonCollider2D>();
        col.pathCount = 1;
        col.SetPath(0, colliderPath.ToArray());
    }

}
