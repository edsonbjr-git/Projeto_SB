package crc64c9507a3a9a8a16d9;


public class SlideMenuDroidRenderer
	extends crc643f46942d9dd1fff9.ViewRenderer_2
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onTouchEvent:(Landroid/view/MotionEvent;)Z:GetOnTouchEvent_Landroid_view_MotionEvent_Handler\n" +
			"";
		mono.android.Runtime.register ("SlideOverKit.Droid.SlideMenuDroidRenderer, SlideOverKit.Droid", SlideMenuDroidRenderer.class, __md_methods);
	}


	public SlideMenuDroidRenderer (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == SlideMenuDroidRenderer.class)
			mono.android.TypeManager.Activate ("SlideOverKit.Droid.SlideMenuDroidRenderer, SlideOverKit.Droid", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public SlideMenuDroidRenderer (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == SlideMenuDroidRenderer.class)
			mono.android.TypeManager.Activate ("SlideOverKit.Droid.SlideMenuDroidRenderer, SlideOverKit.Droid", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public SlideMenuDroidRenderer (android.content.Context p0)
	{
		super (p0);
		if (getClass () == SlideMenuDroidRenderer.class)
			mono.android.TypeManager.Activate ("SlideOverKit.Droid.SlideMenuDroidRenderer, SlideOverKit.Droid", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public boolean onTouchEvent (android.view.MotionEvent p0)
	{
		return n_onTouchEvent (p0);
	}

	private native boolean n_onTouchEvent (android.view.MotionEvent p0);

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
