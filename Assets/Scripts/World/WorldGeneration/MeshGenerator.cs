using UnityEngine;
using System.Collections;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System;

public class MeshGenerator {

    public static TList<ChunkData> MeshList = new TList<ChunkData>();
    public static bool BeginProcessing;
    public static Thread thread;

    public static void BeginMeshGenerator() {
        thread = new Thread(() => {
            while (true) {
                if (BeginProcessing) {
                    BeginProcessing = false;
                    // Mesh generation only happens once terrain generation is complete.);
                    foreach (ChunkData chunk in MeshList) {
                        GenerateChunkMeshData(chunk);
                        MeshList.Remove(chunk);
                        ChunkProcessor.FinishedQueue.Enqueue(chunk);
                    }
                    // We've processed them all, clear them out now.
                    MeshList.Clear();
                }
                Thread.Sleep(10);
            }
        });
        thread.Start();
    }

    public static void StopMeshGenerator() {
        thread.Abort();
    }

    public static void GenerateChunkMeshData(ChunkData chunk) {
        chunk.verts.Clear();
        chunk.uvs.Clear();
        chunk.tris.Clear();
        chunk.colors.Clear();

        SunlightPass(chunk);

        for (int x = 0; x < chunk.Width; x++) {
            for (int y = 0; y < chunk.Height; y++) {
                for (int z = 0; z < chunk.Width; z++) {
                    Block block = chunk.Blocks[x, y, z];
                    if (block.Type == 0 || block.Type == 1) continue;
                    //if(IsBlocked(chunk.Position + new Vector3(x,y,z))) continue;
                    List<bool> neighbors = GetNeighboursBool(chunk.Position + new Vector3(x, y, z)).ToList();
                    Vector3 pos = new Vector3(x, y, z);

                    //left
                    if (IsTransparent(chunk, chunk.Position + pos + Vector3.left))
                        BuildFace(Direction.Left, block, neighbors, BlockTypes.GetBlockType(block.Type).uvLeft(block), pos, Vector3.up, Vector3.forward, false, chunk.verts, chunk.uvs, chunk.tris, chunk.colors);
                    //right
                    if (IsTransparent(chunk, chunk.Position + pos + Vector3.right))
                        BuildFace(Direction.Right, block, neighbors, BlockTypes.GetBlockType(block.Type).uvRight(block), pos + Vector3.right, Vector3.up, Vector3.forward, true, chunk.verts, chunk.uvs, chunk.tris, chunk.colors);

                    //bottom
                    if (IsTransparent(chunk, chunk.Position + pos + Vector3.down))
                        BuildFace(Direction.Down, block, neighbors, BlockTypes.GetBlockType(block.Type).uvDown(block), pos, Vector3.forward, Vector3.right, false, chunk.verts, chunk.uvs, chunk.tris, chunk.colors);
                    //top
                    if (IsTransparent(chunk, chunk.Position + pos + Vector3.up))
                        BuildFace(Direction.Up, block, neighbors, BlockTypes.GetBlockType(block.Type).uvUp(block), pos + Vector3.up, Vector3.forward, Vector3.right, true, chunk.verts, chunk.uvs, chunk.tris, chunk.colors);

                    //back
                    if (IsTransparent(chunk, chunk.Position + pos + Vector3.back))
                        BuildFace(Direction.Back, block, neighbors, BlockTypes.GetBlockType(block.Type).uvBack(block), pos, Vector3.up, Vector3.right, true, chunk.verts, chunk.uvs, chunk.tris, chunk.colors);
                    //front
                    if (IsTransparent(chunk, chunk.Position + pos + Vector3.forward))
                        BuildFace(Direction.Front, block, neighbors, BlockTypes.GetBlockType(block.Type).uvForward(block), pos + Vector3.forward, Vector3.up, Vector3.right, false, chunk.verts, chunk.uvs, chunk.tris, chunk.colors);
                }
            }
        }

    }


    public static void BuildFace(Direction dir, Block block, List<bool> neighbors, Vector2 uvStart, Vector3 corner, Vector3 up, Vector3 right, bool reversed, List<Vector3> verts, List<Vector2> uvs, List<int> tris, List<Color> colors) {

        int index = verts.Count;

        verts.Add(corner);
        verts.Add(corner + up);
        verts.Add(corner + up + right);
        verts.Add(corner + right);

        //Vector2 uvWidth = new Vector2(0.125f, 0.125f);
        //Vector2 uvCorner = new Vector2(0, 0.875f);

        //uvCorner.x += ((float)block.GetBlockType() - 1f) / 8;

        //uvs.Add(uvCorner);
        //uvs.Add(new Vector2(uvCorner.x, uvCorner.y + uvWidth.y));
        //uvs.Add(new Vector2(uvCorner.x + uvWidth.x, uvCorner.y + uvWidth.y));
        //uvs.Add(new Vector2(uvCorner.x + uvWidth.x, uvCorner.y));

        //uvs.Add(new Vector2(0, 0));
        //uvs.Add(new Vector2(0, 0.25f));
        //uvs.Add(new Vector2(0.25f, 0.25f));
        //uvs.Add(new Vector2(0.25f, 0));       
        uvs.Add(uvStart);
        uvs.Add(uvStart + new Vector2(0, 0.03125f));
        uvs.Add(uvStart + new Vector2(0.03125f, 0.03125f));
        uvs.Add(uvStart + new Vector2(0.03125f, 0));

        //uvs.Add(new Vector2(0, 0));
        //uvs.Add(new Vector2(0, 1));
        //uvs.Add(new Vector2(1, 1));
        //uvs.Add(new Vector2(1, 0));


        List<Color> clrs = Ambient.ChangeLight((int)dir, neighbors.ToArray());


        Color c = new Color((1f / 255f) * block.Light, (1f / 255f) * block.Light, (1f / 255f) * block.Light, 255);

        //colors.Add(c);
        //colors.Add(c);
        //colors.Add(c);
        //colors.Add(c);


        if (reversed) {
            if (clrs[0].r + clrs[3].r < clrs[1].r + clrs[2].r) {
                tris.Add(index + 0);
                tris.Add(index + 1);
                tris.Add(index + 2);
                tris.Add(index + 2);
                tris.Add(index + 3);
                tris.Add(index + 0);
            } else {
                tris.Add(index + 0);
                tris.Add(index + 1);
                tris.Add(index + 3);
                tris.Add(index + 2);
                tris.Add(index + 3);
                tris.Add(index + 1);
            }

            colors.Add(c * clrs[2]);
            colors.Add(c * clrs[0]);
            colors.Add(c * clrs[1]);
            colors.Add(c * clrs[3]);
        } else {
            if (clrs[0].r + clrs[3].r < clrs[1].r + clrs[2].r) {
                tris.Add(index + 1);
                tris.Add(index + 0);
                tris.Add(index + 2);
                tris.Add(index + 3);
                tris.Add(index + 2);
                tris.Add(index + 0);
            } else {
                tris.Add(index + 1);
                tris.Add(index + 0);
                tris.Add(index + 3);
                tris.Add(index + 3);
                tris.Add(index + 2);
                tris.Add(index + 1);
            }

            colors.Add(c * clrs[3]);
            colors.Add(c * clrs[1]);
            colors.Add(c * clrs[0]);
            colors.Add(c * clrs[2]);
        }
    }

    public static bool IsBlocked(Vector3 position) {
        List<Block> blocks = new List<Block>();
        Block b = TerrainGenerator.GetVirtualBlock(position + Vector3.left);
        if (!b.IsTransparent())
            blocks.Add(b);
        b = TerrainGenerator.GetVirtualBlock(position + Vector3.right);
        if (!b.IsTransparent())
            blocks.Add(b);
        b = TerrainGenerator.GetVirtualBlock(position + Vector3.forward);
        if (!b.IsTransparent())
            blocks.Add(b);
        b = TerrainGenerator.GetVirtualBlock(position + Vector3.back);
        if (!b.IsTransparent())
            blocks.Add(b);
        b = TerrainGenerator.GetVirtualBlock(position + Vector3.up);
        if (!b.IsTransparent())
            blocks.Add(b);
        b = TerrainGenerator.GetVirtualBlock(position + Vector3.down);
        if (!b.IsTransparent())
            blocks.Add(b);
        if (blocks.Count == 6)
            return true;
        return false;
    }

    public static IEnumerable<Block> GetNeighbours(ChunkData chunk, Vector3 position) {
        for (int i = 0; i < MathHelper._direction.Count(); i++) {
            Vector3 worldpos = position + MathHelper._direction[i];
            yield return GetBlock(chunk, worldpos);
        }
    }

    public static IEnumerable<bool> GetNeighboursBool(Vector3 position) {
        for (int i = 0; i < MathHelper._direction.Count(); i++) {
            Vector3 worldpos = position + MathHelper._direction[i];
            yield return !TerrainGenerator.GetVirtualBlock(worldpos).IsTransparent();
        }
    }

    public static bool IsTransparent(ChunkData chunk, Vector3 pos) {
        Block block = GetBlock(chunk, pos);
        return block.IsTransparent();
    }

    public static Block GetBlock(ChunkData chunk, int x, int y, int z) {
        Vector3 worldPos = new Vector3(x, y, z) + chunk.Position;

        if ((y < 0) || y >= 128)
            return new Block(0, 1);

        if ((x < 0) || (z < 0) || (y < 0) || (x >= World.Current.ChunkWidth) || (z >= World.Current.ChunkWidth) || y >= World.Current.ChunkHeight) {
            ChunkData c = World.FindChunk(worldPos);
            if (c == chunk) return chunk.Blocks[x, y, z];
            if (c == null) {
                return TerrainGenerator.GetVirtualBlock(worldPos);
            }
            return c.Blocks[x, y, z];
            //return TerrainGenerator.GetVirtualBlock(worldPos);
            //return new Block(0);
        }
        return chunk.Blocks[x, y, z];
    }

    public static Block GetBlock(ChunkData chunk, Vector3 worldpos) {
        worldpos -= chunk.Position;
        int x = Mathf.FloorToInt(worldpos.x);
        int y = Mathf.FloorToInt(worldpos.y);
        int z = Mathf.FloorToInt(worldpos.z);
        return GetBlock(chunk, x, y, z);
    }

    public static IEnumerable<Block> NoSunlightBlocks(ChunkData chunk) {
        for (int x = 0; x < chunk.Width; x++) {
            for (int y = 0; y < chunk.Height; x++) {
                for (int z = 0; z < chunk.Width; z++) {
                    if (chunk.Blocks[x, y, z].Type != 0)
                        yield return chunk.Blocks[x, y, z];
                }
            }
        }
    }

    public static void SunlightPass(ChunkData chunk) {
        //List<Block> blocks = MeshGenerator.NoSunlightBlocks(chunk).ToList();
        //for (int i = 0; i < blocks.Count; i++) {
        //    Block a = blocks[i];
        //    byte final = 0;
        //    final = GetBlock(chunk, a.Position + Vector3.left + Vector3.up).Light;
        //    final = (byte)Mathf.Lerp(GetBlock(chunk, a.Position + Vector3.back + Vector3.up).Light, final, 0.5f);
        //    final = (byte)Mathf.Lerp(GetBlock(chunk, a.Position + Vector3.forward + Vector3.up).Light, final, 0.5f);
        //    final = (byte)Mathf.Lerp(GetBlock(chunk, a.Position + Vector3.right + Vector3.up).Light, final, 0.5f);
        //    final = (byte)Mathf.Lerp(GetBlock(chunk, a.Position + Vector3.up).Light, final, 0.5f);

        //    Block b = GetBlock(chunk, a.Position + Vector3.up);
        //    if (b.Light > a.Light) {
        //        byte l = (byte)Mathf.Lerp(b.Light, final, 0.5f);
        //        a.Light = (byte)Mathf.Lerp(b.Light, l, BlockTypes.GetBlockType(a.Type).LightPenetration);
        //        if (a.Light < 15) {
        //            a.Light = 15;
        //        }
        //    }
        //}
        for (int x = 0; x < chunk.Width; x++) {
            for (int y = chunk.Height - 1; y >= 0; y--) {
                for (int z = 0; z < chunk.Width; z++) {
                    Block a = chunk.Blocks[x, y, z];

                    if (a.Type == 0) continue;
                    byte final = 0;
                    Vector3 pos = new Vector3(x - 1, y + 1, z);
                    final = GetBlock(chunk, pos + chunk.Position).Light;
                    pos = new Vector3(x, y + 1, z - 1);
                    final = (byte)Mathf.Lerp(GetBlock(chunk, pos + chunk.Position).Light, final, 0.5f);
                    pos = new Vector3(x, y + 1, z + 1);
                    final = (byte)Mathf.Lerp(GetBlock(chunk, pos + chunk.Position).Light, final, 0.5f);
                    pos = new Vector3(x + 1, y + 1, z);
                    final = (byte)Mathf.Lerp(GetBlock(chunk, pos + chunk.Position).Light, final, 0.5f);
                    pos = new Vector3(x, y + 1, z);
                    final = (byte)Mathf.Lerp(GetBlock(chunk, pos + chunk.Position).Light, final, 0.5f);
                    //byte finalLight = Convert.ToByte(final / 5);
                    //if (final > a.Light) {
                    //    chunk.Blocks[x, y, z].Light = final;//(byte)Mathf.Lerp(final, a.Light, BlockTypes.GetBlockType(a.Type).LightPenetration);
                    //}   

                    Vector3 localpos = new Vector3(x, y + 1, z);
                    Block b = GetBlock(chunk, localpos + chunk.Position);

                    if (b.Light > a.Light) {
                        byte l = (byte)Mathf.Lerp(b.Light, final, 0.5f);
                        chunk.Blocks[x, y, z].Light = (byte)Mathf.Lerp(b.Light, l, BlockTypes.GetBlockType(a.Type).LightPenetration);
                        if (chunk.Blocks[x, y, z].Light < 15) {
                            chunk.Blocks[x, y, z].Light = 15;
                        }
                    }


                }
            }
        }
    }

}

