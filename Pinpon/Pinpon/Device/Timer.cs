using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Pinpon.Device
{
    class Timer
    {
        private float currentTime; // 現在の時間
        private float limitTime; // 制限時間
        private float pTime;

        /// <summary>
        /// デフォルトコンストラクタ
        /// 経過時間を設定
        /// </summary>
        public Timer()
        {
            pTime = 0;
        }

        /// <summary>
        /// 引数ありコンストラクタ
        /// 制限時間
        /// </summary>
        /// <param name="second">（引数）秒に設定</param>
        public Timer(float second)
        {
            limitTime = 60.0f * second; // second秒
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            pTime = 0.0f;
            currentTime = limitTime;
        }
        /// <summary>
        /// 制限時間の変更
        /// </summary>
        /// <param name="limitTime"></param>
        public void Change(float limitTime)
        {
            this.limitTime = limitTime;
            Initialize();
        }
        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            pTime += 0.1f;
            currentTime -= 1.0f;
            if (currentTime < 0.0f)
            {
                currentTime = 0.0f;
            }
        }

        /// <summary>
        /// 時間になったかどうか
        /// </summary>
        /// <returns>// currentTimeが0以下になったらtrue</returns>
        public bool IsTime()
        {
            return currentTime <= 0.0f;
        }
        /// <summary>
        /// 現在の時間を取得
        /// </summary>
        /// <returns>現在の時間</returns>
        public float Now()
        {
            return pTime;
        }

        /// <summary>
        /// 残り時間を取得
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public int Now(int a)
        {
            return (int)currentTime + 1;
        }

        /// <summary>
        /// 割合
        /// </summary>
        /// <returns></returns>
        public float Rate()
        {
            return currentTime / limitTime;
        }
    }
}
