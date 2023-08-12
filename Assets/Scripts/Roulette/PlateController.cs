using UnityEngine;

namespace Roulette
{
    public class PlateController : MonoBehaviour
    {
        public Director director;
        public float PlateSize;
        public float PlateIndex;

        /// <summary>
        /// ���� ī�޶�
        /// </summary>
        GameObject MainCamera;

        /// <summary>
        /// �� �Ƕ��Ⱑ ī�޶� �������� �ֳ���?
        /// </summary>
        bool flag;

        void Start()
        {
            MainCamera = director.MainCamera;
            flag = false;
        }

        void Update()
        {
            float distance = Vector2.Distance(transform.position, MainCamera.transform.position);

            if (distance < PlateSize)
            {
                flag = true;
            }
            else if (flag)
            {
                flag = false;

                // �Ƕ��Ⱑ ī�޶� ������ �̵��߽��ϴ�!
                // ���� �Ƕ����� ���� ��ġ�� ���ؾ� �ϴµ���

                // �켱 ���Ͱ� ������ �ִ� �� ���� �ִ� �Ƕ����� ��ġ�� ���մϴ�
                Vector3 nextPosition = director.TopPlate.transform.position;

                // �� ���� �Ƕ����� ũ�� ��ŭ ���մϴ�
                nextPosition.y += PlateSize;

                // �׷��� �� ��ġ�� �� �Ƕ����� ���ο� ��ġ�Դϴ�
                transform.position = nextPosition;

                // ���ÿ� �� �Ƕ���� ���� ���� �ִ� �Ƕ����Դϴ�
                director.TopPlate = gameObject;
            }

            if (director.isRouletteEnabled)
            {
                transform.Translate(new Vector3(0, -director.Velocity * Time.deltaTime, 0));
            }
        }
    }
}
