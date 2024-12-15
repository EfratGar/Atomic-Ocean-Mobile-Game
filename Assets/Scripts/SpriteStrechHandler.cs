using UnityEngine;

public class SpriteStrechHandler : MonoBehaviour
{
    public bool isAspectRatio;
    public float maxScaleFactor = 1.5f; 

    void Start()
    {
        var topRightCorner = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        var worldSpaceWidth = topRightCorner.x * 2;
        var worldSpaceHeight = topRightCorner.y * 2;

        var spriteSize = GetComponent<SpriteRenderer>().bounds.size;

        var scaleFactorX = worldSpaceWidth / spriteSize.x;
        var scaleFactorY = worldSpaceHeight / spriteSize.y;

        if (isAspectRatio)
        {
            if (scaleFactorX > scaleFactorY)
                scaleFactorY = scaleFactorX;
            else
                scaleFactorX = scaleFactorY;
        }

        scaleFactorX = Mathf.Min(scaleFactorX, maxScaleFactor);
        scaleFactorY = Mathf.Min(scaleFactorY, maxScaleFactor);

        transform.localScale = new Vector3(scaleFactorX, scaleFactorY, 1);
    }
}
