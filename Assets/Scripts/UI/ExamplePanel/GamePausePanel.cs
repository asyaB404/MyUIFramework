using UnityEngine.UI;

namespace UI.ExamplePanel
{
    public class GamePausePanel : BasePanel<GamePausePanel>
    {
        public override void Init()
        {
            base.Init();
            GetControl<Button>("return").onClick.AddListener(HideMe);
            GetControl<Button>("exit").onClick.AddListener(() =>
            {
                GamePanel.Instance.HideMe();
            });
        }
    }
}