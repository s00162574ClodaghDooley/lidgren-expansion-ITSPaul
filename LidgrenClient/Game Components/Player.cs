using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Engine.Engines;

namespace Sprites
{
    public enum DIRECTION { LEFT, RIGHT, UP,DOWN, STARTING }

        class Player : SimpleSprite
        {
            
            protected float playerVelocity = 6.0f;
            public Player(Game g, Texture2D texture, Vector2 userPosition) : base(g,texture,userPosition)
            {

            }



        public override void Update(GameTime gameTime)
        {
           

            if (InputEngine.IsKeyHeld(Keys.D))
            {
                Position += new Vector2(1, 0) * playerVelocity;
                
            }
            if (InputEngine.IsKeyHeld(Keys.A))
            {
                Position += new Vector2(-1, 0) * playerVelocity;
            }
            if (InputEngine.IsKeyHeld(Keys.W)) 
            {
                Position += new Vector2(0, -1) * playerVelocity ;
            }
            if (InputEngine.IsKeyHeld(Keys.S)) 
            {
                Position += new Vector2(0, 1) * playerVelocity;
            }
            if (InputEngine.IsKeyPressed(Keys.Space))
                new Projectile(Game, Game.Content.Load<Texture2D>("Coin"), Position);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
