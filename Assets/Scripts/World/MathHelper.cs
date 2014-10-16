using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public static class MathHelper  {

    public static int ToFlatIndex(this Vector3 index) {
        return (int)(index.x + index.y * World.Current.ChunkWidth + index.z * World.Current.ChunkWidth * World.Current.ChunkHeight);
    }

    public static Vector3 To3DIndex(this int index) {
        return new Vector3(
            index % World.Current.ChunkWidth,
            index / World.Current.ChunkWidth % World.Current.ChunkHeight,
            index / (World.Current.ChunkWidth * World.Current.ChunkHeight) % World.Current.ChunkWidth
        );
    }

    public static float Distance(this Vector3 a, Vector3 b) {
        float deltaX = (float)b.x - (float)a.x;
        float deltaY = (float)b.y - (float)a.y;
        float deltaZ = (float)b.z - (float)a.z;
        return (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);
    }

    public static float Distance(this Vector3 f, Vector3 a, Vector3 b) {
        float deltaX = (float)b.x - (float)a.x;
        float deltaY = (float)b.y - (float)a.y;
        float deltaZ = (float)b.z - (float)a.z;
        return (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);
    }

    public static readonly List<Vector3> _direction = new List<Vector3>(){
		Vector3.left,
		Vector3.right,
		Vector3.up,
		Vector3.down,
		Vector3.forward,
		Vector3.back,
		
		//Corner
		new Vector3(-1,1,1),//LeftTopFront
		new Vector3(1,-1,-1),//LeftBottomFront

		new Vector3(-1,-1,1),//LeftBottomFront
		new Vector3(1,1,-1),//RightTopBack

		new Vector3(-1,1,-1),//LeftTopBack
		new Vector3(1,-1,1),//RightBottomFront

		new Vector3(-1,-1,-1),//LeftBottomBack
		new Vector3(1,1,1),//RightTopFront

		//Edge Corner
		new Vector3(-1,0,1), //Left Front
		new Vector3(1,0,-1), //Right Back
		new Vector3(-1,0,-1), //Left Back
		new Vector3(1,0,1), //Right Front

		new Vector3(-1,1,0), //Left Top
		new Vector3(1,-1,0), //Right Bottom
		new Vector3(-1,-1,0), //Left Bottom
		new Vector3(1,1,0), //Right Top

		new Vector3(0,1,1), //Top Front
		new Vector3(0,-1,-1), //Bottom Back
		new Vector3(0,1,-1), //Top Back
		new Vector3(0,-1,1), //Bottom Front
	};
}
