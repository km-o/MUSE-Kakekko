using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using TMPro;
using UnityEngine.SceneManagement;

public class MUSEScript : MonoBehaviour
{
    public ThirdPersonCharacter character;
    public ThirdPersonUserControl ctrl;

    public TextMeshProUGUI timeText;
    public TextMeshProUGUI speedText;

    public float count = 0f;
    public bool end = false;
    // Start is called before the first frame update
    void Start()
    {
        var server = GetComponent<uOSC.uOscServer>();
        server.onDataReceived.AddListener(OnDataReceived);
    }

    public void OnDataReceived(uOSC.Message message) 
    {
        if(end) {
            ctrl.m_Move = new Vector3(0f, 0f, 0f);
            return;
        }
        // var msg = string.Format("{0}: ", message.address);
        foreach (var value in message.values)
        {
            // if (value is float) msg += (float)value;

            float v = (value is float) ? (float)value : 0f;
            v = (float) (Math.Exp((double)v));
            v = Math.Max(v, 0f);
            Debug.Log(v);
            character.m_MoveSpeedMultiplier = v;
            this.speedText.text = "Speed: " + v.ToString("f2");
            ctrl.m_Move = new Vector3(0f, 0f, 1f);
        }
        // Debug.Log(msg);
    }

    void Update() 
    {
        count += Time.deltaTime;
        this.timeText.text = "Time: " + count.ToString("f2") + "s"; 
    }

    public void Restart()
    {
        Scene loadScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(loadScene.name);
    }
}
