using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    public static LoadingScene Instance;

    private static int _ANI_TRIGGER_OPENSCENE = Animator.StringToHash("isOpenScene");
    private static int _ANI_TRIGGER_CLOSESCENE = Animator.StringToHash("isCloseScene");

    private static string _SCENE_INGAME = "InGame";
    private static string _SCENE_HOME = "Home";
    private float _timeDelay = 3f;


    private Animator _ani;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else
        {
            Destroy(gameObject);
        }

        _ani = GetComponentInChildren<Animator>();
    }

    private IEnumerator FlowChangeScene(string sceneName)
    {
        PlayAniClose();
        yield return new WaitForSeconds(_timeDelay);
        SceneManager.LoadScene(sceneName);
        yield return new WaitForSeconds(_timeDelay);
        PlayAniOpen();
    }   
        

    // =================== Service ===================
    public void PlayAniClose() => _ani.SetTrigger(_ANI_TRIGGER_CLOSESCENE);
    public void PlayAniOpen() => _ani.SetTrigger(_ANI_TRIGGER_OPENSCENE);

    public void LoadSceneIngame()
    {
        StartCoroutine(FlowChangeScene(_SCENE_INGAME));
    }    
}
