// using UnityEngine;
// using UnityEditor;

// [CustomEditor(typeof(PointInfo))]
// public class PointEditor : Editor {
    
//     private SerializedObject serializePoint;//序列化
//     private SerializedProperty m_type,a_int, b_int;//定义类型，变量a，变量b


//     void OnEnable()
//     {
//         serializePoint = new SerializedObject(target);
//         m_type = serializePoint.FindProperty("PointType");//获取m_type
//         a_int = serializePoint.FindProperty("maxSpeed");//获取a_int
//         b_int = serializePoint.FindProperty("accelerateValue");//获取b_int
//     }

//     public override void OnInspectorGUI() {
//         base.OnInspectorGUI();

//         //  EditorGUILayout.PropertyField(m_type);
//         // if (m_type.enumValueIndex == 0) {//当选择第一个枚举类型
//         //     EditorGUILayout.PropertyField(a_int);
//         // }
//         // else if (m_type.enumValueIndex == 1) {
//         //     EditorGUILayout.PropertyField(b_int);
//         // }
      
//         // serializePoint.ApplyModifiedProperties();//应用
        
//     }
// }
