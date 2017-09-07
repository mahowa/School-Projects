/**
 * Created by mahowa on 4/2/2016.
 */


var color_array = [0xff0000, 0x00ff00, 0x0000ff, 0xffff00,0xa74ac7,0xffd700,0x98afc7 ];
var color_index = 5;
var pieces = 7;
var GRID_WIDTH = 10;
var GRID_HEIGHT = 20;
var stage;
var renderer;
var isGameOver = false;
var score = 0;


//Pixi
class Piece {
    constructor(matrix, tetris) {
        var val = color_index % pieces;
        //Generate Pieces
        switch(val){
            //Square
            case 0:
                this.values = [
                    [0, 0],
                    [0, 1],
                    [1, 0],
                    [1, 1]
                ];

                break;
            //Z
            case 1:
                this.values = [
                    [0, 0],
                    [1, 0],
                    [1, 1],
                    [2, 1]
                ];

                break;
            //S
            case 2:
                this.values = [
                    [1, 0],
                    [2, 0],
                    [0, 1],
                    [1, 1]
                ];
                break;
            //I
            case 3:
                this.values = [
                    [0, 0],
                    [0, 1],
                    [0, 2],
                    [0, 3]
                ];
                break;
            //J
            case 4:
                this.values = [
                    [1, 0],
                    [1, 1],
                    [1, 2],
                    [0, 2]
                ];

                break;
            //L
            case 5:
                this.values = [
                    [0, 0],
                    [0, 1],
                    [0, 2],
                    [1, 2]
                ];

                break;
            //T
            case 6:
                this.values = [
                    [1, 0],
                    [0, 1],
                    [1, 1],
                    [2, 1]
                ];

                break;

        }

        //Piece number
        this.index = val;



        //Grid Location
        this.i = 0;
        this.j = 0;
        this.shiftX = 0;
        this.shiftY = 0;

        this.color = color_array[color_index];

        color_index = (color_index + 1) % pieces;

    }
    //Move piece down on grid
    down() {
        this.j++
    }
    //Move piece up on grid
    up() {
        this.j--
    }
    //Move piece left on grid
    left() {
        this.i--;
        if (this.i < 0) {
            this.i = 0
        }
    }
    //Move piece right on grid
    right() {
        this.i++;
        if (this.i > 9) {
            this.i = 9
        }
    }
    //Rotate piece on on grid
    rotateLeft(){

        var maxX=0,minX=GRID_WIDTH,maxY=GRID_HEIGHT,minY=0;
        // calculate mins/maxes
        for (var k = 0; k < this.values.length; k++) {
            if (this.values[k][0] > maxX) {
                maxX = this.values[k][0];
            }
            if (this.values[k][0] < minX) {
                minX = this.values[k][0];
            }
            if (this.values[k][1] > maxY) {
                maxY = this.values[k][1];
            }
            if (this.values[k][1] < minY) {
                minY = this.values[k][1];
            }
        }

        //for I
        if(this.index == 3){
            if (maxX > maxY) {
                this.shiftX = 2;
                this.shiftY = -2;
            }
            if (maxY > maxX) {
                this.shiftX = -2;
                this.shiftY = 2;
            }
        }


        var newpoints = [];
        for (var j = 0; j < this.values.length; j++) {
            newpoints[j] = [];
            var pointX = this.values[j][0] - minX;
            var pointY = this.values[j][1] - minY;
            newpoints[j][0] = pointY;
            newpoints[j][1] = -pointX + maxX - minX;
            newpoints[j][0] += minX + this.shiftX;
            newpoints[j][1] += minY + this.shiftY;
        }

        this.values = newpoints;

    }
    //Draw the piece
    draw(matrix) {
        this.matrix = matrix;

        if(this.i == 0 && this.j ==0){
            if(this.conflict(matrix)){
                gameOver();
            }
        }

        for (var i = 0; i < this.values.length; i++) {
            var cell = matrix[this.i + this.values[i][0]][this.j + this.values[i][1]];
            cell.visible = true;
            cell.color = this.color;
            cell.draw()
        }
    }
    //Using the color fill in the cells
    fill(matrix) {
        for (var i = 0; i < this.values.length; i++) {
            var cell = matrix[this.i + this.values[i][0]][this.j + this.values[i][1]];
            cell.visible = true;
            cell.color = this.color;
            cell.filled = true;
            cell.draw()
        }
    }
    //check if there is a conflict with grid bounds and ocupied cells
    conflict(matrix) {
        for (var i = 0; i < this.values.length; i++) {
            var i_offset = this.i + this.values[i][0];
            var j_offset = this.j + this.values[i][1];
            if (j_offset >= GRID_HEIGHT || j_offset==-1) return true;
            if(i_offset >= GRID_WIDTH || i_offset == -1) return true;
            var cell = matrix[i_offset][j_offset];
            console.log(cell);
            if (cell.filled) return true
        }
        return false
    }

}
class Cell {
    constructor(container, i, j) {
        this.square = new PIXI.Graphics();
        container.addChild(this.square);
        this.square.x = i * 25;
        this.square.y = j * 25;
        this.square.mouseover = function() {
            console.log("mouse over")
        };
        this.filled = false;
        this.color = 0xffffff;
        this.clear_color = 0x000000;
    }
    revert(){
        this.square.clear();
        this.square.beginFill(this.clear_color);
        this.square.drawRect(0, 0, 25, 25);
        this.square.endFill()
    }
    draw() {
        this.square.clear();
        this.square.beginFill(this.color);
        this.square.drawRect(0, 0, 25, 25);
        this.square.endFill()
    }
    move(i,j){
        this.square.x = i * 25;
        this.square.y = j * 25;
    }
    set visible(value) {
        this.square.visible = value
    }
}
class Tetris {
    constructor(stage) {
        console.log("constructor for tetris");
        this.board = new PIXI.Sprite();
        this.outline = new PIXI.Graphics();
        this.board.x = 50;
        this.board.y = 50;
        this.draw_outline();
        this.board.addChild(this.outline);
        this.build_matrix(this.board);
        stage.addChild(this.board);
        this.current_piece = new Piece(this.board_matrix, this);
        this.draw_piece();
        document.addEventListener('keydown', this.handle_key_presses.bind(this));

        //Make the piece drop
        setInterval(this.drop.bind(this),800);
    }
    handle_key_presses(key) {
        //Pause if game area is not in focus
        if(isGameOver){
            return;
        }


        if (key.code == "ArrowDown") {
            this.drop();
            return;
        }

        if (key.code == "ArrowLeft") {
            this.current_piece.left();
            if (this.current_piece.conflict(this.board_matrix)) {
                this.current_piece.right()
            }
        }
        if (key.code == "ArrowRight") {
            this.current_piece.right();
            if (this.current_piece.conflict(this.board_matrix)) {
                this.current_piece.left()
            }
        }
        if (key.code == "ArrowUp") {
            this.current_piece.rotateLeft();
            if (this.current_piece.conflict(this.board_matrix)) {
                this.current_piece.rotateLeft();
            }
        }
        if (key.code == "Space") {
            while(1){
                if(!this.drop())
                    break;
            }
        }

        this.clear_board();
        this.draw_piece()
    }
    drop(){
        ////Pause if game area is not in focus
        //var focus = document.activeElement;
        if(isGameOver){
            return;
        }
        this.current_piece.down();

        if (this.current_piece.conflict(this.board_matrix)) {
            this.current_piece.up();
            this.current_piece.fill(this.board_matrix);
            this.clearRow();
            this.current_piece = new Piece(this.board_matrix, this);
            this.clear_board();
            this.draw_piece()
            return false;
        }
        this.clear_board();
        this.draw_piece()
        return true;
    }
    clearRow() {
        for (var i = 0; i < GRID_HEIGHT; i++) {
            if (this.lineCanBeCleared(i)) {
                this.clearLine(i);
                score++;
                $("#score").html(score*1000)
            }
        }

    }
    clearLine(line) {
        var newgrid = [];
        for (var x = 0; x < GRID_WIDTH; x++) {
            newgrid[x] = [];

            for(var y = line-1; y>=0; y--){
                if(this.board_matrix[x][y].filled == false){
                    this.board_matrix[x][y+1].filled = false;
                    this.board_matrix[x][y+1].color = this.board_matrix[x][y].color;
                    break;
                }
            }

            //// keep other lines the same
            //for (var y = line + 1; y < GRID_HEIGHT; y++) {
            //    newgrid[x][y] = this.board_matrix[x][y];
            //}
        }
        // clear top row
        for (var i = 0; i < GRID_WIDTH; i++) {
            this.board_matrix[i][0] = new Cell(this.board,i,0);
        }
      //  this.board_matrix = newgrid;

    }
    lineCanBeCleared(line) {
        for (var i = 0; i < GRID_WIDTH; i++) {
            if (this.board_matrix[i][line].filled === false) {
                return false;
            }
        }
        return true;
    }
    draw_piece() {
        this.current_piece.draw(this.board_matrix);
    }
    build_matrix(container) {
        this.board_matrix = [];
        for (var i = 0; i < GRID_WIDTH; i++) {
            this.board_matrix[i] = [];
            for (var j = 0; j < GRID_HEIGHT; j++) {
                this.board_matrix[i][j] = new Cell(container, i, j);
                this.board_matrix[i][j].visible = false
            }
        }
    }
    clear_board() {
        for (var i = 0; i < GRID_WIDTH; i++) {
            for (var j = 0; j < GRID_HEIGHT; j++) {
                this.board_matrix[i][j].draw();
                if (this.board_matrix[i][j].filled) {
                    this.board_matrix[i][j].visible = true
                } else {
                    this.board_matrix[i][j].visible = false
                }
            }
        }
    }
    draw_outline() {
        this.outline.clear();
        this.outline.lineStyle(2, 0x777777);
        this.outline.beginFill();
        this.outline.drawRect(0, 0, 250, 500);
        this.outline.endFill()
    }
}

//UI
function gameOver(){
    if(!isGameOver) {
        $('#myModal').modal('show');
        isGameOver = true;
    }
}
function loadTetris() {
    $("#playbtn").hide();

    var width = $(window).width();
    //
    if(width <992)
    {
        $('.collapse').collapse({
            toggle: true
        });
    }

    stage = new PIXI.Container();
    stage.interactive = true;

    renderer = PIXI.autoDetectRenderer(355, 600, { transparent: true });

    var tetris =  $("#tetris");
    tetris.append(renderer.view);
    tetris.append("<h1 >Score: <span id='score'>0</span></h1>");

    requestAnimationFrame(animate);
    new Tetris(stage);
}
function animate() {
    requestAnimationFrame(animate);
    renderer.render(stage);
}
function submitscore(){
    var score = $("#score").html();
    var name = $("#name").val();
    var url_string = "submitscore.php?submit="+score+"&name="+name;
    $.ajax({
        url: url_string,
        type: 'GET',
        success: function (res) {
            location.reload();

        },
        error: function (obj, status, msg) {

        }
    });



}
function getLeaderBoards() {

    var url_string = "submitscore.php";
    $.ajax({
        url: url_string,
        type: 'GET',
        success: function (res) {

            var obj = JSON.parse(res);
            var html = "<tr><th style='border-top: none !important;'>Rank</th><th style='border-top: none !important;'>Name</th><th style='border-top: none !important;'>Score</th></tr>";

            for(var i =0; i<obj.length; i++) {
                html +="<tr><td><span class='badge'>"+(i+1)+"</span></td><td>"+obj[i][0]+"</td><td>"+obj[i][1]+"</td></tr>";
            }


            $('#leaderboard').html(html);
        },
        error: function (obj, status, msg) {

        }
    });

}

$(document).ready(setInterval(getLeaderBoards,1000));
