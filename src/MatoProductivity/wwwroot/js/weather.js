//天气图表
window.weatherWin = {

    //绘制天气温度图表
    DrawCharts: function (chartId, dailyWeather) {

        this.InitCharts(chartId);

        this.FillCharts(dailyWeather);
    },

    //初始化图表
    InitCharts: function (chartId) {

        var weatherChartElement = document.getElementById(chartId);
        if (weatherChartElement == null)
            return;

        // 基于准备好的dom，初始化echarts实例
        var weatherChart = echarts.init(weatherChartElement);

        weatherChart.showLoading();

        window.weatherWin.weatherChart = weatherChart;
    },

    //填充图表的数据
    FillCharts: function (dailyWeather) {

        if (this.weatherChart == null)
            return;

        const days = dailyWeather.days;
        const temperatures = dailyWeather.temperatures;

        var weatherLineOption = {

            xAxis: {
                type: 'category',
                name: '日期',
                data: days,
            },
            yAxis: {
                type: 'value',
                name: '温度',
                min: -20,
                max: 60,
            },

            series: [
                {
                    type: 'line',
                    label: {
                        show: true,
                        position: 'bottom',
                    },
                    data: temperatures,
                },
            ]
        };

        // 使用刚指定的配置项和数据显示图表。
        this.weatherChart.setOption(weatherLineOption);

        this.weatherChart.hideLoading();
    },

};
