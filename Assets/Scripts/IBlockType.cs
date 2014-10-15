using UnityEngine;
using System.Collections;

public interface IBlockType {
    bool IsSolid(Block block);   
    int GetUVRow(Block block);   
}
