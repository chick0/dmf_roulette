using UnityEngine;
using UnityEngine.SceneManagement;

namespace RouletteReady
{
    public class Director : MonoBehaviour
    {
        private void Start()
        {
            SceneManager.LoadScene("Roulette");
        }
    }
}
