using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    private int maxScore = 0;

    public bool flipped;

    public float imageDistance;
    public Image initImageScore;
    public Color scoreActiveColor = Color.black;

    
    private Image[] scoreImages;

    public void Initialize(int _maxScore)
    {
        maxScore = _maxScore;
        InstantiateScoreImages();
    }

    public void InstantiateScoreImages()
    {
        scoreImages = new Image[maxScore];
        
        scoreImages[0] = initImageScore;
        float dist = flipped ? -imageDistance : imageDistance;

        for(int i = 1; i < maxScore; i++)
        {
            scoreImages[i] = Instantiate(initImageScore, this.transform, false);
            float posX = scoreImages[i].transform.localPosition.x + (i * dist);

            scoreImages[i].transform.localPosition = new Vector2(posX, scoreImages[i].transform.localPosition.y);
        }
    }

    public void SetScore(int _currScore)
    {
        if(maxScore == 0) return;

        for(int i = 0; i < maxScore; i++)
        {
            if(i < _currScore)
            {
                scoreImages[i].color = scoreActiveColor;
            }
            else
            {
                scoreImages[i].color = Color.white;
            }
        }
    }
}
