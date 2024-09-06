using UnityEngine.UI;

namespace UI.ExamplePanel
{
    public class GamePanel : BasePanel<GamePanel>
    {
        public override void Init()
        {
            base.Init();
            GetControl<Button>("menu").onClick.AddListener(() => { GamePausePanel.Instance.ShowMe(); });
            GetControl<Button>("lost").onClick.AddListener(() => { GameOverPanel.Instance.ShowMe(); });
        }

        public override void OnPressedEsc()
        {
            GamePausePanel.Instance.ShowMe();
        }
    }
}