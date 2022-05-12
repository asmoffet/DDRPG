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

        private bool combat = false;
        private Character[] party;
        private Character[] enemy;
        private SpriteFont arial;
        private Combat Combat;
        private cube cube;

        private Song backgroundMusic;

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
            party = new Character[] { new Character(20, 20, 6, 6, 10, 5, 20, 3, 1, 0, "speedMan", "Ghost") };
            enemy = new Character[] { new Character(50, 50, 6, 6, 10, 5, 1, 4, 1, 100, "bad dude", "overworldSprite") };
            cube = new cube(this);
            Combat = new Combat(party, enemy, cube, Content);
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
                c.PC = true;
            }
            foreach (Character c in enemy)
            {
                c.LoadContent(Content);
            }
            arial = Content.Load<SpriteFont>("arial");
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
            if (!combat)
            {
                
                if (_tilemap._map[(int)(_player.position.X + _player.position.Y * 10)] == 11)
                {
                    if (Combat.hpSum(enemy))
                    {
                        _graphics.PreferredBackBufferWidth = 640;
                        _graphics.PreferredBackBufferHeight = 640;
                        _graphics.ApplyChanges();
                        _tilemap._map[(int)(_player.position.X + _player.position.Y * 10)] = 12;
                        generateEnemies();
                    }
                    else if (Combat.hpSum(party))
                    {
                        _graphics.PreferredBackBufferWidth = 640;
                        _graphics.PreferredBackBufferHeight = 640;
                        _graphics.ApplyChanges();
                        int i = 0;
                        foreach (Character c in Combat._party)
                        {
                            Combat._party[i].hp = Combat._party[i].maxhp;
                            i++;
                        }
                        _player.position.X -= 1;
                    }
                    else
                    {
                        combat = true;
                        _graphics.PreferredBackBufferWidth = 1280;
                        _graphics.PreferredBackBufferHeight = 640;
                        _graphics.ApplyChanges();
                        Combat.index = 0;
                        
                    }

                }
                _player.Update(gameTime, GraphicsDevice.Viewport);
            }
            else
            {
                combat = Combat.Update(gameTime);
                int i = 0;
                foreach (Character c in Combat._enemies)
                {
                    enemy[i] = c;
                    i++;
                }
                i = 0;
                foreach (Character c in Combat._party)
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
            foreach (Character c in Combat._enemies)
            {
                //hp hpmax mp mpmax str mgc spd def lvl
                Combat._enemies[i] = new Character(mxhp, mxhp, mxmp, mxmp, rng.Next(1, 25), rng.Next(1, 10), rng.Next(1, 25), rng.Next(1, 10), rng.Next(1, 5), rng.Next(1, 300), "bad dude", "overworldSprite");
                Combat._enemies[i].LoadContent(Content);
            }
            i = 0;
            foreach (Character c in Combat._enemies)
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
            if (!combat)
            {
                _tilemap.Draw(gameTime, _spriteBatch);
                _player.Draw(gameTime, _spriteBatch);
            }
            else
            {
                Combat.Draw(gameTime, _spriteBatch, arial);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
