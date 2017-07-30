using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Pinpon.Actor;
using Pinpon.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pinpon.Scene
{
    class GamePlay : IScene
    {
        private GameDevice gameDevice; // ゲームデバイス
        private InputState input; // 入力オブジェクト
        private Sound sound;
        private bool isEnd; // シーン終了フラグ
        private static Random rand; // ランダム

        private Ball ball; // Ballクラス
        private Player1 player1;
        private Player2 player2;
        private Warp warp1, warp2;

        private Timer timer; // 経過時間測定
        private Timer startTime; // 開始時間測定

        private List<GameObject> gameObjects; // ゲームオブジェクト用リスト

        private int player1Point; //PLのポイント
        private int player2Point;

        private bool startFlag = false;  // 開始フラグ

        private Vector2 ballFirstPosition = new Vector2(400, 300);


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="gameDevice"></param>
        public GamePlay(GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            input = gameDevice.GetInputState();
            sound = gameDevice.GetSound();
            isEnd = false;
        }

        /// <summary>
        /// シーンの初期化
        /// </summary>
        public void Initialize()
        {
            // ゲームの経過時間測定用タイマー
            timer = new Timer();

            //ゲーム開始時間カウント用タイマーの実体と初期化
            startTime = new Timer(1);

            //乱数
            rand = new Random();

            //ボールの実体と初期化
            ball = new Ball(ballFirstPosition);
            gameObjects = new List<GameObject>();
            player1 = new Player1(input);
            player2 = new Player2(input);
            warp1 = new Warp(new Vector2(Screen.width / 2 - 16, Screen.height / 3 - 16));
            warp2 = new Warp(new Vector2(Screen.width / 2 - 16, Screen.height / 3 * 2 - 16));
            gameObjects.Add(player1);
            gameObjects.Add(player2);
            gameObjects.Add(warp1);
            gameObjects.Add(warp2);

            //ポイントの初期化
            player1Point = 0;
            player2Point = 0;
            //プレイ時間の初期化
            timer.Initialize();
            //リスタート処理
            Restart();
            //終了フラグ
            isEnd = false;
        }
        /// <summary>
        /// ゲームのリスタート
        /// </summary>
        public void Restart()
        {
            //障害物のクリア
            gameObjects.RemoveAll(b => b is Barrier);
            //開始時間の初期化
            startTime.Initialize();
            //ボールの初期化
            ball.Initialize();
            //開始フラグをfalseに
            startFlag = false;
            //ボールを初期位置にセット
            ball.SetPosition(ballFirstPosition);
            //障害物の配置
            for (int i = 0; i < 4; i++)
            {
                gameObjects.Add(new Barrier(new Vector2
                    (rand.Next(100 + (i / 2) * 350, 350 + (i / 2) * 350 - 32),
                    (rand.Next(100 + (i % 2) * 250, 250 + (i % 2) * 250 - 32)))));
            }
            //  ゲームオブジェクトの初期化
            gameObjects.ForEach(gameobject => gameobject.Initialize());
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            gameDevice.Update(gameTime);
            //ゲームが開始していると
            if (startFlag)
            {
                #region 壁の判定
                //ボールの向きを取得
                Direction direction = ball.GetCurrentDirection();
                GameObject g = SimpleCollision();
                if (g != null)
                {
                    Vector2 delta = ComplexCollision(g);
                    Reflect(g, delta);
                }

                //上の壁衝突判定
                if (ball.GetRectangle().Top < 50f)
                {
                    ball.ReflectY(0.0f);
                    sound.PlaySE("hitse");
                }
                //下の壁衝突判定
                if (ball.GetRectangle().Bottom > Screen.height - 50)
                {
                    ball.ReflectY(0.0f);
                    sound.PlaySE("hitse");
                }

                //上の壁の食い込み処理
                if (ball.GetRectangle().Top < 50f)
                {
                    ball.RevisionY(-(ball.GetRectangle().Top - 50f));
                }
                //下の壁の食い込み処理
                if (ball.GetRectangle().Bottom > Screen.height - 50)
                {
                    ball.RevisionY(-(ball.GetRectangle().Bottom - (Screen.height - 50)));
                }

                #endregion
                //各オブジェクトの更新処理
                gameObjects.ForEach(gameObject => gameObject.Update(gameTime));
                //ボールの更新
                ball.Update(gameTime);
                //タイマーの更新
                timer.Update();
                //BGMの再生
                sound.PlayBGM("BGM2");

                #region 得点
                var ballPosition = ball.GetRectangle().Location;
                //画面外に出たら点を獲得
                if ((ballPosition.X <= -50.0f) || (ballPosition.X > Screen.width - ball.GetRectangle().Width + 50))
                {
                    //左端はPL2の得点
                    if (ballPosition.X <= -50.0f)
                    {
                        player2Point++;
                        sound.PlaySE("goalse");
                    }
                    //右端はPL1の得点
                    else if (ballPosition.X > Screen.width - ball.GetRectangle().Width + 50)
                    {
                        player1Point++;
                        sound.PlaySE("goalse");
                    }

                    //３点先取で終了したかどうか？
                    if (player1Point >= 3 || player2Point >= 3)
                    {
                        isEnd = true;
                        sound.PlaySE("fanfarese");
                    }
                    //終わっていなかったらリスタート
                    else
                    {
                        Restart();
                    }
                }
            }
            #endregion
            //ゲームが開始していないとき
            else
            {
                //カウントダウンを更新する
                startTime.Update();
                //カウントダウンが終了したら
                if (startTime.IsTime())
                {
                    //デュエル開始
                    sound.PlaySE("whistlese");
                    startFlag = true;
                }
            }
        }

        /// <summary>
        /// 簡易的な衝突判定
        /// </summary>
        /// <returns></returns>
        private GameObject SimpleCollision()
        {
            // ゲームオブジェクトのすべてに対して
            foreach (var gameObject in gameObjects)
            {
                // まずは矩形で衝突したかどうか判定
                if (ball.GetRectangle(ball.GetMovement()).Intersects(gameObject.GetRectangle()))
                {
                    //衝突していたら詳細判定に移行
                    return gameObject;
                }
            }
            return null;
        }
        /// <summary>
        ///複雑な衝突判定
        /// </summary>
        /// <param name="c">簡易衝突したオブジェクト</param>
        /// <returns></returns>
        private Vector2 ComplexCollision(GameObject c)
        {
            //ボールの移動している向きを取得
            Direction direction = ball.GetCurrentDirection();

            Vector2 delta = Vector2.Zero;
            // Ballの移動量を1ずつ最大移動量まで加算する
            for (int i = 0; i < ball.GetMovement().Y; i++)
            {
                for (int j = 0; j < ball.GetMovement().X; j++)
                {
                    //方向に応じた差分を取得
                    switch (direction)
                    {
                        case Direction.RIGHTUP:
                            delta = new Vector2(i, -j);
                            break;
                        case Direction.RIGHTDOWN:
                            delta = new Vector2(i, j);
                            break;
                        case Direction.LEFTDOWN:
                            delta = new Vector2(-i, j);
                            break;
                        default:
                            delta = new Vector2(-i, -j);
                            break;
                    }
                    //ボールと障害物の衝突判定詳細版
                    if (ball.IsCollision(c.GetRectangle(), delta))
                    {
                        //衝突したときの差分を返す
                        return delta;
                    }
                }
            }
            //衝突していなかったらとりあえず0を返す
            return Vector2.Zero;
        }

        /// <summary>
        /// 反射処理
        /// </summary>
        /// <param name="c">衝突したオブジェクト</param>
        /// <param name="delta">差分</param>
        private void Reflect(GameObject c, Vector2 delta)
        {
            // オブジェクトの縦横比
            float ratio = 1.0f;
            if (c is Player1 || c is Player2)
            {
                ratio = 0.1875f;
            }
            // ボールの中心の座標を取得
            var ballCenter = ball.GetRectangle(delta).Center;
            // 障害物の中心の座標を取得
            var otherCenter = c.GetRectangle().Center;
            //ボールと障害物の距離を取得
            Vector2 dis = new Vector2(ballCenter.X - otherCenter.X, ballCenter.Y - otherCenter.Y);

            //ワープ以外のオブジェクトの反射
            if (!(c is Warp))
            {
                //横から衝突しているなら
                if (Math.Abs(dis.X) >= Math.Abs(dis.Y * ratio))
                {
                    //横に差分だけ押し出して反射
                    ball.ReflectX((ball.GetMovement() - delta).X);
                }

                //縦から衝突しているなら
                else
                {
                    //衝突しているのがPL1またはPL2なら食い込みを直して反射
                    if (c is Player1)
                    {
                        ball.ReflectY((ball.GetMovement() - delta - (input.P1Velocity() * 10)).Y);
                        if (input.P1Velocity().Y != 0)
                        {
                            ball.RevisionY(input.P1Velocity().Y / 10);
                        }
                    }
                    else if (c is Player2)
                    {
                        ball.ReflectY((ball.GetMovement() - delta - (input.P2Velocity() * 10)).Y);
                        if (input.P2Velocity().Y != 0)
                        {
                            ball.RevisionY(input.P2Velocity().Y / 10);
                        }
                    }
                    //障害物の反射処理
                    else
                    {
                        ball.ReflectY(ball.GetMovement().Y - delta.Y);
                    }
                }
                //衝突音再生
                sound.PlaySE("hitse");
            }
            //ワープと衝突時の処理
            else
            {
                if (c == warp1)
                {
                    Warp(warp1, warp2, delta);
                }
                else
                {
                    Warp(warp2, warp1, delta);
                }
            }
            if (ball.IsCollision(c.GetRectangle(), delta))
            {
                ball.RevisionX(ballCenter.X - otherCenter.X);
            }
        }

        /// <summary>
        /// ワープ処理
        /// </summary>
        /// <param name="collisionWarp">衝突したオブジェクト</param>
        /// <param name="warpPoint">移動先のオブジェクト</param>
        /// <param name="direction"></param>
        private void Warp(Warp collisionWarp, Warp warpPoint, Vector2 delta)
        {
            Vector2 dis =
                new Vector2((ball.GetRectangle(delta).Center.X - collisionWarp.GetRectangle().Center.X),
                (ball.GetRectangle(delta).Center.Y - collisionWarp.GetRectangle().Center.Y));

            ball.SetPosition(new Vector2(warpPoint.GetRectangle().X, warpPoint.GetRectangle().Y) - dis);
            sound.PlaySE("warpse");
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer"></param>
        public void Draw(Renderer renderer)
        {
            //描画開始
            renderer.Begin();
            //各ゲームオブジェクトの描画
            gameObjects.ForEach(gameObject => gameObject.Draw(renderer));
            ball.Draw(renderer);
            //壁の描画
            renderer.DrawTexture("wall" + StageSet.stageSet.ToString(), Vector2.Zero);
            renderer.DrawTexture("wall" + StageSet.stageSet.ToString(), new Vector2(0, Screen.height - 50));

            //ゲームの経過時間の表示
            string gameTime = (timer.Now() / 6).ToString();
            if (gameTime.Length > 4)
            {
                renderer.DrawNumber("number", new Vector2(Screen.width / 2 - 64, 10), Color.White, gameTime, 4, 0.7f);
            }
            else
            {
                renderer.DrawNumber("number", new Vector2(Screen.width / 2 - 64, 10), Color.White, gameTime, gameTime.Length, 0.7f);
            }

            //得点板の表示
            string p1score = player1Point.ToString();
            string p2score = player2Point.ToString();
            renderer.DrawNumber("number", new Vector2(Screen.width / 2 - 64 - 50, 10), Color.Blue, p1score, 1, 0.7f);
            renderer.DrawNumber("number", new Vector2(Screen.width / 2 - 64 + 150, 10), Color.Red, p2score, 1, 0.7f);

            //開始前のカウントの表示
            if (startTime.Rate() >= 0.80f)
            {
                renderer.DrawNumber("number", new Vector2(Screen.width / 2 - 16, Screen.height / 2), 3);
            }
            else if (startTime.Rate() >= 0.60f)
            {
                renderer.DrawNumber("number", new Vector2(Screen.width / 2 - 16, Screen.height / 2), 2);
            }
            else if (startTime.Rate() >= 0.40f)
            {
                renderer.DrawNumber("number", new Vector2(Screen.width / 2 - 16, Screen.height / 2), 1);
            }
            else if (startTime.Rate() >= 0.20f && startTime.Rate() >= 0.0f)
            {
                renderer.DrawTexture("start", Vector2.Zero);
            }
            renderer.End();

        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Shutdown()
        {
            sound.StopBGM();
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
            return Scene.Ending;
        }

        /// <summary>
        /// 勝者を渡す
        /// </summary>
        /// <returns>PL1が勝利していたらtrue</returns>
        public bool Winner()
        {
            if (player1Point > player2Point)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}
