using TMPro;
using UnityEngine;

namespace Roulette
{
    public class RouletteCalibration : MonoBehaviour
    {
        public TMP_Text Label;
        private GameObject MainCamera;
        private bool isStarted;
        private Vector3? FinalNearByPosition;

        private void Start()
        {
            MainCamera = Camera.main.gameObject;
            isStarted = false;
            FinalNearByPosition = null;
        }

        public void Run()
        {
            if (isStarted)
            {
                print("Already Running!");
                return;
            }

            isStarted = true;

            GameObject[] plates = GameObject.FindGameObjectsWithTag("Plate");

            GameObject NearByPlate = plates[0];
            float NearByDistance = Vector2.Distance(MainCamera.transform.position, NearByPlate.transform.position);

            foreach (GameObject ctx in plates)
            {
                float distance = Vector2.Distance(MainCamera.transform.position, ctx.transform.position);

                if (distance < NearByDistance)
                {
                    print($"{ctx.name}(이)가 {NearByPlate.name} 보다 더 가깝다!");

                    NearByPlate = ctx;
                    NearByDistance = distance;
                }
            }

            Plate plate = NearByPlate.GetComponent<PlateStorage>().plate;

            Label.text = $"{plate.Name} {plate.Tune}";
            FinalNearByPosition = new Vector3(0, NearByPlate.transform.position.y, -10);

            Store.Deactivate(plate.Name, plate.Tune);
        }

        private void Update()
        {
            if (isStarted && FinalNearByPosition != null)
            {
                MainCamera.transform.position = Vector3.MoveTowards(MainCamera.transform.position, (Vector3)FinalNearByPosition, 30 * Time.deltaTime);
            }
        }
    }
}
