using UnityEngine;
using System.Collections;
//interface om blokjes op magische wijze van functies te voorzien
public interface IBlockType {
    bool IsSolid(Block block);   
    int GetUVRow(Block block);   
}
