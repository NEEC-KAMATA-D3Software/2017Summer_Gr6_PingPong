using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace Pinpon.Device
{
    class Sound
    {
        private ContentManager contentManager;

        private Dictionary<string, Song> bgms;
        private Dictionary<string, SoundEffect> soundEffects;
        private Dictionary<string, SoundEffectInstance> seInstances;
        private List<SoundEffectInstance> sePlayList;

        private string currentBGM;

        public Sound(ContentManager content)
        {
            contentManager = content;

            MediaPlayer.IsRepeating = true;

            bgms = new Dictionary<string, Song>();
            soundEffects = new Dictionary<string, SoundEffect>();
            seInstances = new Dictionary<string, SoundEffectInstance>();

            sePlayList = new List<SoundEffectInstance>();

            currentBGM = null;
        }

        private string ErrorMessage(string name)
        {
            return "再生する音データのアセット名(" + name + ")がありません\n"
                +
                "アセット名の確認、Dictionaryに登録されているか確認してください\n";
        }

        #region BGM 関連処理

        public void LoadBGM(string name, string filepath = "./BGM/")
        {
            if (bgms.ContainsKey(name))
            {
                return;
            }

            bgms.Add(name, contentManager.Load<Song>(filepath + name));
        }

        public bool IsStoppedBGM()
        {
            return (MediaPlayer.State == MediaState.Stopped);
        }

        public bool IsPlayingBGM()
        {
            return (MediaPlayer.State == MediaState.Playing);
        }

        public bool IsPausedBGM()
        {
            return (MediaPlayer.State == MediaState.Paused);
        }

        public void StopBGM()
        {
            MediaPlayer.Stop();
            currentBGM = null;
        }

        public void PlayBGM(string name)
        {
            Debug.Assert(bgms.ContainsKey(name), ErrorMessage(name));

            if (currentBGM == name)
            {
                return;
            }

            if (IsPlayingBGM())
            {
                StopBGM();
            }

            MediaPlayer.Volume = 0.5f;

            currentBGM = name;

            MediaPlayer.Play(bgms[currentBGM]);
        }

        public void ChangeBGMLoopFlag(bool loopFlag)
        {
            MediaPlayer.IsRepeating = loopFlag;
        }

        #endregion

        #region WAV関連

        public void LoadSE(string name, string filepath = "./SE/")
        {
            if (soundEffects.ContainsKey(name))
            {
                return;
            }

            soundEffects.Add(name, contentManager.Load<SoundEffect>(filepath + name));
        }

        public void CreateSEInstance(string name)
        {
            if (seInstances.ContainsKey(name))
            {
                return;
            }

            Debug.Assert(
                soundEffects.ContainsKey(name),
                "先に" + name + "の読み込み処理をしてください");

            seInstances.Add(name, soundEffects[name].CreateInstance());
        }

        public void PlaySE(string name)
        {
            Debug.Assert(soundEffects.ContainsKey(name), ErrorMessage(name));

            soundEffects[name].Play();
        }

        public void PlaySEInstance(string name, bool loopFlag = false)
        {
            Debug.Assert(seInstances.ContainsKey(name), ErrorMessage(name));

            var date = seInstances[name];
            date.IsLooped = loopFlag;
            date.Play();
            sePlayList.Add(date);
        }

        public void StopSE()
        {
            foreach (var se in sePlayList)
            {
                if (se.State == SoundState.Playing)
                {
                    se.Stop();
                }
            }
        }

        public void PausedSE()
        {
            foreach (var se in sePlayList)
            {
                if (se.State == SoundState.Playing)
                {
                    se.Stop();
                }
            }
        }

        public bool IsPlayingSE()
        {
            bool playingFlag = false;
            foreach (var se in sePlayList)
            {
                if (se.State == SoundState.Playing)
                {
                    playingFlag = true;
                }
            }
            return playingFlag;
        }

        public void RemoveSE()
        {
            sePlayList.RemoveAll(se => (se.State == SoundState.Stopped));
        }

        #endregion

        public void Unload()
        {
            bgms.Clear();
            soundEffects.Clear();
            sePlayList.Clear();
        }




    }
}
