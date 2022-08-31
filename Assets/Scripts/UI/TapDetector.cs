using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using DG.Tweening;

public class TapDetector : MonoBehaviour
{
    // [SerializeField]
    // private bool logging = false;

    private InputManager inputManager = null;

    [SerializeField]
    private GameObject tapVisual = null;

    [SerializeField]
    private float tapDuration = 1.0f;

    [SerializeField]
    private Material material = null;

    private float border = 0.0f;

    private void Awake()
    {
        inputManager = InputManager.Instance;
        UpdateUniform();
    }

    private void OnEnable()
    {
        inputManager.OnEndTouch += Animate;
        inputManager.OnStartTouch += Hold;
    }

    private void OnDisable()
    {
        inputManager.OnEndTouch -= Animate;
        inputManager.OnStartTouch -= Hold;
    }

    private void Animate(Vector2 position, float time)
    {
        border = 1.0f;
        DOTween.To(() => border, x => border = x, 0f, tapDuration).SetEase(Ease.OutSine).OnUpdate(UpdateUniform);
    }

    private void UpdateUniform()
    {
        material.SetFloat("_Border", border);
    }

    private void Hold(Vector2 position, float time)
    {
        border = 1.0f;
        UpdateUniform();
    }

    private void Update()
    {
        tapVisual.transform.position = inputManager.PrimaryPosition2D();
    }
}
