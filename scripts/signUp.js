class User {
  constructor(FirstName, LastName, Username, Number, Password) {
    this.FirstName = FirstName;
    this.LastName = LastName;
    this.Username = Username;
    this.Number = Number;
    this.Password = Password;
  }
}

const API_URL = "http://localhost:5067";
const CadrastroBtn = document.getElementById("CriarContaBtn");
const RedirecionarBtn = document.getElementById("PossuiContaBtn");
const username = document.getElementById("username");
const phoneNumber = document.getElementById("phoneNumber");
const password = document.getElementById("password");
const password2 = document.getElementById("password2");
const form = document.getElementById("Form");

let FirstName = "";
let LastName = "";
let Username = "";
let PhoneNumber = "";
let Password = "";
let Password2 = "";

firstname.addEventListener("input", (event) => {
  FirstName = event.target.value;
});
lastname.addEventListener("input", (event) => {
  LastName = event.target.value;
});
username.addEventListener("input", (event) => {
  Username = event.target.value;
});
phoneNumber.addEventListener("input", (event) => {
  PhoneNumber = event.target.value;
});
password.addEventListener("input", (event) => {
  Password = event.target.value;
});
password2.addEventListener("input", (event) => {
  Password2 = event.target.value;
});
form.addEventListener("submit", async (event) => {
  event.preventDefault();

  try {
    const ResposeData = await Cadrasta();
    console.log(ResposeData);

    if (ResposeData != null) {
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

      if (serverJS) window.location.href = `http://localhost:${PORT}/signin`;
      else if (liveServer)
        window.location.href = `http://127.0.0.1:${PORT}/views/signin.html`; /* Enviando pra 127.0.0.1 apenas pra manter semantica do live server
      //                                                                                      mas poderia ser localhost. */
    }
  } catch (error) {
    console.error("Error during sign up:", error);
    throw error;
  }
});

// Formata o numero de telefone para o padrao brasileiro
phoneNumber.oninput = function () {
  console.log(PhoneNumber);
  PhoneNumber = PhoneNumber.toString().replace(
    /(\d{2})(\d{1})(\d{4})(\d{4})/,
    "($1) $2 $3-$4"
  );
  console.log(PhoneNumber);
  document.getElementById("phoneNumber").value = PhoneNumber;
};

// Verifica se as duas senhas sao iguais
password2.oninput = function () {
  if (Password != Password2) {
    password2.setCustomValidity("As senhas devem ser iguais!");
    password2.reportValidity();
    password2.style.border = "solid red";
  } else {
    password2.setCustomValidity("");
    password2.style.border = "1px solid black";
  }
};

// Envia os dados do cadrasto para o backend
async function Cadrasta() {
  try {
    const userData = new User(
      FirstName,
      LastName,
      Username,
      PhoneNumber,
      Password
    );
    console.log(userData);
    const response = await axios.post(`${API_URL}/users`, userData);
    return response;
  } catch (error) {
    console.error("Error during sign up:", error);
    throw error;
  }
}

// Redireciona o usuario para a pagina de login se o cadrastro for bem-sucedido
// function Redirecionar() {
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

//   if (serverJS) window.location.href = `http://localhost:${PORT}/signin`;
//   else if (liveServer)
//     window.location.href = `http://127.0.0.1:${PORT}/views/signin.html`; /* Enviando pra 127.0.0.1 apenas pra manter semantica do live server
//       //                                                                                      mas poderia ser localhost. */
// }
