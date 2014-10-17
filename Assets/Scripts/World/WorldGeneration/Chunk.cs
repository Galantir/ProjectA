using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshFilter))]
public class Chunk : MonoBehaviour {

    public MeshRenderer MeshRenderer;
    protected MeshCollider meshCollider;
    protected MeshFilter meshFilter;
    public Mesh VisualMesh;

    // Use this for initialization
    void Awake() {

        this.MeshRenderer = GetComponent<MeshRenderer>();
        this.meshCollider = GetComponent<MeshCollider>();
        this.meshFilter = GetComponent<MeshFilter>();
    }

    public void GenerateChunkTerrainMesh(ChunkData chunk) {

        VisualMesh = new Mesh();
        VisualMesh.name = chunk.ToString();
        VisualMesh.vertices = chunk.verts.ToArray();
        VisualMesh.uv = chunk.uvs.ToArray();
        VisualMesh.triangles = chunk.tris.ToArray();
        VisualMesh.colors = chunk.colors.ToArray();
        VisualMesh.RecalculateBounds();
        VisualMesh.RecalculateNormals();

        meshFilter.mesh = null;
        meshFilter.mesh = VisualMesh;
        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = VisualMesh;
        chunk.verts = new List<Vector3>();
        chunk.tris = new List<int>();
        chunk.uvs = new List<Vector2>();
        chunk.colors = new List<Color>();

    }

    // Update is called once per frame
    void Update() {

    }
}

