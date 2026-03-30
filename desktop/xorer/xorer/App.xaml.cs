using Microsoft.Extensions.DependencyInjection;

namespace xorer
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new MainPage();
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);

            window.Width = 800;
            window.Height = 400;

            window.MinimumWidth = 800;
            window.MinimumHeight = 400;

            return window;
        }
    }
}