using Microsoft.Xna.Framework;
using Pinpon.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pinpon.Actor
{
    abstract class GameObject
    {
        protected string name; // アセット名
        protected Vector2 position; // 位置
        protected float speed;
        protected int count;
        protected Timer timer;
        protected int width;
        protected int height;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">アセット名</param>
        public GameObject(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// 抽象初期化
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// 抽象更新
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// 描画処理
        /// </summary>
        /// <param name="renderer"></param>
        public virtual void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name + StageSet.stageSet.ToString(), position); // 描画処理
        }

        /// <summary>
        /// 矩形情報の取得
        /// </summary>
        /// <returns></returns>
        public Rectangle GetRectangle()
        {
            return new Rectangle((int)position.X, (int)position.Y, width, height);
        }
    }
}
