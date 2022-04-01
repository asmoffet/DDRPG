using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
            party = new Character[] { new Character(20, 20, 5, 5, 5, 2, 10, 5, 1, "speedMan", "overworldSprite") };
            enemy = new Character[] { new Character(20, 20, 5, 5, 5, 2, 1, 5, 1, "bad dude", "overworldSprite") };
            Combat = new Combat(party, enemy);
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
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (!combat)
            {
                _player.Update(gameTime, GraphicsDevice.Viewport);
                if (_tilemap._map[(int)(_player.position.X + _player.position.Y * 10)] == 11)
                {
                    combat = true;
                    _graphics.PreferredBackBufferWidth = 1280;
                    _graphics.PreferredBackBufferHeight = 640;
                    _graphics.ApplyChanges();
                    //_tilemap._map[(int)(_player.position.X + _player.position.Y * 10)] = 12;
                }
            }
            else
            {
                Combat.Update();
            }
            
            base.Update(gameTime);
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
