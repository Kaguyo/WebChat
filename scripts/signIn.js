class Login {
    constructor(Nome, Password){
        this.Nome = Nome;
        this.Password = Password;
    }  
}

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

LoginBtn.onclick = function (){
    try {
        const objetoLogin = new Login(Username, Password);
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
        // .then(response => response.bytes)
        .then(data => {
            console.log("Dados recebidos:", data);
            // jsonData = data.json();
            // console.log(jsonData);
            // let jsonObject = JSON.parse(jsonData);
            // console.log(jsonObject);
            // Redirecionar();
        })
        .catch(error => {
            console.log("Erro ao enviar dados:", error);
        });
    } catch(err){
        console.log(`Error. User wasn't created. Error: ${err.message}`);
        console.log(`Error details: ${err}`);
    }
}

function Redirecionar (){
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
    console.log("URL COMPLETO: ", urlAtual);
    // console.log("URL SEM HTTP: ", treatedURL);
    // console.log("PORTA: ", PORT);
    
    if (serverJS) window.location = `http://localhost:${PORT}/index`;
    else if (liveServer) window.location = `http://127.0.0.1:${PORT}/views/index.html`; /* Enviando pra 127.0.0.1 apenas pra manter semantica do live server
    //                                                                                      mas poderia ser localhost. */
}