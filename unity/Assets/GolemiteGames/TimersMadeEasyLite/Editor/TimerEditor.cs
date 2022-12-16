using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[CanEditMultipleObjects, CustomEditor(typeof(Timer))]
public class TimerEditor : Editor
{
    //Add time in seconds to editor to preview*************************************
    public SerializedProperty
        hours_Prop,
        minutes_Prop,
        seconds_Prop,
        textType_Prop,
        standardText_Prop,
        textMeshPro_Prop,
        standardSlider_Prop,
        dialSlider_Prop,
        hoursDisplay_Prop,
        minutesDisplay_Prop,
        secondsDisplay_Prop,
        countMethod_Prop,
        startAtRuntime_Prop,
        timeRemaining_Prop,
        onTimerEnd_Prop;

    static bool showTimeToSetInfo = true;
    static bool showDisplayInfo = false;
    static bool showCustomInfo = false;
    void OnEnable()
    {
        // Setup the SerializedProperties
        hours_Prop = serializedObject.FindProperty("hours");
        minutes_Prop = serializedObject.FindProperty("minutes");
        seconds_Prop = serializedObject.FindProperty("seconds");
        textType_Prop = serializedObject.FindProperty("outputType");
        standardText_Prop = serializedObject.FindProperty("standardText");
        textMeshPro_Prop = serializedObject.FindProperty("textMeshProText");
        standardSlider_Prop = serializedObject.FindProperty("standardSlider");
        dialSlider_Prop = serializedObject.FindProperty("dialSlider");
        hoursDisplay_Prop = serializedObject.FindProperty("hoursDisplay");
        minutesDisplay_Prop = serializedObject.FindProperty("minutesDisplay");
        secondsDisplay_Prop = serializedObject.FindProperty("secondsDisplay");
        countMethod_Prop = serializedObject.FindProperty("countMethod");
        startAtRuntime_Prop = serializedObject.FindProperty("startAtRuntime");
        timeRemaining_Prop = serializedObject.FindProperty("timeRemaining");
        onTimerEnd_Prop = serializedObject.FindProperty("onTimerEnd");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();


        showTimeToSetInfo = EditorGUILayout.BeginFoldoutHeaderGroup(showTimeToSetInfo, "Set time");
        if (showTimeToSetInfo)
        {
            EditorGUILayout.HelpBox("Add the days, hours, minutes and seconds to count. \n" +
                "If counting up it will start at 0 and if counting down it will start at this time. \n" +
                "You can also drop any object in the event box to run any function when the timer ends.", MessageType.None);
            EditorGUILayout.PropertyField(hours_Prop);
            EditorGUILayout.PropertyField(minutes_Prop);
            EditorGUILayout.PropertyField(seconds_Prop);
            GUI.enabled = false;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Time in seconds remaining");
            EditorGUILayout.TextField(timeRemaining_Prop.doubleValue.ToString("F0"));
            EditorGUILayout.EndHorizontal();
            GUI.enabled = true;
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(onTimerEnd_Prop);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        EditorGUILayout.Space();

        showDisplayInfo = EditorGUILayout.BeginFoldoutHeaderGroup(showDisplayInfo, "Display");
        if (showDisplayInfo)
        {
            EditorGUILayout.HelpBox("Options for displaying the set time, including how to change the seperator and linking text objects", MessageType.None);
            EditorGUILayout.PropertyField(hoursDisplay_Prop);
            EditorGUILayout.PropertyField(minutesDisplay_Prop);
            EditorGUILayout.PropertyField(secondsDisplay_Prop);

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(textType_Prop);

            Timer.OutputType tt = (Timer.OutputType)textType_Prop.enumValueIndex;

            switch (tt)
            {
                case Timer.OutputType.StandardText:

                    EditorGUILayout.ObjectField(standardText_Prop);
                    break;

                case Timer.OutputType.TMPro:

                    EditorGUILayout.ObjectField(textMeshPro_Prop);
                    break;

                case Timer.OutputType.HorizontalSlider:

                    EditorGUILayout.PropertyField(standardSlider_Prop);
                    EditorGUILayout.HelpBox("Use any custom slider as long as it uses the 'Slider' component", MessageType.Info);
                    break;

                case Timer.OutputType.Dial:

                    EditorGUILayout.PropertyField(dialSlider_Prop);
                    EditorGUILayout.HelpBox("Use any image with the type set to 'Filled' and the 'Fill Origin' set to top", MessageType.Info);
                    break;
                default:
                    break;
            }


        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        EditorGUILayout.Space();

        showCustomInfo = EditorGUILayout.BeginFoldoutHeaderGroup(showCustomInfo, "Customise");
        if (showCustomInfo)
        {
            EditorGUILayout.HelpBox("General settings for the timer", MessageType.None);
            EditorGUILayout.PropertyField(countMethod_Prop);
            EditorGUILayout.PropertyField(startAtRuntime_Prop);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();


        serializedObject.ApplyModifiedProperties();
    }
}


