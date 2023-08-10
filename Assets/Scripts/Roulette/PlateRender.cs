using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Roulette
{
    public class Plate
    {
        public string Name;
        public string Tune;

        public Plate(string name, string tune)
        {
            Name = name;
            Tune = tune;
        }
    }

    public class PlateRender : MonoBehaviour
    {
        public GameObject displayPrefab;

        public Sprite Tune4;
        public Sprite Tune5;
        public Sprite Tune6;
        public Sprite Tune8;

        private readonly List<Plate> PlateList = new();
        private readonly Dictionary<string, Sprite> Plate = new();
        private int PlateWaiting = 0;

        private readonly List<Music> musicList = MusicData.GetMusicList();

        private void Start()
        {
            List<string> loadingPlate = new();

            // 불러와야 하는 판때기의 목록을 가져옵니다
            foreach (Music music in musicList)
            {
                foreach (string tune in music.Tune)
                {
                    if (Store.IsActivate(music.Name, tune))
                    {
                        PlateList.Add(new Plate(music.Name, tune));

                        if (!loadingPlate.Contains(music.Name))
                        {
                            PlateWaiting += 1;
                            loadingPlate.Add(music.Name);

                            _ = StartCoroutine(ImportPlate(music.Name));
                        }
                    }
                }
            }

            if (PlateList.Count == 0)
            {

                return;
            }

            _ = StartCoroutine(WaitForPlateRender());
        }

        private IEnumerator WaitForPlateRender()
        {
            yield return new WaitUntil(() => PlateWaiting == 0);
            print("모든 판때기가 로딩되었습니다!");

            PlateRener();
            yield return null;
        }

        private IEnumerator ImportPlate(string name)
        {
            print($"{name} 판때기 로딩 시작");

            string path = Path.Combine(Application.streamingAssetsPath, "Plate", $"{name}.png");

            System.Threading.Tasks.Task<byte[]> request = File.ReadAllBytesAsync(path);
            request.Wait();

            Texture2D tex = new(1, 1);
            _ = tex.LoadImage(request.Result);

            Plate[name] = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            PlateWaiting -= 1;

            print($"{name} 판때기 로딩 완료!");
            yield return null;
        }

        private Sprite GetTuneSprite(string tune)
        {
            return tune == "4B" ? Tune4 : tune == "5B" ? Tune5 : tune == "6B" ? Tune6 : Tune8;
        }

        private void PlateRener()
        {
            // 판때기 셔플
            _ = Utils.ShuffleList(PlateList);

            int max = PlateList.Count;

            // 판때기 복제 (2개 추가)
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < max; j++)
                {
                    PlateList.Add(PlateList[j]);
                }
            }

            float PlateSize = displayPrefab.GetComponent<BoxCollider2D>().size.y * displayPrefab.transform.localScale.y;

            for (int i = 0; i < PlateList.Count; i++)
            {
                Plate plate = PlateList[i];
                GameObject go = Instantiate(displayPrefab);
                go.name = plate.Name;

                // 정보 저장
                _ = go.AddComponent<PlateStorage>();
                go.GetComponent<PlateStorage>().plate = plate;

                // 판때기 & 버튼 이미지 저장
                Transform Canvas = go.transform.Find("Canvas");

                Canvas.Find("Plate").GetComponent<Image>().sprite = Plate[plate.Name];
                Canvas.Find("Button").GetComponent<Image>().sprite = GetTuneSprite(plate.Tune);

                go.transform.localPosition = new Vector3(0, -1 * PlateSize * i, 0);

                if ((PlateList.Count - 1) == i)
                {
                    _ = go.AddComponent<LastPlateController>();
                }
            }

            print("판때기 렌더링 끝");

            Director.isRouletteCountdown = true;
        }
    }
}
