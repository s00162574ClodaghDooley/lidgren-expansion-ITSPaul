using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using CameraNS;

namespace Sprites
{
    public class SimpleSprite : DrawableGameComponent
    {
        public Texture2D Image;
        public Vector2 Position;
        public Rectangle BoundingRect;
        public bool Visible = true;

        public SimpleSprite(Game game, Texture2D spriteImage,
                            Vector2 startPosition) : base(game)
        {
            game.Components.Add(this);
            Image = spriteImage;
            Position = startPosition;
            BoundingRect = new Rectangle((int)startPosition.X, (int)startPosition.Y, Image.Width, Image.Height);

        }

        public override void Update(GameTime gameTime)
        {
            BoundingRect = new Rectangle((int)Position.X, (int)Position.Y, Image.Width, Image.Height);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sp = Game.Services.GetService<SpriteBatch>();
            if (sp == null) return;

            if (Visible)
            {
                sp.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Camera.CurrentCameraTranslation);
                sp.Draw(Image, BoundingRect, Color.White);
                sp.End();
            }
        }
        public void Move(Vector2 delta)
        {
            Position += delta;
            BoundingRect = new Rectangle((int)Position.X, (int)Position.Y, Image.Width, Image.Height);
            BoundingRect.X = (int)Position.X;
            BoundingRect.Y = (int)Position.Y;
        }
    }
}
