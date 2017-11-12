using UnityEngine;
/**
 * A camera to help with Orthagonal mode when you need it to lock to pixels.  Desiged to be used on android and retina devices.
 * Use http://docs.unity3d.com/Manual/class-ScriptExecution.html
 */
public class PixelPerfectCam : MonoBehaviour {
	/**
	 * The target size of the view port.
	 */
	public Vector2 targetViewportSizeInPixels = new Vector2(480.0f, 320.0f);
	/**
	 * Snap movement of the camera to pixels.
	 */
	public bool lockToPixels = true;
	/**
	 * The number of target pixels in every Unity unit.
	 */
	public float pixelsPerUnit = 32.0f;
	/**
	 * A game object that the camera will follow the x and y position of.
	 */
	public GameObject followTarget;
	
	private Camera _camera;
	private int _currentScreenWidth = 0;
	private int _currentScreenHeight = 0;
	
	private float _pixelLockedPPU = 32.0f;
	private Vector2 _winSize;
	float initialSize = 0f;
	protected void Start ()
	{
		_camera = this.GetComponent<Camera>();
		if (!_camera)
		{
			Debug.LogWarning( "No camera for pixel perfect cam to use" );
		}
		else
		{
			_camera.orthographic = true;
			ResizeCamToTargetSize();
		}

		if (Screen.fullScreen)
		{
			Cursor.lockState = CursorLockMode.Confined;
		}
	}
	
	public void ResizeCamToTargetSize(){
		if(_currentScreenWidth != Screen.width || _currentScreenHeight != Screen.height){
			// check our target size here to see how much we want to scale this camera
			float percentageX = Screen.width/targetViewportSizeInPixels.x;
			float percentageY = Screen.height/targetViewportSizeInPixels.y;
			float targetSize = 0.0f;
			if(percentageX > percentageY){
				targetSize = percentageY;
			}else{
				targetSize = percentageX;
			}
			int floored = Mathf.FloorToInt(targetSize);
			if(floored < 1){
				floored = 1;
			}
			// now we have our percentage let's make the viewport scale to that
			float camSize = ((Screen.height/2)/floored)/pixelsPerUnit;
			_camera.orthographicSize = camSize;
			_pixelLockedPPU = floored * pixelsPerUnit;
			initialSize = camSize;
		}
		_winSize = new Vector2(Screen.width, Screen.height);
	}

	int boundary = 5;
	int speed = 250;
	// todo: make lock onto any target
	bool cameraLocked = false;
	public Transform center;
	public void LateUpdate ()
	{
		if (_winSize.x != Screen.width || _winSize.y != Screen.height)
		{
			ResizeCamToTargetSize();
		}
		if (_camera && followTarget)
		{
			Vector2 newPosition = new Vector2( followTarget.transform.position.x, followTarget.transform.position.y );
			float nextX = Mathf.Round( _pixelLockedPPU * newPosition.x );
			float nextY = Mathf.Round( _pixelLockedPPU * newPosition.y );
			_camera.transform.position = new Vector3( nextX / _pixelLockedPPU, nextY / _pixelLockedPPU, _camera.transform.position.z );
		}

		if (Input.GetKey( KeyCode.LeftControl ) && Input.GetKeyDown( KeyCode.Return))
		{
			speed = 0;
			cameraLocked = !cameraLocked;

			if (!cameraLocked)
			{
				speed = 200;
			}
		}

		if (Input.GetAxis("Mouse ScrollWheel") < 0) // back
		{
			Camera.main.orthographicSize += 25;
		}
		if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
		{
			Camera.main.orthographicSize -= 25;
		}

		Camera.main.orthographicSize = Mathf.Clamp( Camera.main.orthographicSize, initialSize, initialSize + 300 );

		if (Input.GetKeyDown( KeyCode.Space ))
		{
		  Camera.main.transform.position = new Vector3( center.position.x, center.position.y, Camera.main.transform.position.z );
		}

		if (Input.mousePosition.x > Screen.width - boundary)
		{
			Camera.main.transform.position = new Vector3( Camera.main.transform.position.x + speed * Time.deltaTime, Camera.main.transform.position.y, Camera.main.transform.position.z );
		}

		if (Input.mousePosition.x < 0 + boundary)
		{
			Camera.main.transform.position = new Vector3( Camera.main.transform.position.x - speed * Time.deltaTime, Camera.main.transform.position.y, Camera.main.transform.position.z );
		}

		if (Input.mousePosition.y > Screen.height - boundary)
		{
			Camera.main.transform.position = new Vector3( Camera.main.transform.position.x, Camera.main.transform.position.y + speed * Time.deltaTime, Camera.main.transform.position.z );
		}

		if (Input.mousePosition.y < 0 + boundary)
		{
			Camera.main.transform.position = new Vector3( Camera.main.transform.position.x, Camera.main.transform.position.y - speed * Time.deltaTime, Camera.main.transform.position.z );
		}

	}
}
