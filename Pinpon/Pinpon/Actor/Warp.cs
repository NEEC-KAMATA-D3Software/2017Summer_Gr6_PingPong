using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Pinpon.Actor
{
    class Warp : GameObject
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="position"></param>
        public Warp(Vector2 position)
            : base("warp")
        {
            this.position = position;
            width = 32;
            height = 32;
        }

        public override void Initialize()
        {   }

        public override void Update(GameTime gameTime)
        {   }
    }

}

