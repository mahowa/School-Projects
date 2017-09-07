<?php
/**
 * Created by PhpStorm.
 * User: Matt Howa
 * Date: 1/28/2016
 * Time: 6:03 PM
 */

echo $partial->head("DGS");


echo $dept->html;
?>


    <!--<script src = 'Projects/Grad_Progress/JS/highcharts.js' type = 'text/javascript' ></script >-->
    <!--<script src = 'Projects/Grad_Progress/JS/exporting.js' type = 'text/javascript' ></script >-->
    <script src="https://code.highcharts.com/highcharts.js"></script>
    <script src="https://code.highcharts.com/modules/exporting.js"></script>


    <!-- Modal -->
    <div id="myModal" class="modal fade" role="dialog" >
        <div class="modal-dialog modal-lg">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Summary</h4>
                </div>
                <div class="modal-body" id="quick_view">

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>

    <div class="dropdown"  style="min-width: 310px; max-width: 800px; margin: 0 auto">
        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
            DGS Visual Data <span class="caret"></span></button>
        <ul class="dropdown-menu">
            <li><a href="javascript:void(null)" onclick="gpaChart()">Student GPA's</a></li>
            <li><a href="javascript:void(null)" onclick="signedChart()">Signed Forms</a></li>
            <li><a href="javascript:void(null)" onclick="advisedChart()">Advised By</a></li>
        </ul>
    </div>
    <div id="container" style="min-width: 310px; height: 400px; max-width: 800px; margin: 0 auto"></div>
    <script type="application/javascript">

        $(document).ready(gpaChart);
        var chart;

        var url_string = "../../Api/";

        function gpaChart() {

            chart = new Highcharts.Chart({
                chart: {
                    renderTo: 'container',
                    type: 'scatter',
                    zoomType: 'xy'
                },
                title: {
                    text: 'GPA By Year'
                },
                subtitle: {
                    text: 'Source: Local DB'
                },
                xAxis: {
                    title: {
                        enabled: true,
                        text: 'Year'
                    },
                    startOnTick: true,
                    endOnTick: true,
                    showLastLabel: true
                },
                yAxis: {
                    title: {
                        text: 'GPA',
                        max: "4"
                    }
                },
                legend: {
                    layout: 'vertical',
                    align: 'left',
                    verticalAlign: 'top',
                    x: 100,
                    y: 70,
                    floating: true,
                    backgroundColor: (Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF',
                    borderWidth: 1
                },
                plotOptions: {
                    scatter: {
                        marker: {
                            radius: 5,
                            states: {
                                hover: {
                                    enabled: true,
                                    lineColor: 'rgb(100,100,100)'
                                }
                            }
                        },
                        states: {
                            hover: {
                                marker: {
                                    enabled: false
                                }
                            }
                        },
                        tooltip: {
                            headerFormat: '<b>{series.name}</b><br>',
                            pointFormat: '{point.x} , {point.y} GPA'
                        }
                    }
                },
                series: [{
                    name: 'GPA by Year',
                    data: []
                }]
            });
            $.ajax({
                //    http://localhost/Rudeboi/FrontEnd/View/Product/Full_View.php?product_id=125
                url: url_string + "gpa.php",
                type: 'GET',
                success: function (res) {
                    // longer than 20

                    // add the point
                    chart.series[0].setData(JSON.parse(res));

                    $(".highcharts-button").hide();

                },
                error: function (obj, status, msg) {

                }
            });
            $('text').each(function () {
                var $this = $(this);
                $this.html($this.text().replace(/\bHighcharts.com\b/g, '<span style="display:none">Highcharts.com</span>'));
            });
        }

        function signedChart () {
            // Radialize the colors
            Highcharts.getOptions().colors = Highcharts.map(Highcharts.getOptions().colors, function (color) {
                return {
                    radialGradient: {
                        cx: 0.5,
                        cy: 0.3,
                        r: 0.7
                    },
                    stops: [
                        [0, color],
                        [1, Highcharts.Color(color).brighten(-0.3).get('rgb')] // darken
                    ]
                };
            });

            // Build the chart
            chart = new Highcharts.Chart({
                chart: {
                    renderTo: 'container',
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                title: {
                    text: 'Due Progress Form Status'
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>  {point.y}  total'
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.name}</b>: {point.percentage:.1f}%',
                            style: {
                                color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                            },
                            connectorColor: 'silver'
                        }
                    }
                },
                series: [{
                    name: 'Due Progress Forms',
                    data: []
                }]
            });


            $.ajax({
                url: url_string + "signed.php",
                type: 'GET',
                success: function (res) {
                    // add the point
                    chart.series[0].setData(JSON.parse(res));

                    $(".highcharts-button").hide();
                },
                error: function (obj, status, msg) {

                }
            });

            $('text').each(function () {
                var $this = $(this);
                $this.html($this.text().replace(/\bHighcharts.com\b/g, '<span style="display:none">Highcharts.com</span>'));
            });
        }

        function advisedChart() {
            chart = new Highcharts.Chart({
                chart: {
                    renderTo: 'container',
                    type: 'column'
                },
                title: {
                    text: '# of Students Advised per Advisor'
                },
                subtitle: {
                    text: 'Source: Local DB'
                },
                xAxis: {
                    type: 'category',
                    labels: {
                        rotation: -45,
                        style: {
                            fontSize: '13px',
                            fontFamily: 'Verdana, sans-serif'
                        }
                    }
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: 'Students'
                    }
                },
                legend: {
                    enabled: false
                },
                tooltip: {
                    pointFormat: 'Advised students by Advisor: <b>{point.y}</b>'
                },
                series: [{
                    name: 'Students',
                    data: []}],
                dataLabels: {
                    enabled: true,
                    rotation: -90,
                    color: '#FFFFFF',
                    align: 'right',
                    format: '{point.y}', // one decimal
                    y: 10, // 10 pixels down from the top
                    style: {
                        fontSize: '13px',
                        fontFamily: 'Verdana, sans-serif'
                    }
                }
            });

            $.ajax({
                url: url_string + "advised.php",
                type: 'GET',
                success: function (res) {
                    // add the point
                    chart.series[0].setData(JSON.parse(res));

                    $(".highcharts-button").hide();
                },
                error: function (obj, status, msg) {}
            });

            $('text').each(function () {
                var $this = $(this);
                $this.html($this.text().replace(/\bHighcharts.com\b/g, '<span style="display:none">Highcharts.com</span>'));
            });
        }

        var quickview = false;
        function quickView(id){
            quickview = true;
            $.ajax({
                url: "../Student/progress_form.php?id="+id,
                type: 'GET',
                success: function(res)
                {
                    var html = "<table>"+res+"</table>";
                    $('#quick_view').html(html);
                    quickview = false;
                },
                error: function(obj, status, msg){}
            });
        }
    </script>




    <script>
$(function(){
    $('.table tr[data-href]').each(function(){
        $(this).css('cursor','pointer').hover(
            function(){
                $(this).addClass('active');
            },
            function(){
                $(this).removeClass('active');
            }).click( function(){

            if(!quickview)
                document.location = $(this).attr('data-href');
        }
        );
    });
});
</script>

<?php

echo $partial->foot();