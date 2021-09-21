﻿
using Android.App;
using Android.Content.PM;
using Android.Hardware;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Counter.Droid;
using Xamarin.Forms;

[assembly: Dependency ( typeof ( PlaySound ) )]
namespace Counter.Droid {
        public class PlaySound : IPlaySoundService {
                public void AlertSound ( ) {
                        Android.Net.Uri uri = RingtoneManager.GetDefaultUri ( RingtoneType.Notification );
                        Ringtone ringtone = RingtoneManager.GetRingtone ( MainActivity.Instance.ApplicationContext , uri );
                        ringtone.Play ( );
                }
        }
        [Activity ( Label = "血汗計算機" , Icon = "@mipmap/icon" , Theme = "@style/MainTheme" , MainLauncher = true , ScreenOrientation = ScreenOrientation.Portrait , ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
        public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity, ISensorEventListener {
                public static MainActivity Instance { get; set; }
                protected override void OnCreate ( Bundle savedInstanceState ) {
                        base.OnCreate ( savedInstanceState );

                        Xamarin.Essentials.Platform.Init ( this , savedInstanceState );
                        Forms.Init ( this , savedInstanceState );
                        LoadApplication ( new App ( ) );

                        //proximity sensor
                        SensorManager sm;
                        sm = ( SensorManager ) GetSystemService ( SensorService );
                        if ( sm.GetSensorList ( SensorType.Proximity ) != null ) {
                                Sensor prox = sm.GetDefaultSensor ( SensorType.Proximity );
                                sm.RegisterListener ( this , prox , SensorDelay.Normal );
                        }
                        Instance = this;
                }
                public void OnSensorChanged ( SensorEvent e ) {
                        MessagingCenter.Send ( Xamarin.Forms.Application.Current , "prox" , e.Values [ 0 ].ToString ( ) );
                }
                void ISensorEventListener.OnAccuracyChanged ( Sensor sensor , SensorStatus accuracy ) {
                        return;
                }
                public override void OnRequestPermissionsResult ( int requestCode , string [ ] permissions , [GeneratedEnum] Permission [ ] grantResults ) {
                        Xamarin.Essentials.Platform.OnRequestPermissionsResult ( requestCode , permissions , grantResults );
                        base.OnRequestPermissionsResult ( requestCode , permissions , grantResults );
                }
                protected override void OnResume ( ) {
                        base.OnResume ( );

                        //proximity sensor
                        SensorManager sm;
                        sm = ( SensorManager ) GetSystemService ( SensorService );
                        Sensor prox = sm.GetDefaultSensor ( SensorType.Proximity );
                        sm.RegisterListener ( this , prox , SensorDelay.Normal );
                }
                protected override void OnPause ( ) {
                        base.OnPause ( );

                        //proximity sensor
                        SensorManager sm;
                        sm = ( SensorManager ) GetSystemService ( SensorService );
                        sm.UnregisterListener ( this );
                }
        }
}