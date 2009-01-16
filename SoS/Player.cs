﻿using System;
using System.Collections.Generic;
using System.Text;
using SOS;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SoS
{
    class Player : Being
    {
        float speed = .2f;
        Game1 game;
        bool isMouseDown;
        List<List<Texture2D>> playerMovement;
        Point playerMovementState = new Point(0, 0);

        public Player(int x, int y, Texture2D _pic, Game1 _game) : base(x,y,_pic)
        {
           xVel = 0f; yVel = 0f;
           game = _game;
           isMouseDown = false;
        }
        public Player(Texture2D _pic, Rectangle rect, Color c, Game1 _game)
            : base(_pic, rect, c)
        {
            xVel = 0f; yVel = 0f;
            game = _game;
            isMouseDown = false;
        }
        public Player(Texture2D _pic, Rectangle _picRect, float _rotation, int _velocity, float _health, Color _color, Game1 _game)
                    : base(_pic,_picRect,_rotation,_velocity,_health,_color)
        {
            xVel = 0f; yVel = 0f;
            game = _game;
            isMouseDown = false;
        }
        public void loadMovementList(List<List<Texture2D>> pm)
        {
            playerMovement = pm;
        }
        public override void UpdateMove(GameTime gameTime)
        {
            int elapsedTime = (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            //MouseState mouse = Mouse.GetState();
            //rotation = -(float)Math.Atan2((((picRect.X + (picRect.Width / 2))) - mouse.X) * (Math.PI / 180), (((picRect.Y + (picRect.Height / 2))) - mouse.Y) * (Math.PI / 180));
            KeyboardState keyState = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();
            int mX, mY, pX, pY;
            mX = mouse.X; mY = mouse.Y;
            pX = picRect.X - game.getCamera().X; pY = picRect.Y - game.getCamera().Y;
            rotation = -(float)Math.Atan2(((double)pX - (double)mX), ((double)pY - (double)mY));
            
            xVel = 0f; yVel = 0f;
            if (keyState.IsKeyDown(Keys.W) && canMoveUp)
            {
                yVel = -speed;
                if (playerMovement != null)
                {
                    playerMovementState.X = 1;
                    playerMovementState.Y++;
                    if (playerMovementState.Y > playerMovement[playerMovementState.X].Count)
                    {
                        playerMovementState.Y = 0;
                    }
                    setSprite(playerMovement[playerMovementState.X][playerMovementState.Y]);
                }
            }
            if (keyState.IsKeyDown(Keys.S) && canMoveDown)
            {
                yVel = speed;
                if (playerMovement != null)
                {
                    playerMovementState.X = 1;
                    playerMovementState.Y++;
                    if (playerMovementState.Y > playerMovement[playerMovementState.X].Count)
                    {
                        playerMovementState.Y = 0;
                    }
                    setSprite(playerMovement[playerMovementState.X][playerMovementState.Y]);
                }
            }
            if (keyState.IsKeyDown(Keys.A) && canMoveLeft)
            {
                xVel = -speed;
                if (playerMovement != null)
                {
                    playerMovementState.X = 1;
                    playerMovementState.Y++;
                    if (playerMovementState.Y > playerMovement[playerMovementState.X].Count)
                    {
                        playerMovementState.Y = 0;
                    }
                    setSprite(playerMovement[playerMovementState.X][playerMovementState.Y]);
                }
            }
            if (keyState.IsKeyDown(Keys.D) && canMoveRight)
            {
                xVel = speed;
                if (playerMovement != null)
                {
                    playerMovementState.X = 1;
                    playerMovementState.Y++;
                    if (playerMovementState.Y > playerMovement[playerMovementState.X].Count)
                    {
                        playerMovementState.Y = 0;
                    }
                    setSprite(playerMovement[playerMovementState.X][playerMovementState.Y]);
                }
            }
            if (keyState.IsKeyUp(Keys.W) && keyState.IsKeyUp(Keys.A) && keyState.IsKeyUp(Keys.S) && keyState.IsKeyUp(Keys.D))
            {
                playerMovementState.X = 0;
                playerMovementState.Y = 0;
            }
            picRect.X += (int)(xVel * elapsedTime);
            picRect.Y += (int)(yVel * elapsedTime);
            if (picRect.X <= 0)
            {
                picRect.X = 0;
            }
            if (picRect.Y <= 0)
            {
                picRect.Y = 0;
            }
            if (picRect.X + pic.Width >= Game1.map.getWidth())
            {
                picRect.X = Game1.map.getWidth() - pic.Width;
            }
            if (picRect.Y + pic.Height >= Game1.map.getHeight())
            {
                picRect.Y = Game1.map.getHeight() - pic.Height;
            }
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                if (!isMouseDown)
                {
                    int shotX = picRect.X + (int)((picRect.Width + 20) * Math.Sin(rotation));
                    int shotY = picRect.Y + (int)((picRect.Height + 20)* -Math.Cos(rotation));
                    Projectile shot = new Projectile("pShot", new Rectangle(shotX, shotY, 20, 20), .3f, color,game);
                    shot.setRotation(rotation);
                    game.addProjectile(shot);
                    isMouseDown = true;
                    if (playerMovement != null)
                    {
                        playerMovementState.X = 2;
                        playerMovementState.Y++;
                        if (playerMovementState.Y > playerMovement[playerMovementState.X].Count)
                        {
                            playerMovementState.Y = 0;
                        }
                        setSprite(playerMovement[playerMovementState.X][playerMovementState.Y]);
                    }
                }
            }
            if (mouse.LeftButton == ButtonState.Released)
            {
                isMouseDown = false;
                playerMovementState.X = 0;
                playerMovementState.Y = 0;
            }
            canMoveUp = true; canMoveDown = true; canMoveLeft = true; canMoveRight = true;
        }
        public override Being Predict(GameTime gameTime)
        {
            Player futureSelf = new Player(pic, picRect, color,game);
            futureSelf.setVelocity(new Vector2(xVel, yVel)); futureSelf.setRotation(rotation);
            return (Being)futureSelf;
        }
        public float getSpeed()
        {
            return speed;
        }
    }
}
