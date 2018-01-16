using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPartCreator : MonoBehaviour {


	public void CreateCurve()
	{
		GameObject curve = new GameObject("Curve");
		curve.AddComponent<MeshRenderer>();
		MeshFilter meshFilter = curve.AddComponent<MeshFilter>();
		
		Mesh mesh = new Mesh();
		
		Vector3 p0 = Vector3.zero;
		Vector3 p1 = new Vector3(0, 0, 6);
		Vector3 p2 = new Vector3(-6, 0, 6);
		
		Vector3 p3 = new Vector3(4, 0, 0);
		Vector3 p4 = new Vector3(4, 0, 10);
		Vector3 p5 = new Vector3(-6, 0, 10);
		
		Vector3[] vertices = new Vector3[44];
		int[] triangles = new int[180];
		int indexVertices = 0;
		int indexTriangles = 0;

		for (int i = 0; i <= 10; i++)
		{
			float t = i / 10f;
			float oneMinusT = 1f - t;
			
			Vector3 p = Mathf.Pow(oneMinusT, 2) * p0 + 2 * oneMinusT * t * p1 + Mathf.Pow(t, 2) * p2;
			Vector3 c = Mathf.Pow(oneMinusT, 2) * p3 + 2 * oneMinusT * t * p4 + Mathf.Pow(t, 2) * p5;

			Vector3 pW = p;
			pW.y = 4;
			
			Vector3 cW = c;
			cW.y = 4;

			if (i < 10)
			{
				triangles[indexTriangles++] = i * 2;
				triangles[indexTriangles++] = i * 2 + 2;
				triangles[indexTriangles++] = i * 2 + 1;
			
				triangles[indexTriangles++] = i * 2 + 2;
				triangles[indexTriangles++] = i * 2 + 3;
				triangles[indexTriangles++] = i * 2 + 1;
			}
			
			vertices[indexVertices++] = p;
			vertices[indexVertices++] = c;
		}
		
		for (int i = indexVertices; i < vertices.Length; i++)
		{
			vertices[i] = vertices[i - indexVertices];
			vertices[i].y = 2;

			if (i < vertices.Length - 2)
			{
				if (i % 2 == 0)
				{
					triangles[indexTriangles++] = i - indexVertices;
					triangles[indexTriangles++] = i;
					triangles[indexTriangles++] = i - indexVertices + 2;
					
					triangles[indexTriangles++] = i;
					triangles[indexTriangles++] = i + 2;
					triangles[indexTriangles++] = i - indexVertices + 2;
				}
				else
				{
					triangles[indexTriangles++] = i - indexVertices;
					triangles[indexTriangles++] = i - indexVertices + 2;
					triangles[indexTriangles++] = i;
					
					triangles[indexTriangles++] = i - indexVertices + 2;
					triangles[indexTriangles++] = i + 2;
					triangles[indexTriangles++] = i;
				}
			}
		}
		
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();

		meshFilter.mesh = mesh;
	}

	public void CreateStraight()
	{
		GameObject curve = new GameObject("Straight");
		curve.AddComponent<MeshRenderer>();
		MeshFilter meshFilter = curve.AddComponent<MeshFilter>();
		
		Mesh mesh = new Mesh();

		Vector3[] vertices = {
			Vector3.zero,
			new Vector3(4, 0, 0), 
			new Vector3(0, 0, 10), 
			new Vector3(4, 0, 10), 
			new Vector3(0, 2, 0), 
			new Vector3(4, 2, 0), 
			new Vector3(0, 2, 10), 
			new Vector3(4, 2, 10) 
		};

		int[] triangles = new[]
		{
			0, 2, 1,
			2, 3, 1,
			0, 4, 2,
			4, 6, 2,
			1, 3, 5,
			3, 7, 5
		};

		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();

		meshFilter.mesh = mesh;
	}
}
