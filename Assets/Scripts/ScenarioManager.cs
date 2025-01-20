using UnityEngine;

public class ScenarioManager : MonoBehaviour
{
    private GameObject Road;
    private GameObject LandingSite;
    private Sprite RoadNight;
    private Sprite RoadMorning;
    private Sprite LandingSiteNight;
    private Sprite LandingSiteMorning;

    private bool isMorning;

    void SetIsMorning()
    {
        // Randomly decide if it's morning or night
        isMorning = Random.value > 0.5f;

        if (isMorning)
        {
            SetMorning();
        }
        else
        {
            SetNight();
        }
    }

    void SetMorning()
    {
        Road.GetComponent<SpriteRenderer>().sprite = RoadMorning;
        LandingSite.GetComponent<SpriteRenderer>().sprite = LandingSiteMorning;
    }

    void SetNight()
    {
        Road.GetComponent<SpriteRenderer>().sprite = RoadNight;
        LandingSite.GetComponent<SpriteRenderer>().sprite = LandingSiteNight;
    }
}