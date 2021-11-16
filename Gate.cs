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
            // Draw red background (hitbox)
            spriteBatch.Draw(Art.Pixel, Rect, new Color(255, 0, 0, 32));
            // Draw gate itself
            spriteBatch.Draw(Image, Rect, Color.White);
            // Draw inputs (either one or two)
            if (Inputs.Length == 1) {
                spriteBatch.Draw(Art.Pixel, Camera.WorldToScreen(new Vector2(Position.X - Size.X / 2f, Position.Y)), null, Color.Green, 0f, new Vector2(0.5f), 10f, 0, 0);
            } else if (Inputs.Length == 2) {
                spriteBatch.Draw(Art.Pixel, Camera.WorldToScreen(InputPositions[0]), null, Color.Green, 0f, new Vector2(0.5f), 10f, 0, 0);
                spriteBatch.Draw(Art.Pixel, Camera.WorldToScreen(InputPositions[1]), null, Color.Green, 0f, new Vector2(0.5f), 10f, 0, 0);
            }
            // Draw output (if there is one)
            if (Outputs.Length == 1)
                spriteBatch.Draw(Art.Pixel, Camera.WorldToScreen(new Vector2(Position.X + Size.X / 2f, Position.Y)), null, Color.Green, 0f, new Vector2(0.5f), 10f, 0, 0);
            // If gate is an LED, draw whether it's on or off
            if (Type == "led" && Inputs[0] == true) {
                spriteBatch.Draw(Art.LedOn, Rect, Color.White);
            }
        }
    }
    public class Wire {
        public Vector2 StartPosition;
        public Vector2 EndPosition;
        public const float Radius = 10f;
        public bool activated = false;
        public bool isExpired = false;
        public Wire(Vector2 startPosition, Vector2 endPosition) {
            StartPosition = startPosition;
            EndPosition = endPosition;
        }
        public float DistanceFromPoint(Vector2 point) {
            float m = (EndPosition.Y - StartPosition.Y) / (EndPosition.X - StartPosition.X);
            float d = MathF.Abs(-m * point.X + point.Y + m * StartPosition.X - StartPosition.Y) / MathF.Sqrt(MathF.Pow(m, 2) + 1);
            return d;

        }
        public void Update() {
            activated = false;
            foreach(Gate gate in GateManager.Gates) {
                for (int i = 0; i < gate.Outputs.Length; i++) {
                    if (gate.Outputs[i] && Vector2.DistanceSquared(gate.OutputPositions[i], StartPosition) < Radius * Radius) {
                        activated = true;
                    }
                }
            }
            // If wire was just right clicked, delete it
            if (Input.WasRightButtonJustDown() && DistanceFromPoint(Camera.MouseWorldCoords()) < 10f && new Rectangle((int)Math.Min(EndPosition.X, StartPosition.X) - 5, (int)Math.Min(EndPosition.Y, StartPosition.Y) - 5, (int)Math.Abs(EndPosition.X - StartPosition.X) + 5, (int)Math.Abs(EndPosition.Y - StartPosition.Y) + 5).Contains(Camera.MouseWorldCoords()))
                isExpired = true;
        }
        public void Draw(SpriteBatch spriteBatch) {
            Color pointsColor = Color.Blue;
            if (activated)
                pointsColor = Color.Red;
            Vector2 StartScreen = Camera.WorldToScreen(StartPosition);
            Vector2 EndScreen = Camera.WorldToScreen(EndPosition);
            // end point
            spriteBatch.Draw(Art.Pixel, StartScreen, null, pointsColor, 0f, new Vector2(0.5f), Radius, 0, 0);
            // start point
            spriteBatch.Draw(Art.Pixel, EndScreen, null, pointsColor, 0f, new Vector2(0.5f), Radius, 0, 0);
            // line
            spriteBatch.Draw(Art.Pixel, StartScreen, null, Color.Black, MathF.Atan2(EndScreen.Y - StartScreen.Y, EndScreen.X - StartScreen.X), Vector2.Zero, new Vector2(MathF.Sqrt(MathF.Pow(EndScreen.X - StartScreen.X, 2) + MathF.Pow(EndScreen.Y - StartScreen.Y, 2)), 5f * Camera.Zoom), 0, 0); ;
            // arrow
            spriteBatch.Draw(Art.Arrow, (StartScreen + EndScreen) / 2f, null, Color.Black, MathF.Atan2(EndScreen.Y - StartScreen.Y, EndScreen.X - StartScreen.X), new Vector2(Art.Arrow.Width, Art.Arrow.Height) / 2f, Camera.Zoom, 0, 0); ;
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
            // Updates gates and wires
            foreach (Gate gate in Gates)
                gate.Update();
            foreach (Wire wire in Wires)
                wire.Update();
            Gates = Gates.Where(g => !g.isExpired).ToList();
            Wires = Wires.Where(w => !w.isExpired).ToList();
            if (Input.WasLeftButtonJustDown()) {
                // Default to camera
                selectedType = "camera";
                // Check wire start positions
                for (int i = 0; i < Wires.Count; i++) {
                    Wire wire = Wires[i];
                    if (Vector2.DistanceSquared(wire.StartPosition, Camera.MouseWorldCoords()) < Wire.Radius * Wire.Radius) {
                        selectedIndex = i;
                        selectedType = "wireStart";
                    }
                }
                // Gates
                for (int i = 0; i < Gates.Count; i++) {
                    Gate gate = Gates[i];
                    // Check if moving gate
                    if (gate.Rect.Contains(Input.mouse.Position)) {
                        if (gate.Type == "switch") {
                            gate.Type = "switchClosed";
                            gate.Image = Art.SwitchClosed;
                        } else if (gate.Type == "switchClosed") {
                            gate.Type = "switch";
                            gate.Image = Art.Switch;
                        } else if (gate.Type == "buttonOn") {
                            gate.Type = "buttonOff";
                            gate.Image = Art.ButtonOff;
                        } else if (gate.Type == "buttonOff") {
                            gate.Type = "buttonOn";
                            gate.Image = Art.ButtonOn;
                        }
                        selectedIndex = i;
                        selectedType = "gate";
                    }
                    // Check if adding wire from input
                    foreach (Vector2 pos in gate.OutputPositions) {
                        if (Vector2.DistanceSquared(pos, Camera.MouseWorldCoords()) < Wire.Radius * Wire.Radius) {
                            Add(new Wire(pos, Camera.MouseWorldCoords()));
                            selectedIndex = Wires.Count - 1;
                            selectedType = "wireEnd";
                        }
                    }
                }
                // Check wire end positions
                for (int i = 0; i < Wires.Count; i++) {
                    Wire wire = Wires[i];
                    if (Vector2.DistanceSquared(wire.EndPosition, Camera.MouseWorldCoords()) < Wire.Radius * Wire.Radius) {
                        selectedIndex = i;
                        selectedType = "wireEnd";
                    }
                }
            } else if (Input.WasLeftButtonJustUp()) {
                selectedType = "none";
            }
            switch (selectedType) {
                case "camera":
                    Camera.CameraPosition -= Input.DeltaMouse();
                    break;
                case "gate":
                    Gates[selectedIndex].Position += Input.DeltaMouse();
                    Gates[selectedIndex].UpdateRect();
                    break;
                case "wireStart":
                    Wires[selectedIndex].StartPosition += Input.DeltaMouse();
                    break;
                case "wireEnd":
                    Wires[selectedIndex].EndPosition += Input.DeltaMouse();
                    break;
                default:
                    break;
            }
            // Create new objects
            if (Input.WasKeyJustDown(Keys.D0))
                Add(new Wire(Camera.MouseWorldCoords(), Camera.MouseWorldCoords() + new Vector2(100, 0)));
            if (Input.WasKeyJustDown(Keys.D1))
                Add(new Gate(Camera.MouseWorldCoords(), "and"));
            if (Input.WasKeyJustDown(Keys.D2))
                Add(new Gate(Camera.MouseWorldCoords(), "or"));
            if (Input.WasKeyJustDown(Keys.D3))
                Add(new Gate(Camera.MouseWorldCoords(), "nand"));
            if (Input.WasKeyJustDown(Keys.D4))
                Add(new Gate(Camera.MouseWorldCoords(), "nor"));
            if (Input.WasKeyJustDown(Keys.D5))
                Add(new Gate(Camera.MouseWorldCoords(), "xor"));
            if (Input.WasKeyJustDown(Keys.D6))
                Add(new Gate(Camera.MouseWorldCoords(), "not"));
            if (Input.WasKeyJustDown(Keys.D7))
                Add(new Gate(Camera.MouseWorldCoords(), "one"));
            if (Input.WasKeyJustDown(Keys.D8))
                Add(new Gate(Camera.MouseWorldCoords(), "led"));
            if (Input.WasKeyJustDown(Keys.D9))
                Add(new Gate(Camera.MouseWorldCoords(), "switch"));
            if (Input.WasKeyJustDown(Keys.B))
                Add(new Gate(Camera.MouseWorldCoords(), "buttonOn"));
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
