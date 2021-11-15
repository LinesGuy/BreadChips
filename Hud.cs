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
            spriteBatch.Draw(Art.Hud, Vector2.Zero, Color.White);
        }
    }
}
