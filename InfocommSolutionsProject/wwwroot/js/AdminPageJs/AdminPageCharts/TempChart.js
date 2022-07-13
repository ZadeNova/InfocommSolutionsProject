document.addEventListener("DOMContentLoaded", function () {


    var legendState = true;
    if (window.outerWidth < 576) {
        legendState = false;
    }
    var speed = 250;
    var TempData = [];
    var DateTempData = [];
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


    

    
     

    // Setting up signalR connection
    var Connection = new signalR.HubConnectionBuilder().withUrl("/sensor").build();
    
    Connection.on('Broadcast',
        function (sender, message) {
            var TheData = JSON.parse(message);
            console.log(TheData["temperature"])
            console.log(TheData["DateTime"])
            DateTempData.push(TheData["DateTime"])
            TempData.push(TheData["temperature"])
            //TempData.shift();
            //DateTempData.shift();
            TheTempchart.update();
            console.log(TempData.length)
            console.log(DateTempData.length)
            if (TempData.length > 15 && DateTempData.length > 15) {
                TempData.shift();
                DateTempData.shift();
                console.log("Removing in progress")
            }
           
        });

    Connection.start();



})


