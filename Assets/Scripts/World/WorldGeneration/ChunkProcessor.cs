using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ChunkProcessor {

    public static readonly TQueue<ChunkData> FinishedQueue = new TQueue<ChunkData>();
    public static int NumberOfChunksFinished { get; set; }

    public static void StartWorld() {
        List<ChunkData> chunksSortedByDistance = new List<ChunkData>();
        for (int x = 0 - 64; x < 64; x += World.Current.ChunkWidth) {
            for (int y = 0; y < 128; y += World.Current.ChunkHeight) {
                for (int z = 0 - 64; z < 64; z += World.Current.ChunkWidth) {
                    ChunkData chunk = new ChunkData(new Vector3(x, y, z));
                    chunksSortedByDistance.Add(chunk);

                }
            }
        }

        Vector3 center = new Vector3();
        chunksSortedByDistance = chunksSortedByDistance.OrderBy(c => c.Position.Distance(center)).ToList();
        chunksSortedByDistance.ForEach(chunk => TerrainGenerator.TerrainList.Add(chunk));
        TerrainGenerator.BeginProcessing = true;
        MeshGenerator.BeginProcessing = true;
    }

    public static ChunkData GetAFinishedChunk() {
        if (FinishedQueue.Count > 0) {
            return FinishedQueue.Dequeue();
        }
        return null;
    }

}
