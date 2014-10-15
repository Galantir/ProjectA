using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class GravelBlock :IBlockType {
    public bool IsSolid(Block block) {
        return true;
    }
   
    public int GetUVRow(Block block) {
        return 0;
    }
   
}

