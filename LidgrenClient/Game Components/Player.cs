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
using LidgrenClient;

namespace Sprites
{
    public enum DIRECTION { LEFT, RIGHT, UP,DOWN, STARTING }
    
        class Player : SimpleSprite
        {
            public PlayerData playerData;
            protected float playerVelocity = 6.0f;
            private Vector2 PreviousPosition;
            public Player(Game g, Texture2D texture, Vector2 userPosition) : base(g,texture,userPosition)
            {
            PreviousPosition = userPosition;

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
            if(Position != PreviousPosition)
            {
                // Update the player Data packet
                // Send MovedData Message to the server
                // No need to Retrieve the client GameComponent as we have a staic reference 
                // to the client

                playerData.X = Position.X;
                playerData.Y = Position.Y;
                // Send the message for movement
                DataHandler.sendNetMess<MovedData>(LidgrenGameClient.client,
                           new MovedData { playerID = playerData.playerID,
                                            toX = playerData.X, toY = playerData.Y }, 
                           SENT.FROMCLIENT);

            }

            if (InputEngine.IsKeyPressed(Keys.Space))
                new Projectile(Game, Game.Content.Load<Texture2D>("Coin"), Position);
            PreviousPosition = Position;
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
