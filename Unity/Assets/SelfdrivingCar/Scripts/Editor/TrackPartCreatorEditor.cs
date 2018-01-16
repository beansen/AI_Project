using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TrackPartCreator))]
public class TrackPartCreatorEditor : Editor {

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
        
		TrackPartCreator trackPartCreator = (TrackPartCreator)target;
		
		if(GUILayout.Button("Create Curve"))
		{
			trackPartCreator.CreateCurve();
		}
		
		if(GUILayout.Button("Create Straight"))
		{
			trackPartCreator.CreateStraight();
		}
	}
}
