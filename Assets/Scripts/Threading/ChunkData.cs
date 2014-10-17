using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChunkData {

    public int Width {
        get {
            return World.Current.ChunkWidth;
        }
    }
    public int Height {
        get {
            return World.Current.ChunkHeight;
        }
    }

    public Block[, ,] Blocks;
    public Vector3 Position;
    public List<Vector3> verts = new List<Vector3>();
    public List<Vector2> uvs = new List<Vector2>();
    public List<int> tris = new List<int>();
    public List<Color> colors = new List<Color>();
    public Chunk ChunkPrefab;

    public ChunkData(Vector3 position) {
        Position = position;
    }

    public override string ToString() {
        return string.Format("Chunk_{0}", Position.ToString());
    }
}

