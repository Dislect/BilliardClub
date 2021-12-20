function parallax(event) {
    this.querySelectorAll('.layer').forEach(element => {
        element.style.transform = "translateX(${event.clientX/105}px)";
        console.log(event.clientX);
    });
};

document.addEventListener('mousemove', parallax);;

