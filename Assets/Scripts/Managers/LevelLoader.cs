using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator SceneTransition(string sceneName)
    {
        // Play scene transition
        // Wait until end of animation
        SceneManager.LoadScene(sceneName);
        yield return null;
    }
}
