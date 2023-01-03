using UnityEngine;

public class PermissionAsker : MonoBehaviour
{
	private const string AUDIO_PERMISSION = "android.permission.RECORD_AUDIO";

	public float timer = 10;

	private float runningTime = 0;


	private void Start()
	{
		AndroidPermissionsManager.RequestPermission(new[] {AUDIO_PERMISSION},
			new AndroidPermissionCallback(
				grantedPermission =>
				{
				},
				deniedPermission =>
				{
				}));
	}

	// Update is called once per frame
	void Update()
	{
		timer -= Time.deltaTime;
		if (timer <= 0)
		{
			if (!AndroidPermissionsManager.IsPermissionGranted(AUDIO_PERMISSION))
			{
			}
		}

		runningTime = timer;
	}
}