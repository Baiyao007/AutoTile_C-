// 作成日：2016.12.13
// 作成者：柏
// 作成内容：タイルの分割情報保存
// 最後修正：2016.12.13

using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace AutoTile
{
    class Tile
    {
        List<Rectangle> tileData;
        public Tile() {
            tileData = new List<Rectangle>();
            //タイルを4等分
            for (int i = 0; i < 4; i++)
            {
                tileData.Add(new Rectangle());
            }
        }

        public void SetTile(eTile e,Rectangle rect) {
            tileData[(int)e] = rect;
        }

        public Tile GetTile() {
            return this;
        }

        public Rectangle GetTile(eTile e) {
            return tileData[(int)e];
        }

    }
}
