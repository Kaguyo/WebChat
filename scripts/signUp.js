const CadrastroBtn = document.getElementById("CriarContaBtn");
const username = document.getElementById("username");
const phoneNumber = document.getElementById("phoneNumber");
const password = document.getElementById("password");
const password2 = document.getElementById("password2");

let Username = "";
let PhoneNumber = "";
let Password = "";
let Password2 = "";

username.addEventListener('input', (event) => {
    Username = event.target.value;
});
phoneNumber.addEventListener('input', (event) => {
    PhoneNumber = event.target.value;
});
password.addEventListener('input', (event) => {
    Password = event.target.value;
});
password2.addEventListener('input', (event) => {
    Password2 = event.target.value;
});

CadrastroBtn.onclick = function (){
    try {
        const objetoUsuario = new Usuario(Username, PhoneNumber, Password, Password2);
        const jsonUsuario = JSON.stringify(objetoUsuario);
        console.log(objetoUsuario);
        console.log(jsonUsuario);
        fetch("http://localhost:5000/", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: jsonUsuario
        })
        .then(response => response.json())
        .then(data => {
            console.log("Dados recebidos:", data)
        })
        .catch(error => {
            console.log("Erro ao enviar dados:", error);
        });
    } catch(err){
        console.log(`Error. User wasn't created. Error: ${err.message}`);
        console.log(`Error details: ${err}`);
    }
}
function redirecionar(){
    window.location="http://localhost:2200/signin";
}