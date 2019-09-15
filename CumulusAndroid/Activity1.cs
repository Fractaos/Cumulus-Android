using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace CumulusGame
{
    [Activity(Label = "Cumulus"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.Portrait
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize | ConfigChanges.ScreenLayout)]
    public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            var g = new Game1();
            SetContentView((View)g.Services.GetService(typeof(View)));
            g.Run();

            View vw = (View)g.Services.GetService(typeof(View));
            vw.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.HideNavigation | (StatusBarVisibility)SystemUiFlags.ImmersiveSticky;
            vw.SetOnSystemUiVisibilityChangeListener(new MyUiVisibilityChangeListener(vw));
            SetImmersive();
        }
        private void SetImmersive()
        {
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Kitkat)
            {
                Window.DecorView.SystemUiVisibility = (StatusBarVisibility)(SystemUiFlags.LayoutStable | SystemUiFlags.LayoutHideNavigation | SystemUiFlags.LayoutFullscreen | SystemUiFlags.HideNavigation | SystemUiFlags.Fullscreen | SystemUiFlags.ImmersiveSticky);
            }
        }
        private class MyUiVisibilityChangeListener : Java.Lang.Object, View.IOnSystemUiVisibilityChangeListener
        {
            readonly View _targetView;
            public MyUiVisibilityChangeListener(View v)
            {
                _targetView = v;
            }
            public void OnSystemUiVisibilityChange(StatusBarVisibility v)
            {
                if (_targetView.SystemUiVisibility != ((StatusBarVisibility)SystemUiFlags.HideNavigation | (StatusBarVisibility)SystemUiFlags.Immersive))
                {
                    _targetView.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.HideNavigation | (StatusBarVisibility)SystemUiFlags.ImmersiveSticky;
                }
            }
        }
    }
}

