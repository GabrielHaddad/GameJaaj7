using UnityEngine;

public class CameraFade : MonoBehaviour
{
    public float speedScale = 1f;
    public Color fadeColor = Color.black;
    // Rather than Lerp or Slerp, we allow adaptability with a configurable curve
    public AnimationCurve CurveIn = new AnimationCurve(new Keyframe(0, 1),
        new Keyframe(0.5f, 0.5f, -1.5f, -1.5f), new Keyframe(1, 0));

    public AnimationCurve CurveOut = new AnimationCurve(new Keyframe(1, 0),
        new Keyframe(0.5f, 0.5f, -1.5f, -1.5f), new Keyframe(0, 1));


    private float alpha = 0f; 
    private Texture2D texture;
    private int direction = 0;
    private float time = 0f;

    static CameraFade instance;

    void ManageSingleton()
    {
        if (instance != null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Awake() 
    {
        ManageSingleton();
    }

    public void FadeIn()
    {
        alpha = 0f;
        time = 1f;
        direction = -1;

        texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha));
        texture.Apply();
    }

    public void FadeOut()
    {
        alpha = 1f;
        time = 1f;
        direction = 1;

        texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha));
        texture.Apply();
    }

    public void OnGUI()
    {
        if (alpha > 0f) GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);
        if (direction == -1)
        {
            time += direction * Time.deltaTime * speedScale;
            alpha = CurveIn.Evaluate(time);
            texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha));
            texture.Apply();
            if (alpha <= 0f || alpha >= 1f) direction = 0;
        }
        else if (direction == 1)
        {
            time -= direction * Time.deltaTime * speedScale;
            alpha = CurveOut.Evaluate(time);
            texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha));
            texture.Apply();
            if (alpha <= 0f || alpha >= 1f) direction = 0;
        }
    }
}