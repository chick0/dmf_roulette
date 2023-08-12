using UnityEngine;
using UnityEngine.UI;

namespace Roulette
{
    public class BangEffect : MonoBehaviour
    {
        private RawImage rw;
        private Color rwColor;

        private bool isStarted;

        [SerializeField]
        private float effectTime;

        private float timeflow = 0;

        private void Start()
        {
            rw = GetComponent<RawImage>();

            rwColor = rw.color;
            rwColor.a = 0;

            rw.color = rwColor;
        }

        private void Update()
        {
            if (isStarted)
            {
                timeflow += Time.deltaTime;

                rwColor.a = 1f - (timeflow / effectTime);
                rw.color = rwColor;
            }

            if (timeflow >= effectTime)
            {
                isStarted = false;
            }
        }

        public void Run()
        {
            isStarted = true;
            timeflow = 0;
        }
    }
}
