/*
 *Edited by:
 *      Matthew J. Howa
 *      U0805396
 *      9/8/2016
 *      CS-5630
 *      HW2
 *
 */

/*globals alert, document, d3, console*/
// These keep JSHint quiet if you're using it (highly recommended!)


function interactivity() {
    barInteractivity("barX");
    barInteractivity("barY");

    var circles = d3.select("#scatterplot").selectAll("circle")._groups[0];
    for(var i =0; i< circles.length; i++)
    {
        circles[i].onclick = function (d, i) {
            console.log("CX: " + this.cx.baseVal.value + " CY: " + this.cy.baseVal.value);
            return;
        }
    }

}

function barInteractivity(bar){
    var rects = d3.select("#"+bar).selectAll("rect")._groups[0];

    for(var i = 0; i < rects.length; i++){
        try {
            rects[i].onmouseover = function () {
                this.style.fill = "grey";
            }
            rects[i].onmouseout = function () {
                this.style.fill = "";
            }
        }
        catch(err){console.log(err.message);}
    }

}

function staircase() {
    // ****** TODO: PART II ******

    //loop through charts
    for(var loop=0;loop<2;loop++) {

        var barY = document.getElementById((loop%2==0)?"barY":"barX").childNodes[1];
        var nodes = barY.childNodes;
        var rectangles = [];

        //Get rid of unnecessary nodes
        for (var i = 0; i < nodes.length; i++) {
            if (nodes[i].nodeName == "#text")
                continue;
            rectangles.push(nodes[i]);
        }

        //Sort the rectangles by their width
        rectangles.sort(function (a, b) {
            return a.firstElementChild.width.baseVal.value - b.firstElementChild.width.baseVal.value;
        });

        //Remove the existing nodes
        barY.innerHTML = "";

        var loneRect;

        //Append nodes back in while also changing attributes to look "pretty"
        for (var i = 0; i < rectangles.length; i++) {
            //Log for debugging purposes
            console.log(rectangles[i].firstElementChild.width.baseVal.value);

            //Not all the nodes fit on the screen (this will handle that gracefully
            try {
                rectangles[i].transform.baseVal.getItem(0).setTranslate(0, ((i * 10) + (i * 10)));
            }
            catch (err) {
                loneRect =  rectangles[i];
                rectangles.splice(i--, 1);
                console.log(err.message);
                continue;
            }

            //Change the class so that our styling look like it did when we started
            rectangles[i].firstElementChild.className.baseVal = (i % 2 == 0) ? "a" : "b";
            console.log(rectangles[i].firstElementChild.className.baseVal);
            barY.appendChild(rectangles[i]);
        }
        loneRect.firstElementChild.width.baseVal.value = 0
        barY.appendChild(loneRect);

    }

}

function update(error, data) {

    if (error !== null) {
        alert("Couldn't load the dataset!");
    } else {
        // D3 loads all CSV data as strings;
        // while Javascript is pretty smart
        // about interpreting strings as
        // numbers when you do things like
        // multiplication, it will still
        // treat them as strings where it makes
        // sense (e.g. adding strings will
        // concatenate them, not add the values
        // together, or comparing strings
        // will do string comparison, not
        // numeric comparison).

        // We need to explicitly convert values
        // to numbers so that comparisons work
        // when we call d3.max()
        data.forEach(function (d) {
            d.a = parseInt(d.a);
            d.b = parseFloat(d.b);
        });
    }

    // Set up the scales
    var aScale = d3.scaleLinear()
        .domain([0, d3.max(data, function (d) {
            return d.a;
        })])
        .range([0, 150]);
    var bScale = d3.scaleLinear()
        .domain([0, d3.max(data, function (d) {
            return d.b;
        })])
        .range([0, 150]);
    var iScale = d3.scaleLinear()
        .domain([0, data.length])
        .range([0, 110]);

    // ****** TODO: PART III (you will also edit in PART V) ******

    // TODO: Select and update the 'a' bar chart bars
    changeBarCharts("barY","a",data);

    // TODO: Select and update the 'b' bar chart bars
    changeBarCharts("barX","b",data);

    // TODO: Select and update the 'a' line chart path using this line generator
    var aLineGenerator = d3.line()
        .x(function (d, i) {
            return iScale(i);
        })
        .y(function (d) {
            return aScale(d.a);
        });

    var g = document.getElementById("lineX").firstElementChild;
    g.setAttribute("transform", "scale(1.5,-1) translate(0,-200)")
    var path = g.firstElementChild;
    path.setAttribute("d",aLineGenerator(data));

    // TODO: Select and update the 'b' line chart path (create your own generator)
    var bLineGenerator = d3.line()
        .x(function (d, i) {
            return iScale(i);
        })
        .y(function (d) {
            return bScale(d.b);
        });

    var g = document.getElementById("lineY").firstElementChild;
    g.setAttribute("transform", "scale(1.5,-1) translate(0,-200)")
    var path = g.firstElementChild;
    path.setAttribute("d",bLineGenerator(data));


    // TODO: Select and update the 'a' area chart path using this line generator
    var aAreaGenerator = d3.area()
        .x(function (d, i) {
            return iScale(i);
        })
        .y0(0)
        .y1(function (d) {
            return aScale(d.a);
        });
    var g = document.getElementById("areaX").firstElementChild;
    var path = g.firstElementChild;
    g.setAttribute("transform", "scale(1.5,-1) translate(0,-200)")
    path.setAttribute("d",aAreaGenerator(data));


    // TODO: Select and update the 'b' area chart path (create your own generator)
    var bAreaGenerator = d3.area()
        .x(function (d, i) {
            return iScale(i);
        })
        .y0(0)
        .y1(function (d) {
            return bScale(d.b);
        });
    var g = document.getElementById("areaY").firstElementChild;
    var path = g.firstElementChild;
    g.setAttribute("transform", "scale(1.5,-1) translate(0,-200)")
    path.setAttribute("d",bAreaGenerator(data));


    // TODO: Select and update the scatterplot points

    var circles = d3.select("#scatterplot").selectAll("circle")._groups[0];
    for (var i = 0; i < data.length; i++) {
        circles[i].cx.baseVal.value = data[i].a * 10;
        circles[i].cy.baseVal.value = data[i].b * 10;
        circles[i].r.baseVal.value = 5;
    }

    for(var i = data.length; i< circles.length; i++){
        circles[i].r.baseVal.value = 0;
    }


    // ****** TODO: PART IV ******
    interactivity();



}

function changeData() {
    // // Load the file indicated by the select menu
    var dataFile = document.getElementById('dataset').value;
    if (document.getElementById('random').checked) {
        randomSubset();
    }
    else{
        d3.csv('data/' + dataFile + '.csv', update);
    }
}

function randomSubset() {
    // Load the file indicated by the select menu,
    // and then slice out a random chunk before
    // passing the data to update()
    var dataFile = document.getElementById('dataset').value;
    if (document.getElementById('random').checked) {
        d3.csv('data/' + dataFile + '.csv', function (error, data) {
            var subset = [];
            data.forEach(function (d) {
                if (Math.random() > 0.5) {
                    subset.push(d);
                }
            });
            update(error, subset);
        });
    }
    else{
        changeData();
    }
}


function changeBarCharts(chart,valueSet,data){

    var rectangles = d3.select("#"+chart).selectAll("rect")._groups[0];
    console.log("-------------------------------------"); //for easy reading
    for (var i = 0; i < data.length; i++) {
        rectangles[i].setAttribute('width', data[i][valueSet]);
        console.log(rectangles[i].width.baseVal.value + " : " + data[i][valueSet]);
    }

    for(var i = data.length; i< rectangles.length; i++){
        rectangles[i].setAttribute('width', "0");
    }
}