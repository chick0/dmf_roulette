using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Roulette
{
    public class Director : MonoBehaviour
    {
        public TMP_Text Label;

        public GameObject MainCamera;

        public BangEffect bangEffect;

        public AudioSource NextPlate;

        public AudioSource Result;

        /// <summary>
        /// 최상위 판때기
        /// </summary>
        public GameObject TopPlate;

        /// <summary>
        /// 룰렛 시작 카운트 다운이 시작되었는가
        /// </summary>
        public bool isRouletteCountdown;
        /// <summary>
        /// 룰렛이 돌고있는가
        /// </summary>
        public bool isRouletteEnabled;

        [SerializeField]
        private float RouletteFalldownVelocity;

        public float Velocity => RouletteFalldownVelocity;

        [SerializeField]
        private float RouletteCountdown;
        [SerializeField]
        private float RouletteFalldowntime;

        private float _RouletteCountdown;
        private float _RouletteFalldowntime;

        private void Awake()
        {
            isRouletteCountdown = false;
            isRouletteEnabled = false;

            _RouletteCountdown = RouletteCountdown;
            _RouletteFalldowntime = RouletteFalldowntime;
        }

        private void Update()
        {
            if (isRouletteCountdown)
            {
                RouletteCountdown -= Time.deltaTime;
                Label.text = $"룰렛 시작까지 {RouletteCountdown:0.00}초";

                if (RouletteCountdown < 0)
                {
                    isRouletteCountdown = false;
                    isRouletteEnabled = true;
                }
            }

            if (isRouletteEnabled)
            {
                RouletteFalldowntime -= Time.deltaTime;
                Label.text = $"종료까지 {RouletteFalldowntime:0.00}초";

                if (!NextPlate.isPlaying)
                {
                    // 다음 판때기로 넘어가는 효과음 음소거 해제 및 재생
                    NextPlate.mute = false;
                    NextPlate.Play();
                }

                if (RouletteFalldowntime < 0)
                {
                    isRouletteEnabled = false;

                    // 다음 판때기로 넘어가는 효과음 음소거 및 멈추기
                    NextPlate.mute = true;
                    NextPlate.Stop();

                    // 룰렛 추첨 결과 계산
                    CalcRouletteResult();
                }
            }
        }

        public void Reroll()
        {
            // 상태 값 초기화
            isRouletteCountdown = false;
            isRouletteEnabled = false;

            // 타이머 값 초기화
            RouletteCountdown = _RouletteCountdown;
            RouletteFalldowntime = _RouletteFalldowntime;

            // 전체 판때기 가져온 다음
            GameObject[] plates = GameObject.FindGameObjectsWithTag("Plate");

            // 판때기를 하나씩 삭제합니다
            for (int i = 0; i < plates.Length; i++)
            {
                Destroy(plates[i]);
            }

            // 기존에 추가된 판때기 렌더러를 삭제합니다
            Destroy(GetComponent<PlateRender>());

            // 새로운 판때기 런더러를 선언해 다시 판때기를 생성합니다
            PlateRender pr = gameObject.AddComponent<PlateRender>();
            pr.director = this;
        }

        public void Exit()
        {
            SceneManager.LoadScene("MusicSelect");
        }

        public void CalcRouletteResult()
        {
            // 반짝 효과를 실행합니다
            bangEffect.Run();

            // 전체 판때기 가져온 다음
            GameObject[] plates = GameObject.FindGameObjectsWithTag("Plate");

            // 메인 카메라의 위치 = 룰렛의 중간 지점
            Vector3 MainCamera = this.MainCamera.transform.position;

            // 가장 첫번째로 선언한 판때기를 가장 가까운 판때기로 가정함
            GameObject NearByPlate = TopPlate;

            float GetDistance(GameObject ctx)
            {
                return Vector2.Distance(MainCamera, ctx.transform.position);
            }

            // 가장 가까운 판때기와의 거리
            float NearByDistance = GetDistance(NearByPlate);

            foreach (GameObject ctx in plates)
            {
                float distance = GetDistance(ctx);

                if (distance < NearByDistance)
                {
                    print($"{ctx.name}(이)가 {NearByPlate.name} 보다 더 룰렛의 기준점보다 가깝습니다. ({NearByDistance} -> {distance})");

                    NearByPlate = ctx;
                    NearByDistance = distance;
                }
            }

            // 판때기의 패턴 정보를 불러옵니다
            Plate plate = NearByPlate.GetComponent<PlateStorage>().plate;

            // UI에 선택된 패턴 정보를 표시합니다
            Label.text = $"{plate.Name} {plate.Tune}";

            // 해당 패턴을 룰렛에서 비활성화 합니다
            Store.Deactivate(plate.Name, plate.Tune);

            // 해당 판때기를 중앙으로 이동시킵니다
            NearByPlate.transform.position = new(0, 0, -1);

            Result.Play();
        }
    }
}
