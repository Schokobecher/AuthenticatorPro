﻿using Android.Views;
using Android.Views.Animations;

namespace AuthenticatorPro.Shared.Util
{
    public static class AnimUtil
    {
        public static void FadeInView(View view, int duration, bool overrideAnim = false)
        {
            if(overrideAnim)
            {
                view.Animation?.Cancel();
                view.ClearAnimation();
            }

            if(view.Visibility != ViewStates.Invisible)
                return;

            var anim = new AlphaAnimation(0f, 1f)
            {
                Duration = duration
            };

            anim.AnimationEnd += delegate { view.Visibility = ViewStates.Visible; };
            view.StartAnimation(anim);
        }

        public static void FadeOutView(View view, int duration, bool overrideAnim = false)
        {
            if(overrideAnim)
            {
                view.Animation?.Cancel();
                view.ClearAnimation();
            }

            if(view.Visibility != ViewStates.Visible)
                return;

            var anim = new AlphaAnimation(1f, 0f)
            {
                Duration = duration
            };

            anim.AnimationEnd += delegate { view.Visibility = ViewStates.Invisible; };
            view.StartAnimation(anim);
        }
    }
}