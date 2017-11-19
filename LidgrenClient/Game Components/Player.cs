using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Engine.Engines;
using GameData;
using CameraNS;

namespace Sprites
{
    public enum DIRECTION { LEFT, RIGHT, UP,DOWN, STARTING }
    
        class Player : SimpleSprite
        {
            public PlayerData playerData;
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
            SpriteBatch sp = Game.Services.GetService<SpriteBatch>();
            SpriteFont font = Game.Services.GetService<SpriteFont>();
            // Draw the Image First
            base.Draw(gameTime);
            // Now draw the player id using the camera
            sp.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Camera.CurrentCameraTranslation);
            sp.DrawString(font, playerData.playerID.ToString(), 
                BoundingRect.Center.ToVector2() - font.MeasureString(playerData.playerID)/2, 
                Color.White);
            sp.End();
        }
    }

    
}
