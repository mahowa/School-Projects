/**
 * Constructor for the Year Chart
 *
 * @param electoralVoteChart instance of ElectoralVoteChart
 * @param tileChart instance of TileChart
 * @param votePercentageChart instance of Vote Percentage Chart
 * @param electionInfo instance of ElectionInfo
 * @param electionWinners data corresponding to the winning parties over mutiple election years
 */
function YearChart(electoralVoteChart, tileChart, votePercentageChart, electionWinners) {
    var self = this;

    self.electoralVoteChart = electoralVoteChart;
    self.tileChart = tileChart;
    self.votePercentageChart = votePercentageChart;
    self.electionWinners = electionWinners;
    self.init();
};

/**
 * Initializes the svg elements required for this chart
 */
YearChart.prototype.init = function(){

    var self = this;
    self.margin = {top: 10, right: 20, bottom: 30, left: 50};
    var divyearChart = d3.select("#year-chart").classed("fullView", true);

    //Gets access to the div element created for this chart from HTML
    self.svgBounds = divyearChart.node().getBoundingClientRect();
    self.svgWidth = self.svgBounds.width - self.margin.left - self.margin.right;
    self.svgHeight = 100;

    //creates svg element within the div
    self.svg = divyearChart.append("svg")
        .attr("width",self.svgWidth)
        .attr("height",self.svgHeight)

};

/**
 * Returns the class that needs to be assigned to an element.
 *
 * @param party an ID for the party that is being referred to.
 */
YearChart.prototype.chooseClass = function (party) {
    var self = this;
    if (party == "R") {
        return "yearChart republican";
    }
    else if (party == "D") {
        return "yearChart democrat";
    }
    else if (party == "I") {
        return "yearChart independent";
    }
}


/**
 * Creates a chart with circles representing each election year, populates text content and other required elements for the Year Chart
 */
YearChart.prototype.update = function(){
    var self = this;

    //Domain definition for global color scale
    var domain = [-60,-50,-40,-30,-20,-10,0,10,20,30,40,50,60 ];

    //Color range for global color scale
    var range = ["#0066CC", "#0080FF", "#3399FF", "#66B2FF", "#99ccff", "#CCE5FF", "#ffcccc", "#ff9999", "#ff6666", "#ff3333", "#FF0000", "#CC0000"];

    //Global colorScale to be used consistently by all the charts
    self.colorScale = d3.scaleQuantile()
        .domain(domain).range(range);

    // ******* TODO: PART I *******


    self.xScale = d3.scaleBand()
        .domain(self.electionWinners.map( function(d) { return d.YEAR; }))
        .range([0,self.svgWidth])
        .padding(.1);

    //Style the chart by adding a dashed line that connects all these years.
    //HINT: Use .lineChart to style this dashed line
    self.svg.selectAll("line")
        .data([1])
        .enter()
        .append("line")
        .attr("x1", 0)
        .attr("x2", self.svgWidth)
        .attr("transform", "translate(0,40)")
        .attr("class", "lineChart")


    // Create the chart by adding circle elements representing each election year
    //The circles should be colored based on the winning party for that year
    //HINT: Use the .yearChart class to style your circle elements
    //HINT: Use the chooseClass method to choose the color corresponding to the winning party.
    self.svg.selectAll("circle")
        .data(self.electionWinners)
        .enter()
        .append("circle")
        .attr("r", 20)
        .attr("cx", function(d){return self.xScale(d.YEAR); })
        .attr("cy", 0)
        .attr("transform", "translate(35,40)")
        .attr("class", function(d){return YearChart.prototype.chooseClass(d.PARTY);})
        .style("cursor", "pointer")
        //Clicking on any specific year should highlight that circle and  update the rest of the visualizations
        //HINT: Use .highlighted class to style the highlighted circle
        //Election information corresponding to that year should be loaded and passed to
        // the update methods of other visualizations
        .on("click", function(){
            d3.selectAll(".selected").classed("selected", false);
            d3.select(this).classed("selected", true);

            var data = this.__data__;

            self.electoralVoteChart.update(data, self.colorScale);
            self.votePercentageChart.update(data);
            self.tileChart.update(data,self.colorScale);



        })
        .on("mouseover", function(){
            d3.select(this).classed("highlighted", true);
        })
        .on("mouseout", function(){
            d3.select(this).classed("highlighted", false);
        })


    //Append text information of each year right below the corresponding circle
    //HINT: Use .yeartext class to style your text elements

    self.svg.selectAll("text")
        .data(self.electionWinners)
        .enter().append("text")
        .attr("x", function (d) {return self.xScale(d.YEAR)})
        .attr("y", 50)
        .attr("dx", 0)
        .attr("dy", 0)
        .attr("transform", "translate(15,40)")
        .attr("class","yearText")
        .text(function(d) {
            return d.YEAR})











    //******* TODO: EXTRA CREDIT *******

    //Implement brush on the year chart created above.
    //Implement a call back method to handle the brush end event.
    //Call the update method of shiftChart and pass the data corresponding to brush selection.
    //HINT: Use the .brush class to style the brush.
};
