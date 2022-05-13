using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DDRPG
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Tilemap _tilemap;
        private Player _player;

        private bool cmbt = false;
        private bool bossfight = false;
        private Character[] party;
        private Character[] enemy;
        private Character[] boss;
        private SpriteFont arial;
        private SpriteFont arialBig;
        private Combat combat;
        private Combat bossCombat;
        private cube cube;

        private Song backgroundMusic;

        private bool battleEnd = false;
        private float timer = 0;
        private int qnum = -1;
        // \n every 25
        private string[] quote = { "Why must you go on."
                , "When you keep trying to\ninterject yourself in worlds\nyou do not belong in. "
                , "Oh Ghost, when will\n you stop."
                , "Why do you end it the\n same way."
                , "We still have hope.\nYou can still stop."};

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _graphics.PreferredBackBufferWidth = 640;  
            _graphics.PreferredBackBufferHeight = 640; 
            _graphics.ApplyChanges();
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _tilemap = new Tilemap("map.txt");
            _player = new Player();
            //hp hpmax mp mpmax str mgc spd def lvl
            party = new Character[] { new Character(20, 20, 6, 6, 10, 5, 20, 3, 1, 0, "speedMan", "Ghost", true) };
            enemy = new Character[] { new Character(50, 50, 6, 6, 10, 5, 1, 4, 1, 100, "bad dude", "overworldSprite", false) };
            boss = new Character[] { new Character(500, 500, 6, 6, 250, 50, 15, 40, 100, 0, "boss", "Eternal", false) };
            cube = new cube(this);
            combat = new Combat(party, enemy, cube, Content);
            bossCombat = new Combat(party, boss, cube, Content);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _tilemap.LoadContent(Content);
            _player.LoadContent(Content);
            foreach(Character c in party)
            {
                c.LoadContent(Content);
                
            }
            foreach (Character c in enemy)
            {
                c.LoadContent(Content);
            }
            foreach (Character c in boss)
            {
                c.LoadContent(Content);
            }
            arial = Content.Load<SpriteFont>("arial");
            arialBig = Content.Load<SpriteFont>("arialBig");
            backgroundMusic = Content.Load<Song>("New_Project");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(backgroundMusic);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (!cmbt && !battleEnd)
            {
                
                if (_tilemap._map[(int)(_player.position.X + _player.position.Y * 10)] == 11)
                {
                    if (combat.hpSum(enemy))
                    {
                        _graphics.PreferredBackBufferWidth = 640;
                        _graphics.PreferredBackBufferHeight = 640;
                        _graphics.ApplyChanges();
                        _tilemap._map[(int)(_player.position.X + _player.position.Y * 10)] = 12;
                        generateEnemies();
                        battleEnd = true;
                        timer = 5;
                        qnum++;
                    }
                    else if (combat.hpSum(party))
                    {
                        _graphics.PreferredBackBufferWidth = 640;
                        _graphics.PreferredBackBufferHeight = 640;
                        _graphics.ApplyChanges();
                        int i = 0;
                        foreach (Character c in combat._party)
                        {
                            combat._party[i].hp = combat._party[i].maxhp;
                            i++;
                        }
                        _player.position.X -= 1;
                    }
                    else
                    {
                        cmbt = true;
                        _graphics.PreferredBackBufferWidth = 1280;
                        _graphics.PreferredBackBufferHeight = 640;
                        _graphics.ApplyChanges();
                        combat.index = 0;
                        
                    }

                }
                else if(_tilemap._map[(int)(_player.position.X + _player.position.Y * 10)] == 13)
                {
                    if (bossCombat.hpSum(boss))
                    {
                        _graphics.PreferredBackBufferWidth = 640;
                        _graphics.PreferredBackBufferHeight = 640;
                        _graphics.ApplyChanges();
                        _tilemap._map[(int)(_player.position.X + _player.position.Y * 10)] = 9;
                        
                        battleEnd = true;
                        bossfight = false;
                        timer = 5;
                        qnum++;
                    }
                    else if (bossCombat.hpSum(party))
                    {
                        _graphics.PreferredBackBufferWidth = 640;
                        _graphics.PreferredBackBufferHeight = 640;
                        _graphics.ApplyChanges();
                        int i = 0;
                        foreach (Character c in combat._party)
                        {
                            combat._party[i].hp = combat._party[i].maxhp;
                            i++;
                        }
                        _player.position.X -= 1;
                        bossfight = false;
                    }
                    else
                    {
                        bossfight = true;
                        cmbt = true;
                        //combat._enemies[0] = boss[0];
                        _graphics.PreferredBackBufferWidth = 1280;
                        _graphics.PreferredBackBufferHeight = 640;
                        _graphics.ApplyChanges();
                        
                        bossCombat.index = 0;
                    }
                }
                _player.Update(gameTime, GraphicsDevice.Viewport);
            }
            else if (bossfight)
            {
                cmbt = bossCombat.Update(gameTime);
                int i = 0;
                foreach (Character c in bossCombat._enemies)
                {
                    boss[i] = c;
                    i++;
                }
                i = 0;
                foreach (Character c in bossCombat._party)
                {
                    party[i] = c;
                    i++;
                }
            }
            else if (battleEnd)
            {
                timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if(timer <= 0)
                {
                    battleEnd = false;
                    cmbt = false;
                }
            }
            
            else
            {
                cmbt = combat.Update(gameTime);
                int i = 0;
                foreach (Character c in combat._enemies)
                {
                    enemy[i] = c;
                    i++;
                }
                i = 0;
                foreach (Character c in combat._party)
                {
                    party[i] = c;
                    i++;
                }
            }
            
            base.Update(gameTime);
        }


        private void generateEnemies()
        {
            Random rng = new Random();
            int mxhp = rng.Next(50, 300);
            int mxmp = rng.Next(1, 10);
            int i = 0;
            foreach (Character c in combat._enemies)
            {
                //hp hpmax mp mpmax str mgc spd def lvl
                combat._enemies[i] = new Character(mxhp, mxhp, mxmp, mxmp, rng.Next(1, 25), rng.Next(1, 10), rng.Next(1, 25), rng.Next(1, 10), rng.Next(1, 5), rng.Next(1, 300), "bad dude", "overworldSprite", false);
                combat._enemies[i].LoadContent(Content);
            }
            i = 0;
            foreach (Character c in combat._enemies)
            {
                enemy[i] = c;
                i++;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            if (!cmbt && !battleEnd)
            {
                _tilemap.Draw(gameTime, _spriteBatch);
                _player.Draw(gameTime, _spriteBatch);
            }
            else if (battleEnd)
            {
                if(qnum < quote.Length)
                {
                    
                    _spriteBatch.DrawString(arialBig, quote[qnum], new Vector2(60, (GraphicsDevice.Viewport.Height/2) - 100), Color.White);
                }
                
            }
            else if (bossfight)
            {
                bossCombat.Draw(gameTime, _spriteBatch, arial);
            }
            else
            {
                combat.Draw(gameTime, _spriteBatch, arial);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
