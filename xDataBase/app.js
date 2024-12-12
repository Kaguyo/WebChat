import sqlite3 from 'sqlite3';
import { open } from 'sqlite';


async function criarEPopularTabelaUsuarios(nome, number, password){
    const db = await open({
        filename: './xDataBase/banco.db',
        driver: sqlite3.Database,
    });

    db.run('CREATE TABLE IF NOT EXISTS usuarios (id INTEGER PRIMARY KEY, nome TEXT, number INTEGER, password VARCHAR)');
    db.run('INSERT INTO usuarios (nome, number, password) VALUES (?,?,?)', [nome, number, password])
    // db.run('DELETE FROM usuarios WHERE id = 1' )
    	
}

criarEPopularTabelaUsuarios('Kaguyo', '997296272');