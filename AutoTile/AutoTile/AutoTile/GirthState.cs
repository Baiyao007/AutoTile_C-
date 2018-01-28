// 作成日：2016.12.13
// 作成者：柏
// 作成内容：タイルの周り9マス情報保存
// 最後修正：2016.12.13

namespace AutoTile
{
    class GirthState
    {
        bool[] girth;

        public GirthState() {
            InitializeGirthState(
                false, false, false,
                false, false, false,
                false, false, false
                );
        }

        public void InitializeGirthState(
            bool L_Top,    bool Top,    bool R_Top,
            bool Left,     bool Center, bool Right,
            bool L_Bottom, bool Bottom, bool R_Bottom
            )
        {
            girth = new bool[9] {
                L_Top   ,Top    ,R_Top,
                Left    ,Center ,Right,
                L_Bottom,Bottom ,R_Bottom,
            };
        }


        public void SetGirth(eGirth e , bool state) {
            girth[(int)e] = state;
        }

        public bool GetTargetState(eGirth e) {
            return girth[(int)e];
        }

        public bool IsTile() {
            return girth[(int)eGirth.CENTER];
        }
    }
}
