using UnityEngine;

namespace Roulette
{
    public class PlateController : MonoBehaviour
    {
        public Director director;
        public float PlateSize;
        public float PlateIndex;

        /// <summary>
        /// 메인 카메라
        /// </summary>
        private GameObject MainCamera;

        /// <summary>
        /// 이 판때기가 카메라에 보인적이 있나요?
        /// </summary>
        private bool flag;

        private void Start()
        {
            MainCamera = director.MainCamera;
            flag = false;
        }

        private void Update()
        {
            float distance = Vector2.Distance(transform.position, MainCamera.transform.position);

            if (distance < PlateSize)
            {
                flag = true;
            }
            else if (flag)
            {
                flag = false;

                // 판때기가 카메라 밖으로 이동했습니다!
                // 이제 판때기의 다음 위치를 구해야 하는데요

                // 우선 디렉터가 가지고 있는 맨 위에 있는 판때기의 위치를 구합니다
                Vector3 nextPosition = director.TopPlate.transform.position;

                // 그 다음 판때기의 크기 만큼 더합니다
                nextPosition.y += PlateSize;

                // 그러면 그 위치가 이 판때기의 새로운 위치입니다
                transform.position = nextPosition;

                // 동시에 이 판때기는 가장 위에 있는 판때기입니다
                director.TopPlate = gameObject;
            }

            if (director.isRouletteEnabled)
            {
                transform.Translate(new Vector3(0, -director.Velocity * Time.deltaTime, 0));
            }
        }
    }
}
