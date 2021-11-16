using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace chips
{
    public class Gate
    {
        public Vector2 Position;
        public string Type;
        public Texture2D Image;
        Vector2 Size;
        public Rectangle Rect;
        public bool[] Inputs;
        public bool[] Outputs;
        public bool isExpired = false;
        public Gate(Vector2 position, string type)
        {
            Position = position;
            Type = type;
            switch (type)
            {
                case "and":
                    Image = Art.GateAnd;
                    Inputs = new bool[2] { false, false };
                    Outputs = new bool[1] { false };
                    break;
                case "or":
                    Image = Art.GateOr;
                    Inputs = new bool[2] { false, false };
                    Outputs = new bool[1] { false };
                    break;
                case "nand":
                    Image = Art.GateNand;
                    Inputs = new bool[2] { false, false };
                    Outputs = new bool[1] { false };
                    break;
                case "nor":
                    Image = Art.GateNor;
                    Inputs = new bool[2] { false, false };
                    Outputs = new bool[1] { false };
                    break;
                case "xor":
                    Image = Art.GateXor;
                    Inputs = new bool[2] { false, false };
                    Outputs = new bool[1] { false };
                    break;
                case "not":
                    Image = Art.GateNot;
                    Inputs = new bool[1] { false };
                    Outputs = new bool[1] { false };
                    break;
                case "xnor":
                    Image = Art.GateXnor;
                    Inputs = new bool[1] { false };
                    Outputs = new bool[1] { false };
                    break;
                case "one":
                    Image = Art.One;
                    Inputs = new bool[0];
                    Outputs = new bool[1] { true };
                    break;
                case "led":
                    Image = Art.Led;
                    Inputs = new bool[1] { false };
                    Outputs = new bool[0];
                    break;
                case "switch":
                    Image = Art.Switch;
                    Inputs = new bool[1] { false };
                    Outputs = new bool[1];
                    break;
                case "switchClosed":
                    Image = Art.SwitchClosed;
                    Inputs = new bool[1] { false };
                    Outputs = new bool[1];
                    break;
                case "buttonOn":
                    Image = Art.ButtonOn;
                    Inputs = new bool[0];
                    Outputs = new bool[1] { true };
                    break;
                case "buttonOff":
                    Image = Art.ButtonOff;
                    Inputs = new bool[0];
                    Outputs = new bool[1] { false };
                    break;
            }
            Size = new Vector2(Image.Width, Image.Height);
            UpdateRect();
        }
        public List<Vector2> InputPositions {
            get {
                if (Inputs.Length == 0)
                    return new List<Vector2>();
                else if (Inputs.Length == 1)
                    return new List<Vector2> { new Vector2(Position.X - Size.X / 2f, Position.Y) };
                else //if (Inputs.Length == 2)
                    return new List<Vector2> { new Vector2(Position.X - Size.X / 2f, Position.Y - 20f), new Vector2(Position.X - Size.X / 2f, Position.Y + 20f) };
            }
        }
        public List<Vector2> OutputPositions {
            get {
                if (Outputs.Length == 0)
                    return new List<Vector2>();
                else if (Outputs.Length == 1)
                    return new List<Vector2> { new Vector2(Position.X + Size.X / 2f, Position.Y) };
                else //if (Inputs.Length == 2)
                    return new List<Vector2> { new Vector2(Position.X + Size.X / 2f, Position.Y - 20f), new Vector2(Position.X + Size.X / 2f, Position.Y + 20f) };
            }
        }
        public void Update()
        {
            for (int i = 0; i < Inputs.Length; i++) {
                Inputs[i] = false;
                foreach (Wire wire in GateManager.Wires) {
                    if (wire.activated && Vector2.DistanceSquared(wire.EndPosition, InputPositions[i]) < Wire.Radius * Wire.Radius)
                        Inputs[i] = true;
                }
            }
            switch (Type) {
                case "and":
                    Outputs[0] = Inputs[0] && Inputs[1];
                    break;
                case "buffer": case "switchClosed":
                    Outputs[0] = Inputs[0];
                    break;
                case "nand":
                    Outputs[0] = !(Inputs[0] && Inputs[1]);
                    break;
                case "nor":
                    Outputs[0] = !(Inputs[0] || Inputs[1]);
                    break;
                case "not":
                    Outputs[0] = !Inputs[0];
                    break;
                case "or":
                    Outputs[0] = Inputs[0] || Inputs[1];
                    break;
                case "xnor":
                    Outputs[0] = !(Inputs[0] ^ Inputs[1]);
                    break;
                case "xor":
                    Outputs[0] = Inputs[0] ^ Inputs[1];
                    break;
                case "switch": case "buttonOff":
                    Outputs[0] = false;
                    break;
                case "buttonOn":
                    Outputs[0] = true;
                    break;
                default:
                    break;
            }
            // If gate was just right clicked, delete it
            if (Input.WasRightButtonJustDown() && Rect.Contains(Input.mouse.Position))
                isExpired = true;
        }
        public void UpdateRect() {
            Vector2 screenPos = Camera.WorldToScreen(Position);
            Rect = new Rectangle((int)(screenPos.X - Image.Width / 2f * Camera.Zoom), (int)(screenPos.Y - Image.Height / 2f * Camera.Zoom), (int)(Image.Width * Camera.Zoom), (int)(Image.Height * Camera.Zoom));
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            UpdateRect();
            // Draw gate itself
            spriteBatch.Draw(Image, Rect, Color.White);
            // Draw inputs (either one or two)
            if (Inputs.Length == 1) {
                spriteBatch.Draw(Art.Pixel, Camera.WorldToScreen(new Vector2(Position.X - Size.X / 2f, Position.Y)), null, Inputs[0] ? Color.Red : Color.Green, 0f, new Vector2(0.5f), 10f, 0, 0);
            } else if (Inputs.Length == 2) {
                spriteBatch.Draw(Art.Pixel, Camera.WorldToScreen(InputPositions[0]), null, Inputs[0] ? Color.Red : Color.Green, 0f, new Vector2(0.5f), 10f, 0, 0);
                spriteBatch.Draw(Art.Pixel, Camera.WorldToScreen(InputPositions[1]), null, Inputs[1] ? Color.Red : Color.Green, 0f, new Vector2(0.5f), 10f, 0, 0);
            }
            // Draw output (if there is one)
            if (Outputs.Length == 1)
                spriteBatch.Draw(Art.Pixel, Camera.WorldToScreen(new Vector2(Position.X + Size.X / 2f, Position.Y)), null, Outputs[0] ? Color.Red : Color.Green, 0f, new Vector2(0.5f), 10f, 0, 0);
            // If gate is an LED, draw whether it's on or off
            if (Type == "led" && Inputs[0] == true) {
                spriteBatch.Draw(Art.LedOn, Rect, Color.White);
            }
        }
    }
    

}
