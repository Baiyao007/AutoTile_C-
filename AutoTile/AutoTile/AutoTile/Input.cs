using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AutoTile
{
    class Input
    {
        //マウス関連
        private Vector2 mousePosition;
        private MouseState currentMouse;    //今のフレームのマウス状態
        private MouseState PreMouse;        //1フレーム前、マウスの状態

        public Input() { }

        public void Update() {
            MouseState mouse = Mouse.GetState();
            UpdateMouseState(mouse);
            UpdateMousePosition(mouse);
        }

        private void UpdateMouseState(MouseState mouse) {
            PreMouse = currentMouse;
            currentMouse = mouse;
        }

        private void UpdateMousePosition(MouseState mouse) {
            mousePosition = new Vector2(mouse.X, mouse.Y);
        }

        public bool IsMousePressed_L()
        {
            bool pre = (PreMouse.LeftButton == ButtonState.Pressed);
            bool cur = (currentMouse.LeftButton == ButtonState.Pressed);
            return !pre && cur;
        }
        public bool IsMousePressed_R()
        {
            bool pre = (PreMouse.RightButton == ButtonState.Pressed);
            bool cur = (currentMouse.RightButton == ButtonState.Pressed);
            return !pre && cur;
        }

        public Vector2 GetMousePosition {
            get{ return mousePosition; }
        }


    }
}
