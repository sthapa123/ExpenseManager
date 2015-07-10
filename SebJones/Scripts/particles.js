// Variables
var el = document.getElementById('particles');
var $el = $('#particles');
var canvasSupport = !!document.createElement('canvas').getContext;
var canvas;
var $canvas;
var ctx;
var particles = [];
var winW;
var winH;
var raf;
var paused = false;

// Options
var minSpeedX = 0.1;
var maxSpeedX = 0.7;
var minSpeedY = 0.1;
var maxSpeedY = 0.7;
var directionX = 'center';
var directionY = 'center';
var dotColor = 'whitesmoke';
var lineColor = 'whitesmoke';
var lineWidth = 1;
var density = 10000;
var particleRadius = 7;
var curvedLines = false;
var proximity = 100;
var parallax = false;
var parallaxMultiplier = 5;

// Initialise
function init() {

    // Check browser compatibility
    if (!canvasSupport) {
        return;
    }

    // Create canvas
    $canvas = $('<canvas class="pg-canvas"></canvas>');
    $el.prepend($canvas);
    canvas = $canvas[0];
    ctx = canvas.getContext('2d');
    styleCanvas();

    // Create particles
    var numParticles = Math.round((canvas.width * canvas.height) / density);
    for (var i = 0; i < numParticles; i++) {
        var p = new Particle();
        p.setStackPos(i);
        particles.push(p);
    }

    // Window Resize
    $(window).bind('resize', resizeHandler);

    //$(window).on('resize', function () {
    //    resizeHandler();
    //});

    //$(window).resize(function () {
    //    resizeHandler();
    //});

    draw();
}

// Configure the canvas
function styleCanvas() {
    canvas.width = $el.width();
    canvas.height = $el.height();
    ctx.fillStyle = 'whitesmoke';
    ctx.strokeStyle = 'whitesmoke';
    ctx.lineWidth = 1;
}

// Draw particles
function draw() {

    winW = $(window).width();
    winH = $(window).height();

    // Wipe canvas
    ctx.clearRect(0, 0, canvas.width, canvas.height);

    // Update particle positions
    for (var i = 0; i < particles.length; i++) {
        particles[i].updatePosition();
    }

    // Draw particles
    for (var i = 0; i < particles.length; i++) {
        particles[i].draw();
    }

    // Call this function next time screen is redrawn
    if (!paused) {
        raf = requestAnimationFrame(draw);
    }
}

// Add/remove particles
function resizeHandler() {
    // Resize the canvas
    styleCanvas();

    // Remove particles that are outside the canvas
    for (var i = particles.length - 1; i >= 0; i--) {
        if (particles[i].position.x > $el.width() || particles[i].position.y > $el.height()) {
            particles.splice(i, 1);
        }
    }

    // Adjust particle density
    var numParticles = Math.round((canvas.width * canvas.height) / density);
    if (numParticles > particles.length) {
        while (numParticles > particles.length) {
            var p = new Particle();
            particles.push(p);
        }
    }
    else if (numParticles < particles.length) {
        particles.splice(numParticles);
    }

    // Re-index particles
    for (i = particles.length - 1; i >= 0; i--) {
        particles[i].setStackPos(i);
    }
}

// Pause particle system
function pause() {
    paused = true;
}

// Start particle system
function start() {
    paused = false;
    draw();
}

// Particle class
function Particle() {

    this.active = true;
    this.layer = Math.ceil(Math.random() * 3);
    this.parallaxOffsetX = 0;
    this.parallaxOffsetY = 0;

    // Initial particle position
    this.position = {
        x: Math.ceil(Math.random() * canvas.width),
        y: Math.ceil(Math.random() * canvas.height)
    };

    // Random particle speed, within min and max values
    this.speed = {};
    switch (directionX) {
        case 'left':
            this.speed.x = +(-maxSpeedX + (Math.random() * maxSpeedX) - minSpeedX).toFixed(2);
            break;
        case 'right':
            this.speed.x = +((Math.random() * maxSpeedX) + minSpeedX).toFixed(2);
            break;
        default:
            this.speed.x = +((-maxSpeedX / 2) + (Math.random() * maxSpeedX)).toFixed(2);
            this.speed.x += this.speed.x > 0 ? minSpeedX : -minSpeedX;
            break;
    }
    switch (directionY) {
        case 'up':
            this.speed.y = +(-maxSpeedY + (Math.random() * maxSpeedY) - minSpeedY).toFixed(2);
            break;
        case 'down':
            this.speed.y = +((Math.random() * maxSpeedY) + minSpeedY).toFixed(2);
            break;
        default:
            this.speed.y = +((-maxSpeedY / 2) + (Math.random() * maxSpeedY)).toFixed(2);
            this.speed.x += this.speed.y > 0 ? minSpeedY : -minSpeedY;
            break;
    }
}

// Draw particle
Particle.prototype.draw = function () {

    // Draw circle
    ctx.beginPath();
    ctx.arc(this.position.x + this.parallaxOffsetX, this.position.y + this.parallaxOffsetY,
        particleRadius / 2, 0, Math.PI * 2, true);
    ctx.closePath();
    ctx.fill();

    // Draw lines
    ctx.beginPath();

    // Iterate over all particles which are higher in the stack than this one
    for (var i = particles.length - 1; i > this.stackPos; i--) {
        var p2 = particles[i];

        // Pythagorus theorum to get distance between two points
        var a = this.position.x - p2.position.x;
        var b = this.position.y - p2.position.y;
        var dist = Math.sqrt((a * a) + (b * b)).toFixed(2);

        // If the two particles are in proximity, join them
        if (dist < proximity) {
            ctx.moveTo(this.position.x + this.parallaxOffsetX, this.position.y + this.parallaxOffsetY);
            if (curvedLines) {
                ctx.quadraticCurveTo(Math.max(p2.position.x, p2.position.x), Math.min(p2.position.y, p2.position.y),
                    p2.position.x + p2.parallaxOffsetX, p2.position.y + p2.parallaxOffsetY);
            }
            else {
                ctx.lineTo(p2.position.x + p2.parallaxOffsetX, p2.position.y + p2.parallaxOffsetY);
            }
        }
    }
    ctx.stroke();
    ctx.closePath();

};

// Update particle position
Particle.prototype.updatePosition = function () {

    //if (parallax) {

    //}

    switch (directionX) {
        case 'left':
            if (this.position.x + this.speed.x + this.parallaxOffsetX < 0) {
                this.position.x = $el.width() - this.parallaxOffsetX;
            }
            break;
        case 'right':
            if (this.position.x + this.speed.x + this.parallaxOffsetX > $el.width()) {
                this.position.x = 0 - this.parallaxOffsetX;
            }
            break;
        default:
            // If particle has reached edge of canvas, reverse its direction
            if (this.position.x + this.speed.x + this.parallaxOffsetX > $el.width() ||
                this.position.x + this.speed.x + this.parallaxOffsetX < 0) {
                this.speed.x = -this.speed.x;
            }
            break;
    }

    switch (directionY) {
        case 'up':
            if (this.position.y + this.speed.y + this.parallaxOffsetY < 0) {
                this.position.y = $el.height() - this.parallaxOffsetY;
            }
            break;
        case 'down':
            if (this.position.y + this.speed.y + this.parallaxOffsetY > $el.height()) {
                this.position.y = 0 - this.parallaxOffsetY;
            }
            break;
        default:
            // If particle has reached edge of canvas, reverse its direction
            if (this.position.y + this.speed.y + this.parallaxOffsetY > $el.height() ||
                this.position.y + this.speed.y + this.parallaxOffsetY < 0) {
                this.speed.y = -this.speed.y;
            }
            break;
    }

    // Move particle
    this.position.x += this.speed.x;
    this.position.y += this.speed.y;

};

// Set particle stacking position
Particle.prototype.setStackPos = function (i) {
    this.stackPos = i;
};

init();