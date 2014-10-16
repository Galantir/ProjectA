using UnityEngine;
using System.Collections;
//interface om blokjes op magische wijze van functies te voorzien
public interface IBlockType {
    bool IsSolid();
    bool IsTransparent();
    int GetUVRow();   
}
