class Usuario {
     constructor(name, number, pfp, bio){
         this.name = name;
         this.number = number;
         this.pfp = pfp;
         this.bio = bio;
     }

     get nomeUsuario(){
         return this.name;
     }

     set nomeUsuario(nomeUsuario){
         this.nome = nomeUsuario;
     }

     get numeroUsuario(){
         return this.number
     }

     set numeroUsuario(numeroUsuario){
         this.numero = numeroUsuario;
     }
 }