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
            if (video.Location != null)
            {
                var lat = video.Location.Latitude;
                var lot = video.Location.Longitude;

                CameraPosition cp = new CameraPosition(new LatLng(lat, lot), 18, 30, 0);
                var mCameraUpdate = CameraUpdateFactory.NewCameraPosition(cp);
                handler.PlatformView?.Map.AnimateCamera(mCameraUpdate);
            }

        }
        protected override MapView CreatePlatformView()
        {
            var location = VirtualView.InitLocation;

            //AMapLocationClient.UpdatePrivacyAgree(Context, true);
            //AMapLocationClient.UpdatePrivacyShow(Context, true, true);

            //mapView.OnCreate(Bundle);
            AMapOptions aOptions = new AMapOptions();
            aOptions.InvokeScrollGesturesEnabled(false);// ��ֹͨ�������ƶ���ͼ
            aOptions.InvokeTiltGesturesEnabled(false);// ��ֹͨ��������б��ͼ
            if (location != null)
            {
                CameraPosition initPos = new CameraPosition.Builder()
                    .Target(new LatLng(location.Latitude, location.Longitude))
                    .Zoom(18)
                    .Build();
                aOptions.InvokeCamera(initPos);
            }


            mapView = new Com.Amap.Api.Maps.MapView(Context, aOptions);
            var myLocationStyle = new MyLocationStyle();//��ʼ����λ������ʽ��myLocationStyle.myLocationType(MyLocationStyle.LOCATION_TYPE_LOCATION_ROTATE);//������λ���ҽ��ӽ��ƶ�����ͼ���ĵ㣬��λ�������豸������ת�����һ�����豸�ƶ�����1��1�ζ�λ�����������myLocationType��Ĭ��Ҳ��ִ�д���ģʽ��
            myLocationStyle.InvokeMyLocationType(MyLocationStyle.LocationTypeShow);
            mapView.Map.MyLocationStyle = myLocationStyle;//���ö�λ�����Style                                                         //aMap.getUiSettings().setMyLocationButtonEnabled(true);����Ĭ�϶�λ��ť�Ƿ���ʾ���Ǳ������á�
            mapView.Map.MyLocationEnabled = true;// ����Ϊtrue��ʾ������ʾ��λ���㣬false��ʾ���ض�λ���㲢�����ж�λ��Ĭ����false��

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