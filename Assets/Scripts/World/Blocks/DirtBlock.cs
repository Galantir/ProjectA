using UnityEngine;
using System.Collections;

public class DirtBlock : IBlockType {

    public bool IsSolid() {
        return true;
    }
    
    public int GetUVRow() {
        return 0;
    }

    public bool IsTransparent() {
        return false; ;
    }
}
