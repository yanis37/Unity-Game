using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Diagnostics;

public class DamagePopup : MonoBehaviour
{

    public static DamagePopup Create(Vector3 position, int damageAmount, bool isCriticalHit)
    {
        Transform damagePopupTransform = Instantiate(GameAssets.i.pfDamagePopup, position, Quaternion.identity);
        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(damageAmount, isCriticalHit);
        return damagePopup;
    }

    private const float DISAPPEAR_TIMER_MAX = 1f;
    private static int sortingOrder;

    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    private Vector3 moveVector;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(int damageAmount, bool isCriticalHit)
    {
        textMesh.SetText(damageAmount.ToString());
        textMesh.fontSize = 10;
        
        if(!isCriticalHit)
        {
            textMesh.fontSize = 8;
            textColor = CodeMonkey.Utils.UtilsClass.GetColorFromString("FFFFFF");

        } else {
            textMesh.fontSize = 11;
            textColor = CodeMonkey.Utils.UtilsClass.GetColorFromString("820404");
        }

        textMesh.color = textColor;
        disappearTimer = 0.2f;

        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;

        moveVector = new Vector3(0.7f, 1) * 5f;
    }

    private void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 8f* Time.deltaTime;

        if(disappearTimer > DISAPPEAR_TIMER_MAX * 0.5f)
        {
            // First half of popup lifetime
            float increaseScaleAmount = 1f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        } else
        {
            // Second half of popup lifetime
            float decreaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }

        disappearTimer -= Time.deltaTime;

        if (disappearTimer < 0)
        {
              // start disappearing
            float disappearSpeed = 3f;

            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;

            if (textMesh.color.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }

}
