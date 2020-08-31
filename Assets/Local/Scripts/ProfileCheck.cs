using System;
using UnityEditor;
using UnityEngine;

public class ProfileCheck : MonoBehaviour
{
    public float updateInterval = 0.5f;

    private double lastInterval;

    private int frames = 0;

    private float fps;

    private GUIStyle _fpsStype;

    private GUIStyle _otherStyle;

    [Range(10f, 320f)]
    public int frameRate = 200;

    private int oldFrameRate;

    private string fpsStr;

    public int _fps_fontsize = 50;

    public int _other_fontsize = 40;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        oldFrameRate = frameRate;
        Application.targetFrameRate = frameRate;
        lastInterval = Time.realtimeSinceStartup;
        frames = 0;
        _fpsStype = (GUIStyle)(object)new GUIStyle();
        _fpsStype.fontSize =_fps_fontsize;
        _otherStyle = (GUIStyle)(object)new GUIStyle();
        _otherStyle.fontSize = _other_fontsize;
    }

    private void OnGUI()
    {
        viewStateGUI();
    }

    private void viewStateGUI()
    {
        GUILayout.BeginVertical(("Box"), Array.Empty<GUILayoutOption>());
        if (fps < 80f)
        {
            fpsStr = string.Format("<color=#FFFFFF>FPS:</color> <color=#DD0000>{0}   FullRefuse</color>", fps.ToString("#0.00"));
        }
        else if (80f <= fps && fps < 130f)
        {
            fpsStr = string.Format("<color=#FFFFFF>FPS:</color> <color=#FFFF00>{0}   WarningRefuse</color>", fps.ToString("#0.00"));
        }
        else if (130f <= fps && fps < 180f)
        {
            fpsStr = string.Format("<color=#FFFFFF>FPS:</color> <color=#0000FF>{0}   Pass</color>", fps.ToString("#0.00"));
        }
        else
        {
            fpsStr = string.Format("<color=#FFFFFF>FPS:</color> <color=#00FF00>{0}   PerfectPass</color>", fps.ToString("#0.00"));
        }
        GUILayout.Label(fpsStr, _fpsStype, Array.Empty<GUILayoutOption>());
        #if UNITY_EDITOR
        if (UnityStats.drawCalls > 200)
        {
            GUILayout.Label($"<color=#FFFFFF>Total DrawCall: </color><color=#DD0000>{UnityStats.drawCalls}</color>", _otherStyle, Array.Empty<GUILayoutOption>());
        }
        else if (UnityStats.drawCalls > 150)
        {
            GUILayout.Label($"<color=#FFFFFF>Total DrawCall: </color><color=#FFFF00>{UnityStats.drawCalls}</color>", _otherStyle, Array.Empty<GUILayoutOption>());
        }
        else if (UnityStats.drawCalls > 60)
        {
            GUILayout.Label($"<color=#FFFFFF>Total DrawCall:</color> <color=#0000FF>{UnityStats.drawCalls}</color>", _otherStyle, Array.Empty<GUILayoutOption>());
        }
        else
        {
            GUILayout.Label($"<color=#FFFFFF>Total DrawCall: </color><color=#0000FF>{UnityStats.drawCalls}</color>", _otherStyle, Array.Empty<GUILayoutOption>());
        }

        GUILayout.Label($"<color=#FFFFFF>Batches: {UnityStats.batches}</color>", _otherStyle, Array.Empty<GUILayoutOption>());
        GUILayout.Label($"<color=#FFFFFF>SetPass: {UnityStats.setPassCalls}</color>", _otherStyle, Array.Empty<GUILayoutOption>());
        GUILayout.Label($"<color=#FFFFFF>Static Batch DC: {UnityStats.staticBatchedDrawCalls}</color>", _otherStyle, Array.Empty<GUILayoutOption>());
        GUILayout.Label($"<color=#FFFFFF>Static Batch: {UnityStats.batches}</color>", _otherStyle, Array.Empty<GUILayoutOption>());
        GUILayout.Label($"<color=#FFFFFF>DynamicBatch DC: {UnityStats.dynamicBatchedDrawCalls}</color>", _otherStyle, Array.Empty<GUILayoutOption>());
        GUILayout.Label($"<color=#FFFFFF>DynamicBatch: {UnityStats.dynamicBatches}</color>", _otherStyle, Array.Empty<GUILayoutOption>());
        GUILayout.Label($"<color=#FFFFFF>Triangle: {UnityStats.triangles}</color>", _otherStyle, Array.Empty<GUILayoutOption>());
        GUILayout.Label($"<color=#FFFFFF>Vertices: {UnityStats.vertices}</color>", _otherStyle, Array.Empty<GUILayoutOption>());
#endif
        GUILayout.EndVertical();
    }

    private void Update()
    {
        frames++;
        float realtimeSinceStartup = Time.realtimeSinceStartup;
        if ((double)realtimeSinceStartup > lastInterval + (double)updateInterval)
        {
            fps = (float)((double)frames / ((double)realtimeSinceStartup - lastInterval));
            frames = 0;
            lastInterval = realtimeSinceStartup;
            if (oldFrameRate != frameRate)
            {
                oldFrameRate = frameRate;
                Application.targetFrameRate = frameRate;
            }
        }
    }

}
