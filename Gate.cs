using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace chips
{
    public class Gate
    {
        public Vector2 Position;
        string Type;
        Texture2D Image;
        Vector2 Size;
        public Rectangle Rect;
        bool isBeingDragged = false;
        bool[] Inputs;
        bool[] Outputs;
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
            }
            Size = new Vector2(Image.Width, Image.Height);
            UpdateRect();
        }
        public void Update()
        {
            

            switch (Type) {
                case "and":
                    Outputs[0] = Inputs[0] && Inputs[1];
                    break;
                default:
                    break;
            }
        }
        public void UpdateRect() {
            Rect = new Rectangle((int)(Position.X - Image.Width / 2f), (int)(Position.Y - Image.Height / 2f), Image.Width, Image.Height);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Art.Pixel, Rect, new Color(255, 0, 0, 32));
            spriteBatch.Draw(Image, Position, null, Color.White, 0f, new Vector2(Image.Width, Image.Height) / 2f, 1f, 0, 0);
            spriteBatch.Draw(Art.Pixel, Position, null, Color.Blue, 0f, new Vector2(0.5f), 10f, 0, 0);
            if (Inputs.Length == 1) {
                spriteBatch.Draw(Art.Pixel, new Vector2(Position.X - Size.X / 2f, Position.Y), null, Color.Green, 0f, new Vector2(0.5f), 10f, 0, 0);
            } else if (Inputs.Length == 2) {
                spriteBatch.Draw(Art.Pixel, new Vector2(Position.X - Size.X / 2f, Position.Y - 20f), null, Color.Green, 0f, new Vector2(0.5f), 10f, 0, 0);
                spriteBatch.Draw(Art.Pixel, new Vector2(Position.X - Size.X / 2f, Position.Y + 20f), null, Color.Green, 0f, new Vector2(0.5f), 10f, 0, 0);
            }
            if (Outputs.Length == 1) {
                spriteBatch.Draw(Art.Pixel, new Vector2(Position.X + Size.X / 2f, Position.Y), null, Color.Green, 0f, new Vector2(0.5f), 10f, 0, 0);
            }
            if (Image == Art.Led && Inputs[0] == true) {
                spriteBatch.Draw(Art.LedOn, Position, null, Color.White, 0f, new Vector2(Art.LedOn.Width, Art.LedOn.Height) / 2f, 1f, 0, 0);
            }
        }
    }
    public class Wire {
        public Vector2 StartPosition;
        public Vector2 EndPosition;
        bool activated = false;
        public Wire(Vector2 startPosition, Vector2 endPosition) {
            StartPosition = startPosition;
            EndPosition = endPosition;
        }
        public void Update() {
            activated = false;
            foreach(Gate gate in GateManager.Gates) {
                if (Gate.ty)
            }
        }
        public void Draw(SpriteBatch spriteBatch) {

            spriteBatch.Draw(Art.Pixel, StartPosition, null, Color.Black, MathF.Atan2(EndPosition.Y - StartPosition.Y, EndPosition.X - StartPosition.X), Vector2.Zero, new Vector2(MathF.Sqrt(MathF.Pow(EndPosition.X - StartPosition.X, 2) + MathF.Pow(EndPosition.Y - StartPosition.Y, 2)), 5f), 0, 0); ;
            spriteBatch.Draw(Art.Pixel, StartPosition, null, Color.Blue, 0f, new Vector2(0.5f), 10f, 0, 0);
            spriteBatch.Draw(Art.Pixel, EndPosition, null, Color.Blue, 0f, new Vector2(0.5f), 10f, 0, 0);
        }
    }
    public static class GateManager
    {
        public static List<Gate> Gates = new List<Gate>();
        public static List<Wire> Wires = new List<Wire>();
        private static string selectedType = "none";
        private static int selectedIndex;
        public static void Update()
        {
            foreach (Gate gate in Gates)
                gate.Update();
            foreach (Wire wire in Wires)
                wire.Update();
            if (Input.WasLeftButtonJustDown()) {
                for (int i = 0; i < Gates.Count; i++) {
                    Gate gate = Gates[i];
                    if (gate.Rect.Contains(Input.mouse.Position)) {
                        selectedIndex = i;
                        selectedType = "gate";
                        break;
                    }
                }
                for (int i = 0; i < Wires.Count; i++) {
                    Wire wire = Wires[i];
                    /*
                    if (wire.Rect.Contains(Input.mouse.Position)) {
                        selectedIndex = i;
                        selectedType = "wire";
                        break;
                    }
                    */
                }
            } else if (Input.WasLeftButtonJustUp()) {
                selectedType = "none";
            }
            switch (selectedType) {
                case "gate":
                    Gates[selectedIndex].Position += Input.DeltaMouse();
                    Gates[selectedIndex].UpdateRect();
                    break;
                case "wire":
                    //Wires[selectedIndex].Position += Input.DeltaMouse();
                    break;
                default:
                    break;
            }
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Gate gate in Gates)
                gate.Draw(spriteBatch);
            foreach (Wire wire in Wires)
                wire.Draw(spriteBatch);
        }
        public static void Add(Gate gate)
        {
            Gates.Add(gate);
        }
        public static void Add(Wire wire) {
            Wires.Add(wire);
        }
    }
}
