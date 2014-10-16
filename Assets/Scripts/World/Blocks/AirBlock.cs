using UnityEngine;
using System.Collections;

public class AirBlock : IBlockType {


    public bool IsSolid() {
        return false;
    }    

    public int GetUVRow() {
        return 0;
    }

    public bool IsTransparent() {
        return true;
    }
}
