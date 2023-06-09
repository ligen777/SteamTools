using BD.WTTS.Client.Resources;

namespace BD.WTTS.UI.Views.Pages;

public partial class HomePage : PageBase
{
    public HomePage()
    {
        InitializeComponent();
        DataContext = IViewModelManager.Instance.Get<HomePageViewModel>();

        ControlName = Strings.Welcome;
        ControlNamespace = "11111111111111111111111111111";
        Description = "This is a description";

        App.Instance.Resources.TryGetResource("AppIcon", null, out var icon);
        PreviewImage = icon as IconSource;

        WinUINamespace = "2222222222222222222222222222222";
    }
}
