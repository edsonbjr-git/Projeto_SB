package crc640ecbbb149701c005;


public class SensorGATTCallback
	extends android.bluetooth.BluetoothGattCallback
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onConnectionStateChange:(Landroid/bluetooth/BluetoothGatt;II)V:GetOnConnectionStateChange_Landroid_bluetooth_BluetoothGatt_IIHandler\n" +
			"n_onServicesDiscovered:(Landroid/bluetooth/BluetoothGatt;I)V:GetOnServicesDiscovered_Landroid_bluetooth_BluetoothGatt_IHandler\n" +
			"n_onCharacteristicWrite:(Landroid/bluetooth/BluetoothGatt;Landroid/bluetooth/BluetoothGattCharacteristic;I)V:GetOnCharacteristicWrite_Landroid_bluetooth_BluetoothGatt_Landroid_bluetooth_BluetoothGattCharacteristic_IHandler\n" +
			"n_onDescriptorWrite:(Landroid/bluetooth/BluetoothGatt;Landroid/bluetooth/BluetoothGattDescriptor;I)V:GetOnDescriptorWrite_Landroid_bluetooth_BluetoothGatt_Landroid_bluetooth_BluetoothGattDescriptor_IHandler\n" +
			"n_onCharacteristicChanged:(Landroid/bluetooth/BluetoothGatt;Landroid/bluetooth/BluetoothGattCharacteristic;)V:GetOnCharacteristicChanged_Landroid_bluetooth_BluetoothGatt_Landroid_bluetooth_BluetoothGattCharacteristic_Handler\n" +
			"";
		mono.android.Runtime.register ("BalizaFacil.Droid.Services.SensorGATTCallback, BalizaFacil.Android", SensorGATTCallback.class, __md_methods);
	}


	public SensorGATTCallback ()
	{
		super ();
		if (getClass () == SensorGATTCallback.class)
			mono.android.TypeManager.Activate ("BalizaFacil.Droid.Services.SensorGATTCallback, BalizaFacil.Android", "", this, new java.lang.Object[] {  });
	}


	public void onConnectionStateChange (android.bluetooth.BluetoothGatt p0, int p1, int p2)
	{
		n_onConnectionStateChange (p0, p1, p2);
	}

	private native void n_onConnectionStateChange (android.bluetooth.BluetoothGatt p0, int p1, int p2);


	public void onServicesDiscovered (android.bluetooth.BluetoothGatt p0, int p1)
	{
		n_onServicesDiscovered (p0, p1);
	}

	private native void n_onServicesDiscovered (android.bluetooth.BluetoothGatt p0, int p1);


	public void onCharacteristicWrite (android.bluetooth.BluetoothGatt p0, android.bluetooth.BluetoothGattCharacteristic p1, int p2)
	{
		n_onCharacteristicWrite (p0, p1, p2);
	}

	private native void n_onCharacteristicWrite (android.bluetooth.BluetoothGatt p0, android.bluetooth.BluetoothGattCharacteristic p1, int p2);


	public void onDescriptorWrite (android.bluetooth.BluetoothGatt p0, android.bluetooth.BluetoothGattDescriptor p1, int p2)
	{
		n_onDescriptorWrite (p0, p1, p2);
	}

	private native void n_onDescriptorWrite (android.bluetooth.BluetoothGatt p0, android.bluetooth.BluetoothGattDescriptor p1, int p2);


	public void onCharacteristicChanged (android.bluetooth.BluetoothGatt p0, android.bluetooth.BluetoothGattCharacteristic p1)
	{
		n_onCharacteristicChanged (p0, p1);
	}

	private native void n_onCharacteristicChanged (android.bluetooth.BluetoothGatt p0, android.bluetooth.BluetoothGattCharacteristic p1);

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
