using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace DDRPG
{
    public class Player
    {
        private KeyboardState keyboardState;
        private KeyboardState prevKeyboardState;

        private Texture2D texture;

        int dir = 1;
        public Vector2 position = new Vector2(0, 0);

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("overworldSprites");
        }

        public void Update(GameTime gameTime, Viewport viewport)
        {
            keyboardState = Keyboard.GetState();
            if ((keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))  && prevKeyboardState != keyboardState){ position += new Vector2(0, -1); dir = 3; }
            if ((keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S)) && prevKeyboardState != keyboardState) { position += new Vector2(0, 1); dir = 2; }
            if ((keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A)) && prevKeyboardState != keyboardState) { position += new Vector2(-1, 0); dir = 1; }
            if ((keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D)) && prevKeyboardState != keyboardState) { position += new Vector2(1, 0); dir = 0; }
            CheckBounds(viewport);
            prevKeyboardState = keyboardState;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var source = new Rectangle(dir * 64, 0, 64, 64);
            spriteBatch.Draw(texture, position * 64, source, Color.White);
            
            //spriteBatch.Draw(texture, new Vector2(hb.X, hb.Y), null, Color.Gold);
        }

        private void CheckBounds(Viewport viewport)
        {
            if (position.Y < 0)
            {
                position.Y = 0;

            }
            if ((position.Y + 1) * 64 > viewport.Height)
            {
                position.Y = (viewport.Height/64) - 1;
            }
            if (position.X < 0)
            {
                position.X = 0;
            }
            if ((position.X + 1)* 64 > viewport.Width)
            {
                position.X = (viewport.Width/64) - 1;
            }
        }
    }
}
