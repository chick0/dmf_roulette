using UnityEngine;

namespace Roulette
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private float Velocity;

        public bool Moving { get; private set; } = false;

        public void MovingStart()
        {
            Moving = true;
        }

        public void MovingStop()
        {
            Moving = false;
        }

        private void Update()
        {
            if (Moving)
            {
                transform.Translate(new Vector3(0, -Velocity * Time.deltaTime, 0));
            }
        }
    }
}
