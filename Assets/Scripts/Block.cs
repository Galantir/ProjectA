using UnityEngine;
using System.Collections;
using System;
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

