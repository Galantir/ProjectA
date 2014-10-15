using UnityEngine;
using System.Collections;

public class AirBlock : IBlockType {


    public bool IsSolid(Block block) {
        return false;
    }    

    public int GetUVRow(Block block) {
        return 0;
    }
    
}
