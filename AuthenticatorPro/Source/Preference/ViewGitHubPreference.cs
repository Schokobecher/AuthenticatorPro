using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Uri = Android.Net.Uri;

namespace AuthenticatorPro.Preference
{
    internal class ViewGitHubPreference : AndroidX.Preference.Preference
    {
        public ViewGitHubPreference(Context context) : base(context)
        {

        }

        public ViewGitHubPreference(Context context, IAttributeSet attrs) : base(context, attrs)
        {

        }

        public ViewGitHubPreference(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs,
            defStyleAttr)
        {

        }

        public ViewGitHubPreference(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(
            context, attrs, defStyleAttr, defStyleRes)
        {

        }

        protected ViewGitHubPreference(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference,
            transfer)
        {

        }

        protected override void OnClick()
        {
            var intent = new Intent(Intent.ActionView, Uri.Parse(Constants.GitHubRepo));
            Context.StartActivity(intent);
        }
    }
}