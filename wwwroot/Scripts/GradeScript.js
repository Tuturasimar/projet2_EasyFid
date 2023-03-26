$(document).ready(() => {
    console.log("test");

    
})

function testFunction(arg, turn) {
    var value = parseInt(arg.substring(arg.indexOf("-") + 1)) + 1;
    var string = arg.substring(0, arg.indexOf("-"));

    console.log(value, string);

    var input = document.querySelector(`#${string}${turn}`);
    input.value = value;

    var parentDiv = input.closest(`#${string}Grade`);
    var childStar = parentDiv.children;

    for (var i = 1; i < (childStar.length)-1; i++) {
        if (i <= value) {
            childStar[i].className = "fa fa-star goldStar";
        } else {
            childStar[i].className = "fa fa-star-o RedStar";
        }
    }

   
}
