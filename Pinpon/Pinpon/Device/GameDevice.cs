using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Pinpon.Device
{
    class GameDevice
    {
        private Renderer renderer;
        private InputState input;
        private static Random rand;
        private Sound sound;

        public GameDevice(ContentManager contentManager, GraphicsDevice graphics)
        {
            renderer = new Renderer(contentManager, graphics);
            sound = new Sound(contentManager);
            input = new InputState();
            rand = new Random();
        }

        public void Initialize()
        {

        }

        public void Update(GameTime gameTime)
        {
            input.Update();
        }

        public Renderer GetRenderer()
        {
            return renderer;
        }

        public InputState GetInputState()
        {
            return input;
        }

        public Sound GetSound()
        {
            return sound;
        }

        public Random GetRandom()
        {
            return rand;
        }
    }
}
