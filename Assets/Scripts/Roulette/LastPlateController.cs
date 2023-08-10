using UnityEngine;

namespace Roulette
{
    public class LastPlateController : MonoBehaviour
    {
        private GameObject MainCamera;

        private float PlateSize;
        private float topY;
        private float bottomY;
        private bool flag;

        private void Start()
        {
            PlateSize = GetComponent<BoxCollider2D>().size.y * transform.localScale.y;

            topY = PlateSize;
            bottomY = transform.position.y;
            flag = false;

            MainCamera = Camera.main.gameObject;
        }

        private void Update()
        {
            float distance = Vector2.Distance(MainCamera.transform.position, transform.position);

            // 만약 카메라가 맨 마지막 판때기에 있다면:
            //     카메라와 맨 마지막 팬때기를 최상위 위치로 이동시키기
            // 아니라면:
            //     팬때기는 맨 마지막에서 대기하기
            if (flag == true && distance < PlateSize / 4f)
            {
                transform.position = new Vector3(0, topY, 0);
                MainCamera.transform.position = new Vector3(0, topY, -10);
                flag = false;
            }
            else
            {
                transform.position = new Vector3(0, bottomY, 0);
                flag = true;
            }
        }
    }
}
