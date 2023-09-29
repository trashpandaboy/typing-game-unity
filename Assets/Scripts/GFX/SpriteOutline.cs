using UnityEngine;

[ExecuteInEditMode]
public class SpriteOutline : MonoBehaviour
{
    [SerializeField]
    Color color = Color.red;
    [Range(0, 16)]
    [SerializeField]
    public int outlineSize = 1;

    private SpriteRenderer _spriteRenderer;

    #region Unity

    void OnEnable()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        UpdateOutline(true);
    }

    void OnDisable()
    {
        UpdateOutline(false);
    }

    #endregion

    void UpdateOutline(bool outline)
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        _spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_Outline", outline ? 1f : 0);
        mpb.SetColor("_OutlineColor", color);
        mpb.SetFloat("_OutlineSize", outlineSize);
        _spriteRenderer.SetPropertyBlock(mpb);
    }
}