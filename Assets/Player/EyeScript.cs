using UnityEngine;
using System.Collections;

public class EyeScript : MonoBehaviour {

    public GameObject leftEye;
    public GameObject rightEye;

    public GameObject leftIris;
    public GameObject rightIris;

    public float blinkSpeed = 1.5f;

    Vector2 leftEyeScale;
    Vector2 rightEyeScale;



	// Use this for initialization
	void Start () {
        leftEyeScale = leftEye.transform.localScale ;
        rightEyeScale = rightEye.transform.localScale;
        StartCoroutine(Blink(leftEye,leftEyeScale));
        StartCoroutine(Blink(rightEye, rightEyeScale));
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 leftPos = mousePos - leftIris.transform.position;
        //leftPos = leftPos.normalized;
        //Debug.Log(leftPos);
        leftIris.transform.localPosition = ClampVector2(leftPos * .02f, -.1f, .1f);

        Vector2 rightPos = mousePos - rightIris.transform.position;
        
        rightIris.transform.localPosition = ClampVector2( rightPos * .02f,-.1f,.1f);

	}

    

    IEnumerator Blink(GameObject eye, Vector2 scale)
    {
        while(eye.transform.localScale.y > 0)
        {
            eye.transform.localScale = new Vector3(eye.transform.localScale.x, eye.transform.localScale.y - Time.fixedDeltaTime * blinkSpeed);
            yield return new WaitForEndOfFrame();
        }
        while (scale.y > eye.transform.localScale.y)
        {
            eye.transform.localScale = new Vector3(eye.transform.localScale.x, eye.transform.localScale.y + Time.fixedDeltaTime * blinkSpeed);
            yield return new WaitForEndOfFrame();

        }
        yield return new WaitForSeconds(Random.Range(1f, 5f));
        StartCoroutine(Blink(eye,scale));
    }
    private Vector2 ClampVector2(Vector2 vector, float min, float max)
    {
        vector.x = Mathf.Clamp(vector.x, min, max);
        vector.y = Mathf.Clamp(vector.y, min, max);
        return vector;
    }
}
