using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace chips {
    public static class Hud {
        public static void Update() {

        }
        public static void Draw(SpriteBatch spriteBatch) {
            const int space = 100;
            for (int i = (int)(Camera.ScreenToWorld(Vector2.Zero).X - Camera.ScreenToWorld(Vector2.Zero).X % space); i < Camera.ScreenToWorld(GameRoot.ScreenSize).X + space - Camera.ScreenToWorld(GameRoot.ScreenSize).X % space; i += space)
                spriteBatch.Draw(Art.Pixel, new Rectangle((int)Camera.WorldToScreen(new Vector2(i, 0)).X, 0, 1, (int)GameRoot.ScreenSize.Y), Color.LightBlue);
            for (int i = (int)(Camera.ScreenToWorld(Vector2.Zero).Y - Camera.ScreenToWorld(Vector2.Zero).Y % space); i < Camera.ScreenToWorld(GameRoot.ScreenSize).Y + space - Camera.ScreenToWorld(GameRoot.ScreenSize).Y % space; i += space)
                spriteBatch.Draw(Art.Pixel, new Rectangle(0, (int)Camera.WorldToScreen(new Vector2(0, i)).Y, (int)GameRoot.ScreenSize.X, 1), Color.LightBlue);
            spriteBatch.Draw(Art.Hud, Vector2.Zero, Color.White);
        }
    }
}
