using PlatformJump.Helpers;
using PlatformJump.Inputs;
using PlatformJump.Player;
using UnityEngine;

namespace PlatformJump.Core
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private PlayerView _player;
        [SerializeField] private BoxCollider2D _platform;
        [SerializeField] private PlayerConfig playerConfig;
        [SerializeField] private KeyboardInput _keyboardInput;
        [SerializeField] private TouchInput _touchInput;
        
        private PlayerPresenter _playerPresenter;
        private PlatformHelper _platformHelper;

        private void Awake()
        {
            _platformHelper = new PlatformHelper(_platform);
            _playerPresenter =
                new PlayerPresenter(_platformHelper, playerConfig, _player, _keyboardInput,  _touchInput);
        }

        private void Start()
        {
            _playerPresenter.Initialize();
        }

        private void OnDestroy()
        {
            _playerPresenter.Dispose();
        }
    }
}