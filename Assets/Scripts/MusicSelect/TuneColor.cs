using UnityEngine;
using UnityEngine.UI;

namespace MusicSelect
{
    public class TuneColor : MonoBehaviour
    {
        public string Name = "";
        public string Tune = "";

        private Image image;

        private void Start()
        {
            image = GetComponent<Image>();
        }

        private void Update()
        {
            image.color = Store.IsActivate(Name, Tune) ? Color.white : (Color)new Color32(40, 40, 40, 255);
        }
    }
}
