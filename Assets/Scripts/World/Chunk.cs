using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//componenten die op het GameObject aanwezig MOETEN zijn
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshFilter))]
public class Chunk : MonoBehaviour {

    //internal zie world class
    internal int Width {
        get {
            return World.Current.ChunkWidth;
        }
    }
    internal int Height {
        get {
            return World.Current.ChunkHeight;
        }
    }

    internal Block[, ,] Blocks;
    internal Mesh VisualMesh;
    internal MeshRenderer MeshRenderer;

    internal MeshCollider meshCollider;
    internal MeshFilter meshFilter;
    internal bool Initialized = false;

    //gebruikt voor initialisatie binnen class
	void Start () {
        this.MeshRenderer = GetComponent<MeshRenderer>();
        this.meshCollider = GetComponent<MeshCollider>();
        this.meshFilter = GetComponent<MeshFilter>();

        World.Current.Chunks.Add(this);
        World.Current.ChunksWaiting.Add(this);

        if (World.Current.ChunksWaiting[0] == this && World.Current.ChunksLoading.Count == 0) {
            World.Current.ChunksLoading.Add(this);
            StartCoroutine(CalculateChunk());
        }
	}

    // Update naam zegt genoeg. LET OP NIET BEDOELD VOOR PHYSICS UPDATES
	void Update () {
	
	}

    //vult de chunk met blokjes door GetVirtualBlock voor iedere positie binnen de chunk aan te roepen
    public virtual IEnumerator CalculateChunk() {
        this.Blocks = new Block[Width, Height, Width];
        for (int x = 0; x < Width; x++) {
            for (int y = 0; y < Height; y++) {
                for (int z = 0; z < Width; z++) {
                    this.Blocks[x, y, z] = GetVirtualBlock(new Vector3(x,y,z));                    
                }
            }            
        }
        
        Initialized = true;
        
        yield return 0;
       
        //dit zorgt er puur voor dat er maar 1 chunk op elk moment laadt
        World.Current.ChunksWaiting.Remove(this);
        World.Current.ChunksLoading.Remove(this);
        if (World.Current.ChunksWaiting.Count > 0 && World.Current.ChunksLoading.Count == 0) {
            World.Current.ChunksLoading.Add(World.Current.ChunksWaiting[0]);
            StartCoroutine(World.Current.ChunksWaiting[0].CalculateChunk());
        }
        StartCoroutine(CreateVisualMesh());
    }

    //hier wordt bepaald welke posite welk blokje krijgt
    //Hier moet het dus allemaal gaan gebeuren en dit bevat nu nog puur testcode
    public Block GetVirtualBlock(Vector3 pos) {
        Vector3 worldPos = pos + transform.position;

        if((worldPos.y < 0))
            return new Block(1);

        if (worldPos.y >= World.Current.Height)
            return new Block(0);

        //if ((worldPos.x < 0 - (World.Current.Width / 2)) || worldPos.x >= 0 + (World.Current.Width / 2)) {
        //    return new Block(0);
        //}

        //if ((worldPos.z < 0 - (World.Current.Width / 2)) || worldPos.z >= 0 + (World.Current.Width / 2)) {
        //    return new Block(0);
        //}
        
        if (worldPos.y < 3)
            return new Block(2);
        //if (Random.Range(0, 100) == 1)
        return new Block(0);
        //else
        //    return new Block(0);
    }

    //opbouw van het 3d model van de chunk
    public virtual IEnumerator CreateVisualMesh() {
        VisualMesh = new Mesh();

        List<Vector3> verts = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> tris = new List<int>();

        for (int x = 0; x < Width; x++) {
            for (int y = 0; y < Height; y++) {
                for (int z = 0; z < Width; z++) {
                    Block block = Blocks[x, y, z];
                    if (block.GetBlockType() == 0) continue;

                    //left
                    if (IsTransparent(x - 1, y, z)) {
                        BuildFace(block, new Vector3(x, y, z), new Vector3(x, y, z) + Vector3.up, new Vector3(x, y, z) + Vector3.up + Vector3.forward, new Vector3(x, y, z) + Vector3.forward, false, verts, uvs, tris);
                    }
                    //right
                    if (IsTransparent(x + 1, y, z))
                        BuildFace(block, new Vector3(x + 1, y, z), new Vector3(x + 1, y, z) + Vector3.up, new Vector3(x + 1, y, z) + Vector3.up + Vector3.forward, new Vector3(x + 1, y, z) + Vector3.forward, true, verts, uvs, tris);

                    //bottom
                    if (IsTransparent(x, y - 1, z))
                        BuildFace(block, new Vector3(x, y, z), new Vector3(x, y, z) + Vector3.forward, new Vector3(x, y, z) + Vector3.forward + Vector3.right, new Vector3(x, y, z) + Vector3.right, false, verts, uvs, tris);
                    //top
                    if (IsTransparent(x, y + 1, z)) {
                        BuildFace(block, new Vector3(x, y + 1, z), new Vector3(x, y + 1, z) + Vector3.forward, new Vector3(x, y + 1, z) + Vector3.forward + Vector3.right, new Vector3(x, y + 1, z) + Vector3.right, true, verts, uvs, tris);
                    }

                    //back
                    if (IsTransparent(x, y, z - 1)) {
                        BuildFace(block, new Vector3(x, y, z), new Vector3(x, y, z) + Vector3.up, new Vector3(x, y, z) + Vector3.up + Vector3.right, new Vector3(x, y, z) + Vector3.right, true, verts, uvs, tris);
                    }

                    //front
                    if (IsTransparent(x, y, z + 1)) {
                        BuildFace(block, new Vector3(x, y, z + 1), new Vector3(x, y, z + 1) + Vector3.up, new Vector3(x, y, z + 1) + Vector3.up + Vector3.right, new Vector3(x, y, z + 1) + Vector3.right, false, verts, uvs, tris);
                    }
                }
            }
            yield return 0;
        }

        VisualMesh.vertices = verts.ToArray();
        VisualMesh.uv = uvs.ToArray();
        VisualMesh.triangles = tris.ToArray();
        VisualMesh.RecalculateBounds();
        VisualMesh.RecalculateNormals();
        TangentSolver.calculateMeshTangents(VisualMesh);

        meshFilter.mesh = VisualMesh;
        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = VisualMesh; 
    }

    //gebruikt bij de opbouw van het 3d model
    public virtual void BuildFace(Block block, Vector3 cornerA, Vector3 cornerB, Vector3 cornerC, Vector3 cornerD, bool reversed, List<Vector3> verts, List<Vector2> uvs, List<int> tris) {
        int index = verts.Count;

        verts.Add(cornerA);
        verts.Add(cornerB);
        verts.Add(cornerC);
        verts.Add(cornerD);

        Vector2 uvWidth = new Vector2(0.125f, 0.125f);
        Vector2 uvCorner = new Vector2(0, 0.875f);

        uvCorner.y -= block.GetUVRow() * 0.125f;
        uvCorner.x += ((float)(block.GetBlockType() - (block.GetUVRow() * 8)) - 1f) / 8;

        uvs.Add(uvCorner);
        uvs.Add(new Vector2(uvCorner.x, uvCorner.y + uvWidth.y));
        uvs.Add(new Vector2(uvCorner.x + uvWidth.x, uvCorner.y + uvWidth.y));
        uvs.Add(new Vector2(uvCorner.x + uvWidth.x, uvCorner.y));

        //uvs.Add(new Vector2(0, 0));
        //uvs.Add(new Vector2(0, 0.25f));
        //uvs.Add(new Vector2(0.25f, 0.25f));
        //uvs.Add(new Vector2(0.25f, 0));

        //uvs.Add(new Vector2(0, 0));
        //uvs.Add(new Vector2(0, 1));
        //uvs.Add(new Vector2(1, 1));
        //uvs.Add(new Vector2(1, 0));


        if (reversed) {
            tris.Add(index + 0);
            tris.Add(index + 1);
            tris.Add(index + 2);
            tris.Add(index + 2);
            tris.Add(index + 3);
            tris.Add(index + 0);
        } else {
            tris.Add(index + 1);
            tris.Add(index + 0);
            tris.Add(index + 2);
            tris.Add(index + 3);
            tris.Add(index + 2);
            tris.Add(index + 0);
        }
    }

    //checkt of het een doorzichtig blokje is
    public virtual bool IsTransparent(int x, int y, int z) {
        Block block = GetBlockLocalPosition(x, y, z);
        //return !block.IsSolid();
        if (block.GetBlockType() == 0)//regel hierboven zou moeten werken maar had er problemen mee
            return true;
        else
            return false;
    }

    //zoek het blokje met de lokale positie binnen de chunk
    public virtual Block GetBlockLocalPosition(int x, int y, int z) {
        Vector3 worldPos = new Vector3(x, y, z) + transform.position;

        if (!Initialized)
            return GetVirtualBlock(worldPos - transform.position);

        if ((x < 0) || (z < 0) || (y < 0)|| (x >= Width) || (z >= Width) || (y >= Height)) {

            Chunk chunk = World.FindChunk(worldPos);
            if (chunk == null) {
                return GetVirtualBlock(worldPos - transform.position);
            }
            return chunk.GetBlockWorldPosition(worldPos);
        }       
        return Blocks[x, y, z];
    }

    //zoek blokje op basis van world positie(zelfs in nog niet bestaande chunks)
    public virtual Block GetBlockWorldPosition(Vector3 worldpos) {
        worldpos -= transform.position;
        int x = Mathf.FloorToInt(worldpos.x);
        int y = Mathf.FloorToInt(worldpos.y);
        int z = Mathf.FloorToInt(worldpos.z);
        return GetBlockLocalPosition(x, y, z);

    }

    public bool SetBlock(Block block, Vector3 worldpos) {
        worldpos -= transform.position;
        return SetBlock(block, Mathf.FloorToInt(worldpos.x), Mathf.FloorToInt(worldpos.y), Mathf.FloorToInt(worldpos.z));
    }

    public bool SetBlockNoUpdateWorldPos(Block block, Vector3 worldpos) {
        worldpos -= transform.position;
        return SetBlockNoUpdateLocal(block, Mathf.FloorToInt(worldpos.x), Mathf.FloorToInt(worldpos.y), Mathf.FloorToInt(worldpos.z));
    }

    public bool SetBlock(Block block, int x, int y, int z) {
        if ((x < 0) || (y < 0) || (z < 0) || (x >= Width) || (y >= Height) || (z >= Width))
            return false;
        Blocks[x, y, z] = block;
        StartCoroutine(CreateVisualMesh());
        if (x == 0) {
            Chunk chunk = World.FindChunk(transform.position + new Vector3(x - 1, y, z));
            if (chunk != null)
                StartCoroutine(chunk.CreateVisualMesh());
        }
        if (x == Width - 1) {
            Chunk chunk = World.FindChunk(transform.position + new Vector3(x + 1, y, z));
            if (chunk != null)
                StartCoroutine(chunk.CreateVisualMesh());
        }
        if (z == 0) {
            Chunk chunk = World.FindChunk(transform.position + new Vector3(x, y, z - 1));
            if (chunk != null)
                StartCoroutine(chunk.CreateVisualMesh());
        }
        if (z == Width - 1) {
            Chunk chunk = World.FindChunk(transform.position + new Vector3(x, y, z + 1));
            if (chunk != null)
                StartCoroutine(chunk.CreateVisualMesh());
        }
        return true;
    }

    public bool SetBlockNoUpdateLocal(Block block, int x, int y, int z) {
        if ((x < 0) || (y < 0) || (z < 0) || (x >= Width) || (y >= Height) || (z >= Width))
            return false;
        if (Blocks[x, y, z].GetBlockType() == block.GetBlockType())
            return false;
        Blocks[x, y, z] = block;
        return true;
    }
}

