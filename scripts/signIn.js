const LoginBtn = document.getElementById("LoginBtn");
const username = document.getElementById("username");
const password = document.getElementById("password");

let Username = "";
let Password = "";


username.addEventListener('input', (event) => {
    Username = event.target.value;
});
password.addEventListener('input', (event) => {
    Password = event.target.value;
});

CadrastroBtn.onclick = function (){
    try {
        const objetoLogin = new Login(Username, Password,);
        const jsonLogin = JSON.stringify(objetoLogin);
        console.log(objetoLogin);
        console.log(jsonLogin);
        fetch("http://localhost:5000/", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: jsonLogin
        })
        .then(response => response.json())
        .then(data => {
            console.log("Dados recebidos:", data);
        })
        .catch(error => {
            console.log("Erro ao enviar dados:", error);
        });
    } catch(err){
        console.log(`Error. User wasn't created. Error: ${err.message}`);
        console.log(`Error details: ${err}`);
    }
}