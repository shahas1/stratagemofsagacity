﻿using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace SoS
{
    public class Map
    {
        int width, height;
        
        Color backgroundColor = Color.Beige;
        Texture2D background;
        Rectangle spriteRect;
        int mapHeight, mapWidth;
        List<Obstacle> obs = new List<Obstacle>();
        public Map(String _map, GraphicsDeviceManager _graphics, Texture2D wallSprite){
            readMapFromFile(_map, _graphics, wallSprite);
        }

        public Map(int _width, int _height, Color _background)
        {
            width = _width;
            height = _height;
            backgroundColor = _background;
        }
        public Map(int _width, int _height, Texture2D _background)
        {
            width = _width;
            height = _height;
            background = _background;
            spriteRect = new Rectangle(0, 0, _background.Width, _background.Height);
        }

        public bool loadObstacles(List<Obstacle> obstacles)
        {
            obs.AddRange(obstacles);
            return true;
        }

        public void draw(SpriteBatch batch, Rectangle scope)
        {
            batch.GraphicsDevice.Clear(backgroundColor);
            if (background != null && scope.Intersects(new Rectangle(0,0,background.Width,background.Height)))
            {
                int picWidth = scope.Width, picHeight = scope.Height;
                if (picWidth > background.Width - scope.X)
                    picWidth = background.Width - scope.X;
                if (picHeight > background.Height - scope.Y)
                    picHeight = background.Height - scope.Y;
                batch.Draw(background, new Rectangle(0, 0, picWidth, picHeight), new Rectangle(scope.X,scope.Y, picWidth, picHeight), Color.White);
            }
            foreach (Obstacle o in obs)
            {
                if (o.getRectangle().Intersects((scope)))
                    o.draw(batch, scope);
            }
        }
        public void drawMini(SpriteBatch batch, Rectangle scope, Rectangle mini)
        {
            if (background != null && scope.Intersects(new Rectangle(0, 0, background.Width, background.Height)))
            {
                int picWidth = scope.Width, picHeight = scope.Height;
                double factor = scope.Width / mini.Width;
                if (picWidth > background.Width - scope.X)
                    picWidth = background.Width - scope.X;
                if (picHeight > background.Height - scope.Y)
                    picHeight = background.Height - scope.Y;
                batch.Draw(background, new Rectangle(mini.X, mini.Y, (int)(picWidth/factor), (int)(picHeight/factor)), new Rectangle(scope.X, scope.Y, picWidth, picHeight), Color.White);
            }
            foreach (Obstacle o in obs)
            {
                if (o.getRectangle().Intersects(scope))
                    o.drawMini(batch,scope,mini);
            }
        }
        public int getWidth()
        {
            return width;
        }
        public int getHeight()
        {
            return height;
        }
        protected void readMapFromFile(String _map, GraphicsDeviceManager graphics, Texture2D wallSprite)
        {
            StreamReader sr = File.OpenText(_map); //Open the text file
            String[,] map;
            mapWidth = Convert.ToInt32(sr.ReadLine()); //Get the height
            mapHeight = Convert.ToInt32(sr.ReadLine()); //Get the width
            height = mapHeight; //Convert to height in pixels
            width = mapWidth;
            //if (height < 450)
              //  height = 450;
            //if (width < 450)
              //  width = 450;//Convert to width in pixels
            //graphics.PreferredBackBufferHeight = height; //Change viewport height based on map file height
            //graphics.PreferredBackBufferWidth = width; //Change viewport width based on map file width
            map = new String[mapHeight, mapWidth]; //Initialize the 2d array of Strings
            for (int i = 0; i < mapHeight; i++) //Go through the whole map file storing each individual
            //character as a string in "map"
            {
                String temp = sr.ReadLine();
                for (int j = 0; j < mapWidth; j++)
                {
                    map[i, j] = temp.Substring(j, 1);
                }
            }
            sr.Close(); //Close connection to map file

            for (int i = 0; i < mapHeight; i += wallSprite.Width) //Go through the map array and translate into the player
            //object and the array of GameObjects
            {
                for (int j = 0; j < mapWidth; j += wallSprite.Height)
                {
                    if (map[i, j].Equals("x"))
                        obs.Add(new Wall(wallSprite, i, j, 1, 1)); 
                    //if (map[i, j].Equals("1"))
                       // player1 = new Player(playerTexture, new Rectangle(j * 30, i * 30, 30, 30));
                }
            }
        }
        public List<Obstacle> getObs()
        {
            return obs;
        }
        public bool remove(Obstacle o)
        {
            return obs.Remove(o);
        }
    }
}