class Login {
  constructor(PhoneNumber, Password) {
    this.PhoneNumber = PhoneNumber;
    this.Password = Password;
  }
}

const LoginBtn = document.getElementById("LoginBtn");
const phonenumber = document.getElementById("phoneNumber");
const password = document.getElementById("password");

let PhoneNumber = "";
let Password = "";

phonenumber.addEventListener("input", (event) => {
  PhoneNumber = event.target.value;
});
password.addEventListener("input", (event) => {
  Password = event.target.value;
});

phonenumber.oninput = function () {
  console.log(PhoneNumber);
  PhoneNumber = PhoneNumber.toString().replace(
    /(\d{2})(\d{1})(\d{4})(\d{4})/,
    "($1) $2 $3-$4"
  );
  console.log(PhoneNumber);
  document.getElementById("phoneNumber").value = PhoneNumber;
};

const API_URL = "http://localhost:5067";

LoginBtn.onsubmit = async function () {
  try {
    const userLogin = new Login(PhoneNumber, Password);
    console.log(userLogin);
    console.log("USERNAME :", userLogin.PhoneNumber);
    const response = await axios.post(`${API_URL}/users/`, userLogin);
    console.log(response.data);
    return response.data;
  } catch (error) {
    console.error("Error during sign up:", error);
    throw error;
  }
};
