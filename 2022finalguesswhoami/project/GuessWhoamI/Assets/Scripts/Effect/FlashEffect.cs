using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
public class FlashEffect : MonoBehaviour
{
    private float time = 0.0f;
    private SpriteRenderer sprite;
    private float originAlpha;
    private Tweener currentTweener = null;

    public float frequency = 1.0f;
    public float scale = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        this.sprite = this.GetComponent<SpriteRenderer>();
        this.originAlpha = sprite.color.a;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time >= frequency)
        {
            time = 0.0f;
            if (this.sprite.color.a <= this.scale)
            {
                var originColor = new Color(this.sprite.color.r, this.sprite.color.g,
                    this.sprite.color.b, originAlpha);
                currentTweener = this.sprite.DOColor(originColor, frequency);
            }
            else
            {
                var newColor = new Color(this.sprite.color.r, this.sprite.color.g,
                    this.sprite.color.b, scale);
                currentTweener = this.sprite.DOColor(newColor, frequency);
            }

        }
    }

    private void OnDestroy()
    {
        if (null != currentTweener)
        {
            DOTween.Clear();
        }
    }
}
