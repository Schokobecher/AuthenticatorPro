using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using AuthenticatorPro.Activity;
using AuthenticatorPro.Fragment;

namespace AuthenticatorPro.Preference
{
    internal class AutoBackupPreference : AndroidX.Preference.Preference
    {
        public AutoBackupPreference(Context context) : base(context)
        {
            
        }

        public AutoBackupPreference(Context context, IAttributeSet attrs) : base(context, attrs)
        {

        }

        public AutoBackupPreference(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs,
            defStyleAttr)
        {

        }

        public AutoBackupPreference(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context,
            attrs, defStyleAttr, defStyleRes)
        {

        }

        protected AutoBackupPreference(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {

        }

        protected override void OnClick()
        {
            var activity = (SettingsActivity) Context;
            var fragment = new AutoBackupSetupBottomSheet();
            fragment.Show(activity.SupportFragmentManager, fragment.Tag);
        }
    }
}