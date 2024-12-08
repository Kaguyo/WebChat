const typeMessage = document.getElementById("typeMessage");
const div2 = document.getElementById("div2");
let typeMessageCheck = 1;
let input = document.getElementById("input");
let characterCount = 0;

let whiteSpace = false;

input.addEventListener('input', switchIcon);
input.addEventListener('keydown', (event) => {
    if (event.code == "Backspace" && characterCount >= 1){
        characterCount--;
    } else if (event.code == "Space"){
        whiteSpace = true;
    } if (event.code != "Backspace"){
        characterCount++;
    } if (characterCount == 0){
        whiteSpace = false;
    }
});
function switchIcon(e){
    if (e.target.value != 0 || whiteSpace){
        document.getElementById("submitMessage").style.visibility = "visible";
        document.getElementById("recordAudio").style.visibility = "hidden";
        typeMessageCheck = 0;
    } else {
        document.getElementById("submitMessage").style.visibility = "hidden";
        document.getElementById("recordAudio").style.visibility = "visible";
        typeMessageCheck = 1;
    }
    if (typeMessageCheck == 1){
        typeMessage.textContent = "Digite uma mensagem..." ;
    } else {
        typeMessage.textContent = null;
    }
}