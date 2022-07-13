document.addEventListener("DOMContentLoaded", function () {


    var legendState = true;
    if (window.outerWidth < 576) {
        legendState = false;
    }
    var speed = 250;
    // Creation of Data to be used in the chart
    var TempData = [];
    var DateTempData = [];
    var HumidityData = [];
    var DateHumidityData = [];
    var LightData = [];
    var DateLightData = [];

    //TempData.length = 100;
    //DateTempData.length = 100;
    //TempData.fill(0);
    //DateTempData.fill(0);
    var ChartsList = [];
    var TheTempchart = new Chart(document.getElementById("TempChart"), {
        type: "line",
        options: {
            
            plugins: {
                title: {
                    display: true,
                    text: 'Real Time Temperature Chart',
                    fontColor: 'white',
                }
            },
            scales: {
                xAxes: [
                    {
                        display: true,
                        gridLines: {
                            display: false,
                        },
                    },
                ],
                yAxes: [
                    {
                        ticks: {
                            max: 60,
                            min: 10,
                        },
                        display: true,
                        gridLines: {
                            display: false,
                        },
                    },
                ],
            },
            legend: {
                display: legendState,
            },
            responsive: true,
            animation: {
                duration: speed * 1.5,
                easing: 'linear'
            },
        },
        data: {
            labels: DateTempData,
            datasets: [
                {
                    label: "Temperature",
                    fill: true,
                    lineTension: 0.2,
                    backgroundColor: "transparent",
                    borderColor: "#864DD9",
                    pointBorderColor: "#864DD9",
                    pointHoverBackgroundColor: "#864DD9",
                    borderCapStyle: "butt",
                    borderDash: [],
                    borderDashOffset: 0.0,
                    borderJoinStyle: "miter",
                    borderWidth: 2,
                    pointBackgroundColor: "#fff",
                    pointBorderWidth: 5,
                    pointHoverRadius: 5,
                    pointHoverBorderColor: "#fff",
                    pointHoverBorderWidth: 2,
                    pointRadius: 1,
                    pointHitRadius: 0,
                    data: TempData,
                    spanGaps: false,
                }
            ]
        }
    })

    var TheHumiditychart = new Chart(document.getElementById("HumidityChart"), {
        type: "line",
        options: {

            plugins: {
                title: {
                    display: true,
                    text: 'Real Time Humidity Chart',
                    fontColor: 'white',
                }
            },
            scales: {
                xAxes: [
                    {
                        display: true,
                        gridLines: {
                            display: false,
                        },
                    },
                ],
                yAxes: [
                    {
                        ticks: {
                            max: 60,
                            min: 10,
                        },
                        display: true,
                        gridLines: {
                            display: false,
                        },
                    },
                ],
            },
            legend: {
                display: legendState,
            },
            responsive: true,
            animation: {
                duration: speed * 1.5,
                easing: 'linear'
            },
        },
        data: {
            labels: DateHumidityData,
            datasets: [
                {
                    label: "Humidity",
                    fill: true,
                    lineTension: 0.2,
                    backgroundColor: "transparent",
                    borderColor: "#57DECE",
                    pointBorderColor: "#57DECE",
                    pointHoverBackgroundColor: "#57DECE",
                    borderCapStyle: "butt",
                    borderDash: [],
                    borderDashOffset: 0.0,
                    borderJoinStyle: "miter",
                    borderWidth: 2,
                    pointBackgroundColor: "#fff",
                    pointBorderWidth: 5,
                    pointHoverRadius: 5,
                    pointHoverBorderColor: "#fff",
                    pointHoverBorderWidth: 2,
                    pointRadius: 1,
                    pointHitRadius: 0,
                    data: HumidityData,
                    spanGaps: false,
                }
            ]
        }
    })
    
    var TheLightchart = new Chart(document.getElementById("LightChart"), {
        type: "line",
        options: {

            plugins: {
                title: {
                    display: true,
                    text: 'Real Time Light Chart',
                    fontColor: 'white',
                }
            },
            scales: {
                xAxes: [
                    {
                        display: true,
                        gridLines: {
                            display: false,
                        },
                    },
                ],
                yAxes: [
                    {
                        ticks: {
                            max: 60,
                            min: 10,
                        },
                        display: true,
                        gridLines: {
                            display: false,
                        },
                    },
                ],
            },
            legend: {
                display: legendState,
            },
            responsive: true,
            animation: {
                duration: speed * 1.5,
                easing: 'linear'
            },
        },
        data: {
            labels: DateLightData,
            datasets: [
                {
                    label: "Light",
                    fill: true,
                    lineTension: 0.2,
                    backgroundColor: "transparent",
                    borderColor: "#FE9360",
                    pointBorderColor: "#FE9360",
                    pointHoverBackgroundColor: "#FE9360",
                    borderCapStyle: "butt",
                    borderDash: [],
                    borderDashOffset: 0.0,
                    borderJoinStyle: "miter",
                    borderWidth: 2,
                    pointBackgroundColor: "#fff",
                    pointBorderWidth: 5,
                    pointHoverRadius: 5,
                    pointHoverBorderColor: "#fff",
                    pointHoverBorderWidth: 2,
                    pointRadius: 1,
                    pointHitRadius: 0,
                    data: LightData,
                    spanGaps: false,
                }
            ]
        }
    })
    
     

    // Setting up signalR connection
    var Connection = new signalR.HubConnectionBuilder().withUrl("/sensor").build();
    
    Connection.on('Broadcast',
        function (sender, message) {
            var TheData = JSON.parse(message);
            console.log(TheData["temperature"])
            console.log(TheData["DateTime"])
            DateTempData.push(TheData["DateTime"])
            TempData.push(TheData["temperature"])
            DateHumidityData.push(TheData["DateTime"])
            HumidityData.push(TheData["humidity"])
            LightData.push(TheData["light"])
            DateLightData.push(TheData["DateTime"])
            console.log(LightData);
            TheTempchart.update();
            TheHumiditychart.update();
            TheLightchart.update();

            // This if statement is used to remove previous data points. Currently chart can only hold 7 data points due to the if statement.
            if (TempData.length > 7 && DateTempData.length > 7) {
                TempData.shift();
                DateTempData.shift();
                HumidityData.shift();
                DateHumidityData.shift();
                LightData.shift()
                DateLightData.shift()
                console.log("Removing in progress")
            }

            //if (HumidityData.length > 7 && DateHumidityData > 7) {

            //}
           
        });

    Connection.start();



})


