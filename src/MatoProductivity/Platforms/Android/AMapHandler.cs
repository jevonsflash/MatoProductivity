using System;
using Android.OS;
using Java.Interop;
using Microsoft.Maui.Handlers;
using Com.Amap.Api.Maps;
using Com.Amap.Api.Location;
using static Android.Provider.MediaStore;
using Com.Amap.Api.Maps.Model;

namespace MatoProductivity.Controls
{
    public partial class AMapHandler : ViewHandler<IAMap, MapView>
    {
        private AMapHelper _mapHelper;
        private MapView mapView;
        internal static Bundle Bundle { get; set; }

        public AMapHandler(IPropertyMapper mapper, CommandMapper commandMapper = null) : base(mapper, commandMapper)
        {
        }


        public static void MapAddress(AMapHandler handler, IAMap video)
        {
  
        }
        public static void MapLocation(AMapHandler handler, IAMap video)
        {
            var lat = video.Location.Latitude;
            var lot = video.Location.Longitude;

            CameraPosition cp = new CameraPosition(new LatLng(lat, lot), 18, 30, 0);
            var mCameraUpdate = CameraUpdateFactory.NewCameraPosition(cp);
            handler.PlatformView?.Map.AnimateCamera(mCameraUpdate);
        }
        protected override MapView CreatePlatformView()
        {
            MyLocationStyle myLocationStyle;
            myLocationStyle = new MyLocationStyle();//初始化定位蓝点样式类myLocationStyle.myLocationType(MyLocationStyle.LOCATION_TYPE_LOCATION_ROTATE);//连续定位、且将视角移动到地图中心点，定位点依照设备方向旋转，并且会跟随设备移动。（1秒1次定位）如果不设置myLocationType，默认也会执行此种模式。
            aMap.setMyLocationStyle(myLocationStyle);//设置定位蓝点的Style
                                                     //aMap.getUiSettings().setMyLocationButtonEnabled(true);设置默认定位按钮是否显示，非必需设置。
            aMap.setMyLocationEnabled(true);// 设置为true表示启动显示定位蓝点，false表示隐藏定位蓝点并不进行定位，默认是false。

            //AMapLocationClient.UpdatePrivacyAgree(Context, true);
            //AMapLocationClient.UpdatePrivacyShow(Context, true, true);

            //mapView.OnCreate(Bundle);
            AMapOptions aOptions = new AMapOptions();
            aOptions.InvokeRotateGesturesEnabled(true);
            aOptions.ScrollGesturesEnabled(false);// 禁止通过手势移动地图
            aOptions.tiltGesturesEnabled(false);// 禁止通过手势倾斜地图
            CameraPosition LUJIAZUI = new CameraPosition.Builder()
            .Target(Constants.SHANGHAI).Zoom(18).Bearing(0).Tilt(30).Build();
            aOptions.Camera (LUJIAZUI);

            mapView = new Com.Amap.Api.Maps.MapView(Context,aOptions);
            //mapView.OnCreate(savedInstanceState);
            //SetContentView(mapView);
            return mapView;
        }

        protected override void ConnectHandler(MapView platformView)
        {
            base.ConnectHandler(platformView);


            AMapLocationClient.UpdatePrivacyAgree(Context, true);
            AMapLocationClient.UpdatePrivacyShow(Context, true, true);

            _mapHelper = new AMapHelper(Bundle, platformView);
            //_mapHelper.MapIsReady += _mapHelper_MapIsReady;
            mapView = _mapHelper.CallCreateMap();
        }
    }



    class AMapHelper : Java.Lang.Object
    {

        private Bundle _bundle;
        private MapView _mapView;

        public event EventHandler MapIsReady;

        public MapView Map { get; set; }

        public AMapHelper(Bundle bundle, MapView mapView)
        {
            _bundle = bundle;
            _mapView = mapView;
        }

        public MapView CallCreateMap()
        {

            //AMapLocationClient.UpdatePrivacyAgree(Context, true);
            //AMapLocationClient.UpdatePrivacyShow(Context, true, true);
            _mapView.OnCreate(_bundle);
            return _mapView;
            //_mapView.GetMapAsync(this);
        }


    }

}