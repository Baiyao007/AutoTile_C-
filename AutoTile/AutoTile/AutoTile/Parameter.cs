// 作成日：2016.12.13
// 作成者：柏
// 作成内容：常数
// 最後修正：2016.12.13

using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace AutoTile
{

    enum eTile
    {
        LEFT_TOP,
        RIGHT_TOP,
        LEFT_BOTTOM,
        RIGHT_BOTTOM,
    }

    enum eGirth
    {
        L_TOP = 0,  TOP,    R_TOP,
        LEFT,       CENTER, RIGHT,
        L_BOTTOM,   BOTTOM, R_BOTTOM,
    }

    enum eImageType {
        LAND,
        CROSS,
        SALTIRE,
        TOP_BOTTOM,
        LEFT_RIGHT,
        NONE,
    }

    static class Parameter
    {
        public const int MapWidth  = 15;
        public const int MapHeight = 10;
        public const int TileSize  = 64;
        public const int TileResouceWidth = 4;
        public const int TileResouceHeight = 6;


        public const int ScreenWidth = MapWidth * TileSize;
        public const int ScreenHeight = MapHeight * TileSize;

        public static readonly List<Vector2> TileOffset = new List<Vector2> {
            new Vector2 (0.0f, 0.0f), //TopLeft
            new Vector2 (0.5f, 0.0f), //TopRight
            new Vector2 (0.0f, 0.5f), //BottomLeft
            new Vector2 (0.5f, 0.5f), //BottomRight
        };

    }



}
