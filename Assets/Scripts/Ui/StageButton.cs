using Controller;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class StageButton : MonoBehaviour
    {
        [SerializeField] private Animator animator = null;
        [SerializeField] private RectTransform rectTransform = null;
        [SerializeField] private Button stageButton = null;
        [SerializeField] private Text stageText = null;

        private int _stageNumber;

        public int StageNumber => _stageNumber;
        
        private static readonly int SelectKey = Animator.StringToHash("Select");

        public void Initialize(int stageNumber, Vector2 pos, bool select = false)
        {
            _stageNumber = stageNumber;
            stageText.text = $"Stage{stageNumber + 1}";
            Debug.Log(pos);
            rectTransform.anchoredPosition = pos;
            stageButton.onClick.AddListener(OnclickSelect);
            Select(select);
        }

        private void OnclickSelect()
        {
            Select(true);
            MenuSceneController.Instance.SelectStageButton(_stageNumber);
        }

        public void Select(bool select)
        {
            animator.SetBool(SelectKey, select);
        }
    }
}