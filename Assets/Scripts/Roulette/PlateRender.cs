using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Roulette
{
    public class Plate
    {
        public string Name;
        public string Tune;

        public float SuperSecretValue { get { return Random.Range(0f, 10f); } }

        public Plate(string name, string tune)
        {
            Name = name;
            Tune = tune;
        }
    }

    public class PlateRender : MonoBehaviour
    {
        public Director director;

        private List<Plate> PlateList = new();
        private Dictionary<string, Sprite> Plate = new();
        private int PlateWaiting = 0;

        private GameObject Prefab;

        private readonly List<Music> musicList = MusicData.GetMusicList();

        private void Start()
        {
            Prefab = Resources.Load<GameObject>("MusicPlate");

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
                print("추첨할 악곡이 없습니다.");
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

        private void PlateRener()
        {
            PlateList = PlateList.OrderBy(x => x.SuperSecretValue).ToList();

            float PlateSize = Prefab.GetComponent<BoxCollider2D>().size.y * Prefab.transform.localScale.y;

            for (int i = 0; i < PlateList.Count; i++)
            {
                Plate plate = PlateList[i];
                GameObject go = Instantiate(Prefab);
                go.name = plate.Name;

                // 정보 저장
                _ = go.AddComponent<PlateStorage>();
                go.GetComponent<PlateStorage>().plate = plate;

                // 판때기 & 버튼 이미지 설정
                Transform Canvas = go.transform.Find("Canvas");

                Canvas.Find("Plate").GetComponent<Image>().sprite = Plate[plate.Name];
                Canvas.Find("Button").GetComponent<Image>().sprite = Resources.Load<Sprite>(plate.Tune);

                go.transform.localPosition = new Vector3(0, PlateSize * i, 0);

                var pc = go.AddComponent<PlateController>();
                pc.PlateSize = PlateSize;
                pc.PlateIndex = i;
                pc.director = director;

                // 이 판때기는 가장 위에 있는 판때기이다
                if (i == 0)
                {
                    director.TopPlate = go;
                }
            }

            print("판때기 렌더링 끝");

            // 시작 카운트 다운 시작
            director.isRouletteCountdown = true;
        }
    }
}
