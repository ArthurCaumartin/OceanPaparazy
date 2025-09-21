using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.Overlays;
using UnityEngine.UIElements;
using UnityEngine.Splines;
using System.Collections.Generic;
using System;
using UnityEditor.SearchService;
using UnityEngine.UI;
using Toggle = UnityEngine.UIElements.Toggle;

[Overlay(typeof(SceneView), "Spline Infos", defaultDisplay: true)]
public class SplineInfoOverlay : Overlay
{
    private struct SplineInfo
    {
        public SplineContainer splineContainer;
        public TextField distance;
        public TextField travelTime;
        public FloatField speed;
    }

    private List<SplineInfo> _splineInfoList = new List<SplineInfo>();
    private VisualElement _root;
    public override VisualElement CreatePanelContent()
    {
        _root = new VisualElement();
        UpdatePanelContent();

        Selection.selectionChanged -= UpdatePanelContent;
        Selection.selectionChanged += UpdatePanelContent;

        SceneView.duringSceneGui -= OnSceneGUI;
        SceneView.duringSceneGui += OnSceneGUI;

        return _root;
    }

    private void UpdatePanelContent()
    {
        SetSplineInfoList(Selection.GetFiltered<SplineContainer>(SelectionMode.ExcludePrefab));

        _root.Clear();
        for (int i = 0; i < _splineInfoList.Count; i++)
        {
            _root.Add(new Label("Spline Infos : " + _splineInfoList[i].splineContainer.name));
            _root.Add(_splineInfoList[i].distance);
            _root.Add(_splineInfoList[i].travelTime);
            _root.Add(_splineInfoList[i].speed);
            _root.Add(new Label("____________"));
        }
    }

    public void SetSplineInfoList(SplineContainer[] selection)
    {
        // EditorApplication.update += UpdateFloatFields;
        _splineInfoList.Clear();
        for (int i = 0; i < selection.Length; i++)
        {
            SplineInfo info = new SplineInfo();
            info.splineContainer = selection[i];

            info.distance = new TextField("Distance");
            info.distance.SetEnabled(false);

            info.travelTime = new TextField("Travel Time");
            info.travelTime.SetEnabled(false);

            info.speed = new FloatField("Speed");
            info.speed.value = 10;
            info.speed.RegisterValueChangedCallback((evt) =>
            {
                UpdateFloatFields();
            });

            _splineInfoList.Add(info);
        }

        UpdateFloatFields();
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        UpdateFloatFields();
    }

    private void UpdateFloatFields()
    {
        for (int i = 0; i < _splineInfoList.Count; i++)
        {
            Spline spline = _splineInfoList[i].splineContainer.Spline;
            if (spline == null) continue;

            float distance = spline.GetLength();
            _splineInfoList[i].distance.value = distance.ToString("F1") + "m";

            if (_splineInfoList[i].speed.value != 0)
            {
                float travelTime = distance / _splineInfoList[i].speed.value;

                _splineInfoList[i].travelTime.value =
                TimeSpan.FromSeconds(travelTime).ToString(@"mm\:ss");
            }
        }
    }
}