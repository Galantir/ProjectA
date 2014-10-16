using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class SandBlock : IBlockType {
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

