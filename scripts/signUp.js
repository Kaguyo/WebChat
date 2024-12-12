const CadrastroBtn = document.getElementById("CriarContaBtn");
const username = document.getElementById("username");
const phoneNumber = document.getElementById("phoneNumber");
const password = document.getElementById("password");
const password2 = document.getElementById("password2");

let _username = "";
let _phoneNumber = "";
let _password = "";
let _password2 = "";

username.addEventListener('input', (event) => {
    _username = event.target.value;
});
phoneNumber.addEventListener('input', (event) => {
    _phoneNumber = event.target.value;
});
password.addEventListener('input', (event) => {
    _password = event.target.value;
});
password2.addEventListener('input', (event) => {
    _password2 = event.target.value;
});

CadrastroBtn.onclick = function (){
    Usuario.usersCount ++;
    const usuario = new Usuario(Usuario.usersCount, _username, _phoneNumber, _password, _password2);
    console.log(usuario);
}