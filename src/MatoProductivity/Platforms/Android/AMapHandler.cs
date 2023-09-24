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
            myLocationStyle = new MyLocationStyle();//��ʼ����λ������ʽ��myLocationStyle.myLocationType(MyLocationStyle.LOCATION_TYPE_LOCATION_ROTATE);//������λ���ҽ��ӽ��ƶ�����ͼ���ĵ㣬��λ�������豸������ת�����һ�����豸�ƶ�����1��1�ζ�λ�����������myLocationType��Ĭ��Ҳ��ִ�д���ģʽ��
            aMap.setMyLocationStyle(myLocationStyle);//���ö�λ�����Style
                                                     //aMap.getUiSettings().setMyLocationButtonEnabled(true);����Ĭ�϶�λ��ť�Ƿ���ʾ���Ǳ������á�
            aMap.setMyLocationEnabled(true);// ����Ϊtrue��ʾ������ʾ��λ���㣬false��ʾ���ض�λ���㲢�����ж�λ��Ĭ����false��

            //AMapLocationClient.UpdatePrivacyAgree(Context, true);
            //AMapLocationClient.UpdatePrivacyShow(Context, true, true);

            //mapView.OnCreate(Bundle);
            AMapOptions aOptions = new AMapOptions();
            aOptions.InvokeRotateGesturesEnabled(true);
            aOptions.ScrollGesturesEnabled(false);// ��ֹͨ�������ƶ���ͼ
            aOptions.tiltGesturesEnabled(false);// ��ֹͨ��������б��ͼ
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