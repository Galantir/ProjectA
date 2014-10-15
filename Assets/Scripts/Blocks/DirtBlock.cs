using UnityEngine;
using System.Collections;

public class DirtBlock : IBlockType {

    public bool IsSolid(Block block) {
        return true;
    }
    
    public int GetUVRow(Block block) {
        return 0;
    }
  
}
