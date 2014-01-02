var cnData = new Array();
var mapObject;

var barData = {
    labels: ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10"],
    datasets: [
		{
		    fillColor: "rgba(151,187,205,0.5)",
		    strokeColor: "rgba(151,187,205,1)",
            data: []
		    //data: [65, 59, 90, 81, 56, 55, 40]
		},
		{
		    fillColor: "rgba(151,187,205,0.5)",
		    strokeColor: "rgba(151,187,205,1)",
		    data: [28, 48, 40, 19, 96, 27, 100]
		}
    ]
}

var lineData = {
    labels: ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10"],
    datasets: [
		{
		    fillColor: "rgba(151,187,205,0.5)",
		    strokeColor: "rgba(151,187,205,1)",
		    pointColor: "rgba(220,220,220,1)",
		    pointStrokeColor: "#fff",
            data: []
		    //data: [65, 59, 90, 81, 56, 55, 40]
		}
		//{
		//    fillColor: "rgba(151,187,205,0.5)",
		//    strokeColor: "rgba(151,187,205,1)",
		//    pointColor: "rgba(151,187,205,1)",
		//    pointStrokeColor: "#fff",
		//    data: [28, 48, 40, 19, 96, 27, 100]
		//}
    ]
}

var lineOptions = {
				
    //Boolean - If we show the scale above the chart data			
    scaleOverlay : false,
	
    //Boolean - If we want to override with a hard coded scale
    scaleOverride : false,
	
    //** Required if scaleOverride is true **
    //Number - The number of steps in a hard coded scale
    scaleSteps : null,
    //Number - The value jump in the hard coded scale
    scaleStepWidth : null,
    //Number - The scale starting value
    scaleStartValue : null,

    //String - Colour of the scale line	
    scaleLineColor : "rgba(0,0,0,.1)",
	
    //Number - Pixel width of the scale line	
    scaleLineWidth : 1,

    //Boolean - Whether to show labels on the scale	
    scaleShowLabels : true,
	
    //Interpolated JS string - can access value
    scaleLabel : "<%=value%>",
	
    //String - Scale label font declaration for the scale label
    scaleFontFamily : "'Arial'",
	
    //Number - Scale label font size in pixels	
    scaleFontSize : 12,
	
    //String - Scale label font weight style	
    scaleFontStyle : "normal",
	
    //String - Scale label font colour	
    scaleFontColor : "#666",	
	
    ///Boolean - Whether grid lines are shown across the chart
    scaleShowGridLines : true,
	
    //String - Colour of the grid lines
    scaleGridLineColor : "rgba(0,0,0,.05)",
	
    //Number - Width of the grid lines
    scaleGridLineWidth : 1,	
	
    //Boolean - Whether the line is curved between points
    bezierCurve : true,
	
    //Boolean - Whether to show a dot for each point
    pointDot : true,
	
    //Number - Radius of each point dot in pixels
    pointDotRadius : 3,
	
    //Number - Pixel width of point dot stroke
    pointDotStrokeWidth : 1,
	
    //Boolean - Whether to show a stroke for datasets
    datasetStroke : true,
	
    //Number - Pixel width of dataset stroke
    datasetStrokeWidth : 2,
	
    //Boolean - Whether to fill the dataset with a colour
    datasetFill : true,
	
    //Boolean - Whether to animate the chart
    animation : true,

    //Number - Number of animation steps
    animationSteps : 60,
	
    //String - Animation easing effect
    animationEasing : "easeOutQuart",

    //Function - Fires when the animation is complete
    onAnimationComplete : null
	
}

var barOptions = {

    //Boolean - If we show the scale above the chart data			
    scaleOverlay: false,

    //Boolean - If we want to override with a hard coded scale
    scaleOverride: false,

    //** Required if scaleOverride is true **
    //Number - The number of steps in a hard coded scale
    scaleSteps: null,
    //Number - The value jump in the hard coded scale
    scaleStepWidth: null,
    //Number - The scale starting value
    scaleStartValue: null,

    //String - Colour of the scale line	
    scaleLineColor: "rgba(0,0,0,.1)",

    //Number - Pixel width of the scale line	
    scaleLineWidth: 1,

    //Boolean - Whether to show labels on the scale	
    scaleShowLabels: true,

    //Interpolated JS string - can access value
    scaleLabel: "<%=value%>",

    //String - Scale label font declaration for the scale label
    scaleFontFamily: "'Arial'",

    //Number - Scale label font size in pixels	
    scaleFontSize: 12,

    //String - Scale label font weight style	
    scaleFontStyle: "normal",

    //String - Scale label font colour	
    scaleFontColor: "#666",

    ///Boolean - Whether grid lines are shown across the chart
    scaleShowGridLines: true,

    //String - Colour of the grid lines
    scaleGridLineColor: "rgba(0,0,0,.05)",

    //Number - Width of the grid lines
    scaleGridLineWidth: 1,

    //Boolean - If there is a stroke on each bar	
    barShowStroke: true,

    //Number - Pixel width of the bar stroke	
    barStrokeWidth: 2,

    //Number - Spacing between each of the X value sets
    barValueSpacing: 5,

    //Number - Spacing between data sets within X values
    barDatasetSpacing: 1,

    //Boolean - Whether to animate the chart
    animation: true,

    //Number - Number of animation steps
    animationSteps: 60,

    //String - Animation easing effect
    animationEasing: "easeOutQuart",

    //Function - Fires when the animation is complete
    onAnimationComplete: null
}

    function LoadAndRefreshDMChart() {
        $.ajax({
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            url: 'HackDayService.svc/GetDQData',
            data: {
                date: "2013-11-12",
                page: "MailboxUsage"
            },
            success: function (result) {
                result = result.d;
                DrawDMDivAndChart(result);
            }
        });
    }

    function DrawDMDivAndChart(result) {
        barData.labels = [];
        barData.datasets[0].data = [];
        barData.datasets[1].data = [];
        var maiboxUsageCompletness = 0;
        var connectionByClientTypeCompletness = 0;

        for (var i = 0; i < result.length; i++) {
            var DMItem = {
                DateHour: result[i].DateHour,
                Page: result[i].Page,
                Completeness: result[i].Completeness
            };
            if (DMItem.Page == "MailboxUsage") {
                barData.labels.push(DMItem.DateHour);
                barData.datasets[0].data.push(DMItem.Completeness);
                maiboxUsageCompletness += DMItem.Completeness;
            }
            else if (DMItem.Page == "ConnectionByClientTypeDaily") {
                //barData.labels.push(DMItem.DateHour);
                barData.datasets[1].data.push(DMItem.Completeness);
                connectionByClientTypeCompletness += DMItem.Completeness;
            }
        }

        if (barData.datasets[0].data.length != 0) {
            maiboxUsageCompletness = maiboxUsageCompletness / barData.datasets[0].data.length * 100;
        }

        if (barData.datasets[1].data.length != 0) {
            connectionByClientTypeCompletness = connectionByClientTypeCompletness / barData.datasets[1].data.length * 100;
        }

        var mbxUsageLabel = $("#MbxUsageDMLabel");
        var connectionByClientTypeDMLabel = $("#ConnectionByClientTypeDMLabel");

        mbxUsageLabel.html(maiboxUsageCompletness.toFixed(2));
        connectionByClientTypeDMLabel.html(connectionByClientTypeCompletness.toFixed(2));
        //Get context with jQuery - using jQuery's .get() method.
        var ctx = $("#barChart").get(0).getContext("2d");
        //This will get the first returned node in the jQuery collection.
        var myNewChart = new Chart(ctx);
        myNewChart.Bar(barData, barOptions);
    }

    function LoadAndRefreshAvailabilityChart() {
        $.ajax({
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            url: 'HackDayService.svc/GetTagIDData',
            data: {
                date: "2013-11-12"
            },
            success: function (result) {
                result = result.d;
                DrawAvailabilityDivAndChart(result);
            }
        });
    }

    function DrawAvailabilityDivAndChart(result) {
        var dicTotalTagID = new Array();
        var dicAvailableTagID = new Array();
        lineData.labels = [];
        lineData.datasets[0].data = [];
        var totalTagIDCountDaily = 0;
        var totalSuccessTagIDCountDaily = 0;
        var totalTag8553 = 0;
        var totalTag8661 = 0;
        var totalTag7261 = 0

        for (var i = 0; i < result.length; i++) {
            var tagidItem = {
                DateHour: result[i].DateHour,
                TagID: result[i].TagID,
                Count: result[i].Count
            };
            if (tagidItem.TagID == "8555") {
                dicAvailableTagID[tagidItem.DateHour] = tagidItem.Count;
                totalSuccessTagIDCountDaily += tagidItem.Count;
            }
            else if (tagidItem.TagID == "8819") {
                dicTotalTagID[tagidItem.DateHour] = tagidItem.Count;
                totalTagIDCountDaily += tagidItem.Count;
            }
            else if (tagidItem.TagID == "8553")
            {
                totalTag8553 += tagidItem.Count;
            }
            else if (tagidItem.TagID == "8661") {
                totalTag8661 += tagidItem.Count;
            }
            else if (tagidItem.TagID == "7261") {
                totalTag7261 += tagidItem.Count;
            }
        }

        for (var key in dicTotalTagID) {
            lineData.labels.push(key);
            lineData.datasets[0].data.push(dicAvailableTagID[key] / dicTotalTagID[key] * 100);
        }

        var availabilityDaily;

        if (totalTagIDCountDaily != 0) {
            availabilityDaily = totalSuccessTagIDCountDaily / totalTagIDCountDaily * 100;
        }
        else {
            availabilityDaily = 100;
        }
        var availablityLabel1 = $("#AvailabilityLabel1");
        var exceptionALabel = $("#AvailabilityLabel2");
        var exceptionALabe2 = $("#AvailabilityLabel3");
        var exceptionALabe3 = $("#AvailabilityLabel4");

        availablityLabel1.html(availabilityDaily.toFixed(2));
        exceptionALabel.html(totalTag8553);
        exceptionALabe2.html(totalTag8661);
        exceptionALabe3.html(totalTag7261);

        //Get context with jQuery - using jQuery's .get() method.
        var ctx = $("#tagIDChart").get(0).getContext("2d");
        //This will get the first returned node in the jQuery collection.
        var myNewChart = new Chart(ctx);
        myNewChart.Line(lineData, lineOptions);
    }

    function LoadAndRefreshPagesDiv() {
        $.ajax({
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            url: 'HackDayService.svc/GetPageData',
            data: {
                date: "2013-11-12"
            },
            success: function (result) {
                result = result.d;
                DrawPageDiv(result);
            }
        });
    }

    function DrawPageDiv(result) {
        var totalMbxPageCount = 0;
        var totalConnectionByClientTypeDailyCount = 0;

        for (var i = 0; i < result.length; i++) {
            var pageItem = {
                DateHour: result[i].DateHour,
                Page: result[i].Page,
                Count: result[i].Count
            };
            if (pageItem.Page == "MailboxUsage") {
                totalMbxPageCount += pageItem.Count;
            }
            else if (pageItem.Page == "ConnectionByClientType") {
                totalConnectionByClientTypeDailyCount += pageItem.Count;
            }
        }

        var mbxUsageLabel = $("#MbxUsageUsageLabel");
        var connectionByClientTypeDMLabel = $("#ConnectionByClientTypeUsageLabel");

        mbxUsageLabel.html(totalMbxPageCount);
        connectionByClientTypeDMLabel.html(totalConnectionByClientTypeDailyCount);
    }

    function LoadAndRefreshTenants() {
        $.ajax({
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            url: 'HackDayService.svc/GetTenantsData',
            data: {
                date: "2013-11-12",
            },
            success: function (result) {
                result = result.d;
                DrawTenantsMsg(result);
            }
        });
    }

    function DrawTenantsMsg(result)
    {
        var tenants = "";
        for (var i = 0; i < result.length; i++) {
            tenants += result[i] + "\n";
        }
        var tenantsArea = $("#tenantsArea");
        var msg = tenantsArea.html();
        //if (msg != "") {
        //    msg += tenants;
        //}
        //else
        //{
        //    msg = tenants;
        //}
        msg = tenants;
        tenantsArea.html(msg);
    }
    
    function LoadAndRefreshMap() {
        $.ajax({
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            url: 'HackDayService.svc/GetCountryData',
            data: {
                date: "2013-11-12"
            },
            success: function (result) {
                result = result.d;
                DrawMap(result);
            }
        });
    }

    function DrawMap(result)
    {
        for (var i = 0; i < result.length; i++) {
            cnData[result[i].Country] = result[i].Count;
        }
        mapObject.series.regions[0].setValues(cnData);
    }