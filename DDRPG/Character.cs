using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace DDRPG
{
    public class Character
    {
        string name;
        int lvl;
        
        public bool PC = false;

        //Health Max and Current
        public int hp;
        public int maxhp;

        //Magic Points Max and Current
        public int mp;
        public int maxmp;

        //stat array
        public int str;
        public int mgc;
        public int spd;
        public int def;

        public int exp = 0;
        public int nxtlvl = 100;

        public Texture2D texture;
        private string txtreName;

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>(txtreName);
        }

        //sets the character's stats
        public Character (int h, int mxhp, int m, int mxmp, int st, int mg, int sp, int df, int lv, int xp, string nme, string txtr, bool pc)
        {
            hp = h;
            maxhp = mxhp;
            mp = m;
            maxmp = mxmp;
            str = st;
            mgc = mg;
            spd = sp;
            def = df;
            name = nme;
            lvl = lv;
            txtreName = txtr;
            exp = xp;
            PC = pc;
        }

        //basic physical attack function
        public int phisicalAttack(Character target)
        {
            int dmg = 1;
            if (target.def < (str + lvl))
            { 
                dmg = str + lvl-target.def; 
                
            }
            return dmg;
        }
        //basic magic attack function
        public int magicalAttack( Character target)
        {
            int dmg = 1;
            if (target.def < (mgc + lvl) && mp >= 3)
            {
                dmg = (mgc + lvl - target.def) * 2;
                mp -= 3;
            }

            return dmg;
        }


        public void levelUp()
        {
            Random rng = new Random();
            

            str += rng.Next(1, 5);
            mgc += rng.Next(1, 5);
            spd += rng.Next(1, 5);
            def += rng.Next(1, 5);

            maxmp += rng.Next(1, 5);
            mp = maxmp;

            maxhp += rng.Next(1, 10);
            hp = maxhp;

            exp -= nxtlvl;
            if (exp <= 0)
            {
                exp = 0;
            }
            nxtlvl = 100 * lvl + rng.Next(10, 100);
        }
    }
}
