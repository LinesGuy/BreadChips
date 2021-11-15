using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace chips {
    class Art {
        public static Texture2D GateAnd;
        public static Texture2D Pixel;
        public static Texture2D One;
        public static Texture2D Led;
        public static Texture2D LedOn;
        public static void Load(ContentManager content) {
            GateAnd = content.Load<Texture2D>("and");
            Pixel = content.Load<Texture2D>("pixel");
            One = content.Load<Texture2D>("one");
            Led = content.Load<Texture2D>("Led");
            LedOn = content.Load<Texture2D>("LedOn");
        }
    }
}
