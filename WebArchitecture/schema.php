<?php
include("partial.php");

$partial = new Partial("schema");
echo $partial->head("Schema");
?>

<h1 style="margin-left: 30px"> Current Schema </h1>
<div class="page-header"></div>
<div class="container">
    <img class="img-responsive center-block img-rounded" src="img/Schema.png" alt="Schema Pic"/>
</div>

<?php
echo $partial->foot();
?>