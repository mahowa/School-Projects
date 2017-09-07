/**
 * Created by Matt Howa on 2/20/2016.
 */




//This function creates a milestone ui object and inserts it in the table
function addMilestone(){
    var list = document.getElementById("milestone");
    var option = document.getElementById("milestone-select");


    //TODO: calculate the number of semesters from the start

    //Make sure we can add it
    if(option.options[option.selectedIndex].text == "Select Milestone")
        return;


    var form = document.getElementById("milestone-form");
    var hidden = document.createElement("input");
    hidden.type = "hidden";
    hidden.value = option.options[option.selectedIndex].value;
    hidden.name="milestone[]";
    form.appendChild(hidden);

    var id = option.options[option.selectedIndex].value;


    //create new row
    var row_div = document.createElement("tr");

    var col_1 =document.createElement("td");
    var text = document.createTextNode(option.options[option.selectedIndex].text);
    col_1.appendChild(text);
    row_div.appendChild(col_1);

    var col_2 =document.createElement("td");
    col_2.setAttribute("class","col");
    var good = document.createElement("img");
    good.src = "../../../../img/green-check.png";
    good.width = 25;
    good.height = 25;
    col_2.appendChild(good);
    row_div.appendChild(col_2);

    var col_3 =document.createElement("td");
    col_3.setAttribute("class","col");
    var good1 = document.createElement("img");
    good1.src = "../../../../img/yellow-check.png";
    good1.width = 25;
    good1.height = 25;
    col_3.appendChild(good1);
    row_div.appendChild(col_3);


    var col_4 =document.createElement("td");
    col_4.setAttribute("class","col");
    var good2 = document.createElement("img");
    good2.src = "../../../../img/red-check.png";
    good2.width = 25;
    good2.height = 25;
    col_4.appendChild(good2);
    row_div.appendChild(col_4);



    var col_5 =document.createElement("td");
    col_5.setAttribute("class","col");
    var text2 = document.createTextNode("1");
    col_5.appendChild(text2);
    row_div.appendChild(col_5);

    var col_6 =document.createElement("td");
    col_6.setAttribute("class","col");
    var good3 = document.createElement("img");
    good3.src = "../../../../img/DeleteRed.png";
    good3.width = 25;
    good3.height = 25;
    good3.addEventListener("click", function(){removeMilestone(row_div, hidden,text.text,id)});
//        good3.onclick = removeMilestone(rowCount);
    col_6.appendChild(good3);
    row_div.appendChild(col_6);


    //add li to list
    list.appendChild(row_div);

    option.removeChild(option.options[option.selectedIndex]);
}

//Removes a milestone from the table
function removeMilestone(row,hidden, name, id){
    row.parentNode.removeChild(row);
    hidden.parentNode.removeChild(hidden);

    var option = document.createElement("option");
    option.value = id;
    var text = document.createTextNode(name);
    option.appendChild(text);

    document.getElementById("milestone-select").appendChild(option);
}