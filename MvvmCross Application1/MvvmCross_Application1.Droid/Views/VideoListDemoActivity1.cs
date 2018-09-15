using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Android.Animation;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using FragmentManager = Android.Support.V4.App.FragmentManager;

using Android.Support.V7.App;

namespace MvvmCross_Application1.Droid.Views
{ 
/*
    [Activity( MainLauncher = true, 
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.KeyboardHidden)]
    [Register("com.examples.youtubeapidemo.VideoListDemoActivity")]
    
    public class VideoListDemoActivity1 : Activity, IYouTubePlayerOnFullscreenListener
    {
        // The duration of the animation sliding up the video in portrait.
        private const int AnimationDuration = 300;
        // The padding between the video list and the video in landscape orientation.
        private const int LandscapeVideoPadding = 5;

        // The request code when calling startActivityForResult to recover from an API service error.
        private const int RecoveryDialogRequest = 1;

     // private VideoListFragment listFragment;
        private VideoFragment videoFragment;

        private View videoBox;
        private View closeButton;

        private bool isFullscreen;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
           
            SetContentView(Resource.Layout.video_list_demo);
            // FragmentManager manager = FragmentManager.
            VideoFragment hello = new VideoFragment();
           FragmentTransaction fragmentTransaction = FragmentManager.BeginTransaction();
            fragmentTransaction.Replace(Resource.Id.content_frame , hello);
            fragmentTransaction.Commit();
            
            // listFragment = FragmentManager.FindFragmentById<VideoListFragment>(Resource.Id.list_fragment);
          //  videoFragment = FragmentManager.FindFragmentById<VideoFragment>(Resource.Id.youtube_view);

            videoBox = FindViewById(Resource.Id.video_box);
            closeButton = FindViewById(Resource.Id.close_button);
            closeButton.Click += OnClickClose;

            videoBox.Visibility = ViewStates.Invisible;

            Layout();

            CheckYouTubeApi();
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
                var errorMessage = string.Format("error", errorReason);
                Toast.MakeText(this, errorMessage, ToastLength.Long).Show();
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

         //   listFragment.View.Visibility = isFullscreen ? ViewStates.Gone : ViewStates.Visible;
//listFragment.SetLabelVisibility(isPortrait);
            closeButton.Visibility = isPortrait ? ViewStates.Visible : ViewStates.Gone;

           /* if (isFullscreen)
            {
                videoBox.TranslationY = 0; // Reset any translation that was applied in portrait.
                SetLayoutSize(videoFragment.View, ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
                SetLayoutSizeAndGravity(videoBox, ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent, GravityFlags.Top | GravityFlags.Left);
            }
            else if (isPortrait)
            {
                SetLayoutSize(listFragment.View, ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
                SetLayoutSize(videoFragment.View, ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                SetLayoutSizeAndGravity(videoBox, ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent, GravityFlags.Bottom);
            }
            else
            {
              /*  videoBox.TranslationY = 0; // Reset any translation that was applied in portrait.
                int screenWidth = DpToPx(Resources.Configuration.ScreenWidthDp);
                SetLayoutSize(listFragment.View, screenWidth / 4, ViewGroup.LayoutParams.MatchParent);
                int videoWidth = screenWidth - screenWidth / 4 - DpToPx(LandscapeVideoPadding);
                SetLayoutSize(videoFragment.View, videoWidth, ViewGroup.LayoutParams.WrapContent);
                SetLayoutSizeAndGravity(videoBox, videoWidth, ViewGroup.LayoutParams.WrapContent, GravityFlags.Right | GravityFlags.CenterVertical);*/
           // }
        }

      /*  public void OnClickClose(object sender, EventArgs e)
        {
          //  listFragment.ListView.ClearChoices();
          //  listFragment.ListView.RequestLayout();
          //  videoFragment.Pause();

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
             //   animator.SetListener(new Listener(runnable));
            }
        }
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
                adapter = new PageAdapter(Activity, VideoList);
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

            //    var videoFragment = FragmentManager.FindFragmentById<VideoFragment>(Resource.Id.video_fragment_container);
              //  videoFragment.SetVideoId(videoId);

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

    }
    public class PageAdapter : BaseAdapter,
            YouTubeThumbnailView.IOnInitializedListener,
            IYouTubeThumbnailLoaderOnThumbnailLoadedListener
    {
        private readonly List<VideoEntry> entries;
        private readonly List<View> entryViews;
        private readonly Dictionary<YouTubeThumbnailView, IYouTubeThumbnailLoader> thumbnailViewToLoaderMap;
        private readonly LayoutInflater inflater;

        private bool labelsVisible;

        public PageAdapter(Context context, List<VideoEntry> entries)
        {
            this.entries = entries;

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
                //view = inflater.Inflate(Resource.Layout.video_list_item, parent, false);
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
                    thumbnail.SetImageResource(Resource.Drawable.cart1);
                    loader.SetVideo(entry.VideoId);
                }
            }

            var label = view.FindViewById<TextView>(Resource.Id.text);
            label.Text = entry.Text;
            label.Visibility = labelsVisible ? ViewStates.Visible : ViewStates.Gone;

            return view;
        }

        void YouTubeThumbnailView.IOnInitializedListener.OnInitializationFailure(YouTubeThumbnailView view, YouTubeInitializationResult result)
        {
            view.SetImageResource(Resource.Drawable.cart1);
        }

        void YouTubeThumbnailView.IOnInitializedListener.OnInitializationSuccess(YouTubeThumbnailView view, IYouTubeThumbnailLoader loader)
        {
            loader.SetOnThumbnailLoadedListener(this);
            thumbnailViewToLoaderMap.Add(view, loader);
            view.SetImageResource(Resource.Drawable.cart1);
            var videoId = (string)view.Tag;
            loader.SetVideo(videoId);
        }

        void IYouTubeThumbnailLoaderOnThumbnailLoadedListener.OnThumbnailError(YouTubeThumbnailView view, YouTubeThumbnailLoaderErrorReason errorReason)
        {
            view.SetImageResource(Resource.Drawable.cart1);
        }

        void IYouTubeThumbnailLoaderOnThumbnailLoadedListener.OnThumbnailLoaded(YouTubeThumbnailView view, string videoId)
        {
        }
    }*/

    
