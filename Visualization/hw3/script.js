// Global var for FIFA world cup data
var allWorldCupData;
var selectedRect;
var prevColor;
var prevMouseColor;

function findMax(selectedDimension){
    var max = allWorldCupData[0][selectedDimension];
    allWorldCupData.forEach(function (d) {
        if(max < d[selectedDimension])
            max = d[selectedDimension];
    });
    return max;
}



/**
 * Render and update the bar chart based on the selection of the data type in the drop-down box
 *
 * @param selectedDimension a string specifying which dimension to render in the bar chart
 */
function updateBarChart(selectedDimension) {

    var svgBounds = d3.select("#barChart").node().getBoundingClientRect(),
        xAxisWidth = 45,
        yAxisHeight = 40;

    var svg = d3.select("#barChart");

    svg.selectAll("*").remove();

    // ******* TODO: PART I *******

    //TODO Create the x and y scales; make
    // sure to leave room for the axes

    var xScale = d3.scaleBand()
        .domain(allWorldCupData.map( function(d) { return d.year; }))
        .range([svgBounds.width-xAxisWidth,0])
        .padding(.1);

    var yScale = d3.scaleLinear()
        .domain([findMax(selectedDimension),0])
        .range([0,svgBounds.height - yAxisHeight]);

    //TODO Create colorScale

    var color = d3.scaleLinear()
        .domain([0,d3.max(allWorldCupData,function(d){return d[selectedDimension]})])
        .range(["red","blue"]);



    //TODO Create the axes (hint: use #xAxis and #yAxis)

    var xAxis = d3.axisBottom()
        .ticks(21)
        .tickFormat(d3.format(".0f"));

    xAxis
        .scale(xScale);

    svg.append("g")
        .attr("transform", "translate(" + xAxisWidth + "," + (svgBounds.height - yAxisHeight) + ")")
        .call(xAxis)
        .attr("class", "xAxis")
        .selectAll("text")
        .style("text-anchor","end")
        .attr("dx","-1.2em")
        .attr("dy","-.5em")
        .attr("transform","rotate(-90)");




    var yAxis = d3.axisLeft()
        .scale(yScale);


    svg.append("g")
        .attr("transform", "translate(" + xAxisWidth + "," + 0 + ")")
        .attr("class", "Axis")
        .call(yAxis);

    // Create the bars (hint: use #bars)

    svg.selectAll("rect")
        .data(allWorldCupData)
        .enter(svg)
        .append("rect")
        .merge(svg)
        .attr("x", function(d){return xAxisWidth+xScale(d.year)})
        .attr("y", function(d){return  yScale(d[selectedDimension] )})
        //.attr("transform","scale(-1,1)")
        .attr("width",xScale.bandwidth())
        .attr("height", function(d){return svgBounds.height- yAxisHeight -yScale(d[selectedDimension])})
        .style("fill", goalColorScale);



    // ******* TODO: PART II *******

    // Implement how the bars respond to click events
    // Color the selected bar to indicate is has been selected.
    // Make sure only the selected bar has this new color.

    // Call the necessary update functions for when a user clicks on a bar.
    // Note: think about what you want to update when a different bar is selected.
        svg.selectAll("rect")
            .on("click",function(d){
                updateInfo(d);
                //TODO Debug this code!!!
                var tempRect = selectedRect;

                try {
                    tempRect.style["fill"] = prevColor;
                }
                catch(e) {}
                prevColor = prevMouseColor;
                this.style["fill"] = "orange";
                selectedRect = this;

            })
            .on("mouseover",function(d){
                prevMouseColor = this.style["fill"];
                this.style["fill"] = "red";
            })
            .on("mouseout",function(d){
                if(this.style["fill"] == "orange")
                    return;
                this.style["fill"] = prevMouseColor;
            })
        ;

}

/**
 *  Check the drop-down box for the currently selected data type and update the bar chart accordingly.
 *
 *  There are 4 attributes that can be selected:
 *  goals, matches, attendance and teams.
 */
function chooseData() {

    // ******* TODO: PART I *******
    //Changed the selected data when a user selects a different
    // menu item from the drop down.
    var val = document.getElementById("dataset").value;
    updateBarChart(val)

}

/**
 * Update the info panel to show info about the currently selected world cup
 *
 * @param oneWorldCup the currently selected world cup
 */
function updateInfo(oneWorldCup) {

    // ******* TODO: PART III *******

    // Update the text elements in the infoBox to reflect:
    // World Cup Title, host, winner, runner_up, and all participating teams that year

    // Hint: For the list of teams, you can create an list element for each team.
    // Hint: Select the appropriate ids to update the text content.
    //host,winner,silver,teams
    document.getElementById("host").innerHTML = oneWorldCup.host;
    document.getElementById("winner").innerHTML = oneWorldCup.winner;
    document.getElementById("silver").innerHTML = oneWorldCup.runner_up;

    var teams = oneWorldCup.TEAM_NAMES.split(',');

    var elem = document.createElement("ul");
    teams.forEach(function(d){
        var li = document.createElement("li");
        li.innerHTML = d;
        elem.appendChild(li);
    });
    document.getElementById("teams").innerHTML = "";
    document.getElementById("teams").appendChild(elem);

    updateMap(oneWorldCup);
}

/**
 * Renders and updated the map and the highlights on top of it
 *
 * @param the json data with the shape of all countries
 */
function drawMap(world) {
    //(note that projection is global!
    // updateMap() will need it to add the winner/runner_up markers.)

    projection = d3.geoConicConformal().scale(150).translate([400, 350]);

    // ******* TODO: PART IV *******
    // Draw the background (country outlines; hint: use #map)
    // Make sure and add gridlines to the map
    // Define default path generator
    var path = d3.geoPath()
        .projection(projection);

    var graticule = d3.geoGraticule();



    //Load in GeoJSON data
    //Bind data and create one path per GeoJSON feature
    d3.select("#map")
        .selectAll("path")
        .data(topojson.feature(world, world.objects.countries).features)
        .enter()
        .append("path")
        // here we use the familiar d attribute again to define the path
        .attr("d", path)
        // Hint: assign an id to each country path to make it easier to select afterwards
        // we suggest you use the variable in the data element's .id field to set the id
        .attr("id",function(d){return d.id;} )
        // Make sure and give your paths the appropriate class (see the .css selectors at
        // the top of the provided html file)
        .attr("class", "countries");


    d3.select("#map")
        .append("path")
        .datum(graticule)
        .attr("class", "grat")
        .attr("d", path);


}

/**
 * Clears the map
 */
function clearMap() {

    // ******* TODO: PART V*******
    //Clear the map of any colors/markers; You can do this with inline styling or by
    //defining a class style in styles.css

    //Hint: If you followed our suggestion of using classes to style
    //the colors and markers for hosts/teams/winners, you can use
    //d3 selection and .classed to set these classes on and off here.

    d3.selectAll(".countries")
            .classed("team",false)
            .classed("team-html",false)
            .classed("host",false)
            .classed("gold",false)
            .classed("silver",false)
            .classed("selected",false);


    d3.select("#wPoint").remove();
    d3.select("#rPoint").remove();
}


/**
 * Update Map with info for a specific FIFA World Cup
 * @param the data for one specific world cup
 */
function updateMap(worldcupData) {

    //Clear any previous selections;
    clearMap();

    // ******* TODO: PART V *******

    // Add a marker for the winner and runner up to the map.
    //Hint: remember we have a conveniently labeled class called .winner
    // as well as a .silver. These have styling attributes for the two
    //markers.

    var box =  d3.select("#" + getTeamIso(worldcupData.winner, worldcupData)).node().getBBox();
    var x = box.x + box.width/2;
    var y = box.y + box.height/2;
    d3.select("#points").append("circle") //circles inherit pie chart data from the <g>
        .attr("r", 8)
        .attr("cx", x)
        .attr("cy", y)
        .attr("id","wPoint")
        .classed("gold", true);


    box =  d3.select("#" + getTeamIso(worldcupData.runner_up, worldcupData)).node().getBBox();
    x = box.x + box.width/2;
    y = box.y + box.height/2;
    d3.select("#points").append("circle") //circles inherit pie chart data from the <g>
        .attr("r", 8)
        .attr("cx", x)
        .attr("cy", y)
        .attr("id","rPoint")
        .classed("silver", true);




    //Select the host country and change it's color accordingly.
    d3.select("#" + getTeamIso(worldcupData.host, worldcupData))
        .classed("host", true);

    //Iterate through all participating teams and change their color as well.
    //We strongly suggest using classes to style the selected countries.
    worldcupData.teams_iso.forEach(function(d){
        d3.select("#" + d)
            .classed("team", true);
    });


}

function getTeamIso(team, worldcupData){

    var cNameIndex = worldcupData.teams_names.indexOf(team);

    cNameIndex = (cNameIndex-1)<0?0:cNameIndex-1;

    var teamName = worldcupData.teams_iso[cNameIndex];
    return teamName;
}

/* DATA LOADING */

// This is where execution begins; everything
// above this is just function definitions
// (nothing actually happens)

//Load in json data to make map
d3.json("data/world.json", function (error, world) {
    if (error) throw error;
    drawMap(world);
});

// Load CSV file
d3.csv("data/fifa-world-cup.csv", function (error, csv) {

    csv.forEach(function (d) {

        // Convert numeric values to 'numbers'
        d.year = +d.YEAR;
        d.teams = +d.TEAMS;
        d.matches = +d.MATCHES;
        d.goals = +d.GOALS;
        d.avg_goals = +d.AVERAGE_GOALS;
        d.attendance = +d.AVERAGE_ATTENDANCE;
        //Lat and Lons of gold and silver medals teams
        d.win_pos = [+d.WIN_LON, +d.WIN_LAT];
        d.ru_pos = [+d.RUP_LON, +d.RUP_LAT];

        //Break up lists into javascript arrays
        d.teams_iso = d3.csvParse(d.TEAM_LIST).columns;
        d.teams_names = d3.csvParse(d.TEAM_NAMES).columns;

    });

    // Store csv data in a global variable
    allWorldCupData = csv;
    // Draw the Bar chart for the first time
    updateBarChart('attendance');
});
