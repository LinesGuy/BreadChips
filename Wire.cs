using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace chips {
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
            return MathF.Abs(-m * point.X + point.Y + m * StartPosition.X - StartPosition.Y) / MathF.Sqrt(MathF.Pow(m, 2) + 1);
        }
        public void Update() {
            activated = false;
            foreach (Gate gate in GateManager.Gates)
                for (int i = 0; i < gate.Outputs.Length; i++)
                    if (gate.Outputs[i] && Vector2.DistanceSquared(gate.OutputPositions[i], StartPosition) < Radius * Radius)
                        activated = true;
            if (Input.WasRightButtonJustDown() && DistanceFromPoint(Camera.MouseWorldCoords()) < 10f && new Rectangle((int)Math.Min(EndPosition.X, StartPosition.X) - 5, (int)Math.Min(EndPosition.Y, StartPosition.Y) - 5, (int)Math.Abs(EndPosition.X - StartPosition.X) + 5, (int)Math.Abs(EndPosition.Y - StartPosition.Y) + 5).Contains(Camera.MouseWorldCoords()))
                isExpired = true; // If wire was just right clicked, delete it
        }
        public void Draw(SpriteBatch spriteBatch) {
            Vector2 StartScreen = Camera.WorldToScreen(StartPosition);
            Vector2 EndScreen = Camera.WorldToScreen(EndPosition);
            // End point
            spriteBatch.Draw(Art.Pixel, StartScreen, null, activated ? Color.Red : Color.Green, 0f, new Vector2(0.5f), Radius, 0, 0);
            // Start point
            spriteBatch.Draw(Art.Pixel, EndScreen, null, activated ? Color.Red : Color.Green, 0f, new Vector2(0.5f), Radius, 0, 0);
            // Line
            spriteBatch.Draw(Art.Pixel, StartScreen, null, activated ? Color.DarkRed : Color.Black, MathF.Atan2(EndScreen.Y - StartScreen.Y, EndScreen.X - StartScreen.X), new Vector2(0, 0.5f), new Vector2(MathF.Sqrt(MathF.Pow(EndScreen.X - StartScreen.X, 2) + MathF.Pow(EndScreen.Y - StartScreen.Y, 2)), 3f * Camera.Zoom), 0, 0); ;
            // Arrow
            spriteBatch.Draw(Art.Arrow, (StartScreen + EndScreen) / 2f, null, Color.Black, MathF.Atan2(EndScreen.Y - StartScreen.Y, EndScreen.X - StartScreen.X), new Vector2(Art.Arrow.Width, Art.Arrow.Height) / 2f, Camera.Zoom / 2f, 0, 0); ;
        }
    }
}
