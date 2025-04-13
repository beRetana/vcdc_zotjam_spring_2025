using UnityEngine;
using UnityEngine.UI;

public class FaceUI : MonoBehaviour
{
    [SerializeField] Sprite[] standardFaces;
    [SerializeField] Sprite madFace;
    [SerializeField] Sprite sadFace;

    [SerializeField] Image image;

    private int currentFaceIndex;

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
                ChangeFaceToIndex(6);
                break;
            case > 67:
                ChangeFaceToIndex(5);
                break;
            case > 51:
                ChangeFaceToIndex(4);
                break;
            case > 35:
                ChangeFaceToIndex(3);
                break;
            case > 19:
                ChangeFaceToIndex(2);
                break;
            case > 10:
                ChangeFaceToIndex(1);
                break;
            default:
                ChangeFaceToIndex(0);
                break;

        }
    }

    private void ChangeFaceToIndex(int faceIndex)
    {
        image.sprite = standardFaces[faceIndex];
        if (faceIndex > currentFaceIndex)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.HappyNoise,this.transform.position);
        }
        else if (faceIndex < currentFaceIndex)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.SadNoise,this.transform.position);
        }

        currentFaceIndex = faceIndex;
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
