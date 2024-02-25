using UnityEngine;
using TMPro;
using System.Collections;

public class FPS_Counter : MonoBehaviour
{
    private TMP_Text _text;
    private float _count;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private IEnumerator Start()
    {
        while (true)
        {
            _count = 1 / Time.unscaledDeltaTime;
            _text.text = Mathf.Round(_count).ToString();
            yield return new WaitForSeconds(0.1f);
        }
    }
}
