using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDataSaver : MonoBehaviour//
{

    #region 싱글톤

    protected static SceneDataSaver instance;
    public static SceneDataSaver Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<SceneDataSaver>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    instance = Create();
                }
            }
            return instance;
        }
        private set
        {
            instance = value;
        }
    }

    public static SceneDataSaver Create()
    {
        var SceneLoaderPrefab = Resources.Load<SceneDataSaver>("SceneDataSaver");
        return Instantiate(SceneLoaderPrefab);
    }

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    #endregion

    public int loadNum = 0;

    //여기에 옵션 넣을 예정, 옵션은 세이브데이터에도 저장
}
