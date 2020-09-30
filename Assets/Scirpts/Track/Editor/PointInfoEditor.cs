using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;

namespace Track
{
    [CustomEditor(typeof(PointManager))]
    public class PointManagerEditor : Editor
    {
        private PointManager pointMgr;
        private List<PointInfo> pointInfos;

        void OnEnable()
        {
            pointMgr = (PointManager)target;
            pointInfos = pointMgr.pointList;
        }

        public override void OnInspectorGUI()
        {
                        
            if (GUILayout.Button("Add Point"))
            {
                pointMgr.addPointInfo();
            }
            EditorGUILayout.LabelField("--------------------------------------------------------------------------------------");
            //EditorGUILayout.Space();



            if (pointInfos != null)
            {
                for (int i = 0; i < pointInfos.Count; i++)
                {
                    var tempPointInfo = pointInfos[i];
                    //Debug.Log("poininfo  "+ tempPointInfo.animationName +" fadetime "+tempPointInfo.animationFadeTime);
                    EditorGUILayout.BeginVertical("Box");

                    //显示Point序号， 删除
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Point: "+i,GUILayout.MaxWidth(150));
                    if (GUILayout.Button("Delete"))
                    {
                        if(EditorUtility.DisplayDialog("WARN","确认删除该PointInfo ？" ,"delete","cancel"))
                        {
                            pointMgr.removePointInfo(tempPointInfo);
                            continue;
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    //显示该点位Aniamtion信息
                    GUILayout.BeginHorizontal("Button");
                    GUILayout.Label("AnimateName");
                    tempPointInfo.animationName = EditorGUILayout.TextField(tempPointInfo.animationName, GUILayout.MinWidth(30));
                    GUILayout.Space(20);
                    GUILayout.Label("FadeTime");
                    tempPointInfo.animationFadeTime = EditorGUILayout.FloatField(tempPointInfo.animationFadeTime, GUILayout.MinWidth(30), GUILayout.MaxWidth(80));
                    GUILayout.Space(20);
                    GUILayout.EndHorizontal();

                    //显示动画的模式，分别设置参数
                    GUILayout.BeginVertical("Button");
                    tempPointInfo.pointType = (PointType)EditorGUILayout.EnumPopup("PointType:", tempPointInfo.pointType);
                    switch ((PointType)tempPointInfo.pointType)
                    {
                        case PointType.AttractPoint:
                            tempPointInfo.maxSpeed = EditorGUILayout.FloatField("maxSpeed:", tempPointInfo.maxSpeed);
                            tempPointInfo.accelerateValue = EditorGUILayout.FloatField("acceValue:", tempPointInfo.accelerateValue);
                            tempPointInfo.touchRange = EditorGUILayout.FloatField("touchRange:", tempPointInfo.touchRange);
                            tempPointInfo.rotateFadeTime = EditorGUILayout.FloatField("rotateFadeTime:", tempPointInfo.rotateFadeTime);
                            tempPointInfo.animatorSpeedRate = EditorGUILayout.FloatField("animatorSpeedRate:", tempPointInfo.animatorSpeedRate);
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Label("isFadeStop");
                            tempPointInfo.openFadeStop = EditorGUILayout.Toggle(tempPointInfo.openFadeStop,GUILayout.MinWidth(20));
                            GUILayout.Label("控制最小速度在 0.1f以上");
                            EditorGUILayout.EndHorizontal();

                            if(tempPointInfo.openFadeStop)
                            {
                                EditorGUILayout.BeginHorizontal();
                                GUILayout.MinWidth(10);
                                GUILayout.Label("fadeLength");
                                tempPointInfo.fadeStopLength = EditorGUILayout.FloatField(tempPointInfo.fadeStopLength,GUILayout.MinWidth(20));
                                GUILayout.MinWidth(10);
                                GUILayout.Label("fadeMinSpeed");
                                tempPointInfo.fadeMinSpeed = EditorGUILayout.FloatField(tempPointInfo.fadeMinSpeed,GUILayout.MinWidth(20));

                                EditorGUILayout.EndHorizontal();
                            }
                            break;

                            case PointType.SelfControlPoint:
                                tempPointInfo.maxSpeed = EditorGUILayout.FloatField("maxSpeed:", tempPointInfo.maxSpeed);
                                tempPointInfo.maxAccelerateValue = EditorGUILayout.FloatField("maxAcceValue:", tempPointInfo.maxAccelerateValue);
                                tempPointInfo.touchRange = EditorGUILayout.FloatField("touchRange:", tempPointInfo.touchRange);
                                tempPointInfo.rotateFadeTime = EditorGUILayout.FloatField("rotateFadeTime:", tempPointInfo.rotateFadeTime);
                                tempPointInfo.animatorSpeedRate = EditorGUILayout.FloatField("animatorSpeedRate:", tempPointInfo.animatorSpeedRate);
                                EditorGUILayout.BeginHorizontal();
                                GUILayout.Label("isFadeStop");
                                tempPointInfo.openFadeStop = EditorGUILayout.Toggle(tempPointInfo.openFadeStop,GUILayout.MinWidth(20));
                                GUILayout.Label("控制最小速度在 0.1f以上");
                                EditorGUILayout.EndHorizontal();

                                if(tempPointInfo.openFadeStop)
                                {
                                    EditorGUILayout.BeginHorizontal();
                                    GUILayout.MinWidth(10);
                                    GUILayout.Label("fadeLength");
                                    tempPointInfo.fadeStopLength = EditorGUILayout.FloatField(tempPointInfo.fadeStopLength,GUILayout.MinWidth(20));
                                    GUILayout.MinWidth(10);
                                    GUILayout.Label("fadeMinSpeed");
                                    tempPointInfo.fadeMinSpeed = EditorGUILayout.FloatField(tempPointInfo.fadeMinSpeed,GUILayout.MinWidth(20));
                    
                                    EditorGUILayout.EndHorizontal();
                                }
                
                                break;


                        case PointType.DelayPoint:
                            tempPointInfo.delayTime = EditorGUILayout.FloatField("delayTime:",tempPointInfo.delayTime, GUILayout.MinWidth(50));
                            tempPointInfo.saveLastSpeed = EditorGUILayout.FloatField("saveLastSpeed:",tempPointInfo.saveLastSpeed, GUILayout.MinWidth(50));
                            break;
                    }
                    GUILayout.EndVertical();

                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space();
                }
            }
            
            if (GUI.changed)
            {
                
                EditorUtility.SetDirty(target);
            }

        }
    }
}
