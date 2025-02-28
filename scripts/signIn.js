class Login {
  constructor(Number, Password) {
    this.Number = Number;
    this.Password = Password;
  }
}

const API_URL = "http://localhost:5067";
const LoginBtn = document.getElementById("LoginBtn");
const phonenumber = document.getElementById("phoneNumber");
const password = document.getElementById("password");
const errNumber = document.getElementById("ErrNumber");
const form = document.getElementById("Form");

let Number = "";
let Password = "";

phonenumber.addEventListener("input", (event) => {
  Number = event.target.value;
});
password.addEventListener("input", (event) => {
  Password = event.target.value;
});

<<<<<<< HEAD
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
=======
// form.addEventListener("submit", LoginFunction);

form.addEventListener("submit", async (event) => {
  event.preventDefault();

  try {
    const ResponseData = await LoginFunction();
    console.log("Dentro do form", ResponseData);

    if (ResponseData != null) {
      let urlAtual = window.location.href; // URL completo
      let treatedURL; // Usado pra receber PORT apartir da PORT localizada no URL
      let PORT = "";
      let collectingPort = true; // Bool de permissao para copiar PORT

      let serverJS = false; // Bool pra decisao de redirecionamento
      let liveServer = false; // Bool pra decisao de redirecionamento

      // Verifica URL da que pagina rodando, e pega a PORT dinamicamente
      if (urlAtual.startsWith("http://localhost:")) {
        treatedURL = urlAtual.slice(17);
        for (i = 0; i < treatedURL.length; i++) {
          if (treatedURL[i] != "/" && collectingPort) {
            PORT += treatedURL[i];
          } else {
            collectingPort = false; // Cancela coleta
          }
        }
        serverJS = true;
      } else if (urlAtual.startsWith("http://127.0.0.1:")) {
        treatedURL = urlAtual.slice(17);
        for (i = 0; i < treatedURL.length; i++) {
          if (treatedURL[i] != "/" && collectingPort) {
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

      if (serverJS) window.location.href = `http://localhost:${PORT}/index`;
      else if (liveServer)
        window.location.href = `http://127.0.0.1:${PORT}/views/index.html?=${ResponseData}`; /* Enviando pra 127.0.0.1 apenas pra manter semantica do live server
      //                                                                                      mas poderia ser localhost. */
    }
  } catch (error) {
    console.error("Error during sign up:", error);
    throw error;
  }
});

// Formata o numero de telefone para o padrao brasileiro
phonenumber.oninput = function () {
  console.log(Number);
  Number = Number.toString().replace(
    /(\d{2})(\d{1})(\d{4})(\d{4})/,
    "($1) $2 $3-$4"
  );

  console.log(Number);
  document.getElementById("phoneNumber").value = Number;
};

// Faz aparecer a mensagem de erro em baixo do number
phonenumber.onkeyup = function () {
  if (phonenumber.reportValidity()) {
    errNumber.style.display = "none";
  } else {
    errNumber.style.display = "block";
  }
};

// Funcao para enviar dados do login para o backend
async function LoginFunction() {
  try {
    const userLogin = new Login(Number, Password);
    console.log(userLogin);
    const response = await axios.post(`${API_URL}/users/login/`, userLogin);
    console.log(response.data);
    return response.data;
  } catch (error) {
    window.alert("Numbero ou senha errada!");
    console.error("Error during sign up:", error);
    throw error;
  }
}

// LoginBtn.onclick = () => {
//   let urlAtual = window.location.href; // URL completo
//   let treatedURL; // Usado pra receber PORT apartir da PORT localizada no URL
//   let PORT = "";
//   let collectingPort = true; // Bool de permissao para copiar PORT

//   let serverJS = false; // Bool pra decisao de redirecionamento
//   let liveServer = false; // Bool pra decisao de redirecionamento

//   // Verifica URL da que pagina rodando, e pega a PORT dinamicamente
//   if (urlAtual.startsWith("http://localhost:")) {
//     treatedURL = urlAtual.slice(17);
//     for (i = 0; i < treatedURL.length; i++) {
//       if (treatedURL[i] != "/" && collectingPort) {
//         PORT += treatedURL[i];
//       } else {
//         collectingPort = false; // Cancela coleta
//       }
//     }
//     serverJS = true;
//   } else if (urlAtual.startsWith("http://127.0.0.1:")) {
//     treatedURL = urlAtual.slice(17);
//     for (i = 0; i < treatedURL.length; i++) {
//       if (treatedURL[i] != "/" && collectingPort) {
//         PORT += treatedURL[i];
//       } else {
//         collectingPort = false; // Cancela coleta
//       }
//     }
//     liveServer = true;
//   }

//   // console.log("URL COMPLETO: ", urlAtual);
//   // console.log("URL SEM HTTP: ", treatedURL);
//   // console.log("PORTA: ", PORT);

//   if (serverJS) window.location.href = `http://localhost:${PORT}/index`;
//   else if (liveServer)
//     window.location.href = `http://127.0.0.1:${PORT}/views/index.html`; /* Enviando pra 127.0.0.1 apenas pra manter semantica do live server
//       //                                                                                      mas poderia ser localhost. */
// };
>>>>>>> v2.0
