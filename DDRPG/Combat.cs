﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DDRPG
{
    public class Combat
    {
        public Character[] _enemies;
        public Character[] _party;
        private Character[] que;
        private KeyboardState ks;
        private KeyboardState prevKs;
        cube Cube;
        private int pos = 0;
        public int index = 0;
        private bool targeting = false;
        private int target = 0;
        public Combat(Character[] party, Character[] enemy, cube cube)
        {
            IComparer spdComparer = new SPDCompair();
            que = new Character[party.Length + enemy.Length];
            _enemies = new Character[enemy.Length];
            _party = new Character[party.Length];
            int start = 0;
            start = addToQue(que, party, start);
            addToQue(que, enemy, start);
            addToQue(_enemies, enemy, 0);
            addToQue(_party, party, 0);
            Array.Sort(que, spdComparer);
            Cube = cube;
        }

        private int addToQue(Character[] que, Character[] array,int start)
        {
            foreach(Character c in array)
            {
                que[start] = c;
                start++;
            }
            return start;
        }


        public bool Update(GameTime gameTime)
        {
            if (hpSum(_party) || hpSum(_enemies))
            {
                return false;
            }

            if (que[index].PC)
            {
                ks = Keyboard.GetState();
                if ((ks.IsKeyDown(Keys.Up) || ks.IsKeyDown(Keys.W)) && !targeting && ks != prevKs)
                {
                    pos = 1;
                }
                else if ((ks.IsKeyDown(Keys.Down) || ks.IsKeyDown(Keys.S)) && !targeting && ks != prevKs)
                {
                    pos = 0;
                }
                if (ks.IsKeyDown(Keys.Space) && !targeting && ks != prevKs)
                {
                    targeting = true;
                }
                if ((ks.IsKeyDown(Keys.Up) || ks.IsKeyDown(Keys.W)) && targeting && ks != prevKs)
                {
                    target++;
                    if(target >= _enemies.Length)
                    {
                        target = 0;
                    }
                }
                else if ((ks.IsKeyDown(Keys.Down) || ks.IsKeyDown(Keys.S)) && targeting && ks != prevKs)
                {
                    target--;
                    if (target < 0)
                    {
                        target = _enemies.Length - 1;
                    }
                }
                if (ks.IsKeyDown(Keys.Space) && targeting && ks != prevKs)
                {
                    if(pos == 0)
                    {
                        que[index].phisicalAttack(_enemies[target]);
                    }
                    else
                    {
                        que[index].magicalAttack(_enemies[target]);  
                    }
                    targeting = false;
                    index++;
                }
            }
            else
            {
                Random rn = new Random();
                index = rn.Next(0, _party.Length - 1);
                que[index].phisicalAttack(_party[index]);
            }
            prevKs = ks;
            Cube.Update(gameTime);
            return true;
        }


        public bool hpSum(Character[] hpcheck)
        {
            int sum = 0;
            foreach(Character c in hpcheck)
            {
                sum += c.hp;
            }
            if(sum == 0)
            {
                return true;
            }
            return false;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont sf)
        {
            Vector2 position = new Vector2(50, 50);
            foreach(Character c in _party)
            {
                spriteBatch.Draw(c.texture, position,  null, Color.White, 0f, Vector2.Zero, 2f,SpriteEffects.None, 0);
                spriteBatch.DrawString(sf,"Hp: " + c.hp.ToString(), position - new Vector2(-100, 15), Color.White);
                position.Y += 100;
            }

            position = new Vector2(1080, 50);
            foreach (Character c in _enemies)
            {
                if(c.hp > 0)
                {
                    Cube.Draw();
                    //spriteBatch.Draw(c.texture, position, null, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);
                    spriteBatch.DrawString(sf, "Hp: " + c.hp.ToString(), position - new Vector2(100, 15), Color.White);
                    position.Y += 100;
                }
                
            }
            position = new Vector2(1080 , 25 + 100 * target);
            spriteBatch.DrawString(sf, "Target", position - new Vector2(100, 15), Color.White);
            if(pos == 0)
            {
                spriteBatch.DrawString(sf, "Physical Attack", new Vector2(600, 500), Color.White);
            }
            else if (pos == 1)
            {
                spriteBatch.DrawString(sf, "Magical Attack", new Vector2(600, 500), Color.White);
            }
        }
    }

    public class SPDCompair : IComparer
    {
        public int Compare(Object x, Object y)
        {
            return (new CaseInsensitiveComparer()).Compare(((Character)y).spd, ((Character)x).spd);
        }
    }
}
