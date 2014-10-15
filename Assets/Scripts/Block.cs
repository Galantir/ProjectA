using UnityEngine;
using System.Collections;
using System;

//serializable zodat de editor deze class snapt
//verder simpelweg een struct met daarin gegevens over het blokje
//het is een struct omdat dit minder geheugen gebruikt dan een class
[Serializable]
public struct Block {
    private readonly byte BlockType;
    public readonly Guid Id;   

    public Block(byte type) {
        Id = Guid.NewGuid();
        BlockType = type;
    }

    public byte GetBlockType() {
        return BlockType;
    }

    public bool IsSolid() {
        return BlockTypes.GetBlockType(BlockType).IsSolid(this);
    } 

    public int GetUVRow() {
        return BlockTypes.GetBlockType(BlockType).GetUVRow(this);
    }

}

