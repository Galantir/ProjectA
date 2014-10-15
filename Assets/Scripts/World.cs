using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {

    //internal zodat het niet zichtbaaar is in de editor
    internal List<Chunk> Chunks = new List<Chunk>();
    internal List<Chunk> ChunksWaiting = new List<Chunk>();
    internal List<Chunk> ChunksLoading = new List<Chunk>();

    //static instance van de World class voor makkelijke toegang in andere classes.
    public static World Current { get; set; }

    //public fields zichtbaar in de editor
    public int Width = 128;
    public int Height = 128;
    public int ChunkWidth = 16;
    public int ChunkHeight = 16;

    public Chunk Prefab;

    //wordt als eerste uitgevoerd
    void Awake() {
        Current = this;
    }
	
    //gebruikt voor initialisatie binnen class
	void Start () {
        for (int x = 0 - (Width / 2); x < (Width / 2); x += ChunkWidth) {
            for (int y = 0; y < this.Height; y += this.ChunkHeight) {
                for (int z = 0 - (Width / 2); z < (Width / 2); z += ChunkWidth) {
                    Vector3 pos = new Vector3(x, y, z);
              
                    Chunk chunk = World.FindChunk(pos);
                    //chunk niet gevonden nieuwe aanmaken
                    if (chunk == null) 
                    chunk = (Chunk)Instantiate(Prefab, pos, Quaternion.identity);
                }
            }
        }
	}

    // Update naam zegt genoeg. LET OP NIET BEDOELD VOOR PHYSICS UPDATES
    void Update() {
	
	}

    //zoek chunk in gelade chunks op basis van worldpositite
    public static Chunk FindChunk(Vector3 pos) {
        for (int a = 0; a < World.Current.Chunks.Count; a++) {
            Vector3 cpos = World.Current.Chunks[a].transform.position;
            if ((pos.x < cpos.x) || (pos.z < cpos.z) || (pos.y < cpos.y) || (pos.x >= cpos.x + World.Current.ChunkWidth) || (pos.z >= cpos.z + World.Current.ChunkWidth) || (pos.y >= cpos.y + World.Current.ChunkHeight)) continue;
            return World.Current.Chunks[a];
        }
        return null;
    }
}
