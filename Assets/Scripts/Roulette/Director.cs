using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Roulette
{
    public class Director : MonoBehaviour
    {
        public static bool isRouletteCountdown;

        public TMP_Text Label;

        public CameraController cameraController;
        public RouletteCalibration calibration;

        [SerializeField]
        private float RouletteCountdown;
        [SerializeField]
        private float RouletteFalldowntime;

        private void Update()
        {
            if (isRouletteCountdown)
            {
                RouletteCountdown -= Time.deltaTime;
                Label.text = $"�귿 ���۱��� {RouletteCountdown:0.00}��";

                if (RouletteCountdown < 0)
                {
                    isRouletteCountdown = false;
                    cameraController.MovingStart();
                }
            }

            if (cameraController.Moving)
            {
                RouletteFalldowntime -= Time.deltaTime;
                Label.text = $"������� {RouletteFalldowntime:0.00}��";

                if (RouletteFalldowntime < 0)
                {
                    cameraController.MovingStop();
                    calibration.Run();
                }
            }
        }

        public void Reroll()
        {
            if (!cameraController.Moving)
            {
                SceneManager.LoadScene("RerollReady");
            }
        }

        public void Exit()
        {
            SceneManager.LoadScene("MusicSelect");
        }
    }
}
