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

        public char atktyp;
        public int target;
        public int user;


        //u = top, l = middle up, r = middle bottom, d, = bottom
        public Arrow(char d, int dist, char atk, int trgt, int usr)
        {
            dir = d;
            atktyp = atk;
            target = trgt;
            user = usr;
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

        public void changeArrow(char d, int dist, char atk, int trgt, int usr)
        {
            dir = d;
            atktyp = atk;
            target = trgt;
            user = usr;
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

        //each arrow needs to: move, check user input, if correct input do damage stuff, return damage
        public int update(Character p, Character e, KeyboardState curr, KeyboardState prev)
        {
            int dmg = 0;
            if (active)
            {
                if ((curr.IsKeyDown(Keys.Right) || curr.IsKeyDown(Keys.D)) && curr != prev)
                {
                    dmg = checkKey('r', atktyp, p, e);
                    position = new Vector2(2000, 384);
                    active = false;
                }
                else if ((curr.IsKeyDown(Keys.Left) || curr.IsKeyDown(Keys.A)) && curr != prev)
                {
                    dmg = checkKey('l', atktyp, p, e);
                    position = new Vector2(2000, 384);
                    active = false;
                }
                else if ((curr.IsKeyDown(Keys.Up) || curr.IsKeyDown(Keys.W)) && curr != prev)
                {
                    dmg = checkKey('u', atktyp, p, e);
                    position = new Vector2(2000, 384);
                    active = false;
                }
                else if ((curr.IsKeyDown(Keys.Down) || curr.IsKeyDown(Keys.S)) && curr != prev)
                {
                    dmg = checkKey('d', atktyp, p, e);
                    position = new Vector2(2000, 384);
                    active = false;
                }
                
            }
            
            return dmg;
        }

        public void moveArrow()
        {
            if (active)
            {
                position -= new Vector2(5, 0);
            }
        }

        private int checkKey(char key, char patk, Character p, Character e)
        {
            int dmg = 0;
            if (dir == key && patk == 'p')
            {
                dmg = p.phisicalAttack(e);

            }
            else if (dir == key && patk == 'm')
            {
                dmg = p.magicalAttack(e);
            }
            else if (dir == key && patk == 'e')
            {
                dmg = (e.phisicalAttack(p) * -1) / 5;
                if( dmg <= 0)
                {
                    dmg = -1;
                }
            }
            return dmg;
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
