package crc647e522bc8ebefc632;


public class ViewRenderer
	extends crc648e35430423bd4943.SKCanvasView
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onSizeChanged:(IIII)V:GetOnSizeChanged_IIIIHandler\n" +
			"n_onTouchEvent:(Landroid/view/MotionEvent;)Z:GetOnTouchEvent_Landroid_view_MotionEvent_Handler\n" +
			"";
		mono.android.Runtime.register ("SkiaSharp.Components.Layout.Droid.ViewRenderer, SkiaSharp.Components.Layout.Droid", ViewRenderer.class, __md_methods);
	}


	public ViewRenderer (android.content.Context p0)
	{
		super (p0);
		if (getClass () == ViewRenderer.class)
			mono.android.TypeManager.Activate ("SkiaSharp.Components.Layout.Droid.ViewRenderer, SkiaSharp.Components.Layout.Droid", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public ViewRenderer (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == ViewRenderer.class)
			mono.android.TypeManager.Activate ("SkiaSharp.Components.Layout.Droid.ViewRenderer, SkiaSharp.Components.Layout.Droid", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public ViewRenderer (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == ViewRenderer.class)
			mono.android.TypeManager.Activate ("SkiaSharp.Components.Layout.Droid.ViewRenderer, SkiaSharp.Components.Layout.Droid", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public ViewRenderer (android.content.Context p0, android.util.AttributeSet p1, int p2, int p3)
	{
		super (p0, p1, p2, p3);
		if (getClass () == ViewRenderer.class)
			mono.android.TypeManager.Activate ("SkiaSharp.Components.Layout.Droid.ViewRenderer, SkiaSharp.Components.Layout.Droid", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}


	public void onSizeChanged (int p0, int p1, int p2, int p3)
	{
		n_onSizeChanged (p0, p1, p2, p3);
	}

	private native void n_onSizeChanged (int p0, int p1, int p2, int p3);


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
