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
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Droid.Views.Attributes;
using MvvmCross.Droid.Views.Fragments;
using MvvmCross_Application1.Core.ViewModels;

namespace MvvmCross_Application1.Droid.Views
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, true)]
    [Register("MvvmCross_Application1.Droid.Views.FoodCartFragment")]
    class FoodCartFragment: MvxFragment<FoodCartViewModel>
    {
        private MvxRecyclerView recyclerView;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.FoodRecyclerView, null);

            this.recyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.rvItems);

            var set = this.CreateBindingSet<FoodCartFragment, FoodCartViewModel>();

            //In axml: local: MvxBind = "ItemsSource Foods; ItemClick ShowFoodDetailsCommand"
            set.Bind(this.recyclerView).For(x => x.ItemsSource).To(x => x.Cart.CartFoods.Values );
            set.Bind(this.recyclerView).For(x => x.ItemClick).To(x => x.ShowFoodDetailsCommand);

            set.Apply();


            return view;//this.BindingInflate(Resource.Layout.FoodRecyclerView, null);

           // return this.BindingInflate(Resource.Layout.FoodCart, null);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
        }
    }
}