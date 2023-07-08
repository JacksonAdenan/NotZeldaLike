using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class TestScript : MonoBehaviour
{
    [SerializeField] private float _angle = 45.0f;
    [SerializeField] private float _zScale = 0.5f;
    [SerializeField] private float _zOffset = 0.0f;

    [SerializeField] private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetObliqueness();
    }
    void SetObliqueness()
    {
        Camera camera = Camera.main;
        camera.orthographic = true;
        var orthoHeight = camera.orthographicSize;
        var orthoWidth = camera.aspect * orthoHeight;
        var m = Matrix4x4.Ortho(-orthoWidth, orthoWidth, -orthoHeight, orthoHeight, camera.nearClipPlane, camera.farClipPlane);
        var s = _zScale / orthoHeight;
        m[0, 2] = +s * Mathf.Sin(Mathf.Deg2Rad * -_angle);
        m[1, 2] = -s * Mathf.Cos(Mathf.Deg2Rad * -_angle);
        m[0, 3] = -_zOffset * m[0, 2];
        m[1, 3] = -_zOffset * m[1, 2];
        camera.projectionMatrix = m;

        if (_gameManager && _gameManager.player)
        { 
            camera.transform.position = new Vector3(_gameManager.player.transform.position.x, 
                1, 
                _gameManager.player.transform.position.z);
        }
    }
}
