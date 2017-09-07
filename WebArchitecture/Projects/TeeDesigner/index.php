<?php

session_start();

$server_name  = 'localhost';
$db_user_name = 'root'; //howa
$db_password  = 'root';
$db_name      = 'portfolio';

try {

    $db = new PDO("mysql:host=$server_name;dbname=$db_name;charset=utf8", $db_user_name, $db_password);
    $db->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
    $db->setAttribute(PDO::ATTR_EMULATE_PREPARES, false);

}
catch(PDOException $e)
{
    echo "Connection failed: " . $e->getMessage();
}

function grabCurrentURL(){
    if (isset($_SERVER["HTTPS"]) && $_SERVER["HTTPS"] == "on") {
        $url = "https://";
    }else{
        $url = "http://";
    }
    $url .= $_SERVER['SERVER_NAME'];
    if($_SERVER['SERVER_PORT'] != 80){
        $url .= ":".$_SERVER["SERVER_PORT"].$_SERVER["REQUEST_URI"];
    }else{
        $url .= $_SERVER["REQUEST_URI"];
    }
    return $url;
}



if (isset($_REQUEST["svg"])) {

    $dir    = getcwd().'\\svgs\\'. session_id();

    if (!is_dir($dir)) {
        mkdir($dir);
    }

    $files = scandir($dir);
    $fileCount = count($files);

    $filepath = $dir . '\\' . $fileCount.".svg";
    $file = fopen($filepath, "w");
    fwrite($file, $_REQUEST["svg"]);
    fclose($file);
}




function getProducts($db)
{
    $Products = array();
    try {
        $DBH = $db;
        $query = "SELECT  THUMBNAIL_IMAGE, PRODUCT_TITLE,PRODUCT_DESCRIPTION FROM templates group by STYLE";
        $stmt = $DBH->prepare($query);
        $stmt->execute();

        while ($row = $stmt->fetch()) {
            array_push($Products, array($row["THUMBNAIL_IMAGE"],$row["PRODUCT_TITLE"], $row["PRODUCT_DESCRIPTION"]));
        }
    } catch (PDOException $e) {
        reportDBError($e);
    }
    return $Products;
}

$Products = getProducts($db);

$Options = array();
$count = 1;
foreach($Products as $product){
    $path_parts = pathinfo($product[1]);
    $style = $path_parts['basename'];
    $thumbnail = "../THUMBNAIL_IMAGE/". $product[0];

    array_push($Options, array( "text"=> "$style", "value"=>$count++, "selected"=>false,"description" =>"$product[2]","imageSrc"=>"$thumbnail" ));

}



$optionData =  json_encode($Options);

?>


<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Design Studio</title>

    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="">
    <meta name="author" content="">
    <!-- Le HTML5 shim, for IE6-8 support of HTML elements -->
    <!--[if lt IE 9]>
    <script src="http://html5shim.googlecode.com/svn/trunk/html5.js"></script>
    <![endif]-->
    <!--[if IE]><script type="text/javascript" src="js/excanvas.js"></script><![endif]-->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.0/jquery.min.js"></script>
    <script type="text/javascript" src="js/fabric.js"></script>
    <script type="text/javascript" src="js/tshirtEditor.js"></script>
    <script type="text/javascript" src="bootstrap/js/bootstrap.min.js"></script>
    <script src="pick-a-color/build/dependencies/tinycolor-0.9.15.min.js"></script>
    <script src="pick-a-color/build/1.2.3/js/pick-a-color-1.2.3.min.js"></script>
    <script src="js/jquery.ddslick.min.js"></script>
    <script src="../TeeDesigner/js/mousetrap/mousetrap.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/croppie.js"></script>
    <script type="text/javascript" src="js/dropzone.js"></script>
    <script type="text/javascript" src="js/imageUploader.js"></script>


    <!-- Styles -->
    <link href="bootstrap/css/bootstrap.css" rel="stylesheet">
    <link rel="stylesheet" href="pick-a-color/build/1.2.3/css/pick-a-color-1.2.3.min.css">
    <link href="bootstrap/css/dist/dropzone.css" type="text/css" rel="stylesheet">
    <link href="bootstrap/css/style.css" type="text/css" rel="stylesheet">
    <link href="bootstrap/css/croppie.css" type="text/css" rel="stylesheet">
</head>


<body>

<div  class="container">
    <div class="page-header">
        <h1>
            Product Designer
            <a href="ReadMe.html" id="readMeButton" class="btn btn-lg btn-default pull-right" style="margin-left: 15px;">ReadMe</a>
            <button id="shortCutButton" class="btn btn-lg btn-default pull-right">ShortCuts</button>
        </h1>
    </div>

    <!-- Headings & Paragraph Copy -->
    <div class="row">
        <div class="col-md-4">
            <h2>Design</h2>
            <ul class="nav nav-pills">
                <li class="active"><a data-toggle="pill" href="#home">Options</a></li>
                <li><a data-toggle="pill" href="#menu1">Style</a></li>
            </ul>

            <div class="tab-content">
                <div id="home" class="tab-pane fade in active" style="margin-top: 25px">
                    <div class="form-group">
                        <label for="sel1">Select list:</label>
                        <select class="form-control" id="sel1"></select>
                    </div>
                    <button class="btn btn-danger" id="customProduct">Custom Product</button>
                </div>
                <div id="menu1" class="tab-pane fade">
                    <div >
                        <div class="input-group input-group-lg" style="margin-top: 25px">
                            <input type="text" id="text-string" class="form-control" placeholder="add text here..." aria-describedby="add-text" >
                            <span id="add-text" class="btn input-group-addon" title="Add text" >+</span>
                            <hr/>

                        </div>
                        <button style="margin-top: 25px;margin-bottom: 25px" id="logo_designer" class="btn btn-danger" href="method-draw/index.html" title="Click here to access our advance logo designer">Design Your Logo Now</button>
                        <div id="avatarlist">
                            <?php
                                $dir = getcwd().'\\svgs\\'. session_id();
                                $files = scandir($dir);
                                $url = grabCurrentURL();
                                $index = strpos($url,"index.php");
                                $url = substr($url,0,$index);

                                $path =  $url . 'svgs/'. session_id(). "/";

                                foreach($files as $design){
                                    $path_parts = pathinfo($design);
                                    if($path_parts['extension'] != "svg") continue;
                                    $src = $path . $design;

                                   echo "<img src='$src' class='img-thumbnail img-polaroid' style='height: 100px; width: 100px;'/>";
                                }
                            ?>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-sm-2" >
            <div class="row" id="thumbs" style="max-height: 630px; overflow: auto">

            </div>

        </div>

        <div class="col-sm-10 col-md-6">
            <div align="center" style="min-height: 32px;">
                <div class="clearfix">
                    <div class="form-inline pull-left" id="texteditor" style="display:none">
                        <div class="input-group inline " >
                            <button id="font-family" class="btn dropdown-toggle " data-toggle="dropdown" title="Font Style">
                                <span class="glyphicon glyphicon-font" style="width:19px;height:19px;"></span>
                            </button>
                            <ul class="dropdown-menu" role="menu" aria-labelledby="font-family-X">
                                <li><a tabindex="-1" href="#" onclick="setFont('Arial');" class="Arial">Arial</a></li>
                                <li><a tabindex="-1" href="#" onclick="setFont('Helvetica');" class="Helvetica">Helvetica</a></li>
                                <li><a tabindex="-1" href="#" onclick="setFont('Myriad Pro');" class="MyriadPro">Myriad Pro</a></li>
                                <li><a tabindex="-1" href="#" onclick="setFont('Delicious');" class="Delicious">Delicious</a></li>
                                <li><a tabindex="-1" href="#" onclick="setFont('Verdana');" class="Verdana">Verdana</a></li>
                                <li><a tabindex="-1" href="#" onclick="setFont('Georgia');" class="Georgia">Georgia</a></li>
                                <li><a tabindex="-1" href="#" onclick="setFont('Courier');" class="Courier">Courier</a></li>
                                <li><a tabindex="-1" href="#" onclick="setFont('Comic Sans MS');" class="ComicSansMS">Comic Sans MS</a></li>
                                <li><a tabindex="-1" href="#" onclick="setFont('Impact');" class="Impact">Impact</a></li>
                                <li><a tabindex="-1" href="#" onclick="setFont('Monaco');" class="Monaco">Monaco</a></li>
                                <li><a tabindex="-1" href="#" onclick="setFont('Optima');" class="Optima">Optima</a></li>
                                <li><a tabindex="-1" href="#" onclick="setFont('Hoefler Text');" class="Hoefler Text">Hoefler Text</a></li>
                                <li><a tabindex="-1" href="#" onclick="setFont('Plaster');" class="Plaster">Plaster</a></li>
                                <li><a tabindex="-1" href="#" onclick="setFont('Engagement');" class="Engagement">Engagement</a></li>
                            </ul>
                            <button id="text-bold" class="btn" data-original-title="Bold">
                                <span class="glyphicon glyphicon-bold" style="width:19px;height:19px;"></span>
                            </button>
                            <button id="text-italic" class="btn" data-original-title="Italic">
                                <span class="glyphicon glyphicon-italic" style="width:19px;height:19px;"></span>
                            </button>
                            <button id="text-underline" class="btn" title="Underline" style="">
                                <span class="glyphicon glyphicon-text-color" style="width:19px;height:19px;"></span>
                            </button>

                            <input id="text-fontcolor" type="text" value="000" name="border-color" class="pick-a-color form-control" rel="tooltip" data-placement="top" data-original-title="Font Color"/>
                            <input id="text-strokecolor" type="text" value="000" name="border-color" class="pick-a-color form-control"  rel="tooltip" data-placement="top" data-original-title="Font Border Color"/>

                        </div>
                    </div>
                    <div class="pull-right" align="" id="imageeditor" style="display:none">
                        <div class="btn-group">
                            <button class="btn" id="bring-to-front" title="Bring to Front"><span class="glyphicon glyphicon-triangle-top" style="height:19px;"></span></button>
                            <button class="btn" id="send-to-back" title="Send to Back"><span class="glyphicon glyphicon-triangle-bottom" style="height:19px;"></span></button>
<!--                            <button id="flip" type="button" class="btn" title="Show Back View"><span class="glyphicon glyphicon-retweet" style="height:19px;"></span></button>-->
                            <button id="remove-selected" class="btn" title="Delete selected item"><span class="glyphicon glyphicon-trash" style="height:19px;"></span></button>
                        </div>
                    </div>
                </div>
            </div>
            <!--	EDITOR      -->
            <div class="myCroppie"></div>
            <div id="shirtDiv" class="page" style="width: 530px; height: 630px; position: relative; background-color: rgb(255, 255, 255);">
                <img id="tshirtFacing" src="img/crew_front.png" height="630" width="530"/>
                <div id="drawingArea" style="position: absolute;top: 0px;left: 0px;z-index: 10;width: 530px;height: 630px;">
                    <canvas id="tcanvas" width="530" height="630" class="hover" style="-webkit-user-select: none;"></canvas>
                </div>
            </div>
        </div>
        <div class="col-sm-12" id="imageUploadDiv" style="margin: 10px">
            <form action="upload.php" class="dropzone needsclick dz-clickable" id="myDropzone"  enctype= "multipart/form-data" ></form>
            <div class="demo-wrap">
                <div class="container">
                    <div class="grid">
                        <div class="col-1-2">
                            <div class="actions">
                                <button class="vanilla-result">Result</button>
                                <!--                        <button class="vanilla-rotate" data-deg="-90">Rotate Left</button>-->
                                <!--                        <button class="vanilla-rotate" data-deg="90">Rotate Right</button>-->
                            </div>
                        </div>
                        <div class="col-1-2">
                            <div id="demo-basic"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-sm-12" style="margin: 10px">
            <div class="well">
                <h3>Product Description</h3>
                <br/>
                <table class="table">
                    <tr>
                        <td>Description</td>
                        <td id="description"></td>
                    </tr>
                    <tr>
                        <td>Available Sizes</td>
                        <td id="sizes"></td>
                    </tr>
                    <tr>
                        <td>Price</td>
                        <td id="price"></td>
                    </tr>
                </table>

                <button type="button" class="btn btn-large btn-block btn-danger" name="addToTheBag" id="addToTheBag">Add to bag <i class="icon-briefcase icon-white"></i></button>
            </div>
        </div>
    </div>
</div>


<div id="shortCutsModal" class="modal">

    <!-- Modal content -->
    <div class="modal-content" data-backdrop="false">
        <div class="modal-header">
            <span class="close">×</span>
            <h2>Keyboard Shortcuts<img src="img/logo.gif" height="50px" width="75px" align="right" style="padding-right: 20px"></h2>
        </div>
        <div class="modal-body">
            <table class="table table-bordered table-striped table-hover" style="margin-top: 25px; margin-bottom: 25px">
                <thead><tr><th>Action</th><th>Shortcut</th></tr></thead>
                <tbody>
                <tr><td>Open options tab</td><td>ctrl + o</td></tr>
                <tr><td>Open style tab</td><td>ctrl + st</td></tr>
                <tr><td>Add Text</td><td>txt</td></tr>
                <tr><td>Go to logo designer</td><td>ld</td></tr>
                <tr><td>Bring up font style</td><td>fs</td></tr>
                <tr><td>Make text bold</td><td>ctrl + b</td></tr>
                <tr><td>Make text italicized</td><td>ctrl + i</td></tr>
                <tr><td>Make text underlined</td><td>ctrl + u</td></tr>
                <tr><td>Change text font color</td><td>ctrl + f + c</td></tr>
                <tr><td>Change stroke color</td><td>ctrl + s + c</td></tr>
                <tr><td>Bring to front</td><td>ctrl + f + r</td></tr>
                <tr><td>Send to back</td><td>ctrl + b + a</td></tr>
                <tr><td>Trash</td><td>ctrl + t + r</td></tr>
                <tr><td>Add to bag</td><td>ctrl+ a + d + d</td></tr>
                </tbody>
            </table>
        </div>
        <div class="modal-footer" >
        </div>
    </div>

</div>

<!--Script by Matt Howa-->
<script>
    var TShirtHtml;
    var body;
    $(".pick-a-color").pickAColor({
        showHexInput: false
    });

    $(document).ready(function(){
        $("#logo_designer").click(function(){
            window.location = "method-draw/index.php";
        });
    });

    $(document).ready(function(){
        $('#sel1').ddslick({
            data: <?php echo $optionData ?>,
            width: 320,
            imagePosition: "left",
            truncateDescription: true,
            height:300,
            selectText: "Select a Product",
            onSelected: function (data) {
                $('#imageUploadDiv').hide();
                $('#thumbs').show();
                $("#tshirtFacing").show();
                $('#shirtDiv').show();
                var title = data.selectedData.text;
                var directory = title.substring(title.lastIndexOf('.')+1);

                getFiles(title, directory);
            }
        });

    });

    function getFiles(title,directory){
        title = title.split(' ').join('%20');

        var url_string1 = "http://"+document.location.hostname +
            "/DesignStudio/TeeDesigner/pics.php?directory="+directory.trim();
        var url_string2 = "http://"+document.location.hostname +
            "/DesignStudio/TeeDesigner/product.php?title="+title.trim();

        $.ajax({
            url: url_string1,
            type: 'GET',
            success: function (res) {
                var obj = JSON.parse("["+res +"]");

                var html = "";


                obj[0].forEach(function(item,index){
                    var src = "../COLOR_PRODUCT_IMAGE/"+ directory.trim() + "/" +item;
                    html += "<div class='col-sm-12'><div class='thumbnail' onclick='change(this)'>";
                    html += "<img  src='"+src+"'  height='42' width='42' style='cursor: pointer'/>";
                    html += "</div></div>";
                });

                $('#thumbs').html(html);

                $("#tshirtFacing").attr("src","../COLOR_PRODUCT_IMAGE/"+ directory.trim() + "/"+obj[0][0]);
            }

        });
        $.ajax({
            url: url_string2,
            type: 'GET',
            success: function (res) {
                var obj = JSON.parse(res);
                $("#description").html(obj.PRODUCT_DESCRIPTION);
                $("#sizes").html(obj.AVAILABLE_SIZES);
                $("#price").html("$"+obj.MSRP);
            }

        });
    }

    function backToDesigner(){
        body.html(TShirtHtml);
    }

    function change(elem){
        var src = elem.childNodes[0].src;
        $("#tshirtFacing").attr("src",src);
    }

</script>

<!--Script by Charlie Barber -->
<script>
    // Get the modal
    var modal = document.getElementById('shortCutsModal');

    // Get the button that opens the modal
    var btn = document.getElementById("shortCutButton");

    // Get the <span> element that closes the modal
    var span = document.getElementsByClassName("close")[0];

    // When the user clicks the button, open the modal
    btn.onclick = function() {
        modal.style.display = "block";
    }

    // When the user clicks on <span> (x), close the modal
    span.onclick = function()
    {
        modal.style.display = "none";
    }


    //Using mousetrap javascript libray this bounds the shortcut buttons
    Mousetrap.bind('ctrl+o', function ()
    {
        $('#optionsLink').click();
    });

    Mousetrap.bind('ctrl+s+t', function ()
    {
        $('#styleLink').click();
    });

    Mousetrap.bind('ctrl+e', function ()
    {
        $('#selection').simulate('mousedown');
    });

    Mousetrap.bind('l d', function ()
    {
        $('#logo_designer').click();
    });


    Mousetrap.bind('f s', function ()
    {
        $('#font_family').click();
    });


    Mousetrap.bind('t x t', function ()
    {
        $('#add-text').click();
    })

    Mousetrap.bind('ctrl+b', function ()
    {
        $('#text-bold').click();
    });

    Mousetrap.bind('ctrl+i', function ()
    {
        $('#text-italic').click();
    });

    Mousetrap.bind('ctrl+u', function ()
    {
        $('#text-underline').click();
    });

    Mousetrap.bind('ctrl+f+c', function ()
    {
        $('#text-fontcolor').click();
    });

    Mousetrap.bind('ctrl+s+c', function ()
    {
        $('#text-strokecolor').click();
    });

    Mousetrap.bind('ctrl + f + r', function ()
    {
        $('#bring-to-front').click();
    });

    Mousetrap.bind('ctrl+b+a', function ()
    {
        $('#send-to-back').click();
    });

    Mousetrap.bind('f r b', function ()
    {
        $('#flip').click();
    });

    Mousetrap.bind('ctrl+t+r', function ()
    {
        $('#remove-selected').click();
    });

    Mousetrap.bind('ctrl+a+d+d', function ()
    {
        $('#addToTheBag').click();
    });
</script>

<!--Script by Matt Willden -->
<script>
    var $element = "";
    Dropzone.options.myDropzone = {

        paramName: "file", // The name that will be used to transfer the file
        maxFilesize: 3, // MB
        acceptedFiles: "image/*",
        init: function () {



            this.on("addedfile", function(file) {


                // Create the remove button
                var removeButton = Dropzone.createElement("<button>Remove file</button>");


                // Capture the Dropzone instance as closure.
                var _this = this;

                // Listen to the click event
                removeButton.addEventListener("click", function(e) {
                    // Make sure the button click doesn't submit the form:
                    e.preventDefault();
                    e.stopPropagation();

                    // Remove the file preview.
                    _this.removeFile(file);
                    $('.myCroppie').html("");
                    $(".demo-wrap").hide();

                    // If you want to the delete the file on the server as well,
                    // you can do the AJAX request here.




                });


                file.previewElement.addEventListener("click", function(e) {

                    // Make sure the button click doesn't submit the form:
                    e.preventDefault();
                    e.stopPropagation();

                    if ($element != "") {
                        $('.myCroppie').show();
                        $('.myCroppie').html("");
                        $(".demo-wrap").hide();
                    }

                    //imageCroppie.init();

                    var path = 'uploads/' + file.name;
                    $("#tshirtFacing").attr('src', path);
                    $("#tshirtFacing").hide('fast');


                    $element = $('.myCroppie');
                    $element.croppie({
                        viewport: {
                            width: 530,
                            height: 630
                        },
                        boundary: {
                            width: 560,
                            height: 660
                        }
                    });

                    var path = $('#tshirtFacing').attr("src");
                    $element.croppie('bind', path);
                    $(".demo-wrap").show();
                    $('#shirtDiv').hide();

                    $('.vanilla-result').on("click", function () {
                        $element.croppie('result', 'canvas').then(function (result) {
                            $("#tshirtFacing").attr('src', result);

                            $("#tshirtFacing").show();
                            $('#shirtDiv').show();


                            $('.myCroppie').html("");
                            $('.myCroppie').hide();
                            $(".demo-wrap").hide();
                            $('#imageUploadDiv').hide();

                        });

                    });


                });

                // Add the button to the file preview element.
                file.previewElement.appendChild(removeButton);
            });


        }
    };

</script>


</body>
</html>