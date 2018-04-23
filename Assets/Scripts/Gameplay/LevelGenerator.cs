using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
	public float generatorDeltaX = 1.0f;
	public float generatorY = 0f;
    public float bottomHeight = 0f;

	public float width = 1f;
	public float heightMultiplier = 1f;
	public float levelLength = 2f;
	// Use this for initialization
	void Start ()
    {
		// Collider path, used to create a PolygonCollider2D
		List<Vector2> colliderPath = new List<Vector2> ();

		MeshFilter mf = GetComponent<MeshFilter> ();
		Mesh mesh = new Mesh ();
		mf.mesh = mesh;

		int numBlocks = (int)(levelLength / width);

		Vector3[] verts = new Vector3[(numBlocks + 1) * 2];
		int[] tris = new int[numBlocks * 6];
		Vector3[] normals = new Vector3[(numBlocks + 1) * 2];
		Vector2[] uvs = new Vector2[(numBlocks + 1) * 2];
		float height = heightMultiplier * Mathf.PerlinNoise(0f, generatorY);
		// Create inital first two points
		verts [0] = new Vector3 (0, bottomHeight, 0);
		verts [1] = new Vector3 (0, height, 0);

		// Add both vertices to the collider path
		colliderPath.Add (verts [0]);
		colliderPath.Add (verts [1]);

		normals [0] = -Vector3.forward;
		normals [1] = -Vector3.forward;

		uvs [0] = new Vector2 (0f, 0f);
		uvs [1] = new Vector2 (0f, 1f);
		int triCount = 0;

		for (int i = 0; i < numBlocks; i++)
        {
			// Create two new verties for this block (two verts from previous block will be reused)
			height = heightMultiplier * Mathf.PerlinNoise(((i+1)*generatorDeltaX), generatorY);
			verts [(i * 2) + 2] = new Vector3 ((i + 1) * width, bottomHeight, 0);
			verts [(i * 2) + 3] = new Vector3 ((i + 1) * width, height, 0);

			// Add the top vertex to the collider path
			colliderPath.Add (verts [(i * 2) + 3]);

			// Create normals and uvs
			normals [(i * 2) + 2] = -Vector3.forward;
			normals [(i * 2) + 3] = -Vector3.forward;
			uvs [(i * 2) + 2] = new Vector2 ((i + 1) / (float)numBlocks, 0f);
			uvs [(i * 2) + 3] = new Vector2 ((i + 1) / (float)numBlocks, 1f);

			// Create the two triangles for this block
			tris [(triCount * 3) + 0] = (i * 2) + 0;
			tris [(triCount * 3) + 1] = (i * 2) + 1;
			tris [(triCount * 3) + 2] = (i * 2) + 2;
			triCount++;
			tris [(triCount * 3) + 0] = (i * 2) + 1;
			tris [(triCount * 3) + 1] = (i * 2) + 3;
			tris [(triCount * 3) + 2] = (i * 2) + 2;
			triCount++;
		}

		// Close off the polygon collider path
		colliderPath.Add(verts[verts.Length-2]);
		colliderPath.Add (verts[0]);

		// Assign to mesh
		mesh.vertices = verts;
		mesh.triangles = tris;
		mesh.normals = normals;
		mesh.uv = uvs;

		// Add the polygon collider
		PolygonCollider2D col = GetComponent<PolygonCollider2D>();
		col.pathCount = 1;
		col.SetPath (0, colliderPath.ToArray());
	}

}
