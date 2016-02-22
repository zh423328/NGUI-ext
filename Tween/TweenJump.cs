using UnityEngine;
using System.Collections;

[AddComponentMenu("NGUI/Tween/Tween Jump")]
public class TweenJump : UITweener
{
    public Vector3 from;

    public float mHeight = 0.5f;     //高度
    public float m_nJumps = 1.0f;
    public Vector3 m_delta;

    [HideInInspector]
    public bool worldSpace = false;

    Transform mTrans;
    UIRect mRect;

    public Transform cachedTransform
    {
        get
        {
            if(mTrans == null) mTrans = transform;

            return mTrans;
        }
    }


    /// <summary>
    /// Tween's current value.
    /// </summary>

    public Vector3 value
    {
        get
        {
            return worldSpace ? cachedTransform.position : cachedTransform.localPosition;
        }
        set
        {
            if(mRect == null || !mRect.isAnchored || worldSpace)
            {
                if(worldSpace) cachedTransform.position = value;
                else cachedTransform.localPosition = value;
            }
            else
            {
                value -= cachedTransform.localPosition;
                NGUIMath.MoveRect(mRect, value.x, value.y);
            }
        }
    }

    void Awake()
    {
        mRect = GetComponent<UIRect>();
    }

    /// <summary>
    /// Tween the value.
    /// </summary>

    protected override void OnUpdate(float factor, bool isFinished)
    {
        float frac = factor * m_nJumps % 1.0f;
        float y = mHeight * 4 * frac * (1 - frac);
        y += m_delta.y * factor;
        float x = m_delta.x * factor;
        value = from + new Vector3(x, y, 0.0f);
    }

    /// <summary>
    /// Start the tweening operation.
    /// </summary>

    static public TweenJump Begin(GameObject go, float duration, Vector3 pos, float heights, float nJumps)
    {
        TweenJump comp = UITweener.Begin<TweenJump>(go, duration);
        comp.from = comp.value;
        comp.m_delta = pos;
        comp.mHeight = heights;
        comp.m_nJumps = nJumps;

        if(duration <= 0f)
        {
            comp.Sample(1f, true);
            comp.enabled = false;
        }

        return comp;
    }

    [ContextMenu("Set 'From' to current value")]
    public override void SetStartToCurrentValue()
    {
        from = value;
    }


    [ContextMenu("Assume value of 'From'")]
    void SetCurrentValueToStart()
    {
        value = from;
    }
}
