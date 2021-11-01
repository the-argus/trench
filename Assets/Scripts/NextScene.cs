using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    void Update()
    {
        //print(SceneManager.GetActiveScene().name);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Exit")
        {
            IncrementScene();
        }
    }

    void IncrementScene()
    {
        Scene scene = SceneManager.GetActiveScene();

        // awful hardcoded scene switch
        switch (scene.name)
        {
            case "Orange":
                SceneManager.LoadScene(sceneName: "Black", LoadSceneMode.Single);
                SceneManager.UnloadSceneAsync("Orange");
                break;
            case "Black":
                SceneManager.LoadScene(sceneName: "Butterfly", LoadSceneMode.Single);
                SceneManager.UnloadSceneAsync("Black");
                break;
            case "Butterfly":
                SceneManager.LoadScene(sceneName: "Last", LoadSceneMode.Single);
                SceneManager.UnloadSceneAsync("Butterfly");
                break;
        }
    }
}
