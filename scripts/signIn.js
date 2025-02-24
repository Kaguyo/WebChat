class Login {
    constructor(PhoneNumber, Password){
        this.PhoneNumber = PhoneNumber;
        this.Password = Password;
    }  
}

const LoginBtn = document.getElementById("LoginBtn");
const phonenumber = document.getElementById("username");
const password = document.getElementById("password");

let PhoneNumber = "";
let Password = "";


phonenumber.addEventListener('input', (event) => {
    PhoneNumber = event.target.value;
});
password.addEventListener('input', (event) => {
    Password = event.target.value;
});

const API_URL = 'http://localhost:5067';

LoginBtn.onclick = async function ()    
{
    try{

        const userLogin = new Login(PhoneNumber, Password);
        console.log(userLogin);
        console.log("USERNAME :", userLogin.PhoneNumber);
        const response = await axios.post(`${API_URL}/users/`, userLogin);
        console.log(response.data);
        return response.data;

    } catch (error) {
        
        console.error('Error during sign up:', error);
        throw error;
    }
}

