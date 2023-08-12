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
        /// �ֻ��� �Ƕ���
        /// </summary>
        public GameObject TopPlate;

        /// <summary>
        /// �귿 ���� ī��Ʈ �ٿ��� ���۵Ǿ��°�
        /// </summary>
        public bool isRouletteCountdown;
        /// <summary>
        /// �귿�� �����ִ°�
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
                Label.text = $"�귿 ���۱��� {RouletteCountdown:0.00}��";

                if (RouletteCountdown < 0)
                {
                    isRouletteCountdown = false;
                    isRouletteEnabled = true;
                }
            }

            if (isRouletteEnabled)
            {
                RouletteFalldowntime -= Time.deltaTime;
                Label.text = $"������� {RouletteFalldowntime:0.00}��";

                if (!NextPlate.isPlaying)
                {
                    // ���� �Ƕ���� �Ѿ�� ȿ���� ���Ұ� ���� �� ���
                    NextPlate.mute = false;
                    NextPlate.Play();
                }

                if (RouletteFalldowntime < 0)
                {
                    isRouletteEnabled = false;

                    // ���� �Ƕ���� �Ѿ�� ȿ���� ���Ұ� �� ���߱�
                    NextPlate.mute = true;
                    NextPlate.Stop();

                    // �귿 ��÷ ��� ���
                    CalcRouletteResult();
                }
            }
        }

        public void Reroll()
        {
            // ���� �� �ʱ�ȭ
            isRouletteCountdown = false;
            isRouletteEnabled = false;

            // Ÿ�̸� �� �ʱ�ȭ
            RouletteCountdown = _RouletteCountdown;
            RouletteFalldowntime = _RouletteFalldowntime;

            // ��ü �Ƕ��� ������ ����
            GameObject[] plates = GameObject.FindGameObjectsWithTag("Plate");

            // �Ƕ��⸦ �ϳ��� �����մϴ�
            for (int i = 0; i < plates.Length; i++)
            {
                Destroy(plates[i]);
            }

            // ������ �߰��� �Ƕ��� �������� �����մϴ�
            Destroy(GetComponent<PlateRender>());

            // ���ο� �Ƕ��� �������� ������ �ٽ� �Ƕ��⸦ �����մϴ�
            PlateRender pr = gameObject.AddComponent<PlateRender>();
            pr.director = this;
        }

        public void Exit()
        {
            SceneManager.LoadScene("MusicSelect");
        }

        public void CalcRouletteResult()
        {
            // ��¦ ȿ���� �����մϴ�
            bangEffect.Run();

            // ��ü �Ƕ��� ������ ����
            GameObject[] plates = GameObject.FindGameObjectsWithTag("Plate");

            // ���� ī�޶��� ��ġ = �귿�� �߰� ����
            Vector3 MainCamera = this.MainCamera.transform.position;

            // ���� ù��°�� ������ �Ƕ��⸦ ���� ����� �Ƕ���� ������
            GameObject NearByPlate = TopPlate;

            float GetDistance(GameObject ctx)
            {
                return Vector2.Distance(MainCamera, ctx.transform.position);
            }

            // ���� ����� �Ƕ������ �Ÿ�
            float NearByDistance = GetDistance(NearByPlate);

            foreach (GameObject ctx in plates)
            {
                float distance = GetDistance(ctx);

                if (distance < NearByDistance)
                {
                    print($"{ctx.name}(��)�� {NearByPlate.name} ���� �� �귿�� ���������� �������ϴ�. ({NearByDistance} -> {distance})");

                    NearByPlate = ctx;
                    NearByDistance = distance;
                }
            }

            // �Ƕ����� ���� ������ �ҷ��ɴϴ�
            Plate plate = NearByPlate.GetComponent<PlateStorage>().plate;

            // UI�� ���õ� ���� ������ ǥ���մϴ�
            Label.text = $"{plate.Name} {plate.Tune}";

            // �ش� ������ �귿���� ��Ȱ��ȭ �մϴ�
            Store.Deactivate(plate.Name, plate.Tune);

            // �ش� �Ƕ��⸦ �߾����� �̵���ŵ�ϴ�
            NearByPlate.transform.position = new(0, 0, -1);

            Result.Play();
        }
    }
}
