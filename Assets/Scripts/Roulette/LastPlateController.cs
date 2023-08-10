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

            // ���� ī�޶� �� ������ �Ƕ��⿡ �ִٸ�:
            //     ī�޶�� �� ������ �Ҷ��⸦ �ֻ��� ��ġ�� �̵���Ű��
            // �ƴ϶��:
            //     �Ҷ���� �� ���������� ����ϱ�
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
