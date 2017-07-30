using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Pinpon.Device;

namespace Pinpon.Scene
{
    class Ending : IScene
    {
        private InputState input; // 入力デバイス 
        private Sound sound; // 音
        private bool isEnd; // 終了フラグ
        private IScene gamePlay; // ゲームプレイシーン

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="gameDevice">ゲームデバイス</param>
        /// <param name="gamePlay">ゲームプレイシーン</param>
        public Ending(GameDevice gameDevice, IScene gamePlay)
        {
            input = gameDevice.GetInputState(); // 入力デバイスの取得
            sound = gameDevice.GetSound();//音の取得
            this.gamePlay = gamePlay;//ゲームプレイシーンの取得
            isEnd = false;// 終了フラグ
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            isEnd = false;//終了フラグ
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (!sound.IsPlayingSE())
            {
                sound.PlayBGM("BGM1");
            }
            //スペースが押されたら
            if (input.GetKeyTrigger(Keys.Space))
            {
                //シーン遷移SE再生
                sound.PlaySE("scenese");
                //終了し、次のシーンへ
                isEnd = true;
            }
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer"></param>
        public void Draw(Renderer renderer)
        {
            gamePlay.Draw(renderer);
            renderer.Begin();
            //勝者がPL１だったら、PL用のエンディング画像表示
            if (((GamePlay)gamePlay).Winner())
            {
                renderer.DrawTexture("ending1", Vector2.Zero);
            }
            //PL２用エンディング画像表示
            else
            {
                renderer.DrawTexture("ending2", Vector2.Zero);
            }
            renderer.End();
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Shutdown()
        {
            StageSet.stageSet += 1;
            if (StageSet.stageSet >= 4)
            {
                StageSet.stageSet = 1;
            }
        }

        /// <summary>
        /// 終了しているか？
        /// </summary>
        /// <returns></returns>
        public bool IsEnd()
        {
            return isEnd;
        }

        /// <summary>
        /// 次のシーン名
        /// </summary>
        /// <returns></returns>
        public Scene Next()
        {
            return Scene.Title;
        }
    }
}
