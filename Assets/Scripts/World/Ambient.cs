using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ambient  {

    static Color GetAmbientLightLeft(int index, bool[] neighbors) {

        switch (index) {
            case 0:
                if (
                    neighbors[14] ||
                    neighbors[18] ||
                    neighbors[6]
                    ) { return ambient; }

                break;
            case 1:
                if (
                    neighbors[16] ||
                    neighbors[18] ||
                    neighbors[10]
                    ) { return ambient; }

                break;
            case 2:
                if (
                    neighbors[14] ||
                    neighbors[20] ||
                    neighbors[8]
                    ) { return ambient; }

                break;
            case 3:
                if (
                    neighbors[16] ||
                    neighbors[20] ||
                    neighbors[12]
                    ) { return ambient; }

                break;
        }

        return nonAmbient;

    }
    static Color GetAmbientLightRight(int index, bool[] neighbors) {
        switch (index) {
            case 0:
                if (
                    neighbors[15] ||
                    neighbors[21] ||
                    neighbors[9]
                    ) { return ambient; } break;
            case 1:
                if (
                    neighbors[17] ||
                    neighbors[21] ||
                    neighbors[13]
                    ) { return ambient; } break;
            case 2:
                if (
                    neighbors[15] ||
                    neighbors[19] ||
                    neighbors[7]
                    ) { return ambient; } break;

            case 3:
                if (
                    neighbors[17] ||
                    neighbors[19] ||
                    neighbors[11]
                    ) { return ambient; } break;

        }

        return nonAmbient;
    }
    static Color GetAmbientLightTop(int index, bool[] neighbors) {
        switch (index) {
            case 0:
                if (
                    neighbors[(int)Direction.LeftTop] ||
                    neighbors[(int)Direction.TopFront] ||
                    neighbors[(int)Direction.LeftTopFront]
                    ) { return ambient; } break;
            case 1:
                if (
                    neighbors[(int)Direction.RightTop] ||
                    neighbors[(int)Direction.TopFront] ||
                    neighbors[(int)Direction.RightTopFront]
                    ) { return ambient; } break;
            case 2:
                if (
                    neighbors[(int)Direction.LeftTop] ||
                    neighbors[(int)Direction.TopBack] ||
                    neighbors[(int)Direction.LeftTopBack]
                    ) { return ambient; } break;
            case 3:
                if (
                    neighbors[(int)Direction.RightTop] ||
                    neighbors[(int)Direction.TopBack] ||
                    neighbors[(int)Direction.RightTopBack]
                    ) { return ambient; } break;
        }

        return nonAmbient;
    }
    static Color GetAmbientLightBottom(int index, bool[] neighbors) {
        switch (index) {
            case 0:
                if (
                    neighbors[(int)Direction.LeftBottom] ||
                    neighbors[(int)Direction.BottomBack] ||
                    neighbors[(int)Direction.LeftBottomBack]
                    ) { return ambient; } break;
            case 1:
                if (
                    neighbors[(int)Direction.RightBottom] ||
                    neighbors[(int)Direction.BottomBack] ||
                    neighbors[(int)Direction.RightBottomBack]
                    ) { return ambient; } break;
            case 2:
                if (
                    neighbors[(int)Direction.LeftBottom] ||
                    neighbors[(int)Direction.BottomFront] ||
                    neighbors[(int)Direction.LeftBottomFront]
                    ) { return ambient; } break;
            case 3:
                if (
                    neighbors[(int)Direction.RightBottom] ||
                    neighbors[(int)Direction.BottomFront] ||
                    neighbors[(int)Direction.RightBottomFront]
                    ) { return ambient; } break;
        }

        return nonAmbient;
    }
    static Color GetAmbientLightFront(int index, bool[] neighbors) {
        switch (index) {
            case 0:
                if (
                    neighbors[(int)Direction.RightFront] ||
                    neighbors[(int)Direction.TopFront] ||
                    neighbors[(int)Direction.RightTopFront]
                    ) { return ambient; } break;
            case 1:
                if (
                    neighbors[(int)Direction.LeftFront] ||
                    neighbors[(int)Direction.TopFront] ||
                    neighbors[(int)Direction.LeftTopFront]
                    ) { return ambient; } break;
            case 2:
                if (
                    neighbors[(int)Direction.RightFront] ||
                    neighbors[(int)Direction.BottomFront] ||
                    neighbors[(int)Direction.RightBottomFront]
                    ) { return ambient; } break;
            case 3:
                if (
                    neighbors[(int)Direction.LeftFront] ||
                    neighbors[(int)Direction.BottomFront] ||
                    neighbors[(int)Direction.LeftBottomFront]
                    ) { return ambient; } break;
        }

        return nonAmbient;
    }
    static Color GetAmbientLightBack(int index, bool[] neighbors) {
        switch (index) {
            case 0:
                if (
                    neighbors[(int)Direction.LeftBack] ||
                    neighbors[(int)Direction.TopBack] ||
                    neighbors[(int)Direction.LeftTopBack]
                    ) { return ambient; } break;
            case 1:
                if (
                    neighbors[(int)Direction.RightBack] ||
                    neighbors[(int)Direction.TopBack] ||
                    neighbors[(int)Direction.RightTopBack]
                    ) { return ambient; } break;
            case 2:
                if (
                    neighbors[(int)Direction.LeftBack] ||
                    neighbors[(int)Direction.BottomBack] ||
                    neighbors[(int)Direction.LeftBottomBack]
                    ) { return ambient; } break;
            case 3:
                if (
                    neighbors[(int)Direction.RightBack] ||
                    neighbors[(int)Direction.BottomBack] ||
                    neighbors[(int)Direction.RightBottomBack]
                    ) { return ambient; } break;
        }

        return nonAmbient;
    }
    static Color ambient = new Color(0.4f, 0.4f, 0.4f);
    static Color nonAmbient = new Color(1, 1, 1);

    public static List<Color> ChangeLight(int faceIndex, bool[] neighbors) {
        List<Color> colors = new List<Color>();

        for (int i = 0; i < 4; i++) {

            //Light inside edge
            switch (faceIndex) {
                case (int)Direction.Left: colors.Add(GetAmbientLightLeft(i, neighbors)); break;
                case (int)Direction.Right: colors.Add(GetAmbientLightRight(i, neighbors)); break;
                case (int)Direction.Top: colors.Add(GetAmbientLightTop(i, neighbors)); break;
                case (int)Direction.Bottom: colors.Add(GetAmbientLightBottom(i, neighbors)); break;
                case (int)Direction.Front: colors.Add(GetAmbientLightFront(i, neighbors)); break;
                case (int)Direction.Back: colors.Add(GetAmbientLightBack(i, neighbors)); break;
            }

        }

        return colors;
    }
}
