﻿$(document).ready(function () {

    $.ajax({
        type: 'GET',
        dataType: "json",
        contentType: "application/json",
        url: '/manager/GetAllStatistics',
        success: function (result) {
            google.charts.load('current', {
                'packages': ['corechart']
            });
            google.charts.setOnLoadCallback(function () {
                drawChartStatisticsV1(result);
                drawChartStatisticsV2(result);
            });
        }
    });

    //$.ajax({
    //    type: 'GET',
    //    dataType: "json",
    //    contentType: "application/json",
    //    url: '/manager/GetAllStatistics',
    //    success: function (result) {
    //        google.charts.load('current', {
    //            'packages': ['corechart']
    //        });
    //        google.charts.setOnLoadCallback(function () {
    //            drawChartStatistics(result);
    //        });
    //    }
    //});

   

    function drawChartStatisticsV1(result) {
        var data = new google.visualization.DataTable();
        data.addColumn('date', 'Date');
        data.addColumn('number', 'Ca');
        var dataArray = [];
        $.each(result, function (i, obj) {
            dataArray.push([new Date(obj.date), obj.ca]);
        });


        data.addRows(dataArray);


        var options = {
            title: 'Facturation mission',
            curveType: 'function',
            legend: { position: 'bottom' },
            width: 400,
            height: 300
        };

        var chart = new google.visualization.LineChart(document.getElementById('linechart_div'));

        chart.draw(data, options);
    }

    function drawChartStatisticsV2(result) {

        var data = new google.visualization.DataTable();
        data.addColumn('date', 'Date');
        data.addColumn('number', 'Merge');
        var dataArray = [];
        $.each(result, function (i, obj) {
            dataArray.push([new Date(obj.date), obj.merge]);
        });

        data.addRows(dataArray);

        var piechart_options = {
            title: 'Pie Chart: Taux de marge de la mission',
            width: 400,
            height: 300
        };
        var piechart = new google.visualization.PieChart(document
            .getElementById('piechart_div'));
        piechart.draw(data, piechart_options);

        var barchart_options = {
            title: 'Barchart: taux de marge de la mission',
            width: 400,
            height: 300,
            legend: 'none'
        };
        var barchart = new google.visualization.BarChart(document
            .getElementById('barchart_div'));
        barchart.draw(data, barchart_options);
    }

});