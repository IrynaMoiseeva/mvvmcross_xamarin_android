using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Views.Fragments;
using MvvmCross.Droid.Views.Attributes;

using MvvmCross_Application1.Core.ViewModels;

namespace MvvmCross_Application1.Droid.Views
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, true)]
    [Register("MvvmCross_Application1.Droid.Views.FoodDetailsFragment")]
    class FoodDetailsFragment:MvxFragment<FoodDetailsViewModel>
    {
            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                base.OnCreateView(inflater, container, savedInstanceState);
                return this.BindingInflate(Resource.Layout.Details, null);
            }

            public override void OnViewCreated(View view, Bundle savedInstanceState)
            {
                base.OnViewCreated(view, savedInstanceState);
            //  (this.Activity as MainView).set ("Journey details");
            }
        }
}