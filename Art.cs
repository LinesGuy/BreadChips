using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace chips {
    class Art {
        public static Texture2D GateAnd;
        public static Texture2D Arrow;
        public static Texture2D GateBuffer;
        public static Texture2D GateNand;
        public static Texture2D GateNor;
        public static Texture2D GateNot;
        public static Texture2D GateOr;
        public static Texture2D Switch;
        public static Texture2D SwitchClosed;
        public static Texture2D GateXnor;
        public static Texture2D GateXor;
        public static Texture2D Zero;
        public static Texture2D Pixel;
        public static Texture2D One;
        public static Texture2D Led;
        public static Texture2D LedOn;
        public static Texture2D Hud;
        public static Texture2D ButtonOn;
        public static Texture2D ButtonOff;
        public static void Load(ContentManager content) {
            GateAnd = content.Load<Texture2D>("and");
            Arrow = content.Load<Texture2D>("arrow");
            GateBuffer = content.Load<Texture2D>("buffer");
            GateNand = content.Load<Texture2D>("nand");
            GateNot = content.Load<Texture2D>("not");
            GateNor = content.Load<Texture2D>("nor");
            GateOr = content.Load<Texture2D>("or");
            Switch = content.Load<Texture2D>("switch");
            SwitchClosed = content.Load<Texture2D>("switchClosed");
            GateXnor = content.Load<Texture2D>("xnor");
            GateXor = content.Load<Texture2D>("xor");
            Zero = content.Load<Texture2D>("zero");
            Pixel = content.Load<Texture2D>("pixel");
            One = content.Load<Texture2D>("one");
            Led = content.Load<Texture2D>("Led");
            LedOn = content.Load<Texture2D>("LedOn");
            Hud = content.Load<Texture2D>("hud");
            ButtonOn = content.Load<Texture2D>("buttonOn");
            ButtonOff = content.Load<Texture2D>("buttonOff");
        }
    }
}
