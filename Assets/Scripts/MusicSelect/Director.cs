using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MusicSelect
{
    public class Director : MonoBehaviour
    {
        public List<Music> MusicList { get; } = MusicData.GetMusicList();

        private void Start()
        {
            for (int i = 0; i < MusicList.Count; i++)
            {
                Music music = MusicList[i];
                _ = GetComponent<PreviewDisplay>().Render(music, i);
            }
        }

        public void ActivateAll()
        {
            foreach (Music music in MusicList)
            {
                foreach (string tune in music.Tune)
                {
                    Store.Activate(music.Name, tune);
                }
            }
        }

        public void DeactivateAll()
        {
            foreach (Music music in MusicList)
            {
                foreach (string tune in music.Tune)
                {
                    Store.Deactivate(music.Name, tune);
                }
            }
        }

        public void Activate(string tune)
        {
            foreach (Music music in MusicList)
            {
                if (music.Tune.Contains(tune))
                {
                    Store.Activate(music.Name, tune);
                }
            }
        }

        public void Move()
        {
            SceneManager.LoadScene("Roulette");
        }
    }
}
