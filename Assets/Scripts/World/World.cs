using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {

    //internal zodat het niet zichtbaaar is in de editor
    internal List<Chunk> Chunks = new List<Chunk>();
    internal List<Chunk> ChunksWaiting = new List<Chunk>();    

    //static instance van de World class voor makkelijke toegang in andere classes.
    public static World Current { get; set; }

    //public fields zichtbaar in de editor
    public int ChunkWidth = 16;
    public int ChunkHeight = 128;
    public int ViewRange = 64;
    public int Seed = 0;

    public Chunk Prefab;

    internal Vector3 Grain0Offset;
    internal Vector3 Grain1Offset;
    internal Vector3 Grain2Offset;

    //wordt als eerste uitgevoerd
    void Awake() {
        Current = this;
        if (Seed == 0)
            Seed = Random.Range(0, int.MaxValue);

        Random.seed = Seed;
        Grain0Offset = new Vector3(UnityEngine.Random.value * 10000, UnityEngine.Random.value * 10000, UnityEngine.Random.value * 10000);
        Grain1Offset = new Vector3(UnityEngine.Random.value * 10000, UnityEngine.Random.value * 10000, UnityEngine.Random.value * 10000);
        Grain2Offset = new Vector3(UnityEngine.Random.value * 10000, UnityEngine.Random.value * 10000, UnityEngine.Random.value * 10000);
    }
	
    //gebruikt voor initialisatie binnen class
	void Start () {
       
	}

    // Update naam zegt genoeg. LET OP NIET BEDOELD VOOR PHYSICS UPDATES
    void Update() {
        Vector3 camPos = new Vector3(transform.position.x, 0f, transform.position.z);

        //check if block came in range and add them
        for (float x = camPos.x - ViewRange; x < camPos.x + ViewRange; x += ChunkWidth) {
            //for (float y = camPos.y - ViewRange; y < camPos.y + ViewRange; y += ChunkHeight) {
            for (float z = camPos.z - ViewRange ; z < camPos.z + ViewRange; z += ChunkWidth) {

                Vector3 pos = new Vector3(x, 0, z);                
                pos.x = Mathf.Floor(x / (float)ChunkWidth) * ChunkWidth;
                pos.y = 0;
                pos.z = Mathf.Floor(z / (float)ChunkWidth) * ChunkWidth;

                Vector3 delta = pos - transform.position;
                delta.y = 0;
                if (delta.magnitude > ViewRange) continue;

                Chunk chunk = World.FindChunk(pos);
                if (chunk != null) continue;
                chunk = (Chunk)Instantiate(Prefab, pos, Quaternion.identity);
                
            }
            //}
        }
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
