function cadrasto() {
    let usuario = new Usuario ();
    let nome = document.getElementById("name").value;
    let telefone = document.getElementById("phoneNumber").value;

    console.log(nome)
    usuario.nomeUsuario = nome;
    usuario.numeroUsuario = telefone;
    console.log(usuario.name);

}