using UnityEngine;
using UnityEngine.UI;

public class FaceUI : MonoBehaviour
{
    [SerializeField] Sprite[] standardFaces;
    [SerializeField] Sprite madFace;
    [SerializeField] Sprite sadFace;

    [SerializeField] Image image;

    private bool gameOver = false;

    private float recentLovePercent;

    private void OnEnable()
    {
        Love.OnUpdatedLove += UpdateLoveFaceUI;
    }
    private void OnDisable()
    {
        Love.OnUpdatedLove -= UpdateLoveFaceUI;
    }

    private void UpdateLoveFaceUI(float lovePercent)
    {
        Debug.Log("Love percent: " + 100 * lovePercent);
        recentLovePercent = lovePercent;
        switch (100 * lovePercent)
        {
            case > 84:
                image.sprite = standardFaces[6];
                break;
            case > 67:
                image.sprite = standardFaces[5];
                break;
            case > 51:
                image.sprite = standardFaces[4];
                break;
            case > 35:
                image.sprite = standardFaces[3];
                break;
            case > 19:
                image.sprite = standardFaces[2];
                break;
            case > 10:
                image.sprite = standardFaces[1];
                break;
            default:
                image.sprite = standardFaces[0];
                break;

        }
    }

    public void UpdateUI(float lovePercent)
    {
        if (gameOver) return;
        UpdateLoveFaceUI(lovePercent);
    }

    public void GameOver(float lovePercent)
    {
        if (recentLovePercent > .2f) image.sprite = sadFace;
        else image.sprite = madFace;
        gameOver = true;
    }

}
