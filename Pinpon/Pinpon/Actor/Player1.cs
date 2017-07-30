using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pinpon.Device;

namespace Pinpon.Actor
{
    class Player1 : GameObject
    {
        private InputState input; // 入力デバイス

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="input"></param>
        public Player1(InputState input) : base("player1")
        {
            this.input = input;
            width = 16;
            height = 128;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            position = new Vector2(50f - width, Screen.height / 2 - height / 2);//初期位置
            speed = 6.0f;
            timer = new Timer(1.0f);
            timer.Initialize();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            timer.Update(); // 60フレームカウント
            //60フレームごとに速度上昇
            if (timer.IsTime())
            {
                speed += 0.8f;
                timer.Initialize();
            }
            //スピードは9未満
            if (speed > 9.0f)
            {
                speed = 9.0f;
            }
            //移動処理
            position = position + input.P1Velocity() * speed;
            //壁の判定
            if (position.Y < 50)
            {
                position.Y = 50;
            }
            if (position.Y > Screen.height - height - 50)
            {
                position.Y = Screen.height - height - 50;
            }
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer"></param>
        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, position);
        }
    }
}
