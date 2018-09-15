package mono.com.google.android.exoplayer2.video;


public class VideoListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.google.android.exoplayer2.video.VideoListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onRenderedFirstFrame:()V:GetOnRenderedFirstFrameHandler:Com.Google.Android.Exoplayer2.Video.IVideoListenerInvoker, ExoPlayer.Core\n" +
			"n_onVideoSizeChanged:(IIIF)V:GetOnVideoSizeChanged_IIIFHandler:Com.Google.Android.Exoplayer2.Video.IVideoListenerInvoker, ExoPlayer.Core\n" +
			"";
		mono.android.Runtime.register ("Com.Google.Android.Exoplayer2.Video.IVideoListenerImplementor, ExoPlayer.Core, Version=2.8.8.0, Culture=neutral, PublicKeyToken=null", VideoListenerImplementor.class, __md_methods);
	}


	public VideoListenerImplementor ()
	{
		super ();
		if (getClass () == VideoListenerImplementor.class)
			mono.android.TypeManager.Activate ("Com.Google.Android.Exoplayer2.Video.IVideoListenerImplementor, ExoPlayer.Core, Version=2.8.8.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onRenderedFirstFrame ()
	{
		n_onRenderedFirstFrame ();
	}

	private native void n_onRenderedFirstFrame ();


	public void onVideoSizeChanged (int p0, int p1, int p2, float p3)
	{
		n_onVideoSizeChanged (p0, p1, p2, p3);
	}

	private native void n_onVideoSizeChanged (int p0, int p1, int p2, float p3);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
