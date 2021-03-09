package crc640ecbbb149701c005;


public class ScanCallback
	extends android.bluetooth.le.ScanCallback
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onScanResult:(ILandroid/bluetooth/le/ScanResult;)V:GetOnScanResult_ILandroid_bluetooth_le_ScanResult_Handler\n" +
			"";
		mono.android.Runtime.register ("BalizaFacil.Droid.Services.ScanCallback, BalizaFacil.Android", ScanCallback.class, __md_methods);
	}


	public ScanCallback ()
	{
		super ();
		if (getClass () == ScanCallback.class)
			mono.android.TypeManager.Activate ("BalizaFacil.Droid.Services.ScanCallback, BalizaFacil.Android", "", this, new java.lang.Object[] {  });
	}


	public void onScanResult (int p0, android.bluetooth.le.ScanResult p1)
	{
		n_onScanResult (p0, p1);
	}

	private native void n_onScanResult (int p0, android.bluetooth.le.ScanResult p1);

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
