using System.Collections;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    [SerializeField] private float displayDuration = 0.1f;

    private Coroutine strikeCoroutine;

    public void Strike(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);

        if (strikeCoroutine != null)
        {
            StopCoroutine(strikeCoroutine);
        }

        strikeCoroutine = StartCoroutine(StrikeRoutine());
    }

    private IEnumerator StrikeRoutine()
    {
        yield return new WaitForSeconds(displayDuration);

        gameObject.SetActive(false);
        strikeCoroutine = null;
    }
}
