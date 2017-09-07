/**
 * Constructor for the Vote Percentage Chart
 */
function VotePercentageChart(){

    var self = this;
    self.init();
};

/**
 * Initializes the svg elements required for this chart
 */
VotePercentageChart.prototype.init = function(){
    var self = this;
    self.margin = {top: 30, right: 20, bottom: 30, left: 50};
    var divvotesPercentage = d3.select("#votes-percentage").classed("content", true);

    //Gets access to the div element created for this chart from HTML
    self.svgBounds = divvotesPercentage.node().getBoundingClientRect();
    self.svgWidth = self.svgBounds.width - self.margin.left - self.margin.right;
    self.svgHeight = 200;

    //creates svg element within the div
    self.svg = divvotesPercentage.append("svg")
        .attr("width",self.svgWidth)
        .attr("height",self.svgHeight)
};

/**
 * Returns the class that needs to be assigned to an element.
 *
 * @param party an ID for the party that is being referred to.
 */
VotePercentageChart.prototype.chooseClass = function (party) {
    var self = this;
    if (party == "R"){
        return "republican";
    }
    else if (party == "D"){
        return "democrat";
    }
    else if (party == "I"){
        return "independent";
    }
}

/**
 * Renders the HTML content for tool tip
 *
 * @param tooltip_data information that needs to be populated in the tool tip
 * @return text HTML content for toop tip
 */
VotePercentageChart.prototype.tooltip_render = function (tooltip_data) {
    var self = this;
    var text = "<ul>";
    tooltip_data.result.forEach(function(row){
        text += "<li class = " + self.chooseClass(row.party)+ ">" + row.nominee+":\t\t"+row.votecount+"("+row.percentage+"%)" + "</li>"
    });

    return text;
}

/**
 * Creates the stacked bar chart, text content and tool tips for Vote Percentage chart
 *
 * @param electionResult election data for the year selected
 */
VotePercentageChart.prototype.update = function(electionResult){
    var self = this;

    //for reference:https://github.com/Caged/d3-tip
    //Use this tool tip element to handle any hover over the chart
    tip = d3.tip().attr('class', 'd3-tip')
        .direction('s')
        .offset(function(d,i) {
            if(i==0)
                return [0,125];
            return [0,0];
        })
        .html(function(d) {
            /* populate data in the following format
             * tooltip_data = {
             * "result":[
             * {"nominee": D_Nominee_prop,"votecount": D_Votes_Total,"percentage": D_PopularPercentage,"party":"D"} ,
             * {"nominee": R_Nominee_prop,"votecount": R_Votes_Total,"percentage": R_PopularPercentage,"party":"R"} ,
             * {"nominee": I_Nominee_prop,"votecount": I_Votes_Total,"percentage": I_PopularPercentage,"party":"I"}
             * ]
             * }
             * pass this as an argument to the tooltip_render function then,
             * return the HTML content returned from that method.
             * */
            d = self.election[0];

            var  tooltip_data = {
                 "result":[
                 {"nominee": d.D_Nominee_prop,"votecount": d.D_Votes_Total,"percentage": d.D_PopularPercentage,"party":"D"} ,
                 {"nominee": d.R_Nominee_prop,"votecount": d.R_Votes_Total,"percentage": d.R_PopularPercentage,"party":"R"} ,
                 {"nominee": d.I_Nominee_prop,"votecount": d.I_Votes_Total,"percentage": d.I_PopularPercentage,"party":"I"}
                 ]
             }

            return self.tooltip_render(tooltip_data);
        });

    //Invoke tip for the svg we are calling it on
    self.svg.call(tip);


    // ******* TODO: PART III *******
    //Create the stacked bar chart.
    //Use the global color scale to color code the rectangles.
    //HINT: Use .votesPercentage class to style your bars.
    d3.csv("data/Year_Timeline_"+electionResult.YEAR+".csv", function (error, election) {
        self.election = election;
        var I = election[0].I_PopularPercentage.replace("%", "");
        if(I == "")
            I = 0;
        I = parseFloat(I);

        var D = election[0].D_PopularPercentage.replace("%", "");
        D = parseFloat(D);
        var R = election[0].R_PopularPercentage.replace("%", "");
        R = parseFloat(R);
        var votePercentages = [
            {PopularPercentage:I},
            {PopularPercentage:D},
            {PopularPercentage:R}];

        self.xScale = d3.scaleLinear()
            .domain([0,D+R+I])
            .range([0,self.svgWidth ])

        var stack = d3.stack()
            .keys(["PopularPercentage"]);

        var data = stack(votePercentages);
        self.data = data[0];

        self.svg.selectAll("g").remove();

        //Set up container for bar stack
        var rects = self.svg.selectAll("g")
            .data(data)
            .enter()
            .append("g")
            .selectAll("rect")
            .data(function(d){
                return d;
            });

        //Update bar stack
        rects.enter()
            .append("rect")
            .attr("x", function(d, i) {
                var currentPos = 0;

                for(var ii = 0; ii< i; ii++){
                    currentPos+= self.data[ii][1];
                }
                return  self.xScale(currentPos);
            })
            .attr("y", function(d) { return 0; })
            .attr("height", function(d) { return 50; })
            .attr("width", function(d,i){
                return self.xScale(d[1]);;
            })
            .attr("fill",function(d,i){
                if(i == 0)
                return "green";//colorScale(d.data.RD_Difference);
                if(i==1)
                    return "blue";
                return "red";
            })
            .attr("class",function(d,i){
                if(i == 0)
                    return "votePercentage "+ self.chooseClass("I");
                if(i==1)
                    return "votePercentage "+self.chooseClass("D");
                return "votePercentage "+self.chooseClass("R");
            })
            .on("mouseover", tip.show)
            .on("mouseout", tip.hide)

        //Add Value labels
        self.svg.selectAll("g")
            .selectAll("text")
            .data([I,D,R,data])
            .enter()
            .append("text")
            .text(function(d,i){
                try {
                    if (i == 0)return d==0?"":d+"%";
                }
                catch(err){return ""}
                if(i==1 || i ==2) return d+"%";

                return "Popular Vote(50%)";
            })
            .attr("x", function(d, i){
                if(i==0) return 0;
                if(i==1) {
                    var buffer = 0;
                    if(I < 5 && I != 0)
                        buffer = 6 - I;
                    return self.xScale(I + buffer)

                };
                if(i==2)return self.xScale(97) ;
                return self.xScale(50)
            })
            .attr("fill", function(d,i) {
                if (i == 0)
                    return "green";
                if(i==1) return "blue";
                if(i==2) return "red";
                return "black"
            })
            .attr("dy","-5")
            .attr("class", function(d,i) {
                if(i==0)
                    return  self.chooseClass("I");
                if(i==1)
                    return  self.chooseClass("D");
                if(i==2)
                    return  self.chooseClass("R");

            })
            .attr("text-anchor", function(d,i){
                if(i==0 || i ==1)
                    return  "start";
                if(i==2)
                    return  "end"
                return "middle"
            });


        self.svg.append("g")
            .selectAll("text")
            .data([1,2,3])
            .enter()
            .append("text")
            .text(function(d,i){
                try {
                    if (i == 0)return election[0].I_Nominee;
                }
                catch(err){return ""}
                if(i==1)
                    return election[0].D_Nominee;
                if(i==2)
                    return election[0].R_Nominee;
            })
            .attr("x", function(d, i){
                if(i==0) return 0;
                if(i==1) {
                    return self.xScale((I +D)/2)
                };
                if(i==2)return self.xScale((I+D)+R/2) ;
                return self.xScale(50)
            })
            .attr("fill", function(d,i) {
                if (i == 0)
                    return "green";
                if(i==1) return "blue";
                if(i==2) return "red";
                return "black"
            })
            .attr("dy","-20")
            .attr("class", function(d,i) {
                if(i==0)
                    return  self.chooseClass("I");
                if(i==1)
                    return  self.chooseClass("D");
                if(i==2)
                    return  self.chooseClass("R");

            })
            .attr("text-anchor", "middle");


        self.svg.selectAll("g")
            .attr("transform", "translate(0,40)")

        //Add 50 % mark
        self.svg.append("g")
            .selectAll("rect")
            .data([1])
            .enter()
            .append("rect")
            .attr("x", function(d){
                return self.xScale((I+R+D)/2);
            })
            .attr("width", "2")
            .attr("height", "55")
            .attr("fill", "black")
            .attr("transform", "translate(0,37.5)")
            .attr("class","electoralVotes");

    });

    //Display the total percentage of votes won by each party
    //on top of the corresponding groups of bars.
    //HINT: Use the .votesPercentageText class to style your text elements;  Use this in combination with
    // chooseClass to get a color based on the party wherever necessary

    //Display a bar with minimal width in the center of the bar chart to indicate the 50% mark
    //HINT: Use .middlePoint class to style this bar.

    //Just above this, display the text mentioning details about this mark on top of this bar
    //HINT: Use .votesPercentageNote class to style this text element

    //Call the tool tip on hover over the bars to display stateName, count of electoral votes.
    //then, vote percentage and number of votes won by each party.

    //HINT: Use the chooseClass method to style your elements based on party wherever necessary.

};
