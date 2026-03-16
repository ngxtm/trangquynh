using UnityEngine;
using UnityEngine.Video;
using UnityEngine.EventSystems;

public class VideoManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private VideoPlayer _videoPlayer;
    [SerializeField] private GameObject _endGameCanvas;

    private bool isPaused = false;

    void Start()
    {
        if (_endGameCanvas != null)
            _endGameCanvas.SetActive(false);

        if (_videoPlayer != null)
            _videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        if (_endGameCanvas != null)
        {
            _endGameCanvas.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.anyKeyDown)
        {
            if (!isPaused && _videoPlayer.isPlaying)
            {
                _videoPlayer.Pause();
                OnVideoEnd(_videoPlayer);
                isPaused = true;
            }
            else if (isPaused)
            {
                ContinueVideo();
            }
        }
    }

    void ContinueVideo()
    {

        if (_endGameCanvas != null)
            _endGameCanvas.SetActive(false);

        _videoPlayer.Play();
        isPaused = false;
    }

    void OnDestroy()
    {
        if (_videoPlayer != null)
            _videoPlayer.loopPointReached -= OnVideoEnd;
    }
}