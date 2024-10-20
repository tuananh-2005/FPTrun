using UnityEngine;

public class background : MonoBehaviour
{
    public float scrollSpeed = 0.5f; // Tốc độ cuộn
    private Renderer rend;
    private Vector2 savedOffset;

    void Start()
    {
        rend = GetComponent<Renderer>();

        // Kiểm tra xem material có thuộc tính _MainTex hay không
        if (rend.material.HasProperty("_MainTex"))
        {
            savedOffset = rend.material.GetTextureOffset("_MainTex");
        }
        else
        {
            Debug.LogError("Material does not have a _MainTex property.");
        }
    }

    void Update()
    {
        // Tính toán offset để cuộn texture
        float x = Mathf.Repeat(Time.time * scrollSpeed, 1);
        Vector2 offset = new Vector2(x, savedOffset.y);

        // Thiết lập offset cho texture nếu shader hỗ trợ MainTex
        if (rend.material.HasProperty("_MainTex"))
        {
            rend.material.SetTextureOffset("_MainTex", offset);
        }
    }
}

