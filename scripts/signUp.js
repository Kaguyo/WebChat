class User {
    constructor(Username, Number, Password, Password2){
        this.Username = Username;
        this.Number = Number;
        this.Password = Password;
        this.Password2 = Password2;
    }  
}


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

// CadrastroBtn.onclick = function (){
//     try {
//         const objetoUsuario = new User(Username, PhoneNumber, Password, Password2);
//         const jsonUsuario = JSON.stringify(objetoUsuario);
//         console.log(objetoUsuario);
//         console.log(jsonUsuario);
//         fetch("http://localhost:5067/", {
//             method: "POST",
//             headers: {
//                 "Content-Type": "application/json"
//             },
//             body: jsonUsuario
//         })
//         .then(response => response.json())
//         .then(data => {
//             console.log("Dados recebidos:", data);
//         })
//         .catch(error => {
//             console.log("Erro ao enviar dados:", error);
//         });
//     } catch(err){
//         console.log(`Error. User wasn't created. Error: ${err.message}`);
//         console.log(`Error details: ${err}`);
//     }
// }

const API_URL = 'http://localhost:5067';

CadrastroBtn.onclick = async function ()    
{
    try{

        const userData = new User(Username, PhoneNumber, Password, Password2);
        console.log(userData);
        console.log("USERNAME :", userData.Username);
        const response = await axios.post(`${API_URL}/users`, userData);
        console.log(response.data);
        return response.data;

    } catch (error) {
        
        console.error('Error during sign up:', error);
        throw error;
    }
}

function redirecionar(){
    let urlAtual = window.location.href; // URL completo
    let treatedURL; // Usado pra receber PORT apartir da PORT localizada no URL
    let PORT = "";
    let collectingPort = true; // Bool de permissao para copiar PORT

    let serverJS = false; // Bool pra decisao de redirecionamento
    let liveServer = false; // Bool pra decisao de redirecionamento

    // Verifica URL da que pagina rodando, e pega a PORT dinamicamente
    if (urlAtual.startsWith("http://localhost:")){
        treatedURL = urlAtual.slice(17);
        for (i = 0; i < treatedURL.length; i++){
            if (treatedURL[i] != "/" && collectingPort){
                PORT += treatedURL[i];
            } else {
                collectingPort = false; // Cancela coleta
            }
        }
        serverJS = true;

    } else if (urlAtual.startsWith("http://127.0.0.1:")){
        treatedURL = urlAtual.slice(17);
        for (i = 0; i < treatedURL.length; i++){
            if (treatedURL[i] != "/" && collectingPort){
                PORT += treatedURL[i];
            } else {
                collectingPort = false; // Cancela coleta
            }
        }
        liveServer = true;
    }

    // console.log("URL COMPLETO: ", urlAtual);
    // console.log("URL SEM HTTP: ", treatedURL);
    // console.log("PORTA: ", PORT);
    
    if (serverJS) window.location = `http://localhost:${PORT}/signin`;
    else if (liveServer) window.location = `http://127.0.0.1:${PORT}/views/signin.html`; /* Enviando pra 127.0.0.1 apenas pra manter semantica do live server
    //                                                                                      mas poderia ser localhost. */
}

