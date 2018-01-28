// 作成日：2016.12.13
// 作成者：柏
// 作成内容：AutoTile
// 最後修正：2016.12.22
// 修正内容：SourceMonitorによる改善

using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AutoTile
{
    class AutoTiling
    {
        //タイル4等分により
        private const int size = Parameter.TileSize / 2;
        private const int width = Parameter.TileResouceWidth * 2;

        private List<Rectangle> tiles;  //タイルリソース分割保存
        private int[,] mapData;
        private Tile[,] tilesData;  //mapDataによるタイルの情報を保存
        private Input input;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AutoTiling() {
            tiles = new List<Rectangle>();
            input = new Input();
        }

        /// <summary>
        /// マップ全体をタイルなしで初期化
        /// </summary>
        private void MapDataInitialize() {
            mapData = new int[Parameter.MapHeight, Parameter.MapWidth];
            for (int y = 0; y < Parameter.MapHeight; y++) {
                for (int x = 0; x < Parameter.MapWidth; x++) {
                    mapData[y, x] = -1;
                }
            }
        }

        /// <summary>
        /// マップサイズに合わせて、タイルを生成
        /// </summary>
        private void TilesDataInitialize() {
            tilesData = new Tile[Parameter.MapHeight, Parameter.MapWidth];
            for (int y = 0; y < Parameter.MapHeight; y++)　{
                for (int x = 0; x < Parameter.MapWidth; x++) {
                    tilesData[y, x] = new Tile();
                }
            }
        }

        /// <summary>
        /// ソースの画像を分割
        /// </summary>
        private void TileResouceCut() {
            //一枚のタイルを4等分
            int height = Parameter.TileResouceHeight * 2;
            for (int i = 0; i < height; i++) {  
                for (int j = 0; j < width; j++) {
                    tiles.Add(new Rectangle(j * size, i * size, size, size));
                }
            }
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize() {
            MapDataInitialize();
            TilesDataInitialize();
            TileResouceCut();
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update() {
            input.Update();
            WriteGirth();
            SetMapData();
        }

        /// <summary>
        /// クリック操作によってタイルを描画
        /// </summary>
        private void WriteGirth() {
            int rl; //0:左  1:右
            if (input.IsMousePressed_L()) {
                rl = 0;
                click(rl);
            }
            else if (input.IsMousePressed_R()) {
                rl = 1;
                click(rl);
            }
        }

        /// <summary>
        /// クリック処理
        /// </summary>
        /// <param name="rl">左右（0:左  1:右）</param>
        private void click(int rl) {
            Vector2 mousePosition = input.GetMousePosition;
            Point mapPosition = MapPosition(mousePosition);
            int mapX = mapPosition.X;
            int mapY = mapPosition.Y;
            if (!IsInGameMap(mapX, mapY)) { return; }
            switch (rl) {
                case 0:
                    mapData[mapY, mapX] = mapData[mapY, mapX] == 0 ? -1 : 0;
                    break;
                case 1:
                    mapData[mapY, mapX] = mapData[mapY, mapX] == 4 ? -1 : 4;
                    break;
            }
        }

        /// <summary>
        /// マップに計算済みのタイルを設置
        /// </summary>
        private void SetMapData() {
            for (int y = 0; y < Parameter.MapHeight; y++) {
                for (int x = 0; x < Parameter.MapWidth; x++) {
                    GirthState gs = GetGirthState(x, y);
                    SetTile(gs, x, y);
                }
            }
        }

        /// <summary>
        /// タイルの情報をチェックし、設定
        /// </summary>
        /// <param name="gs">周りマスの情報</param>
        /// <param name="x">マップチップのＸ座標</param>
        /// <param name="y">マップチップのＹ座標</param>
        private void SetTile(GirthState gs, int x, int y)
        {
            eImageType l_top = CheckTileType(gs, eGirth.TOP, eGirth.LEFT);
            eImageType r_top = CheckTileType(gs, eGirth.TOP, eGirth.RIGHT);
            eImageType l_bottom = CheckTileType(gs, eGirth.BOTTOM, eGirth.LEFT);
            eImageType r_bottom = CheckTileType(gs, eGirth.BOTTOM, eGirth.RIGHT);

            tilesData[y,x].SetTile(eTile.LEFT_TOP, GetImageRectangle(eTile.LEFT_TOP, l_top, mapData[y, x]));
            tilesData[y,x].SetTile(eTile.RIGHT_TOP, GetImageRectangle(eTile.RIGHT_TOP, r_top, mapData[y, x]));
            tilesData[y,x].SetTile(eTile.LEFT_BOTTOM, GetImageRectangle(eTile.LEFT_BOTTOM, l_bottom, mapData[y, x]));
            tilesData[y,x].SetTile(eTile.RIGHT_BOTTOM, GetImageRectangle(eTile.RIGHT_BOTTOM, r_bottom, mapData[y, x]));
        }

        /// <summary>
        /// タイルの分割部分の対応画像を取得
        /// </summary>
        /// <param name="eT">タイルの分割部分</param>
        /// <param name="eI">タイルの地形種類</param>
        /// <param name="type">ソースの種類</param>
        /// <returns></returns>
        private Rectangle GetImageRectangle(eTile eT, eImageType eI, int type)
        {
            if (type < 0) { return new Rectangle(); }
            int offset;
            offset = (int)eT < 2 ? (int)eT : (int)eT + width - 2;
            return tiles[width * 2 * (int)eI + offset + type];
        }

        /// <summary>
        /// タイルの地形チェック
        /// </summary>
        /// <param name="gs">周囲マスの情報</param>
        /// <param name="eVertical">垂直ターゲットのマス</param>
        /// <param name="eHorizontal">水平ターゲットのマス</param>
        /// <returns>地形種類</returns>
        private eImageType CheckTileType(GirthState gs, eGirth eVertical, eGirth eHorizontal)
        {
            if (!gs.IsTile()) { return eImageType.NONE; }

            eImageType type = eImageType.LAND;  //「周囲タイルあり」で初期化
            eGirth eDiagonal = (eGirth)((int)eVertical + (int)eHorizontal - 4);     //斜め位置の番号を計算

            //垂直方向チェック
            if (!gs.GetTargetState(eVertical)) {
                type = eImageType.TOP_BOTTOM;
            }
            //水平方向チェック
            if (!gs.GetTargetState(eHorizontal)) {
                type = (type == eImageType.LAND) ? eImageType.LEFT_RIGHT : eImageType.CROSS;
            }
            //斜め方向チェック
            if (!gs.GetTargetState(eDiagonal)) {
                type = (type == eImageType.LAND) ? eImageType.SALTIRE : type;
            }
            return type;
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="sprite">ＸＮＡ描画用</param>
        /// <param name="mapChip">ソース画像</param>
        public void Draw(SpriteBatch sprite, Texture2D mapChip) {
            for (int y = 0; y < mapData.GetLength(0); y++) {
                for (int x = 0; x < mapData.GetLength(1); x++) {
                    DrawTile(sprite, mapChip, x, y);
                }
            }
        }

        /// <summary>
        /// タイルずつ描画
        /// </summary>
        /// <param name="sprite">ＸＮＡ描画用</param>
        /// <param name="mapChip">ソース画像</param>
        /// <param name="x">マップチップのＸ座標</param>
        /// <param name="y">マップチップのＹ座標</param>
        public void DrawTile(SpriteBatch sprite, Texture2D mapChip, int x, int y) {
            if (mapData[y, x] < 0) { return; }

            for (int i = 0; i < 4; i++) {
                float tx = (x + Parameter.TileOffset[i].X) * Parameter.TileSize;
                float ty = (y + Parameter.TileOffset[i].Y) * Parameter.TileSize;
                sprite.Draw(mapChip, new Vector2(tx, ty), tilesData[y, x].GetTile((eTile)i), Color.White);
            }
        }

        /// <summary>
        /// マップ座標からマップチップの座標を取得
        /// </summary>
        /// <param name="position">マップ座標</param>
        /// <returns></returns>
        public Point MapPosition(Vector2 position) {
            int x = (int)position.X / Parameter.TileSize;
            int y = (int)position.Y / Parameter.TileSize;
            return new Point(x, y);
        }

        /// <summary>
        /// ウィンドウズ内かどうか
        /// </summary>
        /// <param name="x">Ｘ座標</param>
        /// <param name="y">Ｙ座標</param>
        /// <returns></returns>
        private bool IsInGameMap(int x, int y) {
            return x >= 0 && x < mapData.GetLength(1) &&
                    y >= 0 && y < mapData.GetLength(0);
        }

        /// <summary>
        /// 周り9マスのマップ情報をとる
        /// </summary>
        /// <param name="x">マップチップのＸ座標</param>
        /// <param name="y">マップチップのＹ座標</param>
        /// <returns></returns>
        private GirthState GetGirthState(int x, int y) {
            GirthState result = new GirthState();

            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    int targetX = x + j - 1;
                    int targetY = y + i - 1;
                    int girthNum = i * 3 + j;
                    bool targetState = IsInGameMap(targetX, targetY) ? (mapData[targetY, targetX] == mapData[y, x]) : false; 
                    result.SetGirth((eGirth)girthNum, targetState);
                }
            }
            return result;
        }

    }
}
