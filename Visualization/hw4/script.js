
/** Global var to store all match data for the 2014 Fifa cup */
var teamData;

/** Global var for list of all elements that will populate the table.*/
var tableElements = [];
var count = 0;
var removeList = 0;

/** Variables to be used when sizing the svgs in the table cells.*/
var cellWidth = 70,
    cellHeight = 20,
    cellBuffer = 15,
    barHeight = 20;

/**Set variables for commonly accessed data columns*/
var goalsMadeHeader = 'Goals Made',
    goalsConcededHeader = 'Goals Conceded';

/** Setup the scales*/
var goalScale = d3.scaleLinear()
    .range([cellBuffer, 2 * cellWidth - cellBuffer]);

/**Used for games/wins/losses*/
var gameScale = d3.scaleLinear()
    .range([0, cellWidth - cellBuffer]);

/**Color scales*/
/**For aggregate columns*/
var aggregateColorScale = d3.scaleLinear()
    .range(['#ece2f0', '#016450']);

/**For goal Column*/
var goalColorScale = d3.scaleQuantize()
    .domain([-1, 1])
    .range(['#cb181d', '#034e7b']);

/**json Object to convert between rounds/results and ranking value*/
var rank = {
    "Winner": 7,
    "Runner-Up": 6,
    'Third Place': 5,
    'Fourth Place': 4,
    'Semi Finals': 3,
    'Quarter Finals': 2,
    'Round of Sixteen': 1,
    'Group': 0
};
var rank2 = {
    7:"Winner",
    6:"Runner-Up",
    5:'Third Place',
    4: 'Fourth Place',
    3:'Semi Finals',
    2:'Quarter Finals',
    1:'Round of Sixteen',
    0: 'Group'
};


//For the HACKER version, comment out this call to d3.json and implement the commented out
// d3.csv call below.

//d3.json('data/fifa-matches.json',function(error,data){
//    teamData = data;
//    createTable();
//    updateTable();
//})


// // ********************** HACKER VERSION ***************************
// /**
//  * Loads in fifa-matches.csv file, aggregates the data into the correct format,
//  * then calls the appropriate functions to create and populate the table.
//  *
//  */
//JSON: "key": "Brazil",
//    "value": {
//        "Goals Made": 11,
//        "Goals Conceded": 14,
//        "Delta Goals": -3,
//        "Wins": 4,
//        "Losses": 2,
//        "Result": {"label": "Third Place", "ranking": 5},
//        "TotalGames": 7,
//        "games": [{
//          "key": "Netherlands",
//          "value": {
//            "Goals Made": "0",
//            "Goals Conceded": "3",
//            "Delta Goals": [],
//            "Wins": [],
//            "Losses": [],
//            "Result": {"label": "Third Place", "ranking": 5},
//            "type": "game",
//            "Opponent": "Brazil"
//        }
//    }
//CSV:Team,Opponent,Goals Made,Goals Conceded,Delta Goals,Wins,Losses,Result
 d3.csv("data/fifa-matches.csv", function (error, csvData) {
    var data = {};
    // ******* TODO: PART I *******
     teamData = d3.nest()
         .key(function (d) {
             return d.Team;
         })

         .rollup(function (leaves) {
             return {
                 "Goals Made":d3.sum(leaves,function(l){return l["Goals Made"]}),
                 "Goals Conceded": d3.sum(leaves,function(l){return l["Goals Conceded"]}),
                 "Delta Goals": d3.sum(leaves,function(l){return l["Delta Goals"]}),
                 "Wins": d3.sum(leaves,function(l){return l["Wins"]}),
                 "Losses": d3.sum(leaves,function(l){return l["Losses"]}),
                 "Result": {"label": d3.map(rank2).get(d3.max(leaves,function(l){return rank[l.Result]})), "ranking": d3.max(leaves,function(l){return rank[l.Result]})},
                 "TotalGames": leaves.length,
                 "games": d3.nest()
                     .key(function (d) {
                         return d.Opponent;
                     })
                     .rollup(function(game){
                         return{
                             "Goals Made": game[0]["Goals Made"],
                             "Goals Conceded": game[0]["Goals Conceded"],
                             "Delta Goals": [],
                             "Wins": [],
                             "Losses": [],
                             "Result": {"label": game[0].Result, "ranking": d3.map(rank).get(game[0].Result)},
                             "type": "game",
                             "Opponent": game[0].Team
                         }
                     })

                     .entries(leaves)
             }})
         .entries(csvData);


    createTable();
    updateTable();
 });



// // ********************** END HACKER VERSION ***************************

/**
 * Loads in the tree information from fifa-tree.csv and calls createTree(csvData) to render the tree.
 *
 */
d3.csv("data/fifa-tree.csv", function (error, csvData) {

    //Create a unique "id" field for each game
    csvData.forEach(function (d, i) {
        d.id = d.Team + d.Opponent + i;
    });

    createTree(csvData);
});

/**
 * Creates a table skeleton including headers that when clicked allow you to sort the table by the chosen attribute.
 * Also calculates aggregate values of goals, wins, losses and total games as a function of country.
 *
 */
function createTable() {

// ******* TODO: PART II *******
//    Now that we have a data array, we can use it to set the domains for the the scales provided at the top of
// script.js. When creating the domain for goalScale, think specially carefully about what attribute you want to use
// to set the domain values. We will be using this scale to visualize goals made, conceded, and the difference between the two.

    goalScale.domain([0, d3.max(teamData, function(t){
        //Return higher value goals made/conceded
        var conceded = t.value["Goals Conceded"];
        var made = t.value["Goals Made"];
        var max;
        if(t.value["Delta Goals"] > 0)
            max = made;
        else
            max= conceded;
        return max;
    })]);

    gameScale.domain([0, d3.max(teamData, function(t){
        return t.value["TotalGames"];
    })])

    aggregateColorScale.domain([0, d3.max(teamData, function(t){
        return t.value["TotalGames"];
    })])


//### Add X axis to Goals Column header
//
//    The next step is creating an x axis underneath the goals column header. Notice that we provided a table cell
// with an id of goalHeader, that is already in the right location.
//
    var xAxis = d3.axisTop()
        .tickFormat(d3.format(".0f"));

    xAxis
        .scale(goalScale);

    var svg = d3.select("#goalHeader").append("svg");
    svg.attr("width",  2 * cellWidth)
        .attr("height", cellHeight +cellBuffer)

    svg.append("g")
        .attr("transform", "translate(0,20)")
        .call(xAxis)

//### Create list to populate table.
//
//        Recall from earlier how our table will be composed of two types of rows. Aggregate rows that contain summary
// values for all games, and game specific rows that contain information for a given match. In order to facilitate the
// process of populating this table, we will be keeping an updated list of all data elements we want to display in the
// table in the global variable `tableElements`. As a first step, just copy  the `teamData` list over to the `tableElements`.
//
//        We are now ready to populate the table with aggregate rows!

    for(var c =0; c<teamData.length; c++){
        teamData[c].value.type = "aggregate";
        tableElements[c] = teamData[c];

    }

// ******* TODO: PART V *******
//Add sorting to table headers

    var compareNode = function (a, b) {
        if(colHeader == "Team"){
            if(isSorted != true)
                return d3.ascending( a.key , b.key);
            else{
                return d3.descending(a.key, b.key);

            }
        }

        if(colHeader == "Goals"){
            var aa =  a.value["Goals Made"];
            var bb =  b.value["Goals Made"];
            if(isSorted == true)
                return d3.ascending( aa, bb);
            else{
                return d3.descending(aa, bb);
            }
        }

        if(colHeader == "Round/Result"){
            var aa =  a.value.Result.ranking;
            var bb =  b.value.Result.ranking;
            if(isSorted == true)
                return d3.ascending( aa, bb);
            else{
                return d3.descending(aa, bb);
            }
        }

        if(colHeader == "Wins"){
            var aa =  a.value.Wins;
            var bb =  b.value.Wins;
            if(isSorted == true)
                return d3.ascending( aa, bb);
            else{
                return d3.descending(aa, bb);
            }
        }
        if(colHeader == "Losses"){
            var aa =  a.value.Losses;
            var bb =  b.value.Losses;
            if(isSorted == true)
                return d3.ascending( aa, bb);
            else{
                return d3.descending(aa, bb);
            }
        }
        if(colHeader == "Total Games"){
            var aa =  a.value.TotalGames;
            var bb =  b.value.TotalGames;
            if(isSorted == true)
                return d3.ascending( aa, bb);
            else{
                return d3.descending(aa, bb);
            }
        }
    }

    var sort = function(header){
        var table = d3.select("#matchRows");
        var tr = table.selectAll("tr")
        header = header.trim('\n');
        try{

            if (header !== colHeader) {
                isSorted = false;
                //var table = d3.select("#matchRows");
                //table.html("")
                //colHeader = null;
                //updateTable();
            }
        }
        catch (err){
            colHeader = header;
            isSorted = false;
        }
        tableElements.sort(compareNode)
        isSorted = !isSorted;
        updateTable();
    }


    var cols = d3.select("#matchTable")
        .select("thead")
        .selectAll("td")
        .on("click", function(d){
            var colName = this.innerText;
            sort(colName);
        })

}



/**
 * Updates the table contents with a row for each element in the global variable tableElements.
 *
 */
function updateTable() {

// ******* TODO: PART III *******
//    We will be populating the table in three steps.
//        Add bar charts for the number of wins, losses and total games.
//var td = tr.selectAll("td").data(function(d){create data array here});
//        Display the Round/Results as a text element.

    var table = d3.select("#matchRows");
    table.html("");
    var tr = table.selectAll("tr")
        .data(tableElements)
        .enter()
        .append("tr")
        .on("click",function(d,i) {
            if(d.value.type =="game")
                return;
            updateList(i);

        });

    var td = tr.selectAll("td")
        .data(function(d){
            //TYPE, VIS, VALUE
            var goals = {};
            goals.conceded = {};
            goals.conceded.type = "conceded";
            goals.conceded.value= d.value["Goals Conceded"];
            goals.conceded.typev = d.value.type;
            goals.made = {};
            goals.made.type = "made";
            goals.made.value = d.value["Goals Made"];
            goals.made.typev = d.value.type;

            var team = {};
            team.t = "team";
            team.type = d.value.type;
            team.vis = "text";
            team.value = d.key;

            var goalsObj  = {};
            goalsObj.type = d.value.type;
            goalsObj.vis = "goals";
            goalsObj.value = goals;

            var result= {};
            result.type = d.value.type;
            result.vis = "text";
            result.value = d.value["Result"].label;

            var wins = {};
            wins.type = d.value.type;
            wins.vis = "bar";
            wins.value =  d.value["Wins"];

            var losses = {};
            losses.type = d.value.type;
            losses.vis = "bar";
            losses.value =  d.value["Losses"];

            var total = {};
            total.type = d.value.type;
            total.vis = "bar";
            total.value =  d.value["TotalGames"];


            return [team, goalsObj, result, wins, losses, total]
        })
        .enter()
        .append("td")
        .html(function(d){
            //Text cells
            if(d.vis == "text")
                return d.value;

        })
        .style("color",function(d){
            if(d.t =="team")
            if(d.vis == "text") {
                if (d.type == "aggregate")return "green";
                else
                    return "grey";
            }
        });


//        Add our custom goals charts for the number of goals made and conceded.

    var goals = td.filter(function (d) {
                            return d.vis == 'goals'
                        });

    var svg = goals.append("svg")
                    .attr("width", 2 * cellWidth - cellBuffer)
                    .attr("height", cellHeight);

    var lineFunction = d3.line();
    svg.selectAll(".link")
        .data(function(d){
            return [d];
        })
        .enter().append("path")
        .attr("class", "link")
        .attr("d", function(d){
            //console.log(d);
            var pts = [[goalScale(d.value.conceded.value), 0],[goalScale(d.value.made.value), 0]];
            return lineFunction(pts);
        })
        .attr("transform", "translate(-5,10)")
        .style("stroke-width", function(d){
            if(d.type=="aggregate")
                return"6px";
            return "2px";
        })
        .style("stroke", function(d){
            var r = goalColorScale(goalScale(d.value.made.value)-goalScale(d.value.conceded.value));
            return r;
        });

    svg.append("g")
        .selectAll("circle")
        .data(function(d){
            return [d.value.conceded, d.value.made];
        })
        .enter()
        .append("circle")
        .style("fill", function(d){
            if(d.typev == "game")
                return "none";
            if(d.type == "conceded")
                return "#be2714";
            if(d.type == "made")
                return "#364e74"})
        .style("stroke", function(d){
            if(d.typev == "aggregate")
                return "none";
            if(d.type == "conceded")
                return "#be2714";
            if(d.type == "made")
                return "#364e74"})
        .attr("transform", "translate(-5,10)")
        .attr("r", 3)
        .attr("cx", function(d){
            if(d.type == "conceded")
                return goalScale(d.value)
            if(d.type == "made")
                return goalScale(d.value)})
        .attr("cy", 0);

    var bars = td.filter(function (d) {
        return d.vis == 'bar'
    });

    var svg2 = bars.append("svg")
        .attr("width", cellWidth)
        .attr("height", cellHeight);

    svg2.append("g").selectAll(".bar")
        .data(function(d){
            return [d];
        })
        .enter().append("rect")
        .attr("class", "bar")
        .attr("x", function(d) {return  0;})
        .attr("y", function(d) { return 0; })
        .attr("width", function (d) {
            return gameScale(d.value);})
        .attr("height", cellHeight)
        .attr("class","goalBar")
        .style("fill", function(d){
            var r = aggregateColorScale(d.value);
            return r;
        })



    svg2.selectAll("text")
        .data(function(d){
            return [d];
        })
        .enter().append("text")
        .attr("x", function (d) {return gameScale(d.value)})
        .attr("dx", 0)
        .attr("dy", 13)
        .attr("text-anchor", "end")
        .text(function(d) { return d.value;})
        .style("fill", "white");
}

/**
 * Collapses all expanded countries, leaving only rows for aggregate values per country.
 *
 */
function collapseList() {
    // ******* TODO: PART IV *******
    removelist = 0;
    for(var i =0; i<tableElements.length; i++){
        if(tableElements[i].value.type == "game") {
            tableElements.splice(i, 1);
            i--;
            removelist++;
        }
    }
    updateTable();
}

/**
 * Updates the global tableElements variable, with a row for each row to be rendered in the table.
 *
 */
function updateList(i) {
    collapseList();
    if(removeList > 0)
        return;
    i = i-removelist;
    // ******* TODO: PART IV *******
    var table = d3.select("#matchRows");
    var row = table.selectAll("tr")
        .filter(function (d, ii) {
            return ii === i;
        })._groups[0][0].__data__.value.games;

    for(var r = 0; r<row.length; r++ ){
        row[r].value.type = "game";
        if(row[r].key[0] !== 'x')
            row[r].key = "x"+ row[r].key;
        tableElements.splice(++i,0,row[r]);
    }
    updateTable();
}

/**
 * Creates a node/edge structure and renders a tree layout based on the input data
 *
 * @param treeData an array of objects that contain parent/child information.
 */
function createTree(treeData) {

    // ******* TODO: PART VI *******
    //Where .id is unique identifier for each node and
    // .parentId indicates what field contains the parent Node to that element.
    var root = d3.stratify()
        .id(function(d) {
            return d.id; })
        .parentId(function(d) {
            if(d.ParentGame =="")
                return "";
            var index=d.ParentGame;
            return treeData[index].id })
        (treeData);

    var tree = d3.tree()
        .size([600, 400]);

    tree(root);
    var svg = d3.select("#tree");
    var link = svg.selectAll(".link")
        .data(root.descendants().slice(1))
        .enter().append("path")
        .attr("class", "link")
        .attr("d", function(d) {

            var dd= diagonal(d);
            return dd;
        });

    var node = svg.selectAll(".node")
        .data(root.descendants())
        .enter().append("g")
        .attr("class", function(d) { return "node" + (d.children ? " node--internal" : " node--leaf"); })
        .attr("transform", function(d) { return "translate(" + d.y + "," + d.x + ")"; });

    node.append("circle")
        .attr("r", 5)
        .attr("fill",function(d){
            if(count==0) {
                count++;
                return "#034e7b";
            }
            if(count %2==0)
            {
                count++;
                return "#cb181d";
            }
            else{
                count++;
                return "#034e7b";
            }

    })

    node.append("text")
        .attr("dy", 3)
        .attr("x", function(d) { return d.children ? -8 : 8; })
        .style("text-anchor", function(d) { return d.children ? "end" : "start"; })
        .text(function(d) {

            return d.data.Team;
        })
};

function diagonal(d) {
    return "M" + d.y + "," + d.x
        + "C" + (d.parent.y + 25) + "," + d.x
        + " " + (d.parent.y + 25) + "," + d.parent.x
        + " " + d.parent.y + "," + d.parent.x;
}

/**
 * Updates the highlighting in the tree based on the selected team.
 * Highlights the appropriate team nodes and labels.
 *
 * @param team a string specifying which team was selected in the table.
 */
function updateTree(row) {

    // ******* TODO: PART VII *******


}

/**
 * Removes all highlighting from the tree.
 */
function clearTree() {

    // ******* TODO: PART VII *******
    

}



