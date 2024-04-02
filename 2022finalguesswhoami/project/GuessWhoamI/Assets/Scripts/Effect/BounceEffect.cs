using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BounceEffect : MonoBehaviour
{
    private float time = 0.0f;
    private Vector3 originScale;
    private Tweener currentTweener = null;

    public float frequency = 1.0f;
    public float size = 1.1f;
    // Start is called before the first frame update
    void Start()
    {
        originScale = this.transform.localScale;
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        
        if (time >= frequency)
        {
            time = 0.0f;
            if (this.transform.localScale == originScale)
            {
                currentTweener = transform.DOScale(size, frequency);
            }
            else
            {
                currentTweener = transform.DOScale(1, frequency);
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
