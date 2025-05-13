const div2 = document.getElementById("div2");
let input = document.getElementById("myInput");
const API_URL = "http://localhost:5067";
const token = localStorage.getItem('token');

// window.onload(TokenCheck())


// async function TokenCheck() {
//     if (!token) {
//     // Se não houver token, redireciona para login
//     window.location.href = 'http://localhost:2200/signin';
//   } else {
//     const response = await axios.get(`${API_URL}/api/auth/login`, {
//         headers: {
//         Authorization: `Bearer ${token}`
//       }
//     })
//     if(response.data){
//         window.alert("Dados protegidos!!!")
//     }
// };
// }

  

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