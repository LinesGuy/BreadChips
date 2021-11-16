using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace chips {
    public static class GateManager {
        public static List<Gate> Gates = new List<Gate>();
        public static List<Wire> Wires = new List<Wire>();
        private static string selectedType = "none";
        private static int selectedIndex;
        public static void Update() {
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
                if (selectedType == "wireEnd") {
                    foreach (Gate gate in Gates) {
                        foreach (Vector2 inputPos in gate.InputPositions) {
                            if (Vector2.DistanceSquared(Wires[selectedIndex].EndPosition, inputPos) < Wire.Radius * Wire.Radius) {
                                Wires[selectedIndex].EndPosition = inputPos;
                            }
                        }
                    }
                }
                if (selectedType == "wireStart") {
                    foreach (Gate gate in Gates) {
                        foreach (Vector2 outputPos in gate.OutputPositions) {
                            if (Vector2.DistanceSquared(Wires[selectedIndex].StartPosition, outputPos) < Wire.Radius * Wire.Radius) {
                                Wires[selectedIndex].StartPosition = outputPos;
                            }
                        }
                    }
                }
                selectedType = "none";

            }
            switch (selectedType) {
                case "camera":
                    Camera.CameraPosition -= Input.DeltaMouse();
                    break;
                case "gate":
                    Gate gate = Gates[selectedIndex];
                    foreach (Vector2 inputPos in gate.InputPositions) {
                        foreach (Wire wire in Wires) {
                            if (wire.EndPosition == inputPos) {
                                wire.EndPosition += Input.DeltaMouse();
                            }
                        }
                    }
                    foreach (Vector2 outputPos in gate.OutputPositions) {
                        foreach (Wire wire in Wires) {
                            if (wire.StartPosition == outputPos) {
                                wire.StartPosition += Input.DeltaMouse();
                            }
                        }
                    }
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
        public static void Draw(SpriteBatch spriteBatch) {
            foreach (Gate gate in Gates)
                gate.Draw(spriteBatch);
            foreach (Wire wire in Wires)
                wire.Draw(spriteBatch);
        }
        public static void Add(Gate gate) {
            Gates.Add(gate);
        }
        public static void Add(Wire wire) {
            Wires.Add(wire);
        }
    }
}
