using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    public Button.ButtonClickedEvent OnClick;
    private bool isActive;

    public void SetActive( bool active )
    {
        isActive = active;

        int child = transform.childCount;
        for ( int i = 0 ; i < child ; i++ )
        {
            transform.GetChild( i ).gameObject.SetActive( active );
        }
    }

    void Update()
    {
        if ( !isActive ) return;

        if ( Input.GetKeyDown( KeyCode.Return ) || Input.GetKeyDown( KeyCode.KeypadEnter ) )
        {
            OnClick.Invoke();
        }
    }
}
