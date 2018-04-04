using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour {

    public bool IsLit { get; private set; }

    public Light lightComponent;
    private Color originalColor;
    private float timePassed;
    private float originalStrength;

    [Header("Light flicker")]
    [SerializeField]
    private float speed = 12;
    [SerializeField] private float multiplier = 0.05f;
    [SerializeField] private float strength = 0.95f;

    public void Start() {
        originalColor = lightComponent.color;
        IsLit = lightComponent.gameObject.activeInHierarchy;
    }

    private void Update() {
        if (IsLit)
            LightFlicker();
    }

    private void LightFlicker() {
        timePassed = Time.time;
        timePassed -= Mathf.Floor(timePassed);

        lightComponent.color = originalColor * CalcChange();
    }

    private float CalcChange() {
        float changeValue = -Mathf.Sin(timePassed * speed * Mathf.PI) * multiplier + strength;
        return changeValue;
    }

    public void SetLightActive(bool active) {
        lightComponent.gameObject.SetActive(active);
        IsLit = active;
    }
}
