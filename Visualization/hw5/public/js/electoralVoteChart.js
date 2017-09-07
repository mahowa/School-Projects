
/**
 * Constructor for the ElectoralVoteChart
 *
 * @param shiftChart an instance of the ShiftChart class
 */
function ElectoralVoteChart(_shiftChart){
    var self = this;
    self.shiftChart = _shiftChart;
    self.init();
};

/**
 * Initializes the svg elements required for this chart
 */
ElectoralVoteChart.prototype.init = function(){
    var self = this;
    self.margin = {top: 30, right: 20, bottom: 30, left: 50};

    //Gets access to the div element created for this chart from HTML
    var divelectoralVotes = d3.select("#electoral-vote").classed("content", true);
    self.svgBounds = divelectoralVotes.node().getBoundingClientRect();
    self.svgWidth = self.svgBounds.width - self.margin.left - self.margin.right;
    self.svgHeight = 150;

    //creates svg element within the div
    self.svg = divelectoralVotes.append("svg")
        .attr("width",self.svgWidth)
        .attr("height",self.svgHeight)
};

/**
 * Returns the class that needs to be assigned to an element.
 *
 * @param party an ID for the party that is being referred to.
 */
ElectoralVoteChart.prototype.chooseClass = function (party) {
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
 * Creates the stacked bar chart, text content and tool tips for electoral vote chart
 *
 * @param electionResult election data for the year selected
 * @param colorScale global quantile scale based on the winning margin between republicans and democrats
 */

ElectoralVoteChart.prototype.update = function(electionResult, colorScale){
    var self = this;

    // ******* TODO: PART II *******
    d3.csv("data/Year_Timeline_"+electionResult.YEAR+".csv", function (error, election) {
        //Group the states based on the winning party for the state;
        //then sort them based on the margin of victory

        var totalEV = d3.sum(election,function(d){return d.Total_EV;});
        self.xScale = d3.scaleLinear()
            .domain([0,totalEV])
            .range([0,self.svgWidth])

        var sortFunction = function(a, b){
            return parseFloat(a.RD_Difference) - parseFloat(b.RD_Difference);
        }
        var D = election.filter(function(d){
            return parseFloat(d.RD_Difference) < 0;
        });
        D.sort(sortFunction);
        var R= election.filter(function(d){
            return parseFloat(d.RD_Difference) > 0;
        });
        R.sort(sortFunction);
        var I = election.filter(function(d){
            return d.RD_Difference == "0";
        });
        I.sort(sortFunction);

        var tempArr = [];
        for(var v = 0; v< election.length; v++){
            tempArr[v] = election[v].RD_Difference;
        }

        var sortedElection = I.concat(D);
        sortedElection = sortedElection.concat(R);

        var _totalEV = d3.sum(sortedElection,function(d){return d.Total_EV;});
        if(_totalEV != totalEV )
            debugger;

        //Create the stacked bar chart.
        //Use the global color scale to color code the rectangles.
        //HINT: Use .electoralVotes class to style your bars.
        var stack = d3.stack()
            .keys(["Total_EV"]);

        var data = stack(sortedElection);
        self.data = data[0];

        self.svg.selectAll("g").remove();

         var rects = self.svg.selectAll("g")
            .data(data)
            .enter()
            .append("g")
            .selectAll("rect")
            .data(function(d){
                return d;
            });

         rects.enter()
            .append("rect")
            .attr("x", function(d, i) {
                var currentPos = 0;

                for(var ii = 0; ii< i; ii++){
                    currentPos+= self.data[ii][1];
                }
                //TODO may need a scaler
                return self.xScale(currentPos);
            })
            .attr("y", function(d) { return 0; })
            .attr("height", function(d) { return 50; })
            .attr("width", function(d,i){
                return self.xScale(d[1]);;
            })
            .attr("fill",function(d,i){

                if(d.data.RD_Difference == "0" )
                    return "green";

                return colorScale(d.data.RD_Difference);
            })
            .attr("class","electoralVotes");
        //TODO Ask TA how this works - Get explanation
        //rects.exit()
        //    .removeALL()

        self.svg.selectAll("g")
            .selectAll("text")
            .data([I,D,R,data])
            .enter()
            .append("text")
            .text(function(d,i){
                try {
                    var I_sum = d3.sum(I, function(d){return d.Total_EV;});
                    if (i == 0)return I_sum ==0?"":I_sum;
                }
                catch(err){return ""}
                if(i==1) return d[0].D_EV_Total;
                if(i==2) return d[0].R_EV_Total;

                var toWin = d3.sum(self.data, function(d){
                   return  d[1];
                })/2 + 1;
                toWin = Math.ceil(toWin);
                return "Electoral Vote ("+toWin+" needed to win)";
            })
            .attr("x", function(d, i){
                if(i==0) return 0;
                if(i==1) {
                    var total = d3.sum(I, function (d) {
                        return d.Total_EV;
                    });


                    if(total < 5 && total !=0)
                        total+=10
                    return self.xScale(total);
                }
                if(i==2)return self.xScale(505);

                var total = d3.sum(sortedElection, function (d) {
                    return d.Total_EV;
                });
                return self.xScale(Math.ceil(total/2)+1 )
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
                if(i!=3)
                    return "electoralVoteText";

            })
            .attr("text-anchor",function(d,i){
                if(i==2)
                    return "right"
                if(i==3) return "middle";
                return "left";
            })



        self.svg.selectAll("g")
            .attr("transform", "translate(0,40)")

        self.svg.append("g")
            .selectAll("rect")
            .data([1])
            .enter()
            .append("rect")
            .attr("x", function(d){
                var sum = Math.ceil(d3.sum(sortedElection, function(d){return d.Total_EV;})/2) +1;
                var offset = d3.sum(I, function(d){return d.Total_EV;});
                return self.xScale(sum) + self.xScale(offset);
            })
            .attr("width", "2")
            .attr("height", "55")
            .attr("fill", "black")
            .attr("transform", "translate(0,37.5)")
            .attr("class","electoralVotes");

        //******* TODO: PART V *******
        //Implement brush on the bar chart created above.
        //Implement a call back method to handle the brush end event.
        //Call the update method of shiftChart and pass the data corresponding to brush selection.
        //HINT: Use the .brush class to style the brush.
        var brush = d3.brushX()
            .extent([[0,0],[self.svgWidth,50]])
            .on("brush end", function(){
                var s = d3.event.selection;

                var states = sortedElection;

                var states2 = states.filter(function(d,i){
                    var currentPos = 0;

                    for(var ii = 0; ii< i; ii++){
                        currentPos+= self.data[ii][1];
                    }
                    //TODO may need a scaler
                    if(self.xScale(currentPos)>= s[0] &&self.xScale(currentPos)<= s[1])
                        return d;

                })


                self.shiftChart.update(states2);
            });

        self.svg.append("g")
            .attr("class", "brush")
            .attr("transform", "translate(0,40)")
            .call(brush)


    });






    //Display total count of electoral votes won by the Democrat and Republican party
    //on top of the corresponding groups of bars.
    //HINT: Use the .electoralVoteText class to style your text elements;  Use this in combination with
    // chooseClass to get a color based on the party wherever necessary

    //Display a bar with minimal width in the center of the bar chart to indicate the 50% mark
    //HINT: Use .middlePoint class to style this bar.

    //Just above this, display the text mentioning the total number of electoral votes required
    // to win the elections throughout the country
    //HINT: Use .electoralVotesNote class to style this text element

    //HINT: Use the chooseClass method to style your elements based on party wherever necessary.



};
