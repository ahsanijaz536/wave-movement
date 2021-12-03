using UnityEngine;

public class InputController : MonoBehaviour
{
	public static InputController instance;

	#region var

	public Vector3 prevPosition;
	public float controllSensitivity;
	public float moveSpeed;
	public float horizontal;
	public float vertical;
	public bool autoMove;

    #endregion

    private void Awake()
    {
		instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
		if (autoMove) vertical = 1;
		else vertical = 0;
    }

    // Update is called once per frame
    void Update()
    {
		PlayerInputControlls();
    }

    #region InputControlls

    void PlayerInputControlls()
    {
		if (Input.GetMouseButtonDown(0))
		{
			prevPosition = Input.mousePosition;
		}

		if (Input.GetMouseButton(0))
		{
			Vector3 touchDelta = Input.mousePosition - prevPosition;
			var positionDelta = touchDelta * controllSensitivity;
			positionDelta.x /= Screen.width / 2f;
			horizontal = Mathf.MoveTowards(horizontal, positionDelta.x, Time.deltaTime * moveSpeed);
			horizontal = positionDelta.x;
			prevPosition = Input.mousePosition;
		}
		else
		{
			prevPosition = Vector3.zero;
			horizontal = 0;
		}
	}


    #endregion
}
