/**
 * Constructor for the TileChart
 */
function TileChart(){

    var self = this;
    self.init();
};

/**
 * Initializes the svg elements required to lay the tiles
 * and to populate the legend.
 */
TileChart.prototype.init = function(){
    var self = this;

    //Gets access to the div element created for this chart and legend element from HTML
    var divTileChart = d3.select("#tiles").classed("content", true);
    var legend = d3.select("#legend").classed("content",true);
    self.margin = {top: 30, right: 20, bottom: 30, left: 50};

    var svgBounds = divTileChart.node().getBoundingClientRect();
    self.svgWidth = svgBounds.width - self.margin.left - self.margin.right;
    self.svgHeight = self.svgWidth/2;
    var legendHeight = 150;

    //creates svg elements within the div
    self.legendSvg = legend.append("svg")
        .attr("width",self.svgWidth)
        .attr("height",legendHeight)
        .attr("transform", "translate(" + self.margin.left + ",0)")

    self.svg = divTileChart.append("svg")
        .attr("width",self.svgWidth)
        .attr("height",self.svgHeight)
        .attr("transform", "translate(" + self.margin.left + ",0)")
        .style("bgcolor","green")

};

/**
 * Returns the class that needs to be assigned to an element.
 *
 * @param party an ID for the party that is being referred to.
 */
TileChart.prototype.chooseClass = function (party) {
    var self = this;
    if (party == "R"){
        return "republican";
    }
    else if (party== "D"){
        return "democrat";
    }
    else if (party == "I"){
        return "independent";
    }
}

/**
 * Renders the HTML content for tool tip.
 *
 * @param tooltip_data information that needs to be populated in the tool tip
 * @return text HTML content for tool tip
 */
TileChart.prototype.tooltip_render = function (tooltip_data) {
    var self = this;
    var text = "<h2 class ="  + self.chooseClass(tooltip_data.winner) + " >" + tooltip_data.state + "</h2>";
    text +=  "Electoral Votes: " + tooltip_data.electoralVotes;
    text += "<ul>"
    tooltip_data.result.forEach(function(row){
        text += "<li class = " + self.chooseClass(row.party)+ ">" + row.nominee+":\t\t"+row.votecount+"("+row.percentage+"%)" + "</li>"
    });
    text += "</ul>";
    return text;
}


/**
 * Creates tiles and tool tip for each state, legend for encoding the color scale information.
 *
 * @param electionResult election data for the year selected
 * @param colorScale global quantile scale based on the winning margin between republicans and democrats
 */
TileChart.prototype.update = function(electionResult, colorScale){
    var self = this;
    // ******* TODO: PART IV *******
    //Tansform the legend element to appear in the center and make a call to this element for it to display.

    //Lay rectangles corresponding to each state according to the 'row' and 'column' information in the data.

    //Display the state abbreviation and number of electoral votes on each of these rectangles

    //Use global color scale to color code the tiles.

    //HINT: Use .tile class to style your tiles;
    // .tilestext to style the text corresponding to tiles

    //Call the tool tip on hover over the tiles to display stateName, count of electoral votes
    //then, vote percentage and number of votes won by each party.
    //HINT: Use the .republican, .democrat and .independent classes to style your elements.

    d3.csv("data/Year_Timeline_"+electionResult.YEAR+".csv", function (error, election) {
//Calculates the maximum number of columns to be laid out on the svg
        self.maxColumns = d3.max(election,function(d){
            return parseInt(d["Space"]);
        });
        self.maxColumns++;
        self.election = election;

        //Calculates the maximum number of rows to be laid out on the svg
        self.maxRows = d3.max(election,function(d){
            return parseInt(d["Row"]);
        });
        self.maxRows++;
        //for reference:https://github.com/Caged/d3-tip
        //Use this tool tip element to handle any hover over the chart
        tip = d3.tip().attr('class', 'd3-tip')
            .direction('se')
            .offset(function() {
                return [0,0];
            })
            .html(function(d) {
                 tooltip_data = {
                     "state": d.State,
                     "winner": d.State_Winner,
                     "electoralVotes" : d.Total_EV,
                     "result":[
                     {"nominee": d.D_Nominee_prop,"votecount": d.D_Votes,"percentage": d.D_Percentage,"party":"D"} ,
                     {"nominee": d.R_Nominee_prop,"votecount": d.R_Votes,"percentage": d.R_Percentage,"party":"R"} ,
                     {"nominee": d.I_Nominee_prop,"votecount": d.I_Votes,"percentage": d.I_Percentage,"party":"I"}
                     ]
                 };


                return self.tooltip_render(tooltip_data);
            });

        self.svg.call(tip);

        //Creates a legend element and assigns a scale that needs to be visualized
        var legendQuantile = d3.legendColor()
            .shapeWidth(90)
            .cells(10)
            .orient('horizontal')
            .scale(colorScale);


        self.legendSvg.append("g")
            .attr("class", "legendQuantile")
            .call(legendQuantile);

        self.width = self.svgWidth/self.maxColumns;
        self.height = self.svgHeight/self.maxRows;


        self.svg.selectAll("g").remove();


        self.svg.append("g")
            .selectAll("rect")
            .data(election)
            .enter()
            .append("rect")
            .attr("width",self.width)
            .attr("height", self.height)
            .attr("x", function(d, i){
                return parseInt(d["Space"]) * self.width;
            })
            .attr("y", function(d,i){
                return parseInt(d["Row"]) * self.height;
            })
            .attr("fill",function(d){
                return colorScale( parseFloat(d.RD_Difference))
            })
            .attr("class","tile")
            .on("mouseover", tip.show)
            .on("mouseout", tip.hide);


        self.svg.append("g")
            .selectAll("text")
            .data(election)
            .enter()
            .append("text")
            .text(function(d){
                return d.Abbreviation;
            })
            .attr("z-index",1)
            .attr("x",function(d){
                return parseInt(d["Space"]) * self.width +self.width/2;
            })
            .attr("y", function(d){
                return parseInt(d["Row"]) * self.height + self.height/2;
            })
            .attr("dy", "-5")
            .attr("text-anchor","middle")
            .attr("class","tiletext")
            .on("mouseover", tip.show)

        self.svg.append("g")
            .selectAll("text")
            .data(election)
            .enter()
            .append("text")
            .text(function(d){
                return d.Total_EV;
            })
            .attr("z-index",1)
            .attr("x",function(d){
                return parseInt(d["Space"]) * self.width +self.width/2;
            })
            .attr("y", function(d){
                return parseInt(d["Row"]) * self.height + self.height/2;
            })
            .attr("dy", "15")
            .attr("text-anchor","middle")
            .attr("class","tiletext")
    });


};
