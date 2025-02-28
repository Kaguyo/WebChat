# CRUD de Usuários - Fan Game Fate Grand Order x Star Rail

Este é um projeto backend para um **CRUD de Usuários**, desenvolvido para um **fan game de Fate Grand Order x Star Rail**. O projeto segue o **Repository Pattern** e utiliza uma API minimalista com .NET 8.0.

## Conceito
- Implementa operações CRUD (Criar, Ler, Atualizar, Deletar) para usuários.
- Utiliza Repository Pattern para separação de lógica e persistência de dados.
- API minimalista com .NET 8.0.

## Aviso Legal
Este projeto é **apenas um fan game sem fins lucrativos**. Todos os créditos pertencem às obras originais:
- [Fate Grand Order](https://www.fate-go.jp/)
- [Honkai: Star Rail](https://hsr.hoyoverse.com/)

## Implementação
Atualmente, **a persistência de dados ainda não está implementada**, sendo utilizada uma lista em memória.

## Exemplo de Retorno (POST /users)
```json
{
  "username": "Jogador1",
  "email": "jogador1@email.com",
  "profileImage": null
}
```

## Autor
Desenvolvido por Kaguyo.

