using UnityEngine;
using System.Collections;
using System.Threading;
using SimplexNoise;


public class TerrainGenerator {

    public static TList<ChunkData> TerrainList = new TList<ChunkData>();
    public static bool BeginProcessing = false;
    public static Thread thread;


    public static void BeginTerrainGenerator() {

        thread = new Thread(() => {
            while (true) {
                if (BeginProcessing) {
                    BeginProcessing = false;

                    foreach (ChunkData chunk in TerrainList) {
                        GenerateTerrain(chunk);
                        MeshGenerator.MeshList.Add(chunk);
                        TerrainList.Remove(chunk);
                    }
                    TerrainList.Clear();
                    MeshGenerator.BeginProcessing = true;
                }
                Thread.Sleep(1);
            }
        });
        thread.Start();
    }

    public static void StopTerrainGenerator() {
        thread.Abort();
    }

    public static void GenerateTerrain(ChunkData chunk) {
        chunk.Blocks = new Block[chunk.Width, chunk.Height, chunk.Width];
        for (int x = 0; x < chunk.Width; x++) {
            for (int y = 0; y < chunk.Height; y++) {
                for (int z = 0; z < chunk.Width; z++) {
                    chunk.Blocks[x, y, z] = GetVirtualBlock(chunk.Position + new Vector3(x, y, z));
                    chunk.Blocks[x, y, z].Position = chunk.Position + new Vector3(x, y, z);
                }
            }
        }
    }

    public static Block GetVirtualBlock(Vector3 position) {
        Block block = new Block();
        if (position.y >= World.Current.ChunkHeight) {
            block = new Block(0, 255);
            block.Position = position;
            return block;
        }
        if (position.y < 0) {
            block = new Block(1, 0);
            block.Position = position;
            return block;
        }
        float noise = 16f * (float)Noise.Generate((position.x + World.Current.Offset.x) * 0.005f, (position.z + World.Current.Offset.z) * 0.005f);
        noise *= (float)Noise.Generate((position.x + World.Current.Offset.x) * 0.01f, (position.z + World.Current.Offset.z) * 0.01f);
        float density = Noise.Generate((position.x + World.Current.Offset.x) * 0.05f, (position.y + World.Current.Offset.y) * 0.05f, (position.z + World.Current.Offset.z) * 0.05f);
        if (position.y <= 64f + noise && density > -0.3) {
            block = new Block(2, 0);
            block.Position = position;
            return block;
        } else if (position.y > 3 && position.y <= 80f && density <= -0.3) {
            block = new Block(1, 0);
            block.Position = position;
            return block;
        } else if (position.y < 2) {
            block = new Block(2, 0);
            block.Position = position;
            return block;
        } else if (position.y <= 64f + noise) {
            block = new Block(1, 0);
            block.Position = position;
            return block;
        } else {
            block = new Block(0, 255);
            block.Position = position;
            return block;
        }
        //return new Block(2);

    }

    static float mult2 = 1f / 6f;
    static float Scale = .01f;
    //public static float GetHeight(float x, float z) {
    //    Biome biome = World.Current.Biomes[0];

    //    float scale = 0;
    //    float n = 0f;
    //    float totalScale = 0;
    //    int j = 0;
    //    for (int i = 1; i < 4; i *= 2) {
    //        scale = biome.Bias.Evaluate(mult2 * j);
    //        totalScale += scale;

    //        n += scale * (float)moduleBase.GetValue((x + World.Current.Offset.x) * Scale, 0, (z + World.Current.Offset.z) * Scale);
    //        //n += scale * (float)SimpleNoise.noise((x + World.Current.Offset.x) * Scale * i * biome.XZScale, (z + World.Current.Offset.z) * Scale * i * biome.XZScale);
    //        //n += scale * Noise.Generate(x * Scale * i * biome.XZScale + World.Current.Offset.x, z * Scale * i * biome.XZScale + World.Current.Offset.y);
    //        //n += scale * Mathf.PerlinNoise((x + World.Current.Offset.x) * Scale * i * biome.XZScale, (z + World.Current.Offset.z) * Scale * i * biome.XZScale);
    //        j++;
    //    }
    //    return n / totalScale;
    //}

}

