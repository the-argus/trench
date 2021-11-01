using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealTracker : MonoBehaviour
{
    private int Medicine = 5;
    [SerializeField] GameObject healprompt;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && healprompt.active)
        {
            Scene scene = SceneManager.GetActiveScene();
            switch (scene.name)
            {
                case "Orange":
                    if (Medicine >= 2)
                    {
                        Medicine -= 2;
                    }
                    break;
                case "Black":
                    if (Medicine >= 3)
                    {
                        Medicine -= 3;
                    }
                    break;
            }
            healprompt.SetActive(false);
        }
    }
}
