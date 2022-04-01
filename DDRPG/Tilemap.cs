using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace DDRPG
{
    public class Tilemap
    {
        int _tilewidth, _tileheight, _mapwidth, _mapheight;

        Texture2D _tilesettexture;

        Rectangle[] _tiles;

        public int[] _map;

        string _filename;

        public Tilemap(string filename)
        {
            _filename = filename;
        }

        public void LoadContent(ContentManager content)
        {
            string data = File.ReadAllText(Path.Join(content.RootDirectory, _filename));
            var lines = data.Split('\n');

            var tilesetFilename = lines[0].Trim();
            _tilesettexture = content.Load<Texture2D>(tilesetFilename);

            //tile size
            var secondLine = lines[1].Split(',');
            _tilewidth = int.Parse(secondLine[0]);
            _tileheight = int.Parse(secondLine[1]);

            //initialize tiles
            int tilesetColumns = _tilesettexture.Width / _tilewidth;
            int tilesetRows = _tilesettexture.Height / _tileheight;
            _tiles = new Rectangle[tilesetColumns * tilesetRows];

            for (int y = 0; y < tilesetColumns; y++)
            {
                for (int x = 0; x < tilesetRows; x++)
                {
                    int index = y * tilesetColumns + x;
                    _tiles[index] = new Rectangle(x * _tilewidth, y * _tileheight, _tilewidth, _tileheight);
                }
            }

            //map size
            var thirdline = lines[2].Split(',');
            _mapwidth = int.Parse(thirdline[0]);
            _mapheight = int.Parse(thirdline[1]);

            //make the map
            var fouthline = lines[3].Split(',');
            _map = new int[_mapwidth * _mapheight];
            for (int i = 0; i < _mapwidth * _mapheight; i++)
            {
                _map[i] = int.Parse(fouthline[i]);
            }


        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int y = 0; y < _mapheight; y++)
            {
                for (int x = 0; x < _mapwidth; x++)
                {
                    int index = _map[y * _mapwidth + x] - 1;
                    if (index == -1) continue;
                    spriteBatch.Draw(_tilesettexture, new Vector2(x * _tilewidth, y * _tileheight), _tiles[index], Color.White);

                }
            }
        }
    }
}

