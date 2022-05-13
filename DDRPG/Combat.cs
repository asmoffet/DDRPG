using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

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
        Texture2D arrowTexture;

        private int pos = 0;
        public int index = 0;
        private bool targeting = false;
        private int target = 0;

        private bool attacking = false;
        private int plyrDmg;
        private int enemDmg;

        private Arrow[] arrows;
        private int arrowQue = 0;

        public Combat(Character[] party, Character[] enemy, cube cube, ContentManager content)
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
            arrows = new Arrow[que.Length * 4];
            for (int i = 0; i < arrows.Length; i++)
            {
                arrows[i] = new Arrow(' ', 1344, ' ', 0, 0);
            }
            arrowTexture = content.Load<Texture2D>("arrows");
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
            ks = Keyboard.GetState();
            if (hpSum(_party))
            {
                return false;
            }
            if (hpSum(_enemies))
            {
                foreach(Character c in _party)
                {
                    c.exp += _enemies[0].exp;
                    if(c.exp >= c.nxtlvl)
                    {
                        c.levelUp();
                    }
                }
                return false;
            }
            //pre arrow phase
            if (!attacking)
            {
                
                if (que[index].PC)
                {
                    //refactor this into a switch using phases. Phase 0 is selecting, Phase 1 is targeting, and phase 2 is the arrow stuff.
                    
                    //choosing attack type this is a really gross way of doing it but i really can't be bothered to program something cleaner
                    if ((ks.IsKeyDown(Keys.Up) || ks.IsKeyDown(Keys.W)) && !targeting && ks != prevKs)
                    {
                        pos = 1;
                    }
                    else if ((ks.IsKeyDown(Keys.Down) || ks.IsKeyDown(Keys.S)) && !targeting && ks != prevKs)
                    {
                        pos = 0;
                    }
                    //locking in your selection and choosing an enemy even though fighting multiple enemies will probably not be implimented
                    if (ks.IsKeyDown(Keys.Space) && !targeting && ks != prevKs)
                    {
                        targeting = true;
                    }
                    //choosing different enemies eventhough it will only be C U B E
                    if ((ks.IsKeyDown(Keys.Up) || ks.IsKeyDown(Keys.W)) && targeting && ks != prevKs)
                    {
                        target++;
                        if (target >= _enemies.Length)
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
                    //locking in your attack needs to be updated...
                    if (ks.IsKeyDown(Keys.Space) && targeting && ks != prevKs)
                    {
                        if (pos == 0)
                        {
                            char[] dirs = { 'u', 'd', 'l', 'r' };
                            Random rn = new Random();
                            rn.Next(0, _party.Length - 1);
                            for (int i = 0; i < 4; i++)
                            {

                                int dist = 1280 + (64 * arrowQue);
                                arrows[arrowQue].changeArrow(dirs[rn.Next(0, 3)], dist, 'p', target, index);
                                arrowQue++;
                            }
                        }
                        else
                        {
                            char[] dirs = { 'u', 'd', 'l', 'r' };
                            Random rn = new Random();
                            rn.Next(0, _party.Length - 1);
                            for (int i = 0; i < 4; i++)
                            {

                                int dist = 1280 + (64 * arrowQue);
                                arrows[arrowQue].changeArrow(dirs[rn.Next(0, 3)], dist, 'm', target, index);
                                arrowQue++;
                            };
                        }
                        targeting = false;
                        index++;
                    }
                }
                //enemy ai for defending
                else
                {
                    char[] dirs = { 'u', 'd', 'l', 'r' };
                    Random rn = new Random();
                    target = rn.Next(0, _party.Length - 1);

                    for (int i = 0; i < 4; i++)
                    {
                        int dist = 1280 + (64 * arrowQue);
                        arrows[arrowQue].changeArrow(dirs[rn.Next(0, 3)], dist, 'e', target, index);
                        arrowQue++;
                    }
                    enemDmg = que[index].phisicalAttack(_party[target]);
                    index++;
                    
                }
                if (index >= que.Length)
                {
                    index = 0;
                    target = 0;
                    attacking = true;
                    
                }
            }

            //in arrow phase display both enemie's damage and player's attack 
            //when enemy goes to attack player decrease the damage the enemy deals
            // when player attacks increase the damage player deals

            //if arrow is < 0 then it was an enemy attack if not player attack
            //denote attack types by using 'p' for physical 'm' for magic 'e' for enemy
            //arrow phase
            else
            {
                bool arrowsActive = false;
                foreach (Arrow a in arrows)
                {
                    if (a != null)
                    {
                        
                        if (a.active)
                        {

                            if (a.atktyp == 'e')
                            {
                                enemDmg += a.update(que[a.target], que[a.user], ks, prevKs);
                                if(enemDmg <= 1)
                                {
                                    enemDmg = 1;
                                }
                            }
                            else
                            {
                                plyrDmg += a.update(que[a.user], que[a.target], ks, prevKs);
                            }
                            arrowsActive = true;
                            break;
                        }
                    }

                }
                if (!arrowsActive)
                {
                    attacking = false;
                    pos = 0;
                    index = 0;
                    target = 0;
                    arrowQue = 0;
                    foreach (Character c in _enemies)
                    {
                        c.hp -= plyrDmg;
                        if(c.hp < 0)
                        {
                            c.hp = 0;
                        }
                    }
                    if (hpSum(_enemies))
                    {
                        
                        foreach (Character c in _party)
                        {
                            c.exp += _enemies[0].exp;
                            if (c.exp >= c.nxtlvl)
                            {
                                c.levelUp();
                            }
                        }
                        return false;
                    }
                    foreach (Character c in _party)
                    {
                        c.hp -= enemDmg;
                        if (c.hp < 0)
                        {
                            c.hp = 0;
                        }
                    }
                    if (hpSum(_party))
                    {
                        
                        return false;
                    }
                    
                }
                else
                {
                    foreach (Arrow a in arrows)
                    {
                        if (a != null && a.active)
                        {
                            a.moveArrow();
                        }
                        if (a != null && a.position.X < -64)
                        {
                            a.active = false;
                        }
                    }
                }
                
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
            foreach(Arrow a in arrows)
            {
                a.Draw(gameTime, spriteBatch, arrowTexture);
            }
            Vector2 position = new Vector2(10, 10);
            foreach(Character c in _party)
            {
                spriteBatch.Draw(c.texture, position,  null, Color.White, 0f, Vector2.Zero, .5f,SpriteEffects.None, 0);
                spriteBatch.DrawString(sf,"Hp: " + c.hp.ToString() + "/" + c.maxhp.ToString(), position + new Vector2(130, -5), Color.White);
                spriteBatch.DrawString(sf, "Mp: " + c.mp.ToString() + "/" + c.maxmp.ToString(), position + new Vector2(130, 10), Color.White);
                spriteBatch.DrawString(sf, "Strength: " + c.str.ToString(), position + new Vector2(130, 25), Color.White);
                spriteBatch.DrawString(sf, "Magic: " + c.mgc.ToString(), position + new Vector2(130, 40), Color.White);
                spriteBatch.DrawString(sf, "Speed: " + c.spd.ToString(), position + new Vector2(130, 55), Color.White);
                position.Y += 100;
            }

            position = new Vector2(1080, 50);
            foreach (Character c in _enemies)
            {
                if(c.hp > 0)
                {
                    if(c.exp == 0)
                    {
                        spriteBatch.Draw(c.texture, new Vector2(745, 10), null, Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0);
                    }
                    else
                    {
                        Cube.Draw();
                        spriteBatch.DrawString(sf, "Hp: " + c.hp.ToString(), position - new Vector2(100, 15), Color.White);
                    }
                    
                    //spriteBatch.Draw(c.texture, position, null, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);
                    
                    position.Y += 100;
                }
                
            }
            position = new Vector2(1080 , 25 + 100 * target);
            spriteBatch.DrawString(sf, "Target", position - new Vector2(100, 15), Color.White);
            if(pos == 0 && !attacking)
            {
                spriteBatch.DrawString(sf, "Physical Attack", new Vector2(600, 500), Color.White);
            }
            else if (pos == 1 && !attacking)
            {
                spriteBatch.DrawString(sf, "Magical Attack", new Vector2(600, 500), Color.White);
            }
            else if (attacking)
            {
                spriteBatch.DrawString(sf, " Enemy Attack: " + (enemDmg).ToString() + " Player Attack: " + plyrDmg.ToString(), new Vector2(500, 300), Color.White);
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
