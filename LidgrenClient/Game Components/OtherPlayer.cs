using Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameData;
using CameraNS;

namespace LidgrenClient.Game_Components
{
    public class OtherPlayer : SimpleSprite
    {
        public PlayerData playerData;
        public OtherPlayer(Game game, Texture2D spriteImage, Vector2 startPosition) : base(game, spriteImage, startPosition)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (Position != new Vector2(playerData.X, playerData.Y))
                Position = new Vector2(playerData.X, playerData.Y);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            // Draw other Player in Red tint 
            SpriteBatch sp = Game.Services.GetService<SpriteBatch>();
            SpriteFont font = Game.Services.GetService<SpriteFont>();
            sp.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Camera.CurrentCameraTranslation);
            sp.Draw(Image, Position, Color.Red);
            sp.DrawString(font, playerData.playerID, 
                BoundingRect.Center.ToVector2() - font.MeasureString(playerData.playerID) / 2,
                Color.White);
            sp.End();
            // Don't draw up as tint will be white again
            //base.Draw(gameTime);
        }
    }
}
