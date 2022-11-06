using System.IO;
using UI.Profile.Models;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;
using UnityEngine.EventSystems;

namespace UI.Profile
{
    public class CreateTeamLogoView : MonoBehaviour
    {
        private string firstLayerColorNumber;
        private string secondLayerColorNumber;
        private string inputLayerColorNumber;
        private string inputGroundColorNumber;
        private LogoPrefab _logoPrefab;
        private readonly string _pathForm = "/Assets/Sprites/Profile/Form/";
        private readonly string _pathPattern = "/Assets/Sprites/Profile";
        private ILogoSaver _logoSaver = new ConsoleLogoSaver();
        private ILogoLoader _logoLoader = new MockLogoLoader();
        private Button _saveButton;
        private Button _resetButton;
        private Button _background;
        private Button _closePopupButton;

       
        
        private void Awake()
        {
            _logoPrefab = Scripts.Utils.FindChild<LogoPrefab>(transform, "Logo");
            _saveButton = Scripts.Utils.FindChild<Button>(transform, "SaveButton");
            _resetButton = Scripts.Utils.FindChild<Button>(transform, "ResetButton");
            _background = Scripts.Utils.FindChild<Button>(transform, "MainBackground");
            _closePopupButton = Scripts.Utils.FindChild<Button>(transform, "ClosePopup");
            _saveButton.onClick.AddListener(Save);
            _resetButton.onClick.AddListener(Load);
            _background.onClick.AddListener(Close);
            _closePopupButton.onClick.AddListener(Close);
            Load();
        }

        private void OnEnable()
        {
            Load();
        }

        public async void Load() // тут пока
        {
            TeamLogo logoData = await _logoLoader.LoadLogo();
            Debug.Log(logoData.form_name);
            Load(logoData);
        }

        private void Load(TeamLogo teamLogo) 
        {
            _logoPrefab.SetData(teamLogo, _pathForm, _pathPattern);
        }
        
        public async void Save()
        {
            await _logoSaver.SaveLogo(_logoPrefab.GetTeamLogo());
            gameObject.SetActive(false);
        }
        
        public void Close()
        {
            gameObject.SetActive(false);
        }
        
        public void ChangeForm(string form)
        {
            _logoPrefab.ChangeLayerForm(form);
        }

        public void ChangePattern(string pattern)
        {
            _logoPrefab.ChangeLayerPattern(pattern);
        }

        public void ChangeFirstLayerColor(string colorNumber)
        {
            _logoPrefab.ChangeFirstLayerColor(colorNumber);
        }
        
        public void ChangeSecondLayerColor(string colorNumber)
        {
            _logoPrefab.ChangeSecondLayerColor(colorNumber);
        }
    }
}