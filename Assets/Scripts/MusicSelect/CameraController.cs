using UnityEngine;

namespace MusicSelect
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private float Velocity = 200;

        [SerializeField]
        private float MaxY = 0;

        [SerializeField]
        private Director director;

        private void Update()
        {
            float Power = Input.mouseScrollDelta.y * Time.deltaTime * Velocity;

            transform.Translate(new(0, Power, 0));

            float safeY = Mathf.Clamp(transform.position.y, director.MusicList.Count * -PreviewDisplay.Height, PreviewDisplay.Height);

            transform.position = new Vector3(0, safeY, -10);

            if (transform.position.y > MaxY)
            {
                transform.position = new Vector3(0, MaxY, -10);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                transform.position = new Vector3(0, 0, -10);
            }
        }
    }
}
