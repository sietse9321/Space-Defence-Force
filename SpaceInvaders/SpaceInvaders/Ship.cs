using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceInvaders
{
    internal class Ship
    {
        private Texture2D _ship;
        private Texture2D _bulletTexture;
        private int _health = 3;
        private Vector2 _position;
        private float _speed;
        private GraphicsDeviceManager _graphics;
        private float _delay = 1f;
        private SpriteFont _scoreFont;
        private int _score;

        private List<Bullet> _bullets;

        public Ship(Texture2D ship, Texture2D bulletTexture, Vector2 position, SpriteFont scoreFont, float speed, GraphicsDeviceManager graphics)
        {
            _ship = ship;
            _position = position;
            _speed = speed;
            _graphics = graphics;
            _bulletTexture = bulletTexture;
            _scoreFont = scoreFont;
            _bullets = new List<Bullet>();
        }
        /// <summary>
        /// public vector2 to return the _position
        /// </summary>
        public Vector2 Position
        {
            get { return _position; }
        }
        /// <summary>
        /// public int to set/change _health
        /// </summary>
        public int SetHealth
        {
            set { _health -= value; }
        }
        private bool CheckCollision(Bullet bullet, Alien alien)
        {
            Rectangle bulletRect = new Rectangle((int)bullet.Position.X, (int)bullet.Position.Y, 8, 8);
            Rectangle alienRect = new Rectangle((int)alien.Position.X - (44 / 2), (int)alien.Position.Y - (44 / 2), 44, 44);

            return bulletRect.Intersects(alienRect);
        }

        /// <summary>
        /// Check on input move ship if near wall stop it.
        /// if spacebar is pressed call shoot method then set _delay to the total gametime in seconds plus 1
        /// </summary>
        /// <param name="gameTime"></param>
        public void MoveShip(GameTime gameTime)
        {
            KeyboardState kstate = Keyboard.GetState();
            if (kstate.IsKeyDown(Keys.A))
            {
                _position.X -= _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (kstate.IsKeyDown(Keys.D))
            {
                _position.X += _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            //if ship pos is bigger then the screen set it as the position
            if (_position.X > _graphics.PreferredBackBufferWidth - _ship.Width / 2)
            {
                _position.X = _graphics.PreferredBackBufferWidth - _ship.Width / 2;
            }
            else if (_position.X < _ship.Width / 2)
            {
                _position.X = _ship.Width / 2;
            }
            if (kstate.IsKeyDown(Keys.Space) && gameTime.TotalGameTime.TotalSeconds > _delay)
            {
                ShootBullet(_bulletTexture);
                _delay = 1f + (float)gameTime.TotalGameTime.TotalSeconds;
            }
        }
        /// <summary>
        /// Create a new instance of Bullet and add it to the list
        /// </summary>
        /// <param name="bulletTexture"></param>
        public void ShootBullet(Texture2D bulletTexture)
        {
            Bullet newBullet = new Bullet(bulletTexture, new Vector2(_position.X, _position.Y - (_ship.Height / 2)), 200f, SpriteEffects.None);
            _bullets.Add(newBullet);
        }
        /// <summary>
        /// Update bullet check foreach bullet if they collided with an alien if so add score +10 and remove the alien from the list
        /// If bullet is outside viewport bounds remove it
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="aliens"></param>
        public void UpdateBullets(GameTime gameTime, List<Alien> aliens)
        {
            // Update all bullets in the list
            foreach (Bullet bullet in _bullets.ToList())
            {
                bullet.Update(gameTime);
                foreach (Alien alien in aliens.ToList())
                {
                    if (CheckCollision(bullet, alien))
                    {
                        _bullets.Remove(bullet);
                        Console.WriteLine("Bullet hit an alien!");

                        _score += 10;

                        aliens.Remove(alien);
                        Console.WriteLine("Removed alien");
                    }
                }
                // Remove bullets that are off-screen
                if (bullet.Position.Y < 0 || bullet.Position.Y > _graphics.PreferredBackBufferHeight)
                {
                    _bullets.Remove(bullet);
                    Console.WriteLine("removing bullet");
                }
            }
        }
        /// <summary>
        /// draws sprites and strings
        /// </summary>
        /// <param name="pSpriteBatch"></param>
        public void Draw(SpriteBatch pSpriteBatch)
        {
            if (_score != 110)
            {
                foreach (Bullet bullet in _bullets)
                {
                    bullet.Draw(pSpriteBatch);
                }
                pSpriteBatch.Draw(_ship, _position, null, Color.White, 0f, new Vector2(_ship.Width / 2, _ship.Height / 2), Vector2.One, SpriteEffects.None, 0f);
                pSpriteBatch.DrawString(_scoreFont, "Score: " + _score, new Vector2(900, 840), Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);
                pSpriteBatch.DrawString(_scoreFont, "Health: " + _health, new Vector2(100, 840), Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);
            }
            else
            {
                pSpriteBatch.DrawString(_scoreFont, "YOU WIN!!\nPress Escape to exit.", new Vector2(500, 512), Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);
            }
        }
    }
}