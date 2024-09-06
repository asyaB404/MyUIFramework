using UnityEngine.UI;

namespace UI.ExamplePanel
{
    public class SettingPanel : BasePanel<SettingPanel>
    {
        public override void Init()
        {
            base.Init();
            GetControl<Button>("return").onClick.AddListener(HideMe);
        }
    }
}