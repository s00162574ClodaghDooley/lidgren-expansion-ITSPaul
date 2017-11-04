using Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CameraNS;
using Engine.Engines;

namespace Sprites
{
    class Projectile : SimpleSprite
    {
        Vector2 P0,P1,P2, P3;
        // Variables to control the Bezier tradjectory based on the initial points
        float width = 200f;
        float height = 100f;
        float time = 0f;
        public Projectile(Game game, Texture2D spriteImage, Vector2 startPosition) : base(game, spriteImage, startPosition)
        {
            P0 = startPosition;
            P1 = new Vector2(P0.X , P0.Y - height);
            P2 = P1 + new Vector2(width, 0);
            P3 = new Vector2(P0.X + width, P0.Y);
            
            
        }

        public override void Update(GameTime gameTime)
        {

            
            if(time < 1)
                time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            else Game.Components.Remove(this);
            Position = GetPoint(time,
                P0, P1, P2,P3);

            if (!Game.GraphicsDevice.Viewport.Bounds.Contains(Position.ToPoint() - Camera.CamPos.ToPoint()))
                Game.Components.Remove(this);
            base.Update(gameTime);
        }

        private Vector2 GetPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            float cx = 3 * (p1.X - p0.X);
            float cy = 3 * (p1.Y - p0.Y);

            float bx = 3 * (p2.X - p1.X) - cx;
            float by = 3 * (p2.Y - p1.Y) - cy;

            float ax = p3.X - p0.X - cx - bx;
            float ay = p3.Y - p0.Y - cy - by;

            float Cube = t * t * t;
            float Square = t * t;

            float resX = (ax * Cube) + (bx * Square) + (cx * t) + p0.X;
            float resY = (ay * Cube) + (by * Square) + (cy * t) + p0.Y;

            return new Vector2(resX, resY);
        }
    }
}
