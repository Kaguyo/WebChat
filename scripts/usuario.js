class Usuario {
    constructor(nome, idade, pfp, bio){
        this.nome = nome;
        this.idade = idade;
        this.pfp = pfp;
        this.bio = bio;
    }   
}

class Conta extends Usuario {
    constructor(number){
        this.number = number
    }
}