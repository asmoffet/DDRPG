using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace DDRPG
{
    public class Arrow
    {
        //if the arrow has been activated
        public bool active = false;

        //the direction of the arrow
        public char dir;
        public int direct;

        //if its a physical or magical arrow
        public char type;

        public Vector2 position;
        public Texture2D texture;

        //u = top, l = middle up, r = middle bottom, d, = bottom
        public Arrow(char d, int dist)
        {
            dir = d;
            switch (d)
            {
                case 'u':
                    position = new Vector2(dist, 320);
                    direct = 2;
                    active = true;
                    break;
                case 'l':
                    position = new Vector2(dist, 384);
                    direct = 0;
                    active = true;
                    break;
                case 'r':
                    position = new Vector2(dist, 448);
                    direct = 1;
                    active = true;
                    break;
                case 'd':
                    position = new Vector2(dist, 512);
                    direct = 3;
                    active = true;
                    break;
                default:
                    break;
            }

        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Texture2D texture)
        {
            if (active)
            {
                var source = new Rectangle(direct * 64, 0, 64, 64);
                spriteBatch.Draw(texture, position, source, Color.White);
            }
            
        }
    }
}
