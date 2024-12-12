class Usuario {
    constructor(id, nome, number, password, password2){
        this.id = id;
        this.nome = nome;
        this.number = number;
        this.password = password;
        this.password2 = password2;
    }
    static usersCount = 0;   
}