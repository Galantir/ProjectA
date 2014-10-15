using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class WaterBlock : IBlockType {
    public bool IsSolid(Block block) {
        return false;
    }
   
    public int GetUVRow(Block block) {
        return 1;
    }  
}

