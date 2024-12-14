class Usuario {
    constructor(Id, Nome, Number, Password, Password2){
        this.Id = Id;
        this.Nome = Nome;
        this.Number = Number;
        this.Password = Password;
        this.Password2 = Password2;
    }
    static usersCount = 0;   
}