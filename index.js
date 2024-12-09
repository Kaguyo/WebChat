const div2 = document.getElementById("div2");
let input = document.getElementById("myInput");

input.addEventListener('input', (event) => {
    let textoDigitado = event.target.value;

    if (textoDigitado.length > 0){
        switchIcon(event, true);
    } else {
        switchIcon(event, false);
    }
});

function switchIcon(event, foundCharacter){
    if (event.target.value.length > 0 && foundCharacter){
        document.getElementById("submitMessage").style.visibility = "visible";
        document.getElementById("recordAudio").style.visibility = "hidden";
    } else if (event.target.value.length == 0) {
        document.getElementById("submitMessage").style.visibility = "hidden";
        document.getElementById("recordAudio").style.visibility = "visible";
    }
}