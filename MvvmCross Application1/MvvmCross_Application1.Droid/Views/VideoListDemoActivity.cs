using System;
using System.Collections.Generic;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Google.Android.Youtube.Player;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Droid.Views;
using MvvmCross_Application1.Core.Model;
using MvvmCross_Application1.Core.ViewModels;
using static Android.Views.View;

namespace MvvmCross_Application1.Droid.Views
{


    namespace YouTubePlayerSample
    {

        // A sample Activity showing how to manage multiple YouTubeThumbnailViews in an adapter for display
        // in a List. When the list items are clicked, the video is played by using a YouTubePlayerFragment.
        // 
        // The demo supports custom fullscreen and transitioning between portrait and landscape without
        // rebuffering.
        [Activity(MainLauncher = true,
            Label = "videolist_demo_name")]


        [Register("com.examples.youtubeapidemo.VideoListDemoActivity")]
        public class VideoListDemoActivity : MvxActivity<PlayVideoViewModel>, 
            YouTubeThumbnailView.IOnInitializedListener,
            IYouTubeThumbnailLoaderOnThumbnailLoadedListener,
            IYouTubePlayerOnFullscreenListener, IOnClickListener
        {

            public VideoListDemoActivity()
            {
                thumbnailViewToLoaderMap = new Dictionary<YouTubeThumbnailView, IYouTubeThumbnailLoader>();
            }
            public PlayVideoViewModel ViewModel
            {
                get { return (PlayVideoViewModel)base.ViewModel; }
                set { base.ViewModel = value; }
            }


            // The duration of the animation sliding up the video in portrait.
            private const int AnimationDuration = 300;
            // The padding between the video list and the video in landscape orientation.
            private const int LandscapeVideoPadding = 5;
            public const int PlayActivityRequestCode = 42;
            // The request code when calling startActivityForResult to recover from an API service error.
            private const int RecoveryDialogRequest = 1;
            private const String PlaylistId = "PLFEgnf4tmQe_L3xlmtFwX8Qm5czqwCcVi";//"ECAE6B03CA849AD332";
            private VideoListFragment listFragment;
            private VideoFragment videoFragment;
            private TextView text;
            private View videoBox;
            private View closeButton;
            private MvxListView listView;
            private bool isFullscreen;
            private readonly Dictionary<YouTubeThumbnailView, IYouTubeThumbnailLoader> thumbnailViewToLoaderMap;

            private MyAdapter af;

            private YouTubeThumbnailView thumbnail_channel;
            private IYouTubeThumbnailLoader thumbnailLoader;

            public void OnClick(View v)
            {
                ToFullScreen();
            }
            protected override void OnCreate(Bundle savedInstanceState)
            {
                base.OnCreate(savedInstanceState);

                SetContentView(Resource.Layout.video_list_demo);

                listFragment = FragmentManager.FindFragmentById<VideoListFragment>(Resource.Id.list_fragment);
                //videoFragment = FragmentManager.FindFragmentById<VideoFragment>(Resource.Id.video_fragment_container);
                listView = FindViewById<MvxListView>(Resource.Id.VideoItems);
                
                /*  VideoFragment youTube = new VideoFragment();
                  Android.App.FragmentTransaction fragmentTransaction = FragmentManager.BeginTransaction();
                  fragmentTransaction.Replace(Resource.Id.video_fragment_container, youTube);
                  fragmentTransaction.Commit();*/
                videoBox = FindViewById(Resource.Id.video_box);
                closeButton = FindViewById(Resource.Id.close_button);
                closeButton.Click += OnClickClose;


                videoBox.Visibility = ViewStates.Visible; //invisible

                // Layout();

                CheckYouTubeApi();

                text = this.FindViewById<TextView>(Resource.Id.text);
                /*var set = this.CreateBindingSet<VideoListDemoActivity, PlayVideoViewModel>();
                set.Bind(this).For(v => v.text ).To(vm => vm.VideoUrl).OneWay();*/
                //var view = this.BindingInflate(Resource.Layout.FoodRecyclerView, null);


               thumbnail_channel = FindViewById<YouTubeThumbnailView>(Resource.Id.thumbnail_channel);
                //thumbnail_channel = new YouTubeThumbnailView(this);
               // thumbnail_channel.Tag = ViewModel.PlayListUrl;//"Y_UmWdcTrrc";

                thumbnail_channel.Initialize(DeveloperKey.Key, this);

               // MaybeStartDemo();
              //video.Url; //entry.VideoId;
                // thumbnail_channel.Initialize(DeveloperKey.Key, this);




                var set = this.CreateBindingSet<VideoListDemoActivity, PlayVideoViewModel>();
                //In axml: local: MvxBind = "ItemsSource Foods; ItemClick ShowFoodDetailsCommand"
                set.Bind(this.listView).For(x => x.ItemsSource).To(x => x.YoutubeItems );
              //  set.Bind(this.thumbnail_channel ).For(x => x.Tag).To(x => x.PlayListUrl );
                set.Bind(this.listView).For(x => x.ItemClick).To(x => x.ShowVideoCommand);
                set.Apply();

                this.listView = this.FindViewById<MvxListView>(Resource.Id.VideoItems);
                af = new MyAdapter(this, (IMvxAndroidBindingContext)BindingContext); ;

                listView.Adapter = af;

                listView.ItemSelected += (object sender, ListView.ItemSelectedEventArgs e) =>
                {

                    var n = e.Position;
                    ViewModel.method(n);



                };
                listView.ItemSelected += (sender, e) =>
                {
                    var n = e.Position;
                };

                /*    ListView.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
                    {
                        Toast.MakeText(Application, ((TextView)args.View).Text, ToastLength.Short).Show();
                    };*/
                //listView.ItemClick +=
                //  listView. += (sender, e) => { ToFullScreen(); };
                // Perform action on click;
            }
            void YouTubeThumbnailView.IOnInitializedListener.OnInitializationFailure(YouTubeThumbnailView view, YouTubeInitializationResult result)
            {
                view.SetImageResource(Resource.Drawable.cart1);// no_thumbnail
            }

            void YouTubeThumbnailView.IOnInitializedListener.OnInitializationSuccess(YouTubeThumbnailView view, IYouTubeThumbnailLoader loader)
            {
                /*  loader.SetOnThumbnailLoadedListener(this);
                  thumbnailViewToLoaderMap.Add(view, loader);
                  view.SetImageResource(Resource.Drawable.cart1);
                  var videoId = (string)view.Tag;
                  loader.SetVideo(videoId);

                /*  loader.SetOnThumbnailLoadedListener(this);
                  // thumbnailViewToLoaderMap.Add(view, loader);
                  view.SetImageResource(Resource.Drawable.cart1);//loading_thumbnail
                  var videoId = (string)view.Tag;
                //  loader = thumbnailViewToLoaderMap[thumbnail_channel];
                  //loader.SetVideo(videoId);
                  if (view == null)
                  {
                      thumbnail_channel.Tag = videoId;
                      thumbnail_channel.Initialize(DeveloperKey.Key, this);
                  }
                  else
                  {
                      var loader = thumbnailViewToLoaderMap[thumbnail_channel];
                      if (loader == null)
                      {
                          // 2) The view is already created, and is currently being initialized. We store the
                          //    current videoId in the tag.
                          thumbnail_channel.Tag = videoId;
                      }
                      else
                      {
                          // 3) The view is already created and already initialized. Simply set the right videoId
                          //    on the loader.
                          thumbnail_channel.SetImageResource(Resource.Drawable.cart1);//loading_thumbnail
                                                                                      // loader.SetVideo(entry.VideoId);
                      }
                  }*/

                this.thumbnailLoader = loader;
                
                thumbnailLoader.SetOnThumbnailLoadedListener(this);
                thumbnailLoader.SetPlaylist(PlaylistId);
                // thumbnail_channel.Tag = "Y_UmWdcTrrc";
                //MaybeStartDemo();

            }
            protected override void OnResume()
            {
                base.OnResume();

               // activityResumed = true;
               /* if (thumbnailLoader != null && player != null)
                {
                    if (state == State.Uninitialized)
                    {
                        MaybeStartDemo();
                    }
                    else if (state == State.LoadingThumbnails)
                    {
                        LoadNextThumbnail();
                    }
                    else
                    {
                        if (state == State.VideoPlaying)
                        {
                            player.Play();
                        }
                        flipDelayHandler.SendEmptyMessageDelayed(0, FlipDuration);
                    }
                }*/
            }
            void IYouTubeThumbnailLoaderOnThumbnailLoadedListener.OnThumbnailLoaded(YouTubeThumbnailView thumbnail, string videoId)
            {
              /*  nextThumbnailLoaded = true;

                if (activityResumed)
                {
                    if (state == State.LoadingThumbnails)
                    {
                        FlipNext();
                    }
                    else if (state == State.VideoFlippedOut)
                    {
                        // load player with the video of the next thumbnail being flipped in
                        state = State.VideoLoading;
                        player.CueVideo(videoId);
                    }
                }*/
            }
            private void MaybeStartDemo()
            {
              //  if ( thumbnailLoader != null)
                //{
                    thumbnailLoader.SetPlaylist(PlaylistId); // loading the first thumbnail will kick off demo
                   
                //state = State.LoadingThumbnails;
               // }
            }

        

        void IYouTubeThumbnailLoaderOnThumbnailLoadedListener.OnThumbnailError(YouTubeThumbnailView view, YouTubeThumbnailLoaderErrorReason errorReason)
        {
            view.SetImageResource(Resource.Drawable.cart1);//no_thumbnail
        }

      /*  void IYouTubeThumbnailLoaderOnThumbnailLoadedListener.OnThumbnailLoaded(YouTubeThumbnailView view, string videoId)
        {
        }*/

        void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //Get our item from the list adapter
            var item = this.af.GetRawItem(e.Position);
        }
        private void ToFullScreen()
        {
            var intent = new Intent(this, typeof(VideoFragment));
            intent.PutExtra(PlayVideoActivity.ExtraUrlKey, "Y_UmWdcTrrc");
            StartActivityForResult(intent, PlayActivityRequestCode);
        }
        private void CheckYouTubeApi()
        {
            var errorReason = YouTubeApiServiceUtil.IsYouTubeApiServiceAvailable(this);
            if (errorReason.IsUserRecoverableError)
            {
                errorReason.GetErrorDialog(this, RecoveryDialogRequest).Show();
            }
            else if (errorReason != YouTubeInitializationResult.Success)
            {
                //var errorMessage = string.Format(GetString(Resource.String.error_player), errorReason);
                //  Toast.MakeText(this, errorMessage, ToastLength.Long).Show();
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == RecoveryDialogRequest)
            {
                // Recreate the activity if user performed a recovery action
                Recreate();
            }
        }

        public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            Layout();
        }


        void IYouTubePlayerOnFullscreenListener.OnFullscreen(bool isFullscreen)
        {
            this.isFullscreen = isFullscreen;
            Layout();
        }

        // Sets up the layout programatically for the three different states. Portrait, landscape or
        // fullscreen+landscape. This has to be done programmatically because we handle the orientation
        // changes ourselves in order to get fluent fullscreen transitions, so the xml layout resources
        // do not get reloaded.
        private void Layout()
        {
            var isPortrait = Resources.Configuration.Orientation == Android.Content.Res.Orientation.Portrait;

            //listFragment.View.Visibility = isFullscreen ? ViewStates.Gone : ViewStates.Visible;
            //listFragment.SetLabelVisibility(isPortrait);
            closeButton.Visibility = isPortrait ? ViewStates.Visible : ViewStates.Gone;

            if (isFullscreen)
            {
                videoBox.TranslationY = 0; // Reset any translation that was applied in portrait.
                                           //  SetLayoutSize(videoFragment.View, ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
                SetLayoutSizeAndGravity(videoBox, ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent, GravityFlags.Top | GravityFlags.Left);
            }
            else if (isPortrait)
            {
                //    SetLayoutSize(listFragment.View, ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
                SetLayoutSize(videoFragment.View, ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                SetLayoutSizeAndGravity(videoBox, ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent, GravityFlags.Bottom);
            }
            else
            {
                videoBox.TranslationY = 0; // Reset any translation that was applied in portrait.
                int screenWidth = DpToPx(Resources.Configuration.ScreenWidthDp);
                //  SetLayoutSize(listFragment.View, screenWidth / 4, ViewGroup.LayoutParams.MatchParent);
                int videoWidth = screenWidth - screenWidth / 4 - DpToPx(LandscapeVideoPadding);
                SetLayoutSize(videoFragment.View, videoWidth, ViewGroup.LayoutParams.WrapContent);
                SetLayoutSizeAndGravity(videoBox, videoWidth, ViewGroup.LayoutParams.WrapContent, GravityFlags.Right | GravityFlags.CenterVertical);
            }
        }

        private void OnClickClose(object sender, EventArgs e)
        {
            listFragment.ListView.ClearChoices();
            listFragment.ListView.RequestLayout();
            videoFragment.Pause();

            var animator = videoBox
                .Animate()
                .TranslationYBy(videoBox.Height)
                .SetDuration(AnimationDuration);
            RunOnAnimationEnd(animator, () => videoBox.Visibility = ViewStates.Invisible);
        }

        private void RunOnAnimationEnd(ViewPropertyAnimator animator, Action runnable)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.JellyBean)
            {
                animator.WithEndAction(new Java.Lang.Runnable(runnable));
            }
            else
            {
                animator.SetListener(new Listener(runnable));
            }
        }

        private class Listener : Java.Lang.Object, Animator.IAnimatorListener
        {
            private Action runnable;

            public Listener(Action runnable)
            {
                this.runnable = runnable;
            }

            public void OnAnimationCancel(Animator animation) { }

            public void OnAnimationEnd(Animator animation)
            {
                runnable?.Invoke();
            }

            public void OnAnimationRepeat(Animator animation) { }

            public void OnAnimationStart(Animator animation) { }
        }

        // A fragment that shows a static list of videos.
        public class VideoListFragment : ListFragment
        {
            private static readonly List<VideoEntry> VideoList = new List<VideoEntry> {
                new VideoEntry("YouTube Collection", "Y_UmWdcTrrc"),
                new VideoEntry("GMail Tap", "1KhZKNZO8mQ"),
                new VideoEntry("Chrome Multitask", "UiLSiqyDf4Y"),
                new VideoEntry("Google Fiber", "re0VRK6ouwI"),
                new VideoEntry("Autocompleter", "blB_X38YSxQ"),
                new VideoEntry("GMail Motion", "Bu927_ul_X0"),
                new VideoEntry("Translate for Animals", "3I24bSteJpw"),
            };

            private PageAdapter adapter;
            private View videoBox;

            public override void OnCreate(Bundle savedInstanceState)
            {
                base.OnCreate(savedInstanceState);
                adapter = new PageAdapter(Activity);//, VideoList);
                                                    //  adapter = new MyAdapter(Activity, (IMvxAndroidBindingContext)Context);//, VideoList);
            }

            public override void OnActivityCreated(Bundle savedInstanceState)
            {
                base.OnActivityCreated(savedInstanceState);

                videoBox = Activity.FindViewById(Resource.Id.video_box);
                ListView.ChoiceMode = ChoiceMode.Single;
                ListAdapter = adapter;
            }

            public override void OnListItemClick(ListView l, View v, int position, long id)
            {
                var videoId = VideoList[position].VideoId;

                var videoFragment = FragmentManager.FindFragmentById<VideoFragment>(Resource.Id.video_fragment_container);
                videoFragment.SetVideoId(videoId);

                // The videoBox is INVISIBLE if no video was previously selected, so we need to show it now.
                if (videoBox.Visibility != ViewStates.Visible)
                {
                    if (Resources.Configuration.Orientation == Android.Content.Res.Orientation.Portrait)
                    {
                        // Initially translate off the screen so that it can be animated in from below.
                        videoBox.TranslationY = videoBox.Height;
                    }
                    videoBox.Visibility = ViewStates.Visible;
                }

                // If the fragment is off the screen, we animate it in.
                if (videoBox.TranslationY > 0)
                {
                    videoBox.Animate().TranslationY(0).SetDuration(AnimationDuration);
                }
            }

            public override void OnDestroyView()
            {
                base.OnDestroyView();

                adapter.ReleaseLoaders();
            }

            public void SetLabelVisibility(bool visible)
            {
                adapter.SetLabelVisibility(visible);
            }
        }



        private class MyAdapter : MvxAdapter,
        YouTubeThumbnailView.IOnInitializedListener,
        IYouTubeThumbnailLoaderOnThumbnailLoadedListener
        {
            private Context mcon;
            private readonly List<VideoEntry> entries;
            private readonly List<View> entryViews;
            private readonly Dictionary<YouTubeThumbnailView, IYouTubeThumbnailLoader> thumbnailViewToLoaderMap;
            private readonly LayoutInflater inflater;
            private VideoFragment videoFragment;
            private MvxListView listview;
            private bool labelsVisible;
            public MyAdapter(Context context, IMvxAndroidBindingContext bindingContext) : base(context, bindingContext)
            {
                // this.entries = 
                mcon = context;
                entryViews = new List<View>();
                thumbnailViewToLoaderMap = new Dictionary<YouTubeThumbnailView, IYouTubeThumbnailLoader>();
                inflater = LayoutInflater.From(context);
                labelsVisible = true;

                FragmentManager fm = ((VideoListDemoActivity)Context).FragmentManager;
                videoFragment = (VideoFragment)fm.FindFragmentById(Resource.Id.video_fragment_container);


            }

            public override View GetView(int position, View convertView, ViewGroup parent)
            {
                var view = convertView;
                // var entry = entries[position];
                var video = (YoutubeItem)GetRawItem(position);
                  
                // There are three cases here
                if (view == null)
                {


                    // 1) The view has not yet been created - we need to initialize the YouTubeThumbnailView.
                    view = inflater.Inflate(Resource.Layout.video_list_item, parent, false);
                    listview = view.FindViewById<MvxListView>(Resource.Id.VideoItems);


                     
                        
                    var thumbnail = view.FindViewById<YouTubeThumbnailView>(Resource.Id.thumbnail);
                    thumbnail.Tag = video.VideoId ; //entry.VideoId;
                    thumbnail.Initialize(DeveloperKey.Key, this);
                    thumbnail.Click += (sender, args) =>
                    {

                        var intent = new Intent(mcon, typeof(PlayVideoActivity));
                        intent.AddFlags(ActivityFlags.NewTask);

                        intent.PutExtra(PlayVideoActivity.ExtraUrlKey, video.VideoId );
                        mcon.StartActivity(intent);

                            //listview = view.FindViewById<MvxListView>(Resource.Id.VideoItems);
                            //listview.PerformItemClick(view, position, GetItemId(position));
                        };
                }
                else
                {
                    var thumbnail = view.FindViewById<YouTubeThumbnailView>(Resource.Id.thumbnail);
                    var loader = thumbnailViewToLoaderMap[thumbnail];
                    if (loader == null)
                    {
                        // 2) The view is already created, and is currently being initialized. We store the
                        //    current videoId in the tag.
                        //  thumbnail.Tag = entry.VideoId;
                    }
                    else
                    {
                        // 3) The view is already created and already initialized. Simply set the right videoId
                        //    on the loader.
                        thumbnail.SetImageResource(Resource.Drawable.cart1);//loading_thumbnail
                                                                            // loader.SetVideo(entry.VideoId);
                    }
                }



                var label = view.FindViewById<TextView>(Resource.Id.text);
                // label.Text = entry.Text;
                label.Visibility = labelsVisible ? ViewStates.Visible : ViewStates.Gone;
                view.Click += openvideo;

                return view;
            }
            private void openvideo(object sender, EventArgs e)
            {
                // var videoId = VideoList[position].VideoId;
                //  int position = 
                //    .GetChildAdapterPosition((View)sender);


                //var videoFragment = new VideoFragment();
           //     videoFragment.SetVideoId("Y_UmWdcTrrc");

                // The videoBox is INVISIBLE if no video was previously selected, so we need to show it now.

            }






            public void ReleaseLoaders()
            {
                foreach (IYouTubeThumbnailLoader loader in thumbnailViewToLoaderMap.Values)
                {
                    loader.Release();
                }
            }
            void YouTubeThumbnailView.IOnInitializedListener.OnInitializationFailure(YouTubeThumbnailView view, YouTubeInitializationResult result)
            {
                view.SetImageResource(Resource.Drawable.cart1);// no_thumbnail
            }

            void YouTubeThumbnailView.IOnInitializedListener.OnInitializationSuccess(YouTubeThumbnailView view, IYouTubeThumbnailLoader loader)
            {
                loader.SetOnThumbnailLoadedListener(this);
                thumbnailViewToLoaderMap.Add(view, loader);
                view.SetImageResource(Resource.Drawable.cart1);//loading_thumbnail
                var videoId = (string)view.Tag;
                loader.SetVideo(videoId);
            }

            void IYouTubeThumbnailLoaderOnThumbnailLoadedListener.OnThumbnailError(YouTubeThumbnailView view, YouTubeThumbnailLoaderErrorReason errorReason)
            {
                view.SetImageResource(Resource.Drawable.cart1);//no_thumbnail
            }

            void IYouTubeThumbnailLoaderOnThumbnailLoadedListener.OnThumbnailLoaded(YouTubeThumbnailView view, string videoId)
            {
            }
        }
        // Adapter for the video list. Manages a set of YouTubeThumbnailViews, including initializing each
        // of them only once and keeping track of the loader of each one. When the ListFragment gets
        // destroyed it releases all the loaders.
        private class PageAdapter : BaseAdapter,
            YouTubeThumbnailView.IOnInitializedListener,
            IYouTubeThumbnailLoaderOnThumbnailLoadedListener
        {
            private readonly List<VideoEntry> entries;
            private readonly List<View> entryViews;
            private readonly Dictionary<YouTubeThumbnailView, IYouTubeThumbnailLoader> thumbnailViewToLoaderMap;
            private readonly LayoutInflater inflater;

            private bool labelsVisible;
            /*  public PageAdapter( IMvxAndroidBindingContext bindingContext)

              { }*/
            public PageAdapter(Context context)//, List<VideoEntry> entries)
            {
                // this.entries = entries;

                entryViews = new List<View>();
                thumbnailViewToLoaderMap = new Dictionary<YouTubeThumbnailView, IYouTubeThumbnailLoader>();
                inflater = LayoutInflater.From(context);

                labelsVisible = true;
            }

            public void ReleaseLoaders()
            {
                foreach (IYouTubeThumbnailLoader loader in thumbnailViewToLoaderMap.Values)
                {
                    loader.Release();
                }
            }

            public void SetLabelVisibility(bool visible)
            {
                labelsVisible = visible;
                foreach (View view in entryViews)
                {
                    view.FindViewById(Resource.Id.text).Visibility = visible ? ViewStates.Visible : ViewStates.Gone;
                }
            }

            public override int Count => entries.Count;

            public override Java.Lang.Object GetItem(int position) => entries[position];

            public override long GetItemId(int position) => 0;

            public override View GetView(int position, View convertView, ViewGroup parent)
            {
                var view = convertView;
                var entry = entries[position];

                // There are three cases here
                if (view == null)
                {

                    // 1) The view has not yet been created - we need to initialize the YouTubeThumbnailView.
                    view = inflater.Inflate(Resource.Layout.video_list_item, parent, false);
                    var thumbnail = view.FindViewById<YouTubeThumbnailView>(Resource.Id.thumbnail);
                    thumbnail.Tag = entry.VideoId;
                    thumbnail.Initialize(DeveloperKey.Key, this);
                }
                else
                {
                    var thumbnail = view.FindViewById<YouTubeThumbnailView>(Resource.Id.thumbnail);
                    var loader = thumbnailViewToLoaderMap[thumbnail];
                    if (loader == null)
                    {
                        // 2) The view is already created, and is currently being initialized. We store the
                        //    current videoId in the tag.
                        thumbnail.Tag = entry.VideoId;
                    }
                    else
                    {
                        // 3) The view is already created and already initialized. Simply set the right videoId
                        //    on the loader.
                        thumbnail.SetImageResource(Resource.Drawable.cart1);//loading_thumbnail
                        loader.SetVideo(entry.VideoId);
                    }
                }

                var label = view.FindViewById<TextView>(Resource.Id.text);
                label.Text = entry.Text;
                label.Visibility = labelsVisible ? ViewStates.Visible : ViewStates.Gone;
                // label.SetOnClickListener(new OnClickListener())
                return view;
            }

            void YouTubeThumbnailView.IOnInitializedListener.OnInitializationFailure(YouTubeThumbnailView view, YouTubeInitializationResult result)
            {
                view.SetImageResource(Resource.Drawable.cart1);// no_thumbnail
            }

            void YouTubeThumbnailView.IOnInitializedListener.OnInitializationSuccess(YouTubeThumbnailView view, IYouTubeThumbnailLoader loader)
            {
                loader.SetOnThumbnailLoadedListener(this);
                thumbnailViewToLoaderMap.Add(view, loader);
                view.SetImageResource(Resource.Drawable.cart1);//loading_thumbnail
                var videoId = (string)view.Tag;
                loader.SetVideo(videoId);
            }

            void IYouTubeThumbnailLoaderOnThumbnailLoadedListener.OnThumbnailError(YouTubeThumbnailView view, YouTubeThumbnailLoaderErrorReason errorReason)
            {
                view.SetImageResource(Resource.Drawable.cart1);//no_thumbnail
            }

            void IYouTubeThumbnailLoaderOnThumbnailLoadedListener.OnThumbnailLoaded(YouTubeThumbnailView view, string videoId)
            {
            }
        }

        public class VideoFragment : YouTubePlayerFragment, IYouTubePlayerOnInitializedListener
        {
            private IYouTubePlayer player;
            private string videoId;

            public override void OnCreate(Bundle savedInstanceState)
            {
                base.OnCreate(savedInstanceState);
                /* var intent = Intent;
                 videoUrl = intent?.GetStringExtra(ExtraUrlKey);*/

                Initialize(DeveloperKey.Key, this);
            }

            public override void OnDestroy()
            {
                if (player != null)
                {
                    player.Release();
                }

                base.OnDestroy();
            }

            public void SetVideoId(string videoId)
            {
                if (videoId != null && videoId != this.videoId)
                { //Y_UmWdcTrrc
                    this.videoId = videoId;
                    if (player != null)
                    {
                        player.CueVideo(videoId);
                    }
                }
            }

            public void Pause()
            {
                if (player != null)
                {
                    player.Pause();
                }
            }

            void IYouTubePlayerOnInitializedListener.OnInitializationFailure(IYouTubePlayerProvider provider, YouTubeInitializationResult result)
            {
                player = null;
            }

            void IYouTubePlayerOnInitializedListener.OnInitializationSuccess(IYouTubePlayerProvider provider, IYouTubePlayer player, bool restored)
            {
                this.player = player;
                player.AddFullscreenControlFlag(YouTubePlayer.FullscreenFlagCustomLayout);
                player.SetOnFullscreenListener((VideoListDemoActivity)Activity);
                if (!restored && videoId != null)
                {
                    player.CueVideo(videoId);
                }
            }

            public static VideoFragment Create() => new VideoFragment();
        }

        private class VideoEntry : Java.Lang.Object
        {
            public VideoEntry(string text, string videoId)
            {
                Text = text;
                VideoId = videoId;
            }

            public string Text { get; private set; }
            public string VideoId { get; private set; }
        }

        // Utility methods for layouting.

        private int DpToPx(int dp)
        {
            return (int)(dp * Resources.DisplayMetrics.Density + 0.5f);
        }

        private static void SetLayoutSize(View view, int width, int height)
        {
            var param = view.LayoutParameters;
            param.Width = width;
            param.Height = height;
            view.LayoutParameters = param;
        }

        private static void SetLayoutSizeAndGravity(View view, int width, int height, GravityFlags gravity)
        {
            var param = (FrameLayout.LayoutParams)view.LayoutParameters;
            param.Width = width;
            param.Height = height;
            param.Gravity = gravity;
            view.LayoutParameters = param;
        }
    }
}
}