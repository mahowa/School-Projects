<?php
include ("partial.php");

$partial = new Partial("home");
echo $partial->head("Home");
?>

<h1> <p class="typing">hello world, my name is _matt_howa<span class="cursor">|</span></p> </h1>


<div id='myCarousel' class='carousel slide' data-ride='carousel'>
    <!-- Indicators -->
    <ol class='carousel-indicators'>
        <li data-target='#myCarousel' data-slide-to='0' class='active'></li>
        <li data-target='#myCarousel' data-slide-to='1'></li>
        <li data-target='#myCarousel' data-slide-to='2'></li>
        <li data-target='#myCarousel' data-slide-to='3'></li>
    </ol>

    <!-- Wrapper for slides -->
    <div class='carousel-inner' role='listbox'>
        <div class='item active'>
            <img src="img/campus.jpg" alt="">
        </div>

        <div class='item'>
            <img src="img/servers.jpeg" alt="">
        </div>

        <div class='item'>
            <img src="img/motherboard.jpg" alt="">
        </div>

        <div class='item'>
            <img src="img/keyboard.png" alt="">
        </div>

    </div>

    <!-- Left and right controls -->
    <a class='left carousel-control' href='#myCarousel' role='button' data-slide='prev'>
        <span class='glyphicon glyphicon-chevron-left' aria-hidden='true'></span>
        <span class='sr-only'>Previous</span>
    </a>
    <a class='right carousel-control' href='#myCarousel' role='button' data-slide='next'>
        <span class='glyphicon glyphicon-chevron-right' aria-hidden='true'></span>
        <span class='sr-only'>Next</span>
    </a>
</div>


<div class="container-fluid">
    <p></p>
    <p>
        <img class="img-circle img-responsive center-block" src="img/profile.JPG" alt="Pic of Me" />
    </p>
    <p></p>
</div>

<?php
echo $partial->foot();

?>
