using UnityEngine;
using System.Collections;

public static class BlockTypes {

    private static IBlockType[] types = new IBlockType[] { new AirBlock(), new BedrockBlock(), new RockBlock(), new DirtBlock(), new GravelBlock(), new SandBlock(), new GrassBlock(), new SnowBlock(), new ObsidianBlock(), new WaterBlock() };

    public static IBlockType GetBlockType(int type) {
        return types[type];
    }

}
