using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Target;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Droid.Views.Attributes;
using MvvmCross.Droid.Views.Fragments;
using MvvmCross.Platform.Converters;
using MvvmCross_Application1.Core.ViewModels;
using static Android.Support.V7.Widget.RecyclerView;

namespace MvvmCross_Application1.Droid.Views
{

   [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, true)]
   [Register("MvvmCross_Application1.Droid.Views.FoodRecyclerFragment")]
    public class FoodRecyclerFragment : MvxFragment<FoodRecyclerViewModel>
    {
        public FoodRecyclerFragment() { }
        private MvxRecyclerView recyclerView;
        //public static  ImageView img;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.FoodRecyclerView, null);

            this.recyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.rvItems );
            //this.recyclerView.ItemTemplateId = Resource.Layout.MyCell;

            var set = this.CreateBindingSet<FoodRecyclerFragment, FoodRecyclerViewModel>();

            //In axml: local: MvxBind = "ItemsSource Foods; ItemClick ShowFoodDetailsCommand"
            set.Bind(this.recyclerView).For(x => x.ItemsSource).To(x => x.Foods);
            set.Bind(this.recyclerView).For(x => x.ItemClick).To(x => x.ShowFoodDetailsCommand);

            set.Apply();
          //  CustomAdapter adapter = new CustomAdapter(this.Context);
            //recyclerView.SetAdapter(adapter)

            // img = view.FindViewById<ImageView>(Resource.Id.image);
            // Tab Pages


            return view;//this.BindingInflate(Resource.Layout.FoodRecyclerView, null);


        }
        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
         
        }


    /*  public class CustomAdapter : MvxRecyclerAdapter 
             {
                 Context context;
                 bool isLandscape;
                 public CustomAdapter(bool isLandscape, Context context, IMvxAndroidBindingContext bindingContext)
                     : base(bindingContext)
                 {
                     this.context = context;
                     this.isLandscape = isLandscape;
                 }

                 public override ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
                 {
                     View itemView;

                     if (viewType == 0)
                     {
                         itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ItemView, parent, false);
                         return new ItemViewHolderForHeader(itemView, this.BindingContext);
                     }
                   /*  else
                     {
                         itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.productlistviewitem, parent, false);
                         return new ItemViewHolder(itemView, this.BindingContext);
                     }*/
                // }
        }
    public class ImageConverter : MvxValueConverter<string, int>
    {
       public static string FileDir;
        protected override int Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            var imagefileName = value.Replace(".jpg", "").Replace(".png", "");
           int id = (int)typeof(Resource.Drawable).GetField(imagefileName).GetValue(null);
            return id;
          
        }
    }


    
}

