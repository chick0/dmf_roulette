using System.Collections;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MusicSelect
{
    public class PreviewDisplay : MonoBehaviour
    {
        public GameObject Prefab;

        [SerializeField]
        private float Size;
        [SerializeField]
        private float Weight;

        private static PreviewDisplay instance;
        public static float Height => instance.Size + instance.Size;

        private void Awake()
        {
            instance = this;
        }

        public GameObject Render(Music music, int index = 0)
        {
            GameObject result = Instantiate(Prefab);
            result.transform.position = new Vector3(0, -Height * index, 0);
            result.name = music.Name;

            Transform canvas = result.transform.Find("Canvas");

            // 커버 이미지 교체
            _ = StartCoroutine(UpdateCover(canvas.Find("Cover").gameObject, music.Name));

            // 곡 이름 교체
            canvas.Find("Title").GetComponent<TMP_Text>().text = music.Name;

            // 비활성화된 튠의 버튼을 비활성화 합니다.
            foreach (string tune in new string[4] { "4B", "5B", "6B", "8B" })
            {
                GameObject go = canvas.Find(tune).gameObject;

                if (music.Tune.Contains(tune))
                {
                    go.GetComponent<Button>().onClick.AddListener(() => TuneClicked(music.Name, tune));

                    // 버튼 색상 자동으로 변경하도록 컴퍼넌트 추가
                    _ = go.AddComponent<TuneColor>();
                    go.GetComponent<TuneColor>().Name = music.Name;
                    go.GetComponent<TuneColor>().Tune = tune;
                }
                else
                {
                    go.SetActive(false);
                }
            }

            return result;
        }

        private IEnumerator UpdateCover(GameObject go, string name)
        {
            string path = Path.Combine(Application.streamingAssetsPath, "Cover", $"{name}.png");

            System.Threading.Tasks.Task<byte[]> request = File.ReadAllBytesAsync(path);
            request.Wait();

            Texture2D tex = new(1, 1);
            _ = tex.LoadImage(request.Result);

            go.GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0));

            yield return null;
        }

        private void TuneClicked(string name, string tune)
        {
            if (Store.IsActivate(name, tune))
            {
                Store.Deactivate(name, tune);
            }
            else
            {
                Store.Activate(name, tune);
            }
        }
    }
}
