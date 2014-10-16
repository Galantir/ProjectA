using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class WaterBlock : IBlockType {
    public bool IsSolid() {
        return false;
    }
   
    public int GetUVRow() {
        return 1;
    }

    public bool IsTransparent() {
        return false; ;
    }
}

