using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace chips {
    static class Camera {
        public static Vector2 CameraPosition = Vector2.Zero;
        public static float Zoom = 1f;
        public static Vector2 WorldToScreen(Vector2 worldPosition) { return ((worldPosition - CameraPosition) * Zoom) + GameRoot.ScreenSize / 2f; }
        public static Vector2 ScreenToWorld(Vector2 screenPos) { return (screenPos - GameRoot.ScreenSize / 2) / Zoom + CameraPosition; }
        public static void Update() {
            // Move with WASD
            Vector2 direction = Vector2.Zero;
            if (Input.keyboard.IsKeyDown(Keys.A))
                direction.X -= 1;
            if (Input.keyboard.IsKeyDown(Keys.D))
                direction.X += 1;
            if (Input.keyboard.IsKeyDown(Keys.W))
                direction.Y -= 1;
            if (Input.keyboard.IsKeyDown(Keys.S))
                direction.Y += 1;
            if (direction != Vector2.Zero)
                CameraPosition += direction * 5 / Zoom;
            // Zoom
            if (Input.keyboard.IsKeyDown(Keys.Q))
                Zoom /= 1.03f;
            if (Input.keyboard.IsKeyDown(Keys.E))
                Zoom *= 1.03f;
            // Zoom (mouse wheel)
            if (Input.DeltaScrollWheelValue() < 0)
                Zoom *= 1.1f;
            else if (Input.DeltaScrollWheelValue() > 0)
                Zoom /= 1.1f;
            // Zoom bounds
            if (Zoom > 3f)
                Zoom = 3f;
            if (Zoom < 0.1f)
                Zoom = 0.1f;
        }
        public static Vector2 MouseWorldCoords() {
            return ScreenToWorld(Input.mouse.Position.ToVector2());
        }
    }
}
